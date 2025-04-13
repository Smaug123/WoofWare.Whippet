namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Text.Range

/// Extension methods to hold `SynExpr.CreateConst`.
[<AutoOpen>]
module SynExprExtensions =
    type SynExpr with
        /// Create the constant expression which is the given string.
        /// For example, create the F# expression `"hi"` by calling `SynExpr.CreateConst "hi"`.
        static member CreateConst (s : string) : SynExpr =
            SynExpr.Const (SynConst.Create s, range0)

        /// Create the constant unit expression.
        static member CreateConst () : SynExpr = SynExpr.Const (SynConst.Unit, range0)

        /// Create the constant expression which is the given bool.
        /// For example, create the F# expression `false` by calling `SynExpr.CreateConst false`.
        static member CreateConst (b : bool) : SynExpr = SynExpr.Const (SynConst.Bool b, range0)

        /// Create the constant expression which is the given char.
        /// For example, create the F# expression `'c'` by calling `SynExpr.CreateConst 'c'`.
        ///
        /// There appears to be a bug in Fantomas in at least one version, causing incorrect formatting when this is
        /// done naively; so this function instead gives you `(char {appropriate-integer})`.
        static member CreateConst (c : char) : SynExpr =
            // apparent Fantomas bug: `IndexOf '?'` gets formatted as `IndexOf ?` which is clearly wrong
            SynExpr.App (
                ExprAtomicFlag.NonAtomic,
                false,
                SynExpr.Ident (Ident.create "char"),
                SynExpr.CreateConst (int c),
                range0
            )
            |> fun e -> SynExpr.Paren (e, range0, Some range0, range0)

        /// Create the constant expression which is the given int.
        /// For example, create the F# expression `3` by calling `SynExpr.CreateConst 3`.
        static member CreateConst (i : int32) : SynExpr =
            SynExpr.Const (SynConst.Int32 i, range0)

/// Methods for manipulating `SynExpr`, which represents an F# expression.
[<RequireQualifiedAccess>]
module SynExpr =

    /// {f} {x}
    let applyFunction (f : SynExpr) (x : SynExpr) : SynExpr =
        SynExpr.App (ExprAtomicFlag.NonAtomic, false, f, x, range0)

    /// {f} {x}
    let inline applyTo (x : SynExpr) (f : SynExpr) : SynExpr = applyFunction f x

    let inline private createAppInfix (f : SynExpr) (x : SynExpr) =
        SynExpr.App (ExprAtomicFlag.NonAtomic, true, f, x, range0)

    /// An expression which is just "this name".
    /// For example, `Foo.Blah`.
    let inline createLongIdent'' (ident : SynLongIdent) : SynExpr =
        SynExpr.LongIdent (false, ident, None, range0)

    /// An expression which is just "this name".
    /// For example, `Foo.Blah`.
    let inline createLongIdent' (ident : Ident list) : SynExpr =
        createLongIdent'' (SynLongIdent.create ident)

    /// An expression which is just "this name".
    /// For example, `Foo.Blah`.
    let inline createLongIdent (ident : string list) : SynExpr =
        createLongIdent' (ident |> List.map Ident.create)

    /// {expr} |> {func}
    let pipeThroughFunction (func : SynExpr) (expr : SynExpr) : SynExpr =
        createAppInfix (createLongIdent'' SynLongIdent.pipe) expr |> applyTo func

    /// if {cond} then {trueBranch} else {falseBranch}
    /// Note that this function puts the trueBranch last, for pipelining convenience:
    /// we assume that the `else` branch is more like an error case and is less interesting.
    let ifThenElse (cond : SynExpr) (falseBranch : SynExpr) (trueBranch : SynExpr) : SynExpr =
        SynExpr.IfThenElse (
            cond,
            trueBranch,
            Some falseBranch,
            DebugPointAtBinding.Yes range0,
            false,
            range0,
            {
                IfKeyword = range0
                IsElif = false
                ThenKeyword = range0
                ElseKeyword = Some range0
                IfToThenRange = range0
            }
        )

    /// try {body} with | {exc} as exc -> {handler}
    let pipeThroughTryWith (exc : SynPat) (handler : SynExpr) (body : SynExpr) : SynExpr =
        let clause =
            SynMatchClause.create (SynPat.As (exc, SynPat.named "exc", range0)) handler

        SynExpr.TryWith (
            body,
            [ clause ],
            range0,
            DebugPointAtTry.Yes range0,
            DebugPointAtWith.Yes range0,
            {
                TryKeyword = range0
                TryToWithRange = range0
                WithKeyword = range0
                WithToEndRange = range0
            }
        )

    /// {a} = {b}
    let equals (a : SynExpr) (b : SynExpr) =
        createAppInfix (createLongIdent'' SynLongIdent.eq) a |> applyTo b

    /// {a} && {b}
    let booleanAnd (a : SynExpr) (b : SynExpr) =
        createAppInfix (createLongIdent'' SynLongIdent.booleanAnd) a |> applyTo b

    /// {a} || {b}
    let booleanOr (a : SynExpr) (b : SynExpr) =
        createAppInfix (createLongIdent'' SynLongIdent.booleanOr) a |> applyTo b

    /// {a} + {b}
    let plus (a : SynExpr) (b : SynExpr) =
        createAppInfix (createLongIdent'' SynLongIdent.plus) a |> applyTo b

    /// {a} * {b}
    let times (a : SynExpr) (b : SynExpr) =
        createAppInfix (createLongIdent'' SynLongIdent.times) a |> applyTo b

    /// Strip all outer parentheses from the expression: `((e))` -> `e`.
    let rec stripOptionalParen (expr : SynExpr) : SynExpr =
        match expr with
        | SynExpr.Paren (expr, _, _, _) -> stripOptionalParen expr
        | expr -> expr

    /// {obj}.{field}
    let dotGet (field : string) (obj : SynExpr) : SynExpr =
        SynExpr.DotGet (
            obj,
            range0,
            SynLongIdent.SynLongIdent (id = [ Ident.create field ], dotRanges = [], trivia = [ None ]),
            range0
        )

    /// {obj}.{meth} {arg}
    let callMethodArg (meth : string) (arg : SynExpr) (obj : SynExpr) : SynExpr = dotGet meth obj |> applyTo arg

    /// {obj}.{meth}()
    let callMethod (meth : string) (obj : SynExpr) : SynExpr =
        callMethodArg meth (SynExpr.CreateConst ()) obj

    /// `{operand}<{types}>`
    let typeApp (types : SynType list) (operand : SynExpr) =
        SynExpr.TypeApp (operand, range0, types, List.replicate (types.Length - 1) range0, Some range0, range0, range0)

    /// {obj}.{meth}<types,...>()
    let callGenericMethod (meth : SynLongIdent) (types : SynType list) (obj : SynExpr) : SynExpr =
        SynExpr.DotGet (obj, range0, meth, range0)
        |> typeApp types
        |> applyTo (SynExpr.CreateConst ())

    /// {obj}.{meth}<ty>()
    let callGenericMethod' (meth : string) (ty : string) (obj : SynExpr) : SynExpr =
        callGenericMethod (SynLongIdent.createS meth) [ SynType.createLongIdent' [ ty ] ] obj

    /// {obj}.[{property}]
    let inline index (property : SynExpr) (obj : SynExpr) : SynExpr =
        SynExpr.DotIndexedGet (obj, property, range0, range0)

    /// `{arr}.[{start} .. {end}]`
    ///
    /// You can set either the start or end to None to omit them from the range.
    let inline arrayIndexRange (start : SynExpr option) (endRange : SynExpr option) (arr : SynExpr) : SynExpr =
        SynExpr.DotIndexedGet (
            arr,
            (SynExpr.IndexRange (start, range0, endRange, range0, range0, range0)),
            range0,
            range0
        )

    /// Wraps the expression in parentheses: `({e})`
    let inline paren (e : SynExpr) : SynExpr =
        SynExpr.Paren (e, range0, Some range0, range0)

    /// (fun {varName} -> {body})
    let createLambda (varName : string) (body : SynExpr) : SynExpr =
        let parsedDataPat = [ SynPat.named varName ]

        SynExpr.Lambda (
            false,
            false,
            SynSimplePats.create [ SynSimplePat.createId (Ident.create varName) ],
            body,
            Some (parsedDataPat, body),
            range0,
            {
                ArrowRange = Some range0
            }
        )
        |> paren

    /// `(fun () -> {body})`
    let createThunk (body : SynExpr) : SynExpr =
        SynExpr.Lambda (
            false,
            false,
            SynSimplePats.create [],
            body,
            Some ([ SynPat.unit ], body),
            range0,
            {
                ArrowRange = Some range0
            }
        )
        |> paren

    /// Just the plain expression `s`, referring to a variable.
    let inline createIdent (s : string) : SynExpr = SynExpr.Ident (Ident (s, range0))

    /// Just the plain expression `x`, referring to a variable.
    let inline createIdent' (i : Ident) : SynExpr = SynExpr.Ident i

    /// `{arg1}, {arg2}, ...`
    let tupleNoParen (args : SynExpr list) : SynExpr =
        SynExpr.Tuple (false, args, List.replicate (args.Length - 1) range0, range0)

    /// `({arg1}, {arg2}, ...)`
    let inline tuple (args : SynExpr list) = args |> tupleNoParen |> paren

    /// {body} |> fun a -> Async.StartAsTask (a, ?cancellationToken=ct)
    let startAsTask (ct : Ident) (body : SynExpr) =
        let lambda =
            [
                createIdent "a"
                equals
                    (SynExpr.LongIdent (true, SynLongIdent.createS "cancellationToken", None, range0))
                    (createIdent' ct)
            ]
            |> tuple
            |> applyFunction (createLongIdent [ "Async" ; "StartAsTask" ])
            |> createLambda "a"

        pipeThroughFunction lambda body

    /// `for {pat} in {enumExpr} do {body}`
    let inline createForEach (pat : SynPat) (enumExpr : SynExpr) (body : SynExpr) : SynExpr =
        SynExpr.ForEach (
            DebugPointAtFor.No,
            DebugPointAtInOrTo.No,
            SeqExprOnly.SeqExprOnly false,
            true,
            pat,
            enumExpr,
            body,
            range0
        )

    /// `let {binding1 = binding1}; let {binding2 = binding2}; {body}
    let inline createLet (bindings : SynBinding list) (body : SynExpr) : SynExpr =
        SynExpr.LetOrUse (false, false, bindings, body, range0, SynExprLetOrUseTrivia.empty)

    /// `do {body}`
    let inline createDo (body : SynExpr) : SynExpr = SynExpr.Do (body, range0)

    /// `match {matchOn} with {cases}`
    let inline createMatch (matchOn : SynExpr) (cases : SynMatchClause list) : SynExpr =
        SynExpr.Match (
            DebugPointAtBinding.Yes range0,
            matchOn,
            cases,
            range0,
            {
                MatchKeyword = range0
                WithKeyword = range0
            }
        )

    /// `{expr} : {ty}`
    let typeAnnotate (ty : SynType) (expr : SynExpr) : SynExpr = SynExpr.Typed (expr, ty, range0)

    /// `new {ty} ({args})`
    let inline createNew (ty : SynType) (args : SynExpr) : SynExpr =
        SynExpr.New (false, ty, paren args, range0)

    /// `while {cond} do {body}`
    let inline createWhile (cond : SynExpr) (body : SynExpr) : SynExpr =
        SynExpr.While (DebugPointAtWhile.Yes range0, cond, body, range0)

    /// `null`
    let inline createNull () : SynExpr = SynExpr.Null range0

    /// `reraise ()`
    let reraise : SynExpr = createIdent "reraise" |> applyTo (SynExpr.CreateConst ())

    /// Semantically this is:
    /// {elt1} ; {elt2} ; ...
    /// except we probably end up formatting without the semicolons.
    let sequential (exprs : SynExpr list) : SynExpr =
        exprs
        |> List.reduce (fun a b -> SynExpr.Sequential (DebugPointAtSequential.SuppressNeither, false, a, b, range0))

    /// [ {elt1} ; {elt2} ; ... ]
    let listLiteral (elts : SynExpr list) : SynExpr =
        SynExpr.ArrayOrListComputed (false, sequential elts, range0)

    /// [| {elt1} ; {elt2} ; ... |]
    let arrayLiteral (elts : SynExpr list) : SynExpr =
        SynExpr.ArrayOrListComputed (true, sequential elts, range0)

    /// {compExpr} { {lets} ; return {ret} }
    let createCompExpr (compExpr : string) (retBody : SynExpr) (lets : CompExprBinding list) : SynExpr =
        let retStatement = SynExpr.YieldOrReturn ((false, true), retBody, range0)

        let contents : SynExpr =
            (retStatement, List.rev lets)
            ||> List.fold (fun state binding ->
                match binding with
                | LetBang (lhs, rhs) ->
                    SynExpr.LetOrUseBang (
                        DebugPointAtBinding.Yes range0,
                        false,
                        true,
                        SynPat.named lhs,
                        rhs,
                        [],
                        state,
                        range0,
                        {
                            EqualsRange = Some range0
                        }
                    )
                | Let (lhs, rhs) -> createLet [ SynBinding.basic [ Ident.create lhs ] [] rhs ] state
                | Use (lhs, rhs) ->
                    SynExpr.LetOrUse (
                        false,
                        true,
                        [ SynBinding.basic [ Ident.create lhs ] [] rhs ],
                        state,
                        range0,
                        {
                            SynExprLetOrUseTrivia.InKeyword = None
                        }
                    )
                | Do body -> sequential [ SynExpr.Do (body, range0) ; state ]
            )

        applyFunction (createIdent compExpr) (SynExpr.ComputationExpr (false, contents, range0))

    /// {expr} |> Async.AwaitTask
    let awaitTask (expr : SynExpr) : SynExpr =
        expr |> pipeThroughFunction (createLongIdent [ "Async" ; "AwaitTask" ])

    /// {ident}.ToString ()
    /// with special casing for some types like DateTime
    let toString (ty : SynType) (ident : SynExpr) =
        match ty with
        | DateOnly -> ident |> callMethodArg "ToString" (SynExpr.CreateConst "yyyy-MM-dd")
        | DateTime -> ident |> callMethodArg "ToString" (SynExpr.CreateConst "yyyy-MM-ddTHH:mm:ss")
        | _ -> callMethod "ToString" ident

    /// {e} :> {ty}
    let upcast' (ty : SynType) (e : SynExpr) = SynExpr.Upcast (e, ty, range0)

    /// {ident} - {rhs}
    let minus (ident : SynLongIdent) (rhs : SynExpr) : SynExpr =
        createAppInfix (createLongIdent'' SynLongIdent.sub) (createLongIdent'' ident)
        |> applyTo rhs

    /// {ident} - {n}
    let minusN (ident : SynLongIdent) (n : int) : SynExpr = minus ident (SynExpr.CreateConst n)

    /// {y} > {x}
    let greaterThan (x : SynExpr) (y : SynExpr) : SynExpr =
        createAppInfix (createLongIdent'' SynLongIdent.gt) y |> applyTo x

    /// {y} < {x}
    let lessThan (x : SynExpr) (y : SynExpr) : SynExpr =
        createAppInfix (createLongIdent'' SynLongIdent.lt) y |> applyTo x

    /// {y} >= {x}
    let greaterThanOrEqual (x : SynExpr) (y : SynExpr) : SynExpr =
        createAppInfix (createLongIdent'' SynLongIdent.geq) y |> applyTo x

    /// {y} <= {x}
    let lessThanOrEqual (x : SynExpr) (y : SynExpr) : SynExpr =
        createAppInfix (createLongIdent'' SynLongIdent.leq) y |> applyTo x

    /// {x} :: {y}
    let listCons (x : SynExpr) (y : SynExpr) : SynExpr =
        createAppInfix
            (SynExpr.LongIdent (
                false,
                SynLongIdent.SynLongIdent (
                    [ Ident.create "op_ColonColon" ],
                    [],
                    [ Some (IdentTrivia.OriginalNotation "::") ]
                ),
                None,
                range0
            ))
            (tupleNoParen [ x ; y ])
        |> paren

    /// `{lhs} <- {rhs}`
    let assign (lhs : SynLongIdent) (rhs : SynExpr) : SynExpr = SynExpr.LongIdentSet (lhs, rhs, range0)

    /// `{lhs}.[{index}] <- {rhs}`
    let assignIndex (lhs : SynExpr) (index : SynExpr) (rhs : SynExpr) : SynExpr =
        SynExpr.DotIndexedSet (lhs, index, rhs, range0, range0, range0)

    /// { x = 3 ; y = 4 }, or { foo with x = 3 }, for example.
    let createRecord (updateFrom : SynExpr option) (fields : (SynLongIdent * SynExpr) list) : SynExpr =
        let updateFrom =
            updateFrom
            |> Option.map (fun updateFrom -> updateFrom, (range0, Some Fantomas.FCS.Text.Position.pos0))

        let fields =
            fields
            |> List.map (fun (rfn, synExpr) -> SynExprRecordField ((rfn, true), Some range0, Some synExpr, None))

        SynExpr.Record (None, updateFrom, fields, range0)
