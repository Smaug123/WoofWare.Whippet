namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

[<RequireQualifiedAccess>]
module SynArgInfo =
    let empty = SynArgInfo.SynArgInfo ([], false, None)
