namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Text.Range

/// Methods for manipulating `SynModuleDecl`, the type representing a single definition within a module.
/// For example, an `open` statement, a `let` binding, a type definition, etc.
[<RequireQualifiedAccess>]
module SynModuleDecl =

    /// Open this target (e.g. `open MyModule` or `open MyNamespace`).
    let inline openAny (ident : SynOpenDeclTarget) : SynModuleDecl = SynModuleDecl.Open (ident, range0)

    /// Add consecutive `let`-bindings, non-recursive.
    let inline createLets (bindings : SynBinding list) : SynModuleDecl =
        SynModuleDecl.Let (false, bindings, range0)

    /// Add a single `let`-binding. (Use `createLets` for *multiple* bindings.)
    let inline createLet (binding : SynBinding) : SynModuleDecl = createLets [ binding ]

    /// Add type definitions.
    let inline createTypes (tys : SynTypeDefn list) : SynModuleDecl = SynModuleDecl.Types (tys, range0)

    /// Add a nested module definition.
    let nestedModule (info : SynComponentInfo) (decls : SynModuleDecl list) : SynModuleDecl =
        SynModuleDecl.NestedModule (
            info,
            false,
            decls,
            false,
            range0,
            {
                ModuleKeyword = Some range0
                EqualsRange = Some range0
            }
        )
