namespace WoofWare.Whippet

open System
open System.IO
open System.Reflection

// Fix for https://github.com/Smaug123/unofficial-nunit-runner/issues/8
// (This tells the DLL loader to look next to the input DLL for dependencies.)
/// Context manager to set the AppContext.BaseDirectory of the executing DLL.
type SetBaseDir (testDll : FileInfo) =
    let oldBaseDir = AppContext.BaseDirectory

    let setData =
        let appContext = Type.GetType "System.AppContext"

        if Object.ReferenceEquals (appContext, (null : obj)) then
            ignore<string * string>
        else

        let setDataMethod =
            appContext.GetMethod ("SetData", BindingFlags.Static ||| BindingFlags.Public)

        if Object.ReferenceEquals (setDataMethod, (null : obj)) then
            ignore<string * string>
        else

        fun (k, v) -> setDataMethod.Invoke ((null : obj), [| k ; v |]) |> unbox<unit>

    do setData ("APP_CONTEXT_BASE_DIRECTORY", testDll.Directory.FullName)

    interface IDisposable with
        member _.Dispose () =
            setData ("APP_CONTEXT_BASE_DIRECTORY", oldBaseDir)
