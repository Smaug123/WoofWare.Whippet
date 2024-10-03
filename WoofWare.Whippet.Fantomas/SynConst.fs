namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

[<AutoOpen>]
module SynConstExt =
    type SynConst with
        static member Create (s : string) : SynConst =
            SynConst.String (s, SynStringKind.Regular, range0)
