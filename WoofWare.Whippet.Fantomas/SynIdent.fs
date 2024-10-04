namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

/// Methods for manipulating the SynIdent AST type. This is basically an Ident, but specifically one which is appearing
/// in the AST at some position.
[<RequireQualifiedAccess>]
module SynIdent =
    /// Create a SynIdent from an Ident.
    let inline createI (i : Ident) : SynIdent = SynIdent.SynIdent (i, None)

    /// Create a SynIdent from a string (which we'll convert into an Ident for you en route).
    let inline createS (i : string) : SynIdent =
        SynIdent.SynIdent (Ident.create i, None)
