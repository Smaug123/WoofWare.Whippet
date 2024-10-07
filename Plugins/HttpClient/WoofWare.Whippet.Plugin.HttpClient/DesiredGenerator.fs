namespace WoofWare.Whippet.Plugin.HttpClient

type internal DesiredGenerator =
    | HttpClient of extensionMethod : bool option

    static member Parse (s : string) =
        match s with
        | "HttpClient" -> DesiredGenerator.HttpClient None
        | "HttpClient(true)" -> DesiredGenerator.HttpClient (Some true)
        | "HttpClient(false)" -> DesiredGenerator.HttpClient (Some false)
        | _ -> failwith $"Failed to parse as a generator specification: %s{s}"
