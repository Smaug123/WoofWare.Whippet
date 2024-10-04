namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Extension methods for creating SynConst AST objects (which represent literal constants such as `3` or `"hi"`).
[<AutoOpen>]
module SynConstExt =
    type SynConst with
        /// Create the constant string with this value.
        static member Create (s : string) : SynConst =
            SynConst.String (s, SynStringKind.Regular, range0)
