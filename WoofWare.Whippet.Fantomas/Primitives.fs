namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Module for manipulating primitive types like `int` or `string`.
[<RequireQualifiedAccess>]
module Primitives =
    /// Given e.g. "byte", returns "System.Byte".
    let qualifyType (typeName : string) : LongIdent option =
        match typeName with
        | "float32"
        | "single" -> [ "System" ; "Single" ] |> Some
        | "float"
        | "double" -> [ "System" ; "Double" ] |> Some
        | "byte"
        | "uint8" -> [ "System" ; "Byte" ] |> Some
        | "sbyte"
        | "int8" -> [ "System" ; "SByte" ] |> Some
        | "int16" -> [ "System" ; "Int16" ] |> Some
        | "int"
        | "int32" -> [ "System" ; "Int32" ] |> Some
        | "int64" -> [ "System" ; "Int64" ] |> Some
        | "uint16" -> [ "System" ; "UInt16" ] |> Some
        | "uint"
        | "uint32" -> [ "System" ; "UInt32" ] |> Some
        | "uint64" -> [ "System" ; "UInt64" ] |> Some
        | "char" -> [ "System" ; "Char" ] |> Some
        | "decimal" -> [ "System" ; "Decimal" ] |> Some
        | "string" -> [ "System" ; "String" ] |> Some
        | "bool" -> [ "System" ; "Boolean" ] |> Some
        | _ -> None
        |> Option.map (List.map (fun i -> (Ident (i, range0))))
