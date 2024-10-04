namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating `SynAttribute`, which represents a single attribute such as `[<Foo("bar")>]`.
[<RequireQualifiedAccess>]
module SynAttribute =
    /// Create an attribute. If you want no arguments, use `SynExpr.CreateConst ()` as the `arg`.
    let inline create (typeName : SynLongIdent) (arg : SynExpr) : SynAttribute =
        {
            TypeName = typeName
            ArgExpr = arg
            Target = None
            AppliesToGetterAndSetter = false
            Range = range0
        }

    /// The `[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]` attribute, for use on modules.
    let compilationRepresentation : SynAttribute =
        [ "CompilationRepresentationFlags" ; "ModuleSuffix" ]
        |> SynExpr.createLongIdent
        |> SynExpr.paren
        |> create (SynLongIdent.createS "CompilationRepresentation")

    /// The `[<RequireQualifiedAccess>]` attribute.
    let requireQualifiedAccess : SynAttribute =
        create (SynLongIdent.createS "RequireQualifiedAccess") (SynExpr.CreateConst ())

    /// The `[<AutoOpen>]` attribute.
    let autoOpen : SynAttribute =
        create (SynLongIdent.createS "AutoOpen") (SynExpr.CreateConst ())
