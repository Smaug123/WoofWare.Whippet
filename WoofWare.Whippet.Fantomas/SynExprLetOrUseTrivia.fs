namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.SyntaxTrivia

/// Methods for manipulating SynExprLetOrUseTrivia, which gives non-semantically-relevant information about the
/// formatting of a `let` or `use` binding.
[<RequireQualifiedAccess>]
module SynExprLetOrUseTrivia =
    /// Specify that this is just a plain `let` or `use` binding. You basically always want this.
    let empty : SynExprLetOrUseTrivia =
        {
            InKeyword = None
        }
