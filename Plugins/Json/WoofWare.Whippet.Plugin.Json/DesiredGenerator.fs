namespace WoofWare.Whippet.Plugin.Json

type internal DesiredGenerator =
    | JsonParse of extensionMethod : bool option
    | JsonSerialize of extensionMethod : bool option

    static member Parse (s : string) =
        match s with
        | "JsonParse" -> DesiredGenerator.JsonParse None |> Some
        | "JsonParse(true)" -> DesiredGenerator.JsonParse (Some true) |> Some
        | "JsonParse(false)" -> DesiredGenerator.JsonParse (Some false) |> Some
        | "JsonSerialize" -> DesiredGenerator.JsonSerialize None |> Some
        | "JsonSerialize(true)" -> DesiredGenerator.JsonSerialize (Some true) |> Some
        | "JsonSerialize(false)" -> DesiredGenerator.JsonSerialize (Some false) |> Some
        | _ -> None
