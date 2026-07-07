namespace WoofWare.Whippet.Fantomas.Test

open Fantomas.FCS.Syntax
open NUnit.Framework
open FsUnitTyped
open WoofWare.Whippet.Fantomas

[<TestFixture>]
module TestCompExpr =

    /// Render a computation expression consisting of the given bindings and return body,
    /// wrapped in `let f () = async { ... }` inside a namespace, to a formatted string.
    let private renderCompExpr (retBody : SynExpr) (bindings : CompExprBinding list) : string =
        let compExpr = SynExpr.createCompExpr "async" retBody bindings

        let binding = SynBinding.basic [ Ident.create "f" ] [ SynPat.unit ] compExpr

        [ SynModuleDecl.createLet binding ]
        |> SynModuleOrNamespace.createNamespace [ Ident.create "Foo" ]
        |> List.singleton
        |> Ast.render
        |> Option.get

    [<Test>]
    let ``Use renders as use, not let`` () =
        let rendered =
            renderCompExpr
                (SynExpr.createIdent "resource")
                [
                    Use ("resource", SynExpr.applyFunction (SynExpr.createIdent "acquire") (SynExpr.CreateConst ()))
                ]

        rendered.Contains "use resource =" |> shouldEqual true
        rendered.Contains "let resource =" |> shouldEqual false

    [<Test>]
    let ``Let still renders as let`` () =
        let rendered =
            renderCompExpr (SynExpr.createIdent "x") [ Let ("x", SynExpr.CreateConst 3) ]

        rendered.Contains "let x =" |> shouldEqual true

    [<Test>]
    let ``LetBang renders as let-bang`` () =
        let rendered =
            renderCompExpr (SynExpr.createIdent "x") [ LetBang ("x", SynExpr.createIdent "thing") ]

        rendered.Contains "let! x =" |> shouldEqual true
