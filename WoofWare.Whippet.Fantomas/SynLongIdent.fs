namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Text.Range
open Fantomas.FCS.Syntax

/// Methods for manipulating the SynLongIdent type. This is a LongIdent, but specifically one which is appearing in the
/// AST somewhere.
[<RequireQualifiedAccess>]
module SynLongIdent =

    /// The ">=" identifier.
    let geq =
        SynLongIdent.SynLongIdent (
            [ Ident.create "op_GreaterThanOrEqual" ],
            [],
            [ Some (IdentTrivia.OriginalNotation ">=") ]
        )

    /// The "<=" identifier.
    let leq =
        SynLongIdent.SynLongIdent (
            [ Ident.create "op_LessThanOrEqual" ],
            [],
            [ Some (IdentTrivia.OriginalNotation "<=") ]
        )

    /// The ">" identifier.
    let gt =
        SynLongIdent.SynLongIdent ([ Ident.create "op_GreaterThan" ], [], [ Some (IdentTrivia.OriginalNotation ">") ])

    /// The "<" identifier.
    let lt =
        SynLongIdent.SynLongIdent ([ Ident.create "op_LessThan" ], [], [ Some (IdentTrivia.OriginalNotation "<") ])

    /// The "-" identifier.
    let sub =
        SynLongIdent.SynLongIdent ([ Ident.create "op_Subtraction" ], [], [ Some (IdentTrivia.OriginalNotation "-") ])

    /// The "=" identifier.
    let eq =
        SynLongIdent.SynLongIdent ([ Ident.create "op_Equality" ], [], [ Some (IdentTrivia.OriginalNotation "=") ])

    /// The "&&" identifier.
    let booleanAnd =
        SynLongIdent.SynLongIdent ([ Ident.create "op_BooleanAnd" ], [], [ Some (IdentTrivia.OriginalNotation "&&") ])

    /// The "||" identifier.
    let booleanOr =
        SynLongIdent.SynLongIdent ([ Ident.create "op_BooleanOr" ], [], [ Some (IdentTrivia.OriginalNotation "||") ])

    /// The "+" identifier.
    let plus =
        SynLongIdent.SynLongIdent ([ Ident.create "op_Addition" ], [], [ Some (IdentTrivia.OriginalNotation "+") ])

    /// The "*" identifier.
    let times =
        SynLongIdent.SynLongIdent ([ Ident.create "op_Multiply" ], [], [ Some (IdentTrivia.OriginalNotation "*") ])

    /// The "|>" identifier.
    let pipe =
        SynLongIdent.SynLongIdent ([ Ident.create "op_PipeRight" ], [], [ Some (IdentTrivia.OriginalNotation "|>") ])

    /// Convert this SynLongIdent into a human-readable string for display.
    /// This is not quite round-trippable, but it should be round-trippable if your identifiers are not pathological
    /// (e.g. you should not have a full stop in your identifiers; that's the job of LongIdent, not Ident).
    let toString (sli : SynLongIdent) : string =
        sli.LongIdent |> List.map _.idText |> String.concat "."

    /// Build a SynLongIdent from a LongIdent (by attaching dummy location information to it).
    let create (ident : LongIdent) : SynLongIdent =
        let commas =
            match ident with
            | [] -> []
            | _ :: commas -> commas |> List.map (fun _ -> range0)

        SynLongIdent.SynLongIdent (ident, commas, List.replicate ident.Length None)

    /// Build a SynLongIdent from an Ident. Please don't put full stops in the argument, I don't know what will happen;
    /// use `create` if you have multiple components.
    let inline createI (i : Ident) : SynLongIdent = create [ i ]

    /// Build a SynLongIdent from a string which is expected to be "basically an Ident" - no full stops, for example.
    /// Use `createS'` if you have multiple components.
    let inline createS (s : string) : SynLongIdent = createI (Ident (s, range0))

    /// Build a SynLongIdent from its components.
    let inline createS' (s : string list) : SynLongIdent =
        create (s |> List.map (fun i -> Ident (i, range0)))

    /// Determine whether this ident is the identifier of the `unit` type.
    let isUnit (ident : SynLongIdent) : bool =
        match ident.LongIdent with
        | [ i ] when System.String.Equals (i.idText, "unit", System.StringComparison.OrdinalIgnoreCase) -> true
        | _ -> false

    /// Determine whether this ident is the identifier of the `list` (F# list) type.
    let isList (ident : SynLongIdent) : bool =
        match ident.LongIdent with
        | [ i ] when System.String.Equals (i.idText, "list", System.StringComparison.OrdinalIgnoreCase) -> true
        // TODO: consider FSharpList or whatever it is
        | _ -> false

    /// Determine whether this ident is an identifier of the array type.
    let isArray (ident : SynLongIdent) : bool =
        match ident.LongIdent with
        | [ i ] when
            System.String.Equals (i.idText, "array", System.StringComparison.OrdinalIgnoreCase)
            || System.String.Equals (i.idText, "[]", System.StringComparison.Ordinal)
            ->
            true
        | _ -> false

    /// Determine whether this ident is an identifier of the F# option type.
    let isOption (ident : SynLongIdent) : bool =
        match ident.LongIdent with
        | [ i ] when System.String.Equals (i.idText, "option", System.StringComparison.OrdinalIgnoreCase) -> true
        // TODO: consider Microsoft.FSharp.Option or whatever it is
        | _ -> false

    /// Determine whether this ident is an identifier of the F# Choice type.
    let isChoice (ident : SynLongIdent) : bool =
        match ident.LongIdent with
        | [ i ] when System.String.Equals (i.idText, "Choice", System.StringComparison.Ordinal) -> true
        // TODO: consider Microsoft.FSharp.Choice or whatever it is
        | _ -> false

    /// Determine whether this ident is an identifier of the System.Nullable type.
    let isNullable (ident : SynLongIdent) : bool =
        match ident.LongIdent |> List.map _.idText with
        | [ "System" ; "Nullable" ]
        | [ "Nullable" ] -> true
        | _ -> false

    /// Determine whether this ident is an identifier of the RestEase.Response type.
    let isResponse (ident : SynLongIdent) : bool =
        match ident.LongIdent |> List.map _.idText with
        | [ "Response" ]
        | [ "RestEase" ; "Response" ] -> true
        | _ -> false

    /// Determine whether this ident is an identifier of the F# Map type.
    let isMap (ident : SynLongIdent) : bool =
        match ident.LongIdent |> List.map _.idText with
        | [ "Map" ] -> true
        | _ -> false

    /// Determine whether this ident is an identifier of the System.Collections.Generic.IReadOnlyDictionary type.
    /// This is purely syntactic: it will not say that e.g. the Dictionary type is an IReadOnlyDictionary.
    let isReadOnlyDictionary (ident : SynLongIdent) : bool =
        match ident.LongIdent |> List.map _.idText with
        | [ "IReadOnlyDictionary" ]
        | [ "Generic" ; "IReadOnlyDictionary" ]
        | [ "Collections" ; "Generic" ; "IReadOnlyDictionary" ]
        | [ "System" ; "Collections" ; "Generic" ; "IReadOnlyDictionary" ] -> true
        | _ -> false

    /// Determine whether this ident is an identifier of the System.Collections.Generic.Dictionary type.
    let isDictionary (ident : SynLongIdent) : bool =
        match ident.LongIdent |> List.map _.idText with
        | [ "Dictionary" ]
        | [ "Generic" ; "Dictionary" ]
        | [ "Collections" ; "Generic" ; "Dictionary" ]
        | [ "System" ; "Collections" ; "Generic" ; "Dictionary" ] -> true
        | _ -> false

    /// Determine whether this ident is an identifier of the System.Collections.Generic.IDictionary type.
    /// This is purely syntactic: it will not say that e.g. the Dictionary type is an IDictionary.
    let isIDictionary (ident : SynLongIdent) : bool =
        match ident.LongIdent |> List.map _.idText with
        | [ "IDictionary" ]
        | [ "Generic" ; "IDictionary" ]
        | [ "Collections" ; "Generic" ; "IDictionary" ]
        | [ "System" ; "Collections" ; "Generic" ; "IDictionary" ] -> true
        | _ -> false
