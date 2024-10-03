namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

[<RequireQualifiedAccess>]
module SynSimplePats =

    let create (pats : SynSimplePat list) : SynSimplePats =
        match pats with
        | [] -> SynSimplePats.SimplePats ([], [], range0)
        | pats -> SynSimplePats.SimplePats (pats, List.replicate (pats.Length - 1) range0, range0)
