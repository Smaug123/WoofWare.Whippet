namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating SynMatchClause, a type representing a single arm of a `match` pattern match.
/// For example, the entire line of code `| Foo (a, b) -> bar` is represented by one of these.
[<RequireQualifiedAccess>]
module SynMatchClause =
    /// Represents the `match` arm `| {lhs} -> {rhs}`.
    let create (lhs : SynPat) (rhs : SynExpr) : SynMatchClause =
        SynMatchClause.SynMatchClause (
            lhs,
            None,
            rhs,
            range0,
            DebugPointAtTarget.Yes,
            {
                ArrowRange = Some range0
                BarRange = Some range0
            }
        )

    /// Replace the `where` clause (or add a new one, if there isn't one already) in this `match` arm.
    let withWhere (where : SynExpr) (m : SynMatchClause) : SynMatchClause =
        match m with
        | SynMatchClause (synPat, _, resultExpr, range, debugPointAtTarget, synMatchClauseTrivia) ->
            SynMatchClause (synPat, Some where, resultExpr, range, debugPointAtTarget, synMatchClauseTrivia)
