namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating `SynPat`, which represents a pattern being matched on in any context.
[<RequireQualifiedAccess>]
module SynPat =
    /// Wrap this pattern in parentheses.
    let inline paren (pat : SynPat) : SynPat = SynPat.Paren (pat, range0)

    /// The anonymous pattern, `_`.
    let anon : SynPat = SynPat.Wild range0

    /// `{pat} : {ty}`
    let inline annotateTypeNoParen (ty : SynType) (pat : SynPat) = SynPat.Typed (pat, ty, range0)

    /// `({pat} : {ty})`
    let inline annotateType (ty : SynType) (pat : SynPat) = paren (annotateTypeNoParen ty pat)

    /// The pattern that is just the given name, `x` (e.g. in `match "hi" with | x -> ...`)
    let inline named (s : string) : SynPat =
        SynPat.Named (SynIdent.SynIdent (Ident (s, range0), None), false, None, range0)

    /// The pattern that is just the given name, `i` (e.g. in `match "hi" with | i -> ...`)
    let inline namedI (i : Ident) : SynPat =
        SynPat.Named (SynIdent.SynIdent (i, None), false, None, range0)

    /// The pattern that is the given name applied to these arguments, e.g. in `match "hi" with | Foo (a, b) -> ...`
    let inline identWithArgs (i : LongIdent) (args : SynArgPats) : SynPat =
        SynPat.LongIdent (SynLongIdent.create i, None, None, args, None, range0)

    /// The pattern that is the given name applied to these arguments, e.g. in `match "hi" with | Foo (a, b) -> ...`
    let inline nameWithArgs (i : string) (args : SynPat list) : SynPat =
        identWithArgs [ Ident.create i ] (SynArgPats.create args)

    /// The tuple pattern, `{el1}, {el2}, ...`
    let inline tupleNoParen (elements : SynPat list) : SynPat =
        match elements with
        | [] -> failwith "Can't tuple no elements in a pattern"
        | [ p ] -> p
        | elements -> SynPat.Tuple (false, elements, List.replicate (elements.Length - 1) range0, range0)

    /// The tuple pattern, `({el1}, {el2}, ...)`
    ///
    /// Consider `tupleNoParen` if you don't want parentheses.
    let inline tuple (elements : SynPat list) : SynPat = tupleNoParen elements |> paren

    /// A constant pattern, e.g. in `match foo with | "hi" -> ...`
    let inline createConst (c : SynConst) = SynPat.Const (c, range0)

    /// The unit pattern, as in e.g. `let foo () = ...`
    let unit = createConst SynConst.Unit

    /// The null pattern, as in e.g. `match foo with | null -> ...`
    let createNull = SynPat.Null range0

    /// The empty list pattern, as in e.g. `match foo with | [] -> ...`
    let emptyList = SynPat.ArrayOrList (false, [], range0)

    /// The list cons pattern, as in e.g. `match foo with | lhs :: rhs -> ...`
    let listCons (lhs : SynPat) (rhs : SynPat) =
        SynPat.ListCons (
            lhs,
            rhs,
            range0,
            {
                ColonColonRange = range0
            }
        )

    /// The empty array pattern, as in e.g. `match foo with | [||] -> ...`
    let emptyArray = SynPat.ArrayOrList (true, [], range0)
