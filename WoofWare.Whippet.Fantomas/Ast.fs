﻿namespace WoofWare.Whippet.Fantomas

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

        let output = CodeFormatter.FormatASTAsync (parseTree, cfg) |> Async.RunSynchronously
        Some output

    /// For each namespace in the AST, returns the types defined therein.
    let getTypes (ast : ParsedInput) : (LongIdent * SynTypeDefn list) list =
        match ast with
        | ParsedInput.SigFile psfi -> failwith "Signature files not supported"
        | ParsedInput.ImplFile pifi ->
            pifi.Contents
            |> List.collect (fun (SynModuleOrNamespace.SynModuleOrNamespace (ns, _, _, decls, _, _, _, _, _)) ->
                decls
                |> List.collect (fun decl ->
                    match decl with
                    | SynModuleDecl.Types (defns, _) -> defns |> List.map (fun defn -> ns, defn)
                    | _ -> []
                )
            )
        |> List.map (fun (li, ty) -> (li |> List.map _.idText), (ty, li))
        |> List.groupBy fst
        |> List.map (fun (_name, data) ->
            let ns = snd (snd data.[0])
            let data = data |> List.map (fun (_, (ty, _)) -> ty)
            ns, data
        )

    /// For each namespace in the AST, returns the records contained therein.
    let getRecords (ast : ParsedInput) : (LongIdent * RecordType list) list =
        match ast with
        | ParsedInput.SigFile psfi ->
            psfi.Contents
            |> List.collect (fun (SynModuleOrNamespaceSig.SynModuleOrNamespaceSig (ns, _, _, decls, _, _, _, _, _)) ->
                decls
                |> List.collect (fun decl ->
                    match decl with
                    | SynModuleSigDecl.Types (defns, _) ->
                        defns
                        |> List.choose (fun defn ->
                            match defn with
                            | SynTypeDefnSig.SynTypeDefnSig (sci, typeRepr, members, _, _) ->
                                match typeRepr with
                                | SynTypeDefnSigRepr.Simple (SynTypeDefnSimpleRepr.Record (access, fields, _), _) ->
                                    (ns,
                                     RecordType.OfRecord
                                         sci
                                         (failwith "Signature files not yet supported")
                                         access
                                         fields)
                                    |> Some
                                | _ -> None
                        )
                    | _ -> []
                )
            )
        | ParsedInput.ImplFile pifi ->
            pifi.Contents
            |> List.collect (fun (SynModuleOrNamespace.SynModuleOrNamespace (ns, _, _, decls, _, _, _, _, _)) ->
                decls
                |> List.collect (fun decl ->
                    match decl with
                    | SynModuleDecl.Types (defns, _) ->
                        defns
                        |> List.choose (fun defn ->
                            match defn with
                            | SynTypeDefn.SynTypeDefn (sci, typeRepr, smd, _, _, _) ->
                                match typeRepr with
                                | SynTypeDefnRepr.Simple (SynTypeDefnSimpleRepr.Record (access, fields, _), _) ->
                                    (ns, RecordType.OfRecord sci smd access fields) |> Some
                                | _ -> None
                        )
                    | _ -> []
                )
            )
        |> List.map (fun (li, ty) -> (li |> List.map _.idText), (ty, li))
        |> List.groupBy fst
        |> List.map (fun (_name, data) ->
            let ns = snd (snd data.[0])
            let data = data |> List.map (fun (_, (ty, _)) -> ty)
            ns, data
        )
