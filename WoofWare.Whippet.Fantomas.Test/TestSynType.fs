namespace WoofWare.Whippet.Fantomas.Test

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text
open NUnit.Framework
open FsUnitTyped
open WoofWare.Whippet.Fantomas
open Fantomas.FCS

[<TestFixture>]
module TestSynType =
    let typeToStringCases =
        [ "string", "string" ; "ResizeArray<int>", "ResizeArray<int32>" ]
        |> List.map TestCaseData

    [<TestCaseSource(nameof typeToStringCases)>]
    let ``Snapshot tests for SynType.toHumanReadableString`` (fsharpTypeString : string, expected : string) =
        let parsed, diags =
            Parse.parseFile false (SourceText.ofString $"let x : %s{fsharpTypeString} = failwith \"\"") []

        diags |> shouldBeEmpty

        let (SynModuleOrNamespace (decls = parsed)) =
            match parsed with
            | ParsedInput.ImplFile parsedImplFileInput -> parsedImplFileInput.Contents.[0]
            | ParsedInput.SigFile _ -> failwith "logic error"

        let (SynBinding (expr = parsed)) =
            match List.exactlyOne parsed with
            | SynModuleDecl.Let (bindings = bindings) -> bindings.[0]
            | _ -> failwith "logic error"

        let ty =
            match parsed with
            | SynExpr.Typed (targetType = targetType) -> targetType
            | _ -> failwith $"logic error: %O{parsed}"

        SynType.toHumanReadableString ty |> shouldEqual expected
