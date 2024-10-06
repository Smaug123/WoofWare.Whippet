namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range
open Fantomas.FCS.Xml
open WoofWare.Whippet.Fantomas

/// Information about a single parameter to a method.
/// Several of these might be tupled up.
type ParameterInfo =
    {
        /// Attributes which apply to this parameter.
        Attributes : SynAttribute list
        /// True if the parameter is optional (i.e. `?foo`)
        IsOptional : bool
        /// Name of the parameter, if there is one.
        Id : Ident option
        /// Type of the parameter.
        Type : SynType
    }

/// A combination of several ParameterInfo tupled together.
type TupledArg =
    {
        /// True if there are parentheses around this tuple.
        HasParen : bool
        /// The various parameters tupled into this arg.
        Args : ParameterInfo list
    }

/// A member of an interface definition.
type MemberInfo =
    {
        /// The type of the member, e.g. the `int` in `member this.Foo : unit -> int`.
        ReturnType : SynType
        /// Accessibility modifier, e.g. `member private this.Foo ...`
        Accessibility : SynAccess option
        /// Each element of this list is a list of args in a tuple, or just one arg if not a tuple.
        Args : TupledArg list
        /// Name of this member.
        Identifier : Ident
        /// Attribute applying to this member.
        Attributes : SynAttribute list
        /// Docstring of this member.
        XmlDoc : PreXmlDoc option
        /// True if there's an `inline` modifier on this member.
        IsInline : bool
        /// True if there's a `mutable` modifier on this member.
        IsMutable : bool
    }

/// A property can be `with get() = ...`, `with set param = ...`, or `with get() = ... and set param = ...`.
[<RequireQualifiedAccess>]
type PropertyAccessors =
    /// This property has only a getter.
    | Get
    /// This property has only a setter.
    | Set
    /// This property has both a getter and a setter.
    | GetSet

/// A property in an interface definition, i.e. something with a `get` and/or `set`.
type PropertyInfo =
    {
        /// Type of the property.
        Type : SynType
        /// Accessibility modifier of the property.
        Accessibility : SynAccess option
        /// Attributes on the property.
        Attributes : SynAttribute list
        /// Docstring of the property.
        XmlDoc : PreXmlDoc option
        /// How this property can be accessed.
        Accessors : PropertyAccessors
        /// True if there's an `inline` modifier on this property.
        IsInline : bool
        /// Name of the property.
        Identifier : Ident
    }

/// Structured representation of an interface type definition.
type InterfaceType =
    {
        /// Attributes on the type.
        Attributes : SynAttribute list
        /// Name of the type being defined.
        Name : LongIdent
        /// Any `inherit` statements: `type Foo = inherit Bar`.
        Inherits : SynType list
        /// Interface members.
        Members : MemberInfo list
        /// Interface properties.
        Properties : PropertyInfo list
        /// Any generic type parameters the type has.
        Generics : SynTyparDecls option
        /// Accessibility modifier: `type private Foo = ...`
        Accessibility : SynAccess option
    }

/// Structured representation of a record type definition.
type RecordType =
    {
        /// Name of the type being defined.
        Name : Ident
        /// Fields in the record.
        Fields : SynField list
        /// Any additional members which are not record fields.
        Members : SynMemberDefns option
        /// Docstring of the type.
        XmlDoc : PreXmlDoc option
        /// Any generic parameters the type has.
        Generics : SynTyparDecls option
        /// Accessibility: `type private Foo = { ... }`
        TypeAccessibility : SynAccess option
        /// Accessibility: `type Foo = private { ... }`
        ImplAccessibility : SynAccess option
        /// Attributes on the type.
        Attributes : SynAttribute list
    }

    /// Parse from the AST.
    static member OfRecord
        (sci : SynComponentInfo)
        (smd : SynMemberDefns)
        (access : SynAccess option)
        (recordFields : SynField list)
        : RecordType
        =
        match sci with
        | SynComponentInfo.SynComponentInfo (attrs, typars, _, longId, doc, _, implAccess, _) ->
            {
                Name = List.last longId
                Fields = recordFields
                Members = if smd.IsEmpty then None else Some smd
                XmlDoc = if doc.IsEmpty then None else Some doc
                Generics = typars
                ImplAccessibility = implAccess
                TypeAccessibility = access
                Attributes = attrs |> List.collect (fun l -> l.Attributes)
            }

    /// Construct the AST object which represents the definition of this record type.
    static member ToAst (record : RecordType) : SynTypeDefn =
        let name =
            SynComponentInfo.create record.Name
            |> SynComponentInfo.setAccessibility record.TypeAccessibility
            |> match record.XmlDoc with
               | None -> id
               | Some doc -> SynComponentInfo.withDocString doc
            |> SynComponentInfo.setGenerics record.Generics

        SynTypeDefnRepr.recordWithAccess record.ImplAccessibility (Seq.toList record.Fields)
        |> SynTypeDefn.create name
        |> SynTypeDefn.withMemberDefns (defaultArg record.Members SynMemberDefns.Empty)

/// Methods for manipulating UnionCase.
[<RequireQualifiedAccess>]
module UnionCase =
    /// Construct our structured `UnionCase` from an FCS `SynUnionCase`: extract everything
    /// we care about from the AST representation.
    let ofSynUnionCase (case : SynUnionCase) : UnionCase<Ident option> =
        match case with
        | SynUnionCase.SynUnionCase (attributes, ident, caseType, xmlDoc, access, _, _) ->

        let ident =
            match ident with
            | SynIdent.SynIdent (ident, _) -> ident

        let fields =
            match caseType with
            | SynUnionCaseKind.Fields cases -> cases
            | SynUnionCaseKind.FullType _ -> failwith "unexpected FullType union"

        {
            Name = ident
            XmlDoc = if xmlDoc.IsEmpty then None else Some xmlDoc
            Access = access
            Attributes = attributes |> List.collect (fun t -> t.Attributes)
            Fields = fields |> List.map SynField.extract
        }

    /// Functorial `map`.
    let mapIdentFields<'a, 'b> (f : 'a -> 'b) (unionCase : UnionCase<'a>) : UnionCase<'b> =
        {
            Attributes = unionCase.Attributes
            Name = unionCase.Name
            Access = unionCase.Access
            XmlDoc = unionCase.XmlDoc
            Fields = unionCase.Fields |> List.map (SynField.mapIdent f)
        }

/// Structured representation of a discriminated union definition.
type UnionType =
    {
        /// The name of the DU: for example, `type Foo = | Blah` has this being `Foo`.
        Name : Ident
        /// Any additional members which are not union cases.
        Members : SynMemberDefns option
        /// Any docstring associated with the DU itself (not its cases).
        XmlDoc : PreXmlDoc option
        /// Generic type parameters this DU takes: `type Foo<'a> = | ...`.
        Generics : SynTyparDecls option
        /// Attributes of the DU (not its cases): `[<Attr>] type Foo = | ...`
        Attributes : SynAttribute list
        /// Accessibility modifier of the DU: `type private Foo = ...`
        TypeAccessibility : SynAccess option
        /// Accessibility modifier of the DU's implementation: `type Foo = private | ...`
        ImplAccessibility : SynAccess option
        /// The actual DU cases themselves.
        Cases : UnionCase<Ident option> list
    }

    /// Build a structured representation from a DU pulled from the AST.
    static member OfUnion
        (sci : SynComponentInfo)
        (smd : SynMemberDefns)
        (access : SynAccess option)
        (cases : SynUnionCase list)
        : UnionType
        =
        match sci with
        | SynComponentInfo.SynComponentInfo (attrs, typars, _, longId, doc, _, implAccess, _) ->
            {
                Name = List.last longId
                Members = if smd.IsEmpty then None else Some smd
                XmlDoc = if doc.IsEmpty then None else Some doc
                Generics = typars
                Attributes = attrs |> List.collect (fun l -> l.Attributes)
                TypeAccessibility = access
                ImplAccessibility = implAccess
                Cases = cases |> List.map UnionCase.ofSynUnionCase
            }

/// Miscellaneous methods for manipulating AST types.
[<RequireQualifiedAccess>]
module AstHelper =

    /// Construct a record from the given fields.
    let instantiateRecord (fields : (SynLongIdent * SynExpr) list) : SynExpr =
        let fields =
            fields
            |> List.map (fun (rfn, synExpr) -> SynExprRecordField ((rfn, true), Some range0, Some synExpr, None))

        SynExpr.Record (None, None, fields, range0)

    let rec private extractOpensFromDecl (moduleDecls : SynModuleDecl list) : SynOpenDeclTarget list =
        moduleDecls
        |> List.choose (fun moduleDecl ->
            match moduleDecl with
            | SynModuleDecl.Open (target, _) -> Some target
            | _ -> None
        )

    /// Get all the `open` statements from the input.
    let extractOpens (ast : ParsedInput) : SynOpenDeclTarget list =
        match ast with
        | ParsedInput.SigFile _ -> failwith "Signature files not yet supported"
        | ParsedInput.ImplFile (ParsedImplFileInput (_, _, _, _, _, modules, _, _, _)) ->
            modules
            |> List.collect (fun (SynModuleOrNamespace (_, _, _, decls, _, _, _, _, _)) -> extractOpensFromDecl decls)

    let rec private convertSigParam (ty : SynType) : ParameterInfo * bool =
        match ty with
        | SynType.Paren (inner, _) ->
            let result, _ = convertSigParam inner
            result, true
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            {
                Attributes = []
                IsOptional = false
                Id = None
                Type = SynType.createLongIdent ident
            },
            false
        | SynType.SignatureParameter (attrs, opt, id, usedType, _) ->
            let attrs = attrs |> List.collect (fun attrs -> attrs.Attributes)

            {
                Attributes = attrs
                IsOptional = opt
                Id = id
                Type = usedType
            },
            false
        | SynType.Var (typar, _) ->
            {
                Attributes = []
                IsOptional = false
                Id = None
                Type = SynType.var typar
            },
            false
        | _ -> failwithf "expected SignatureParameter, got: %+A" ty

    let rec private extractTupledTypes (tupleType : SynTupleTypeSegment list) : TupledArg =
        match tupleType with
        | [] ->
            {
                HasParen = false
                Args = []
            }
        | [ SynTupleTypeSegment.Type param ] ->
            let converted, hasParen = convertSigParam param

            {
                HasParen = hasParen
                Args = [ converted ]
            }
        | SynTupleTypeSegment.Type param :: SynTupleTypeSegment.Star _ :: rest ->
            let rest = extractTupledTypes rest
            let converted, _ = convertSigParam param

            {
                HasParen = false
                Args = converted :: rest.Args
            }
        | _ -> failwithf "Didn't have alternating type-and-star in interface member definition: %+A" tupleType

    let private parseMember (slotSig : SynValSig) (flags : SynMemberFlags) : Choice<MemberInfo, PropertyInfo> =
        if not flags.IsInstance then
            failwith "member was not an instance member"

        let propertyAccessors =
            match flags.MemberKind with
            | SynMemberKind.Member -> None
            | SynMemberKind.PropertyGet -> Some PropertyAccessors.Get
            | SynMemberKind.PropertySet -> Some PropertyAccessors.Set
            | SynMemberKind.PropertyGetSet -> Some PropertyAccessors.GetSet
            | kind -> failwithf "Unrecognised member kind: %+A" kind

        match slotSig with
        | SynValSig (attrs,
                     SynIdent.SynIdent (ident, _),
                     _typeParams,
                     synType,
                     _arity,
                     isInline,
                     isMutable,
                     xmlDoc,
                     accessibility,
                     synExpr,
                     _,
                     _) ->

            match synExpr with
            | Some _ -> failwith "literal members are not supported"
            | None -> ()

            let attrs = attrs |> List.collect _.Attributes

            let args, ret = SynType.getType synType

            let args =
                args
                |> List.map (fun (args, hasParen) ->
                    match args with
                    | SynType.Tuple (false, path, _) -> extractTupledTypes path
                    | SynType.SignatureParameter _ ->
                        let arg, hasParen = convertSigParam args

                        {
                            HasParen = hasParen
                            Args = [ arg ]
                        }
                    | SynType.LongIdent (SynLongIdent (ident, _, _)) ->
                        {
                            HasParen = false
                            Args =
                                {
                                    Attributes = []
                                    IsOptional = false
                                    Id = None
                                    Type = SynType.createLongIdent ident
                                }
                                |> List.singleton
                        }
                    | SynType.Var (typar, _) ->
                        {
                            HasParen = false
                            Args =
                                {
                                    Attributes = []
                                    IsOptional = false
                                    Id = None
                                    Type = SynType.var typar
                                }
                                |> List.singleton
                        }
                    | arg ->
                        {
                            HasParen = false
                            Args =
                                {
                                    Attributes = []
                                    IsOptional = false
                                    Id = None
                                    Type = arg
                                }
                                |> List.singleton
                        }
                    |> fun ty ->
                        { ty with
                            HasParen = ty.HasParen || hasParen
                        }
                )

            match propertyAccessors with
            | None ->
                {
                    ReturnType = ret
                    Args = args
                    Identifier = ident
                    Attributes = attrs
                    XmlDoc = Some xmlDoc
                    Accessibility = accessibility
                    IsInline = isInline
                    IsMutable = isMutable
                }
                |> Choice1Of2
            | Some accessors ->
                {
                    Type = ret
                    Accessibility = accessibility
                    Attributes = attrs
                    XmlDoc = Some xmlDoc
                    Accessors = accessors
                    IsInline = isInline
                    Identifier = ident
                }
                |> Choice2Of2

    /// Assumes that the input type is an ObjectModel, i.e. a `type Foo = member ...`
    let parseInterface (interfaceType : SynTypeDefn) : InterfaceType =
        let (SynTypeDefn (SynComponentInfo (attrs, typars, _, interfaceName, _, _, accessibility, _),
                          synTypeDefnRepr,
                          _,
                          _,
                          _,
                          _)) =
            interfaceType

        let attrs = attrs |> List.collect (fun s -> s.Attributes)

        let members, inherits =
            match synTypeDefnRepr with
            | SynTypeDefnRepr.ObjectModel (_kind, members, _) ->
                members
                |> List.map (fun defn ->
                    match defn with
                    | SynMemberDefn.AbstractSlot (slotSig, flags, _, _) -> Choice1Of2 (parseMember slotSig flags)
                    | SynMemberDefn.Inherit (baseType, _asIdent, _) -> Choice2Of2 baseType
                    | _ -> failwith $"Unrecognised member definition: %+A{defn}"
                )
            | _ -> failwith $"Unrecognised SynTypeDefnRepr for an interface type: %+A{synTypeDefnRepr}"
            |> List.partitionChoice

        let members, properties = members |> List.partitionChoice

        {
            Members = members
            Properties = properties
            Name = interfaceName
            Inherits = inherits
            Attributes = attrs
            Generics = typars
            Accessibility = accessibility
        }
