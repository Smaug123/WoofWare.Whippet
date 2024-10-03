namespace WoofWare.Whippet.Fantomas

open Fantomas.Core
open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia

[<RequireQualifiedAccess>]
module Ast =
    let parse (fileContents : string) : ParsedInput =
        CodeFormatter.ParseAsync (false, fileContents)
        |> Async.RunSynchronously
        |> Array.head
        |> fst

    /// This can return `null`, if the input was empty.
    /// Not using FSharpOption here to keep the API as uniform as possible.
    let render (contents : SynModuleOrNamespace list) : string =
        if contents.IsEmpty then
            null
        else

        let parseTree =
            ParsedInput.ImplFile (
                ParsedImplFileInput.ParsedImplFileInput (
                    "file.fs",
                    false,
                    QualifiedNameOfFile.QualifiedNameOfFile (Ident.create "file"),
                    [],
                    [],
                    contents,
                    (false, false),
                    {
                        ParsedImplFileInputTrivia.CodeComments = []
                        ConditionalDirectives = []
                    },
                    Set.empty
                )
            )

        let cfg = FormatConfig.Default

        CodeFormatter.FormatASTAsync (parseTree, cfg) |> Async.RunSynchronously
