namespace WoofWare.Whippet.Fantomas

open System
open System.Text
open System.Text.RegularExpressions
open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating the `Ident` type. This type indicates a variable or type identifier in many contexts:
/// whenever the AST needs to name something, it is usually with the `Ident` type.
[<RequireQualifiedAccess>]
module Ident =
    /// "Cast" a string into an `Ident`. Probably don't put any weird characters in this; I don't know how well it will
    /// work!
    let inline create (s : string) = Ident (s, range0)

    /// Fantomas bug, perhaps? "type" is not rendered as ``type``, although the ASTs are identical
    /// apart from the ranges?
    /// Awful hack: here is a function that does this sort of thing.
    let createSanitisedParamName (s : string) =
        match s with
        | "type" -> create "type'"
        | "private" -> create "private'"
        | _ ->

        let result = StringBuilder ()

        for i = 0 to s.Length - 1 do
            if Char.IsLetter s.[i] then
                result.Append s.[i] |> ignore<StringBuilder>
            elif Char.IsNumber s.[i] then
                if result.Length > 0 then
                    result.Append s.[i] |> ignore<StringBuilder>
            elif s.[i] = '_' || s.[i] = '-' then
                result.Append '_' |> ignore<StringBuilder>
            else
                failwith $"could not convert to ident: %s{s}"

        create (result.ToString ())

    let private alnum = Regex @"^[a-zA-Z][a-zA-Z0-9]*$"

    /// Create an `Ident` for this string, suitable for use as a type name. So, for example, we'll put it into
    /// PascalCase if it was in snake_case, we'll remove non-alphanumeric characters, and so on.
    let createSanitisedTypeName (s : string) : Ident =
        let result = StringBuilder ()
        let mutable capitalize = true

        for i = 0 to s.Length - 1 do
            if Char.IsLetter s.[i] then
                if capitalize then
                    result.Append (Char.ToUpperInvariant s.[i]) |> ignore<StringBuilder>
                    capitalize <- false
                else
                    result.Append s.[i] |> ignore<StringBuilder>
            elif Char.IsNumber s.[i] then
                if result.Length > 0 then
                    result.Append s.[i] |> ignore<StringBuilder>
            elif s.[i] = '_' then
                capitalize <- true

        if result.Length = 0 then
            failwith $"String %s{s} was not suitable as a type identifier"

        Ident (result.ToString (), range0)

    /// Create an Ident which is the same as the input but which has its initial char lowercased if it's a letter.
    let lowerFirstLetter (x : Ident) : Ident =
        let result = StringBuilder x.idText.Length
        result.Append (Char.ToLowerInvariant x.idText.[0]) |> ignore
        result.Append x.idText.[1..] |> ignore
        create ((result : StringBuilder).ToString ())
