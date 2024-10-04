namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

/// Module for manipulating the SynValInfo AST type (which represents the arguments and return type of a `let` binding).
[<RequireQualifiedAccess>]
module SynValInfo =
    /// No arguments, no return info.
    let empty = SynValInfo.SynValInfo ([], SynArgInfo.empty)
