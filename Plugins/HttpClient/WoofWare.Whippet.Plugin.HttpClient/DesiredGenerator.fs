namespace WoofWare.Whippet.Plugin.HttpClient

type internal DesiredGenerator =
    | HttpClient of extensionMethod : bool option

    static member Parse (s : string) =
        match s with
        | "HttpClient" -> DesiredGenerator.HttpClient None |> Some
        | "HttpClient(true)" -> DesiredGenerator.HttpClient (Some true) |> Some
        | "HttpClient(false)" -> DesiredGenerator.HttpClient (Some false) |> Some
        | _ -> None
