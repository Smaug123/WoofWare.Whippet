namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

[<RequireQualifiedAccess>]
module SynValInfo =
    let empty = SynValInfo.SynValInfo ([], SynArgInfo.empty)
