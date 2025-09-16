namespace WoofWare.Whippet

open System
open System.IO
open System.Reflection
open Ionide.ProjInfo
open Ionide.ProjInfo.Types

open WoofWare.DotnetRuntimeLocator
open WoofWare.Whippet.Core

type Args =
    {
        InputFile : FileInfo
        Plugins : FileInfo list
    }

type WhippetTarget =
    {
        InputSource : FileInfo
        GeneratedDest : FileInfo
        Params : Map<string, string>
        /// Suppress plugins with any of these names.
        Suppress : string Set
    }

module Program =
    let parseArgs (argv : string array) =
        let inputFile = argv.[0] |> FileInfo

        {
            InputFile = inputFile
            Plugins = argv.[1..] |> Seq.map FileInfo |> Seq.toList
        }

    let getGenerateRawFromRaw (host : obj) : (RawSourceGenerationArgs -> string option) option =
        let pluginType = host.GetType ()

        let generateRawFromRaw =
            match
                pluginType.GetMethod (
                    "GenerateRawFromRaw",
                    BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.FlattenHierarchy
                )
                |> Option.ofObj
            with
            | None ->
                pluginType.GetInterfaces ()
                |> Array.tryPick (fun interf ->
                    interf.GetMethod (
                        "GenerateRawFromRaw",
                        BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.FlattenHierarchy
                    )
                    |> Option.ofObj
                )
            | Some generateRawFromRaw -> Some generateRawFromRaw

        match generateRawFromRaw with
        | None -> None
        | Some generateRawFromRaw ->
            let pars = generateRawFromRaw.GetParameters ()

            if pars.Length <> 1 then
                failwith
                    $"Expected GenerateRawFromRaw to take exactly one parameter: a RawSourceGenerationArgs. Got %i{pars.Length}"

            if pars.[0].ParameterType.FullName <> typeof<RawSourceGenerationArgs>.FullName then
                failwith
                    $"Expected GenerateRawFromRaw to take exactly one parameter: a RawSourceGenerationArgs. Got %s{pars.[0].ParameterType.FullName}"

            let retType = generateRawFromRaw.ReturnType

            if retType <> typeof<string> then
                failwith
                    $"Expected GenerateRawFromRaw method to have return type `string`, but was: %s{retType.FullName}"

            fun args ->
                let args =
                    Activator.CreateInstance (
                        pars.[0].ParameterType,
                        [| box args.FilePath ; box args.FileContents ; box args.Parameters |]
                    )

                generateRawFromRaw.Invoke (host, [| args |]) |> unbox<string> |> Option.ofObj
            |> Some

    [<EntryPoint>]
    let main argv =
        let args = parseArgs argv

        let projectDirectory = args.InputFile.Directory
        let toolsPath = Init.init projectDirectory None
        let defaultLoader = WorkspaceLoader.Create (toolsPath, [])

        use subscription =
            defaultLoader.Notifications.Subscribe (fun msg ->
                match msg with
                | WorkspaceProjectState.Loading projectFilePath ->
                    Console.Error.WriteLine $"Loading: %s{projectFilePath}"
                | WorkspaceProjectState.Loaded (loadedProject, _knownProjects, fromCache) ->
                    let fromCache = if fromCache then " (from cache)" else ""
                    Console.Error.WriteLine $"Loaded %s{loadedProject.ProjectFileName}%s{fromCache}"
                | WorkspaceProjectState.Failed (projectFilePath, errors) ->
                    let errors = errors.ToString ()
                    failwith $"Failed to load project %s{projectFilePath}: %s{errors}"
            )

        let projectOptions =
            defaultLoader.LoadProjects [ args.InputFile.FullName ] |> Seq.toArray

        let desiredProject =
            projectOptions
            |> Array.find (fun po -> po.ProjectFileName = args.InputFile.FullName)

        let toGenerate =
            desiredProject.Items
            |> List.choose (fun (ProjectItem.Compile (_name, fullPath, metadata)) ->
                match metadata with
                | None -> None
                | Some metadata ->

                match Map.tryFind "WhippetFile" metadata with
                | None -> None
                | Some myriadFile ->

                let pars =
                    metadata
                    |> Map.toSeq
                    |> Seq.choose (fun (key, value) ->
                        if key.StartsWith ("WhippetParam", StringComparison.Ordinal) then
                            Some (key.Substring "WhippetParam".Length, value)
                        else
                            None
                    )
                    |> Map.ofSeq

                let suppress =
                    Map.tryFind "WhippetSuppressPlugin" metadata
                    |> Option.map (fun s -> s.Split ',' |> Set.ofArray)
                    |> Option.defaultValue Set.empty

                let inputSource =
                    FileInfo (Path.Combine (Path.GetDirectoryName desiredProject.ProjectFileName, myriadFile))

                let generatedDest = FileInfo fullPath

                if inputSource.FullName = generatedDest.FullName then
                    failwith $"Input source %s{inputSource.FullName} was identical to output path; aborting."

                {
                    GeneratedDest = generatedDest
                    InputSource = inputSource
                    Params = pars
                    Suppress = suppress
                }
                |> Some
            )

        let runtime =
            Assembly.GetExecutingAssembly().Location
            |> DotnetRuntime.SelectForDll
            |> Seq.map DirectoryInfo
            |> Seq.toList

        let plugins =
            args.Plugins
            |> List.map (fun pluginDll ->
                let ctx = Ctx (pluginDll, runtime)

                let pluginAssembly = ctx.LoadFromAssemblyPath pluginDll.FullName

                // We will look up any member called GenerateRawFromRaw and/or GenerateFromRaw.
                // It's your responsibility to decide whether to do anything with this call; you return null if you don't want
                // to do anything.
                // Alternatively, return the text you want to output.
                // We provide you with the input file contents.
                // GenerateRawFromRaw should return plain text.
                // GenerateFromRaw should return a Fantomas AST.
                let applicablePlugins =
                    pluginAssembly.ExportedTypes
                    |> Seq.choose (fun ty ->
                        if
                            ty.CustomAttributes
                            |> Seq.exists (fun attr ->
                                attr.AttributeType.Name = typeof<WhippetGeneratorAttribute>.Name
                            )
                        then
                            Some (ty, Activator.CreateInstance ty)
                        else
                            None
                    )
                    |> Seq.toList

                pluginAssembly, applicablePlugins
            )

        for item in toGenerate do
            use output = item.GeneratedDest.Open (FileMode.Create, FileAccess.Write)
            use outputWriter = new StreamWriter (output, leaveOpen = true)

            let plugins =
                plugins
                |> Seq.map (fun (_, plugins) ->
                    plugins |> List.filter (fun (ty, _) -> not (Set.contains ty.Name item.Suppress))
                )

            for applicablePlugins in plugins do
                for plugin, hostClass in applicablePlugins do
                    match getGenerateRawFromRaw hostClass with
                    | None -> ()
                    | Some generateRawFromRaw ->
                        let fileContents = File.ReadAllBytes item.InputSource.FullName

                        let args =
                            {
                                RawSourceGenerationArgs.FilePath = item.InputSource.FullName
                                FileContents = fileContents
                                Parameters = item.Params
                            }

                        let result = generateRawFromRaw args

                        match result with
                        | None
                        | Some null -> ()
                        | Some result ->
                            Console.Error.WriteLine
                                $"Writing output for generator %s{plugin.Name} to file %s{item.GeneratedDest.FullName}"

                            outputWriter.Write result
                            outputWriter.Write "\n"

        0
