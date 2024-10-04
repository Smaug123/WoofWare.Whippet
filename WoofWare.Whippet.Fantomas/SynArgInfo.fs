namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

/// Module for manipulating the SynArgInfo AST type (which represents a single argument, possibly within a tuple,
/// in a `let` binding).
[<RequireQualifiedAccess>]
module SynArgInfo =
    let empty = SynArgInfo.SynArgInfo ([], false, None)
