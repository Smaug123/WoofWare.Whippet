namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating SynArgPats, a type which represents the arguments to a pattern.
[<RequireQualifiedAccess>]
module SynArgPats =
    /// Create a SynArgPats which represents the tuple of the given case names.
    /// For example, this specifies the `(a, b, c)` in the pattern match arm `| Foo (a, b, c)`.
    let createNamed (caseNames : string list) : SynArgPats =
        match caseNames.Length with
        | 0 -> SynArgPats.Pats []
        | 1 ->
            SynPat.Named (SynIdent.createS caseNames.[0], false, None, range0)
            |> List.singleton
            |> SynArgPats.Pats
        | len ->
            caseNames
            |> List.map (fun name -> SynPat.Named (SynIdent.createS name, false, None, range0))
            |> fun t -> SynPat.Tuple (false, t, List.replicate (len - 1) range0, range0)
            |> fun t -> SynPat.Paren (t, range0)
            |> List.singleton
            |> SynArgPats.Pats

    /// Create a SynArgPats representing the tuple of the given patterns.
    /// For example, if the input `SynPat`s are `[Named "a" ; Named "b" ; Named "c"]`, then this would
    /// specify the `(a, b, c)` in the pattern match arm `| Foo (a, b, c)`.
    let create (pats : SynPat list) : SynArgPats =
        match pats.Length with
        | 0 -> SynArgPats.Pats []
        | 1 -> [ pats.[0] ] |> SynArgPats.Pats
        | len ->
            SynPat.Paren (SynPat.Tuple (false, pats, List.replicate (len - 1) range0, range0), range0)
            |> List.singleton
            |> SynArgPats.Pats
