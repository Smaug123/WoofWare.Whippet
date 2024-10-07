namespace WoofWare.Whippet.Plugin.Json

type internal DesiredGenerator =
    | JsonParse of extensionMethod : bool option
    | JsonSerialize of extensionMethod : bool option

    static member Parse (s : string) =
        match s with
        | "JsonParse" -> DesiredGenerator.JsonParse None
        | "JsonParse(true)" -> DesiredGenerator.JsonParse (Some true)
        | "JsonParse(false)" -> DesiredGenerator.JsonParse (Some false)
        | "JsonSerialize" -> DesiredGenerator.JsonSerialize None
        | "JsonSerialize(true)" -> DesiredGenerator.JsonSerialize (Some true)
        | "JsonSerialize(false)" -> DesiredGenerator.JsonSerialize (Some false)
        | _ -> failwith $"Failed to parse as a generator specification: %s{s}"
