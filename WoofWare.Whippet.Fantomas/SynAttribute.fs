namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

[<RequireQualifiedAccess>]
module SynAttribute =
    let inline create (typeName : SynLongIdent) (arg : SynExpr) : SynAttribute =
        {
            TypeName = typeName
            ArgExpr = arg
            Target = None
            AppliesToGetterAndSetter = false
            Range = range0
        }

    let compilationRepresentation : SynAttribute =
        [ "CompilationRepresentationFlags" ; "ModuleSuffix" ]
        |> SynExpr.createLongIdent
        |> SynExpr.paren
        |> create (SynLongIdent.createS "CompilationRepresentation")

    let requireQualifiedAccess : SynAttribute =
        create (SynLongIdent.createS "RequireQualifiedAccess") (SynExpr.CreateConst ())

    let autoOpen : SynAttribute =
        create (SynLongIdent.createS "AutoOpen") (SynExpr.CreateConst ())
