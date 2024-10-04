namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating the SynSimplePats AST type. This type represents a collection of SynSimplePat entries,
/// all tupled together (e.g. as in `fun (a, b : int) -> ...`).
[<RequireQualifiedAccess>]
module SynSimplePats =

    /// Build a SynSimplePats by tupling together a bunch of individual patterns.
    let create (pats : SynSimplePat list) : SynSimplePats =
        match pats with
        | [] -> SynSimplePats.SimplePats ([], [], range0)
        | pats -> SynSimplePats.SimplePats (pats, List.replicate (pats.Length - 1) range0, range0)
