﻿namespace WoofWare.Whippet.Plugin.InterfaceMock

open System
open Fantomas.FCS.Syntax
open Fantomas.FCS.Xml
open WoofWare.Whippet.Core
open WoofWare.Whippet.Fantomas

type internal GenerateMockOutputSpec =
    {
        IsInternal : bool
    }

[<RequireQualifiedAccess>]
module internal InterfaceMockGenerator =
    open Fantomas.FCS.Text.Range

    let private getName (SynField (_, _, id, _, _, _, _, _, _)) =
        match id with
        | None -> failwith "Expected record field to have a name, but it was somehow anonymous"
        | Some id -> id

    [<RequireQualifiedAccess>]
    type private KnownInheritance = | IDisposable

    let createType
        (spec : GenerateMockOutputSpec)
        (name : string)
        (interfaceType : InterfaceType)
        (xmlDoc : PreXmlDoc)
        (fields : SynField list)
        : SynModuleDecl
        =
        let inherits =
            interfaceType.Inherits
            |> Seq.map (fun ty ->
                match ty with
                | SynType.LongIdent (SynLongIdent.SynLongIdent (name, _, _)) ->
                    match name |> List.map _.idText with
                    | [] -> failwith "Unexpected empty identifier in inheritance declaration"
                    | [ "IDisposable" ]
                    | [ "System" ; "IDisposable" ] -> KnownInheritance.IDisposable
                    | _ -> failwithf "Unrecognised inheritance identifier: %+A" name
                | x -> failwithf "Unrecognised type in inheritance: %+A" x
            )
            |> Set.ofSeq

        let failwithFun (SynField (_, _, idOpt, _, _, _, _, _, _)) =
            let failString =
                match idOpt with
                | None -> SynExpr.CreateConst "Unimplemented mock function"
                | Some ident -> SynExpr.CreateConst $"Unimplemented mock function: %s{ident.idText}"

            SynExpr.createLongIdent [ "System" ; "NotImplementedException" ]
            |> SynExpr.applyTo failString
            |> SynExpr.paren
            |> SynExpr.applyFunction (SynExpr.createIdent "raise")
            |> SynExpr.createLambda "_"

        let constructorReturnType =
            match interfaceType.Generics with
            | None -> SynType.createLongIdent' [ name ]
            | Some generics ->

            let generics =
                generics.TyparDecls
                |> List.map (fun (SynTyparDecl (_, typar)) -> SynType.var typar)

            SynType.app name generics

        let constructorFields =
            let extras =
                if inherits.Contains KnownInheritance.IDisposable then
                    let unitFun = SynExpr.createThunk (SynExpr.CreateConst ())

                    [ SynLongIdent.createS "Dispose", unitFun ]
                else
                    []

            let nonExtras =
                fields
                |> List.map (fun field -> SynLongIdent.createI (getName field), failwithFun field)

            extras @ nonExtras

        let constructor =
            SynBinding.basic
                [ Ident.create "Empty" ]
                (if interfaceType.Generics.IsNone then
                     []
                 else
                     [ SynPat.unit ])
                (AstHelper.instantiateRecord constructorFields)
            |> SynBinding.withXmlDoc (PreXmlDoc.create "An implementation where every method throws.")
            |> SynBinding.withReturnAnnotation constructorReturnType
            |> SynMemberDefn.staticMember

        let fields =
            let extras =
                if inherits.Contains KnownInheritance.IDisposable then
                    {
                        Attrs = []
                        Ident = Some (Ident.create "Dispose")
                        Type = SynType.funFromDomain SynType.unit SynType.unit
                    }
                    |> SynField.make
                    |> SynField.withDocString (PreXmlDoc.create "Implementation of IDisposable.Dispose")
                    |> List.singleton
                else
                    []

            extras @ fields

        let interfaceMembers =
            let members =
                interfaceType.Members
                |> List.map (fun memberInfo ->
                    let headArgs =
                        memberInfo.Args
                        |> List.mapi (fun i tupledArgs ->
                            let args =
                                tupledArgs.Args
                                |> List.mapi (fun j ty ->
                                    match ty.Type with
                                    | UnitType -> SynPat.unit
                                    | _ -> SynPat.named $"arg_%i{i}_%i{j}"
                                )

                            match args with
                            | [] -> failwith "somehow got no args at all"
                            | [ arg ] -> arg
                            | args -> SynPat.tuple args
                            |> fun i -> if tupledArgs.HasParen then SynPat.paren i else i
                        )

                    let body =
                        let tuples =
                            memberInfo.Args
                            |> List.mapi (fun i args ->
                                args.Args
                                |> List.mapi (fun j arg ->
                                    match arg.Type with
                                    | UnitType -> SynExpr.CreateConst ()
                                    | _ -> SynExpr.createIdent $"arg_%i{i}_%i{j}"
                                )
                                |> SynExpr.tuple
                            )

                        match tuples |> List.rev with
                        | [] -> failwith "expected args but got none"
                        | last :: rest ->

                        (last, rest)
                        ||> List.fold SynExpr.applyTo
                        |> SynExpr.applyFunction (
                            SynExpr.createLongIdent' [ Ident.create "this" ; memberInfo.Identifier ]
                        )

                    SynBinding.basic [ Ident.create "this" ; memberInfo.Identifier ] headArgs body
                    |> SynMemberDefn.memberImplementation
                )

            let properties =
                interfaceType.Properties
                |> List.map (fun pi ->
                    SynExpr.createLongIdent' [ Ident.create "this" ; pi.Identifier ]
                    |> SynExpr.applyTo (SynExpr.CreateConst ())
                    |> SynBinding.basic [ Ident.create "this" ; pi.Identifier ] []
                    |> SynMemberDefn.memberImplementation
                )

            let interfaceName =
                let baseName = SynType.createLongIdent interfaceType.Name

                match interfaceType.Generics with
                | None -> baseName
                | Some generics ->
                    let generics =
                        match generics with
                        | SynTyparDecls.PostfixList (decls, _, _) -> decls
                        | SynTyparDecls.PrefixList (decls, _) -> decls
                        | SynTyparDecls.SinglePrefix (decl, _) -> [ decl ]
                        |> List.map (fun (SynTyparDecl (_, typar)) -> SynType.var typar)

                    SynType.app' baseName generics

            SynMemberDefn.Interface (interfaceName, Some range0, Some (members @ properties), range0)

        let access =
            match interfaceType.Accessibility, spec.IsInternal with
            | Some (SynAccess.Public _), true
            | None, true -> SynAccess.Internal range0
            | Some (SynAccess.Public _), false -> SynAccess.Public range0
            | None, false -> SynAccess.Public range0
            | Some (SynAccess.Internal _), _ -> SynAccess.Internal range0
            | Some (SynAccess.Private _), _ -> SynAccess.Private range0

        let extraInterfaces =
            inherits
            |> Seq.map (fun inheritance ->
                match inheritance with
                | KnownInheritance.IDisposable ->
                    let mem =
                        SynExpr.createLongIdent [ "this" ; "Dispose" ]
                        |> SynExpr.applyTo (SynExpr.CreateConst ())
                        |> SynBinding.basic [ Ident.create "this" ; Ident.create "Dispose" ] [ SynPat.unit ]
                        |> SynBinding.withReturnAnnotation SynType.unit
                        |> SynMemberDefn.memberImplementation

                    SynMemberDefn.Interface (
                        SynType.createLongIdent' [ "System" ; "IDisposable" ],
                        Some range0,
                        Some [ mem ],
                        range0
                    )
            )
            |> Seq.toList

        let record =
            {
                Name = Ident.create name
                Fields = fields
                Members = Some ([ constructor ; interfaceMembers ] @ extraInterfaces)
                XmlDoc = Some xmlDoc
                Generics = interfaceType.Generics
                TypeAccessibility = Some access
                ImplAccessibility = None
                Attributes = []
            }

        let typeDecl = RecordType.ToAst record

        SynModuleDecl.Types ([ typeDecl ], range0)

    let private buildType (x : ParameterInfo) : SynType =
        if x.IsOptional then
            SynType.app "option" [ x.Type ]
        else
            x.Type

    let private constructMemberSinglePlace (tuple : TupledArg) : SynType =
        tuple.Args
        |> List.map buildType
        |> SynType.tupleNoParen
        |> Option.defaultWith (fun () -> failwith "no-arg functions not supported yet")
        |> if tuple.HasParen then SynType.paren else id

    let constructMember (mem : MemberInfo) : SynField =
        let inputType = mem.Args |> List.map constructMemberSinglePlace

        let funcType = SynType.toFun inputType mem.ReturnType

        {
            Type = funcType
            Attrs = []
            Ident = Some mem.Identifier
        }
        |> SynField.make
        |> SynField.withDocString (mem.XmlDoc |> Option.defaultValue PreXmlDoc.Empty)

    let constructProperty (prop : PropertyInfo) : SynField =
        {
            Attrs = []
            Ident = Some prop.Identifier
            Type = SynType.toFun [ SynType.unit ] prop.Type
        }
        |> SynField.make
        |> SynField.withDocString (prop.XmlDoc |> Option.defaultValue PreXmlDoc.Empty)

    let createRecord
        (namespaceId : LongIdent)
        (opens : SynOpenDeclTarget list)
        (interfaceType : SynTypeDefn, spec : GenerateMockOutputSpec)
        : SynModuleOrNamespace
        =
        let interfaceType = AstHelper.parseInterface interfaceType

        let fields =
            interfaceType.Members
            |> List.map constructMember
            |> List.append (interfaceType.Properties |> List.map constructProperty)

        let docString = PreXmlDoc.create "Mock record type for an interface"

        let name =
            List.last interfaceType.Name
            |> _.idText
            |> fun s ->
                if s.StartsWith 'I' && s.Length > 1 && Char.IsUpper s.[1] then
                    s.Substring 1
                else
                    s
            |> fun s -> s + "Mock"

        let typeDecl = createType spec name interfaceType docString fields

        [ yield! opens |> List.map SynModuleDecl.openAny ; yield typeDecl ]
        |> SynModuleOrNamespace.createNamespace namespaceId

/// Whippet generator that creates a record which implements the given interface,
/// but with every field mocked out.
[<WhippetGenerator>]
type InterfaceMockGenerator () =

    interface IGenerateRawFromRaw with
        member _.GenerateRawFromRaw (context : RawSourceGenerationArgs) =
            if not (context.FilePath.EndsWith (".fs", StringComparison.Ordinal)) then
                null
            else

            let targetedTypes =
                context.Parameters
                |> Seq.map (fun (KeyValue (k, v)) -> k, v.Split '!' |> Array.toList |> List.map DesiredGenerator.Parse)
                |> Map.ofSeq

            let ast = Ast.parse (System.Text.Encoding.UTF8.GetString context.FileContents)

            let types = Ast.getTypes ast

            let namespaceAndInterfaces =
                types
                |> List.choose (fun (ns, types) ->
                    types
                    |> List.choose (fun typeDef ->
                        let name = SynTypeDefn.getName typeDef |> List.map _.idText |> String.concat "."

                        match Map.tryFind name targetedTypes with
                        | Some desired ->
                            desired
                            |> List.tryPick (fun generator ->
                                match generator with
                                | Some (DesiredGenerator.InterfaceMock arg) ->
                                    let spec =
                                        {
                                            IsInternal = arg |> Option.defaultValue true
                                        }

                                    Some (typeDef, spec)
                                | _ -> None
                            )
                        | _ -> None
                    )
                    |> function
                        | [] -> None
                        | ty -> Some (ns, ty)
                )

            let opens = AstHelper.extractOpens ast

            let modules =
                namespaceAndInterfaces
                |> List.collect (fun (ns, records) ->
                    records |> List.map (InterfaceMockGenerator.createRecord ns opens)
                )

            Ast.render modules |> Option.toObj
