namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

[<RequireQualifiedAccess>]
module SynSimplePat =

    let createId (id : Ident) : SynSimplePat =
        SynSimplePat.Id (id, None, false, false, false, range0)
