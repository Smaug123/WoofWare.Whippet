namespace WoofWare.Whippet

open System.IO
open System.Reflection
open System.Runtime.Loader

type Ctx (dll : FileInfo, runtimes : DirectoryInfo list) =
    inherit AssemblyLoadContext ()

    override this.Load (target : AssemblyName) : Assembly =
        let localPath = Path.Combine (dll.Directory.FullName, $"%s{target.Name}.dll")

        runtimes
        |> List.tryPick (fun di ->
            let path = Path.Combine (di.FullName, $"%s{target.Name}.dll")

            if File.Exists path then
                this.LoadFromAssemblyPath path |> Some
            else
                None
        )
        |> Option.defaultWith (fun () ->
            if File.Exists localPath then
                this.LoadFromAssemblyPath localPath
            else
                null
        )
