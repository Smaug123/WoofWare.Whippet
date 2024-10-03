namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

[<RequireQualifiedAccess>]
module SynIdent =
    let inline createI (i : Ident) : SynIdent = SynIdent.SynIdent (i, None)

    let inline createS (i : string) : SynIdent =
        SynIdent.SynIdent (Ident.create i, None)
