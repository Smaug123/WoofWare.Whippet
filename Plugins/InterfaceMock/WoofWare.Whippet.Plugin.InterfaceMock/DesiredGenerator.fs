namespace WoofWare.Whippet.Plugin.InterfaceMock

type internal DesiredGenerator =
    | InterfaceMock of isInternal : bool option

    static member Parse (s : string) =
        match s with
        | "InterfaceMock" -> DesiredGenerator.InterfaceMock None |> Some
        | "InterfaceMock(true)" -> DesiredGenerator.InterfaceMock (Some true) |> Some
        | "InterfaceMock(false)" -> DesiredGenerator.InterfaceMock (Some false) |> Some
        | _ -> None
