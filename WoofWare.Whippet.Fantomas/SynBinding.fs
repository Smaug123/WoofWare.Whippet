namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Xml
open Fantomas.FCS.Text.Range

/// Methods for manipulating `SynBinding`, which represents a `let`-binding or a member definition.
[<RequireQualifiedAccess>]
module SynBinding =

    let rec private stripParen (pat : SynPat) =
        match pat with
        | SynPat.Paren (p, _) -> stripParen p
        | _ -> pat

    let rec private getName (pat : SynPat) : Ident option =
        match stripParen pat with
        | SynPat.Named (SynIdent.SynIdent (name, _), _, _, _) -> Some name
        | SynPat.Typed (pat, _, _) -> getName pat
        | SynPat.LongIdent (SynLongIdent.SynLongIdent (longIdent, _, _), _, _, _, _, _) ->
            match longIdent with
            | [ x ] -> Some x
            | _ -> failwithf "got long ident %O ; can only get the name of a long ident with one component" longIdent
        | _ -> None

    let private getArgInfo (pat : SynPat) : SynArgInfo list =
        // TODO: this only copes with one layer of tupling
        match stripParen pat with
        | SynPat.Tuple (_, pats, _, _) -> pats |> List.map (fun pat -> SynArgInfo.SynArgInfo ([], false, getName pat))
        | pat -> [ SynArgInfo.SynArgInfo (SynAttributes.Empty, false, getName pat) ]

    /// The basic `SynBindingTrivia` which means "there's nothing special about this binding".
    /// You tell us whether this is a `member Foo = ...` versus a `let foo = ...`.
    let triviaZero (isMember : bool) : SynBindingTrivia =
        {
            SynBindingTrivia.EqualsRange = Some range0
            InlineKeyword = None
            LeadingKeyword =
                if isMember then
                    SynLeadingKeyword.Member range0
                else
                    SynLeadingKeyword.Let range0
        }

    /// A simple binding:
    /// `let {name} {args} = {body}`
    ///
    /// If you want this to become an instance member, you need to make sure the `this.` is present as a component
    /// of the `name`.
    let basic (name : LongIdent) (args : SynPat list) (body : SynExpr) : SynBinding =
        let valInfo : SynValInfo =
            args
            |> List.map getArgInfo
            |> fun x -> SynValInfo.SynValInfo (x, SynArgInfo.SynArgInfo ([], false, None))

        SynBinding.SynBinding (
            None,
            SynBindingKind.Normal,
            false,
            false,
            [],
            PreXmlDoc.Empty,
            SynValData.SynValData (None, valInfo, None),
            SynPat.identWithArgs name (SynArgPats.Pats args),
            None,
            body,
            range0,
            DebugPointAtBinding.Yes range0,
            triviaZero false
        )

    /// Set the mutability of this binding: `let mutable i = ...` (or remove the word `mutable`, if `mut` is false).
    let withMutability (mut : bool) (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (pat, kind, inl, _, attrs, xml, valData, headPat, returnInfo, expr, range, debugPoint, trivia) ->
            SynBinding (pat, kind, inl, mut, attrs, xml, valData, headPat, returnInfo, expr, range, debugPoint, trivia)

    /// Set the `rec` keyword on this binding: `let rec foo = ...` (or remove the word `rec`, if `isRec` is false).
    let withRecursion (isRec : bool) (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (pat, kind, inl, mut, attrs, xml, valData, headPat, returnInfo, expr, range, debugPoint, trivia) ->
            let trivia =
                { trivia with
                    LeadingKeyword =
                        match trivia.LeadingKeyword with
                        | SynLeadingKeyword.Let _ ->
                            if isRec then
                                SynLeadingKeyword.LetRec (range0, range0)
                            else
                                trivia.LeadingKeyword
                        | SynLeadingKeyword.LetRec _ ->
                            if isRec then
                                trivia.LeadingKeyword
                            else
                                trivia.LeadingKeyword
                        | existing ->
                            failwith
                                $"WoofWare.Whippet.Fantomas doesn't yet let you adjust the recursion modifier on a binding with modifier %O{existing}"
                }

            SynBinding (pat, kind, inl, mut, attrs, xml, valData, headPat, returnInfo, expr, range, debugPoint, trivia)

    /// Override the accessibility modifier on this binding, or clear it: `let private foo = ...`
    let withAccessibility (acc : SynAccess option) (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (_, kind, inl, mut, attrs, xml, valData, headPat, returnInfo, expr, range, debugPoint, trivia) ->
            let headPat =
                match headPat with
                | SynPat.LongIdent (ident, extra, options, argPats, _, range) ->
                    SynPat.LongIdent (ident, extra, options, argPats, acc, range)
                | _ -> failwithf "unrecognised head pattern: %O" headPat

            SynBinding (acc, kind, inl, mut, attrs, xml, valData, headPat, returnInfo, expr, range, debugPoint, trivia)

    /// Set the XML docstring on this binding: `/// blah\nlet foo = ...`
    let withXmlDoc (doc : PreXmlDoc) (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (acc, kind, inl, mut, attrs, _, valData, headPat, returnInfo, expr, range, debugPoint, trivia) ->
            SynBinding (acc, kind, inl, mut, attrs, doc, valData, headPat, returnInfo, expr, range, debugPoint, trivia)

    /// Set the return type annotation: `let foo : int = ...`
    let withReturnAnnotation (ty : SynType) (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (acc, kind, inl, mut, attrs, doc, valData, headPat, _, expr, range, debugPoint, trivia) ->
            let retInfo =
                SynBindingReturnInfo.SynBindingReturnInfo (
                    ty,
                    range0,
                    [],
                    {
                        ColonRange = Some range0
                    }
                )

            SynBinding (
                acc,
                kind,
                inl,
                mut,
                attrs,
                doc,
                valData,
                headPat,
                Some retInfo,
                expr,
                range,
                debugPoint,
                trivia
            )

    /// Make the definition an `inline` definition: `let inline foo = ...`
    let inline makeInline (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (acc, kind, _, mut, attrs, doc, valData, headPat, ret, expr, range, debugPoint, trivia) ->
            SynBinding (
                acc,
                kind,
                true,
                mut,
                attrs,
                doc,
                valData,
                headPat,
                ret,
                expr,
                range,
                debugPoint,
                { trivia with
                    InlineKeyword = Some range0
                }
            )

    /// Make the definition not be an `inline` definition: that is, turn `let inline foo = ...` into `let foo = ...`.
    /// This is a no-op if the binding is already not inline.
    let inline makeNotInline (binding : SynBinding) : SynBinding =
        match binding with
        | SynBinding (acc, kind, _, mut, attrs, doc, valData, headPat, ret, expr, range, debugPoint, trivia) ->
            SynBinding (
                acc,
                kind,
                false,
                mut,
                attrs,
                doc,
                valData,
                headPat,
                ret,
                expr,
                range,
                debugPoint,
                { trivia with
                    InlineKeyword = None
                }
            )

    /// Set or remove the `inline` keyword on the given binding.
    let inline setInline (isInline : bool) (binding : SynBinding) : SynBinding =
        if isInline then
            makeInline binding
        else
            makeNotInline binding

    /// Convert this member definition to a `static` member.
    let makeStaticMember (binding : SynBinding) : SynBinding =
        let memberFlags =
            {
                SynMemberFlags.IsInstance = false
                SynMemberFlags.IsDispatchSlot = false
                SynMemberFlags.IsOverrideOrExplicitImpl = false
                SynMemberFlags.IsFinal = false
                SynMemberFlags.GetterOrSetterIsCompilerGenerated = false
                SynMemberFlags.MemberKind = SynMemberKind.Member
            }

        match binding with
        | SynBinding (acc, kind, inl, mut, attrs, doc, valData, headPat, ret, expr, range, debugPoint, trivia) ->
            let valData =
                match valData with
                | SynValData.SynValData (_, valInfo, _) -> SynValData.SynValData (Some memberFlags, valInfo, None)

            let trivia =
                { trivia with
                    LeadingKeyword = SynLeadingKeyword.StaticMember (range0, range0)
                }

            SynBinding (acc, kind, inl, mut, attrs, doc, valData, headPat, ret, expr, range, debugPoint, trivia)

    /// Convert this member definition to an instance member, `member this.Foo = ...`.
    /// You need to make sure the `this` is present in the name of the binding.
    let makeInstanceMember (binding : SynBinding) : SynBinding =
        let memberFlags =
            {
                SynMemberFlags.IsInstance = true
                SynMemberFlags.IsDispatchSlot = false
                SynMemberFlags.IsOverrideOrExplicitImpl = true
                SynMemberFlags.IsFinal = false
                SynMemberFlags.GetterOrSetterIsCompilerGenerated = false
                SynMemberFlags.MemberKind = SynMemberKind.Member
            }

        match binding with
        | SynBinding (acc, kind, inl, mut, attrs, doc, valData, headPat, ret, expr, range, debugPoint, trivia) ->
            let valData =
                match valData with
                | SynValData.SynValData (_, valInfo, _) -> SynValData.SynValData (Some memberFlags, valInfo, None)

            let trivia =
                { trivia with
                    LeadingKeyword = SynLeadingKeyword.Member range0
                }

            SynBinding (acc, kind, inl, mut, attrs, doc, valData, headPat, ret, expr, range, debugPoint, trivia)
