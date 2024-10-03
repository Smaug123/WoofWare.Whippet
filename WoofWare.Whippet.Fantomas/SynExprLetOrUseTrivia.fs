namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.SyntaxTrivia

[<RequireQualifiedAccess>]
module SynExprLetOrUseTrivia =
    let empty : SynExprLetOrUseTrivia =
        {
            InKeyword = None
        }
