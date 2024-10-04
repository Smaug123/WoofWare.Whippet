namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax

/// A little DSL for defining the contents of a computation expression.
/// Pass these to `SynExpr.createCompExpr`.
type CompExprBinding =
    /// `let! {varName} = {rhs}`
    | LetBang of varName : string * rhs : SynExpr
    /// `let {varName} = {rhs}`
    | Let of varName : string * rhs : SynExpr
    /// `use {varName} = {rhs}`
    | Use of varName : string * rhs : SynExpr
    /// `do {body}`
    | Do of body : SynExpr

(*
Potential API!
type internal CompExprBindings =
    private
        {
            /// These are stored in reverse.
            Bindings : CompExprBinding list
            CompExprName : string
        }

[<RequireQualifiedAccess>]
module internal CompExprBindings =
    let make (name : string) : CompExprBindings =
        {
            Bindings = []
            CompExprName = name
        }

    let thenDo (body : SynExpr) (bindings : CompExprBindings) =
        { bindings with
            Bindings = (Do body :: bindings.Bindings)
        }

    let thenLet (varName : string) (value : SynExpr) (bindings : CompExprBindings) =
        { bindings with
            Bindings = (Let (varName, value) :: bindings.Bindings)
        }

    let thenLetBang (varName : string) (value : SynExpr) (bindings : CompExprBindings) =
        { bindings with
            Bindings = (LetBang (varName, value) :: bindings.Bindings)
        }


    let thenUse (varName : string) (value : SynExpr) (bindings : CompExprBindings) =
        { bindings with
            Bindings = (LetBang (varName, value) :: bindings.Bindings)
        }
*)
