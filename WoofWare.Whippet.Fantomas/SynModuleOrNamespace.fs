namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Xml
open Fantomas.FCS.Text.Range

/// Methods for manipulating SynModuleOrNamespace, which defines a module or namespace definition.
[<RequireQualifiedAccess>]
module SynModuleOrNamespace =

    /// Create a namespace with the given name and with the given declarations inside it.
    let createNamespace (name : LongIdent) (decls : SynModuleDecl list) : SynModuleOrNamespace =
        SynModuleOrNamespace.SynModuleOrNamespace (
            name,
            false,
            SynModuleOrNamespaceKind.DeclaredNamespace,
            decls,
            PreXmlDoc.Empty,
            [],
            None,
            range0,
            {
                LeadingKeyword = SynModuleOrNamespaceLeadingKeyword.Namespace range0
            }
        )
