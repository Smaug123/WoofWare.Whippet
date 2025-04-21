namespace WoofWare.Whippet.Fantomas

open System
open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Patterns to let you match on a `SynType` to discover whether it's one of a well-known variety.
[<AutoOpen>]
module SynTypePatterns =
    /// An `option` type. You get access to the type argument of `option`.
    let (|OptionType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ innerType ], _, _, _, _) when SynLongIdent.isOption ident ->
            Some innerType
        | _ -> None

    /// A `Choice` type. You get access to the type arguments; for example, a two-case `Choice<int, string>` would match
    /// `ChoiceType [PrimitiveType "System.Int32" ; PrimitiveType "System.String"]`.
    let (|ChoiceType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, inner, _, _, _, _) when SynLongIdent.isChoice ident -> Some inner
        | _ -> None

    /// A `System.Nullable` type. You get access to its type argument.
    let (|NullableType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ innerType ], _, _, _, _) when SynLongIdent.isNullable ident ->
            Some innerType
        | _ -> None

    /// An F# list type. You get access to its type argument.
    let (|ListType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ innerType ], _, _, _, _) when SynLongIdent.isList ident ->
            Some innerType
        | _ -> None

    /// An array type. You get access to its type argument.
    let (|ArrayType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ innerType ], _, _, _, _) when SynLongIdent.isArray ident ->
            Some innerType
        | SynType.Array (1, innerType, _) -> Some innerType
        | _ -> None

    /// The `RestEase.Response` type.
    let (|RestEaseResponseType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ innerType ], _, _, _, _) when SynLongIdent.isResponse ident ->
            Some innerType
        | _ -> None

    /// A System.Collections.Generic.Dictionary<_,_> type. You get access to its key and value argument types.
    let (|DictionaryType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ key ; value ], _, _, _, _) when SynLongIdent.isDictionary ident ->
            Some (key, value)
        | _ -> None

    /// A System.Collections.Generic.IDictionary<_,_> type. You get access to its key and value argument types.
    /// Note that this is purely syntactic: a plain `Dictionary<_, _>` won't match this.
    let (|IDictionaryType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ key ; value ], _, _, _, _) when SynLongIdent.isIDictionary ident ->
            Some (key, value)
        | _ -> None

    /// A System.Collections.Generic.IReadOnlyDictionary<_,_> type. You get access to its key and value argument types.
    /// Note that this is purely syntactic: a plain `Dictionary<_, _>` won't match this.
    let (|IReadOnlyDictionaryType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ key ; value ], _, _, _, _) when
            SynLongIdent.isReadOnlyDictionary ident
            ->
            Some (key, value)
        | _ -> None

    /// An F# Map<_, _> type. You get access to its key and value argument types.
    let (|MapType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.App (SynType.LongIdent ident, _, [ key ; value ], _, _, _, _) when SynLongIdent.isMap ident ->
            Some (key, value)
        | _ -> None

    /// A System.Numerics.BigInteger type (which can be denoted `bigint`).
    let (|BigInt|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent |> List.map _.idText with
            | [ "bigint" ]
            | [ "BigInteger" ]
            | [ "Numerics" ; "BigInteger" ]
            | [ "System" ; "Numerics" ; "BigInteger" ] -> Some ()
            | _ -> None
        | _ -> None

    /// Returns the type, qualified as in e.g. `System.Boolean`.
    let (|PrimitiveType|_|) (fieldType : SynType) : LongIdent option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent with
            | [ i ] -> Primitives.qualifyType i.idText
            | _ -> None
        | _ -> None

    /// The `string` type.
    let (|String|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent with
            | [ i ] ->
                [ "string" ]
                |> List.tryFind (fun s -> s = i.idText)
                |> Option.map ignore<string>
            | _ -> None
        | _ -> None

    /// The `byte` type.
    let (|Byte|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent with
            | [ i ] -> [ "byte" ] |> List.tryFind (fun s -> s = i.idText) |> Option.map ignore<string>
            | _ -> None
        | _ -> None

    /// The `System.Guid` type.
    let (|Guid|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent |> List.map (fun i -> i.idText) with
            | [ "System" ; "Guid" ]
            | [ "Guid" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.Net.Http.HttpResponseMessage` type.
    let (|HttpResponseMessage|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent |> List.map (fun i -> i.idText) with
            | [ "System" ; "Net" ; "Http" ; "HttpResponseMessage" ]
            | [ "Net" ; "Http" ; "HttpResponseMessage" ]
            | [ "Http" ; "HttpResponseMessage" ]
            | [ "HttpResponseMessage" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.Net.Http.HttpContent` type.
    let (|HttpContent|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent |> List.map (fun i -> i.idText) with
            | [ "System" ; "Net" ; "Http" ; "HttpContent" ]
            | [ "Net" ; "Http" ; "HttpContent" ]
            | [ "Http" ; "HttpContent" ]
            | [ "HttpContent" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.IO.Stream` type.
    let (|Stream|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent |> List.map (fun i -> i.idText) with
            | [ "System" ; "IO" ; "Stream" ]
            | [ "IO" ; "Stream" ]
            | [ "Stream" ] -> Some ()
            | _ -> None
        | _ -> None

    /// A numeric primitive type, like `byte` or `float` but not `char`.
    /// You get access to the fully-qualified type name, like `System.Int32`.
    let (|NumberType|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent ident ->
            match ident.LongIdent with
            | [ i ] ->
                // We won't bother with the case that the user has done e.g. `Single` (relying on `System` being open).
                match Primitives.qualifyType i.idText with
                | Some qualified ->
                    match i.idText with
                    | "char"
                    | "string" -> None
                    | _ -> Some qualified
                | None -> None
            | _ -> None
        | _ -> None

    /// A type with a unit of measure. Returns the name of the measure, and the outer type to which the measure was
    /// applied.
    let (|Measure|_|) (fieldType : SynType) : (Ident * LongIdent) option =
        match fieldType with
        | SynType.App (NumberType outer,
                       _,
                       [ SynType.LongIdent (SynLongIdent.SynLongIdent ([ ident ], _, _)) ],
                       _,
                       _,
                       _,
                       _) -> Some (ident, outer)
        | _ -> None

    /// The `System.Text.Json.Nodes.JsonNode` type.
    let (|JsonNode|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "Text" ; "Json" ; "Nodes" ; "JsonNode" ]
            | [ "Text" ; "Json" ; "Nodes" ; "JsonNode" ]
            | [ "Json" ; "Nodes" ; "JsonNode" ]
            | [ "Nodes" ; "JsonNode" ]
            | [ "JsonNode" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `unit` type.
    let (|UnitType|_|) (fieldType : SynType) : unit option =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText.ToLowerInvariant ()) with
            | [ "microsoft" ; "fsharp" ; "core" ; "unit" ]
            | [ "fsharp" ; "core" ; "unit" ]
            | [ "core" ; "unit" ]
            | [ "unit" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.DateOnly` type.
    let (|DateOnly|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "DateOnly" ]
            | [ "DateOnly" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.DateTime` type.
    let (|DateTime|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "DateTime" ]
            | [ "DateTime" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.DateTimeOffset` type.
    let (|DateTimeOffset|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "DateTimeOffset" ]
            | [ "DateTimeOffset" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.Uri` type.
    let (|Uri|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "Uri" ]
            | [ "Uri" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.Threading.Tasks.Task<_>` type. You get access to the generic argument.
    /// Due to a design error which I haven't yet fixed, this throws on the non-generic Task; please raise an issue
    /// if you run into this.
    let (|Task|_|) (fieldType : SynType) : SynType option =
        match fieldType with
        | SynType.App (SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)), _, args, _, _, _, _) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "Task" ]
            | [ "Tasks" ; "Task" ]
            | [ "Threading" ; "Tasks" ; "Task" ]
            | [ "System" ; "Threading" ; "Tasks" ; "Task" ] ->
                match args with
                | [ arg ] -> Some arg
                | _ -> failwithf "Expected Task to be applied to exactly one arg, but got: %+A" args
            | _ -> None
        | _ -> None

    /// The `System.IO.DirectoryInfo` type.
    let (|DirectoryInfo|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "IO" ; "DirectoryInfo" ]
            | [ "IO" ; "DirectoryInfo" ]
            | [ "DirectoryInfo" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.IO.FieldInfo` type.
    let (|FileInfo|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "IO" ; "FileInfo" ]
            | [ "IO" ; "FileInfo" ]
            | [ "FileInfo" ] -> Some ()
            | _ -> None
        | _ -> None

    /// The `System.TimeSpan` type.
    let (|TimeSpan|_|) (fieldType : SynType) =
        match fieldType with
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) ->
            match ident |> List.map (fun i -> i.idText) with
            | [ "System" ; "TimeSpan" ]
            | [ "TimeSpan" ] -> Some ()
            | _ -> None
        | _ -> None

/// Methods for manipulating SynType, which represents a type as might appear e.g. in a type annotation.
[<RequireQualifiedAccess>]
module SynType =

    /// Strip all outer parentheses from this SynType.
    ///
    /// Most functions by default will not strip parentheses. Note, for example, that `let foo : (int) = 3` is fine.
    /// Use `stripOptionalParen` to turn that `(int)` into `int`, so that e.g. you can match on it using the patterns
    /// in `SynTypePatterns`.
    let rec stripOptionalParen (ty : SynType) : SynType =
        match ty with
        | SynType.Paren (ty, _) -> stripOptionalParen ty
        | ty -> ty

    /// Wrap this type in parentheses.
    let inline paren (ty : SynType) : SynType = SynType.Paren (ty, range0)

    /// Define a SynType by just a name.
    let inline createLongIdent (ident : LongIdent) : SynType =
        SynType.LongIdent (SynLongIdent.create ident)

    /// Define a SynType by just a name.
    let inline createLongIdent' (ident : string list) : SynType =
        SynType.LongIdent (SynLongIdent.createS' ident)

    /// Define a SynType by just a name. Use `createLongIdent'` if you want more components in this name,
    /// so e.g. don't pass `System.Collections.Generic.Dictionary` to `named`.
    let inline named (name : string) = createLongIdent' [ name ]

    /// {name}<{args}>
    let inline app' (name : SynType) (args : SynType list) : SynType =
        if args.IsEmpty then
            failwith "Type cannot be applied to no arguments"

        SynType.App (name, Some range0, args, List.replicate (args.Length - 1) range0, Some range0, false, range0)

    /// {name}<{args}>
    let inline app (name : string) (args : SynType list) : SynType = app' (named name) args

    /// Returns None if the input list was empty.
    let inline tupleNoParen (ty : SynType list) : SynType option =
        match List.rev ty with
        | [] -> None
        | [ t ] -> Some t
        | t :: rest ->
            ([ SynTupleTypeSegment.Type t ], rest)
            ||> List.fold (fun ty nextArg -> SynTupleTypeSegment.Type nextArg :: SynTupleTypeSegment.Star range0 :: ty)
            |> fun segs -> SynType.Tuple (false, segs, range0)
            |> Some

    /// `{arg} {name}`, e.g. `int option`.
    let inline appPostfix (name : string) (arg : SynType) : SynType =
        SynType.App (named name, None, [ arg ], [], None, true, range0)

    /// `{arg} {name1.name2.name3}`, e.g. `int System.Nullable`.
    let inline appPostfix' (name : string list) (arg : SynType) : SynType =
        SynType.App (createLongIdent' name, None, [ arg ], [], None, true, range0)

    /// The type `{domain} -> {range}`.
    let inline funFromDomain (domain : SynType) (range : SynType) : SynType =
        SynType.Fun (
            domain,
            range,
            range0,
            {
                ArrowRange = range0
            }
        )

    /// In an abstract type definition like `type Foo = abstract Blah : x : int -> string`,
    /// this represents one single `x : int` part.
    let inline signatureParamOfType
        (attrs : SynAttribute list)
        (ty : SynType)
        (optional : bool)
        (name : Ident option)
        : SynType
        =
        SynType.SignatureParameter (
            attrs
            |> List.map (fun attr ->
                {
                    Attributes = [ attr ]
                    Range = range0
                }
            ),
            optional,
            name,
            ty,
            range0
        )

    /// Create a type which refers to a generic type parameter. For example, `'a` (assuming there's already
    /// some language construct causing the generic `'a` to be in scope).
    let inline var (ty : SynTypar) : SynType = SynType.Var (ty, range0)

    /// The `unit` type.
    let unit : SynType = named "unit"

    /// The `obj` type.
    let obj : SynType = named "obj"

    /// The `bool` type.
    let bool : SynType = named "bool"

    /// The `int` type.
    let int : SynType = named "int"

    /// The type `{elt} array`.
    let array (elt : SynType) : SynType = SynType.Array (1, elt, range0)

    /// The type `{elt} list` (i.e. an F# list).
    let list (elt : SynType) : SynType =
        SynType.App (named "list", None, [ elt ], [], None, true, range0)

    /// The type `{elt} option`.
    let option (elt : SynType) : SynType =
        SynType.App (named "option", None, [ elt ], [], None, true, range0)

    /// The anonymous type, i.e. the `_` in the type-annotated `x : _`.
    let anon : SynType = SynType.Anon range0

    /// The type `System.Threading.Tasks.Task<{elt}>`.
    let task (elt : SynType) : SynType =
        SynType.App (
            createLongIdent' [ "System" ; "Threading" ; "Tasks" ; "Task" ],
            None,
            [ elt ],
            [],
            None,
            true,
            range0
        )

    /// The type `string`.
    let string : SynType = named "string"

    /// Given ['a1, 'a2] and 'ret, returns 'a1 -> 'a2 -> 'ret.
    let toFun (inputs : SynType list) (ret : SynType) : SynType =
        (ret, List.rev inputs) ||> List.fold (fun ty input -> funFromDomain input ty)

    /// Convert a canonical form like `System.Int32` to a human-readable form like `int32`.
    /// Throws on unrecognised inputs.
    let primitiveToHumanReadableString (name : LongIdent) : string =
        match name |> List.map _.idText with
        | [ "System" ; "Single" ] -> "single"
        | [ "System" ; "Double" ] -> "double"
        | [ "System" ; "Byte" ] -> "byte"
        | [ "System" ; "SByte" ] -> "signed byte"
        | [ "System" ; "Int16" ] -> "int16"
        | [ "System" ; "Int32" ] -> "int32"
        | [ "System" ; "Int64" ] -> "int64"
        | [ "System" ; "UInt16" ] -> "uint16"
        | [ "System" ; "UInt32" ] -> "uint32"
        | [ "System" ; "UInt64" ] -> "uint64"
        | [ "System" ; "Char" ] -> "char"
        | [ "System" ; "Decimal" ] -> "decimal"
        | [ "System" ; "String" ] -> "string"
        | [ "System" ; "Boolean" ] -> "bool"
        | ty ->
            ty
            |> String.concat "."
            |> failwithf "could not create human-readable string for primitive type %s"

    /// Attempt to create a human-readable representation of this type, for use in error messages.
    /// This function throws if we couldn't decide on a human-readable representation.
    let rec toHumanReadableString (ty : SynType) : string =
        match ty with
        | PrimitiveType t1 -> primitiveToHumanReadableString t1
        | OptionType t1 -> toHumanReadableString t1 + " option"
        | NullableType t1 -> toHumanReadableString t1 + " Nullable"
        | ChoiceType ts ->
            ts
            |> List.map toHumanReadableString
            |> String.concat ", "
            |> sprintf "Choice<%s>"
        | MapType (k, v)
        | DictionaryType (k, v)
        | IDictionaryType (k, v)
        | IReadOnlyDictionaryType (k, v) -> sprintf "map<%s, %s>" (toHumanReadableString k) (toHumanReadableString v)
        | ListType t1 -> toHumanReadableString t1 + " list"
        | ArrayType t1 -> toHumanReadableString t1 + " array"
        | Task t1 -> toHumanReadableString t1 + " Task"
        | UnitType -> "unit"
        | FileInfo -> "FileInfo"
        | DirectoryInfo -> "DirectoryInfo"
        | Uri -> "URI"
        | Stream -> "Stream"
        | Guid -> "GUID"
        | BigInt -> "bigint"
        | DateTimeOffset -> "DateTimeOffset"
        | DateOnly -> "DateOnly"
        | TimeSpan -> "TimeSpan"
        | SynType.LongIdent (SynLongIdent.SynLongIdent (ident, _, _)) -> ident |> List.map _.idText |> String.concat "."
        | SynType.App (ty, _, args, _, _, _, _) ->
            let args = args |> Seq.map toHumanReadableString |> String.concat ", "
            $"%s{toHumanReadableString ty}<%s{args}>"
        | ty -> failwithf "could not compute human-readable string for type: %O" ty

    /// Guess whether the types are equal. We err on the side of saying "no, they're different".
    let rec provablyEqual (ty1 : SynType) (ty2 : SynType) : bool =
        if Object.ReferenceEquals (ty1, ty2) then
            true
        else

        match ty1 with
        | PrimitiveType t1 ->
            match ty2 with
            | PrimitiveType t2 -> (t1 |> List.map _.idText) = (t2 |> List.map _.idText)
            | _ -> false
        | OptionType t1 ->
            match ty2 with
            | OptionType t2 -> provablyEqual t1 t2
            | _ -> false
        | NullableType t1 ->
            match ty2 with
            | NullableType t2 -> provablyEqual t1 t2
            | _ -> false
        | ChoiceType t1 ->
            match ty2 with
            | ChoiceType t2 ->
                t1.Length = t2.Length
                && List.forall (fun (a, b) -> provablyEqual a b) (List.zip t1 t2)
            | _ -> false
        | DictionaryType (k1, v1) ->
            match ty2 with
            | DictionaryType (k2, v2) -> provablyEqual k1 k2 && provablyEqual v1 v2
            | _ -> false
        | IDictionaryType (k1, v1) ->
            match ty2 with
            | IDictionaryType (k2, v2) -> provablyEqual k1 k2 && provablyEqual v1 v2
            | _ -> false
        | IReadOnlyDictionaryType (k1, v1) ->
            match ty2 with
            | IReadOnlyDictionaryType (k2, v2) -> provablyEqual k1 k2 && provablyEqual v1 v2
            | _ -> false
        | MapType (k1, v1) ->
            match ty2 with
            | MapType (k2, v2) -> provablyEqual k1 k2 && provablyEqual v1 v2
            | _ -> false
        | ListType t1 ->
            match ty2 with
            | ListType t2 -> provablyEqual t1 t2
            | _ -> false
        | ArrayType t1 ->
            match ty2 with
            | ArrayType t2 -> provablyEqual t1 t2
            | _ -> false
        | Task t1 ->
            match ty2 with
            | Task t2 -> provablyEqual t1 t2
            | _ -> false
        | UnitType ->
            match ty2 with
            | UnitType -> true
            | _ -> false
        | FileInfo ->
            match ty2 with
            | FileInfo -> true
            | _ -> false
        | DirectoryInfo ->
            match ty2 with
            | DirectoryInfo -> true
            | _ -> false
        | Uri ->
            match ty2 with
            | Uri -> true
            | _ -> false
        | Stream ->
            match ty2 with
            | Stream -> true
            | _ -> false
        | Guid ->
            match ty2 with
            | Guid -> true
            | _ -> false
        | BigInt ->
            match ty2 with
            | BigInt -> true
            | _ -> false
        | DateTimeOffset ->
            match ty2 with
            | DateTimeOffset -> true
            | _ -> false
        | DateOnly ->
            match ty2 with
            | DateOnly -> true
            | _ -> false
        | _ ->

        match ty1, ty2 with
        | SynType.LongIdent (SynLongIdent (ident1, _, _)), SynType.LongIdent (SynLongIdent (ident2, _, _)) ->
            let ident1 = ident1 |> List.map _.idText
            let ident2 = ident2 |> List.map _.idText
            ident1 = ident2
        | _, _ -> false

    /// Returns the args (where these are tuple types if curried) in order, and the return type.
    let rec getType (ty : SynType) : (SynType * bool) list * SynType =
        match ty with
        | SynType.Paren (ty, _) -> getType ty
        | SynType.Fun (argType, returnType, _, _) ->
            let args, ret = getType returnType
            // TODO this code is clearly wrong
            let (inputArgs, inputRet), hasParen =
                match argType with
                | SynType.Paren (argType, _) -> getType argType, true
                | _ -> getType argType, false

            ((toFun (List.map fst inputArgs) inputRet), hasParen) :: args, ret
        | _ -> [], ty
