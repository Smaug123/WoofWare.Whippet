namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating the SynSimplePat AST type. This type represents the left-hand side of a pattern match,
/// e.g. the `x` in `fun x -> foo`.
[<RequireQualifiedAccess>]
module SynSimplePat =

    /// Create a SynSimplePat that is a bare identifier, no type information, non-optional.
    let createId (id : Ident) : SynSimplePat =
        SynSimplePat.Id (id, None, false, false, false, range0)
