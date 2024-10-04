namespace WoofWare.Whippet.Fantomas

open Fantomas.Core
open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia

/// Helper methods to convert between source code and FCS ASTs.
[<RequireQualifiedAccess>]
module Ast =
    /// Given the contents of an F# source file, parse it into an AST. This is sync-over-async internally, which is
    /// naughty.
    let parse (fileContents : string) : ParsedInput =
        CodeFormatter.ParseAsync (false, fileContents)
        |> Async.RunSynchronously
        |> Array.head
        |> fst

    /// Concatenate the input modules/namespaces and render them as a single F# source file.
    ///
    /// This can return `None`, if the input was empty.
    /// This is sync-over-async internally, which is naughty.
    let render (contents : SynModuleOrNamespace list) : string option =
        if contents.IsEmpty then
            None
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

        CodeFormatter.FormatASTAsync (parseTree, cfg) |> Async.RunSynchronously |> Some
