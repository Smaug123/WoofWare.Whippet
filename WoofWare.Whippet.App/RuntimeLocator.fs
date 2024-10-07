namespace WoofWare.Whippet

open System
open System.IO
open WoofWare.DotnetRuntimeLocator

/// Functions for locating .NET runtimes.
[<RequireQualifiedAccess>]
module DotnetRuntime =
    let private selectRuntime
        (config : RuntimeOptions)
        (f : DotnetEnvironmentInfo)
        : Choice<DotnetEnvironmentFrameworkInfo, DotnetEnvironmentSdkInfo> option
        =
        let rollForward =
            match Environment.GetEnvironmentVariable "DOTNET_ROLL_FORWARD" with
            | null ->
                config.RollForward
                |> Option.map RollForward.Parse
                |> Option.defaultValue RollForward.Minor
            | s -> RollForward.Parse s

        let desiredVersions =
            match config.Framework with
            | Some f -> [ Version f.Version, f.Name ]
            | None ->

            match config.Frameworks with
            | Some f -> f |> List.map (fun f -> Version f.Version, f.Name)
            | None ->
                failwith
                    "Could not deduce a framework version due to lack of either Framework or Frameworks in runtimeconfig"

        let compatiblyNamedRuntimes =
            f.Frameworks
            |> Seq.collect (fun availableFramework ->
                desiredVersions
                |> List.choose (fun (desiredVersion, desiredName) ->
                    if desiredName = availableFramework.Name then
                        Some
                            {|
                                Desired = desiredVersion
                                Name = desiredName
                                Installed = availableFramework
                                InstalledVersion = Version availableFramework.Version
                            |}
                    else
                        None
                )
            )
            |> Seq.toList

        match rollForward with
        | RollForward.Minor ->
            let available =
                compatiblyNamedRuntimes
                |> Seq.filter (fun data ->
                    data.InstalledVersion.Major = data.Desired.Major
                    && data.InstalledVersion.Minor >= data.Desired.Minor
                )
                |> Seq.groupBy (fun data -> data.Name)
                |> Seq.map (fun (name, data) ->
                    let data =
                        data
                        |> Seq.minBy (fun data -> data.InstalledVersion.Minor, data.InstalledVersion.Build)

                    name, data.Installed
                )
                // TODO: how do we select between many available frameworks?
                |> Seq.tryHead

            match available with
            | Some (_, f) -> Some (Choice1Of2 f)
            | None ->
                // TODO: maybe we can ask the SDK. But we keep on trucking: maybe we're self-contained,
                // and we'll actually find all the runtime next to the DLL.
                None
        | _ -> failwith "non-minor RollForward not supported yet; please shout if you want it"

    /// Given an executable DLL, locate the .NET runtime that can best run it.
    let locate (dll : FileInfo) : DirectoryInfo list =
        let runtimeConfig =
            let name =
                if not (dll.Name.EndsWith (".dll", StringComparison.OrdinalIgnoreCase)) then
                    failwith $"Expected DLL %s{dll.FullName} to end in .dll"

                dll.Name.Substring (0, dll.Name.Length - 4)

            Path.Combine (dll.Directory.FullName, $"%s{name}.runtimeconfig.json")
            |> File.ReadAllText
            |> System.Text.Json.Nodes.JsonNode.Parse
            |> RuntimeConfig.jsonParse
            |> fun f -> f.RuntimeOptions

        let availableRuntimes = DotnetEnvironmentInfo.Get ()

        let runtime = selectRuntime runtimeConfig availableRuntimes

        match runtime with
        | None ->
            // Keep on trucking: let's be optimistic and hope that we're self-contained.
            [ dll.Directory ]
        | Some (Choice1Of2 runtime) -> [ dll.Directory ; DirectoryInfo $"%s{runtime.Path}/%s{runtime.Version}" ]
        | Some (Choice2Of2 sdk) -> [ dll.Directory ; DirectoryInfo sdk.Path ]
