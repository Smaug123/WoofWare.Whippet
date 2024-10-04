namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Text.Range
open Fantomas.FCS.Xml

/// Methods for manipulating SynMemberDefn, which specifies interface members and other such fields in a type declaration.
[<RequireQualifiedAccess>]
module SynMemberDefn =
    let private interfaceMemberSlotFlags =
        {
            SynMemberFlags.IsInstance = true
            SynMemberFlags.IsDispatchSlot = true
            SynMemberFlags.IsOverrideOrExplicitImpl = false
            SynMemberFlags.IsFinal = false
            SynMemberFlags.GetterOrSetterIsCompilerGenerated = false
            SynMemberFlags.MemberKind = SynMemberKind.Member
        }

    /// An `abstract Foo : blah`.
    /// You specify the shape of any arguments via the input `arity`.
    let abstractMember
        (attrs : SynAttribute list)
        (ident : SynIdent)
        (typars : SynTyparDecls option)
        (arity : SynValInfo)
        (xmlDoc : PreXmlDoc)
        (returnType : SynType)
        : SynMemberDefn
        =
        let slot =
            SynValSig.SynValSig (
                attrs
                |> List.map (fun attr ->
                    {
                        Attributes = [ attr ]
                        Range = range0
                    }
                ),
                ident,
                SynValTyparDecls.SynValTyparDecls (typars, true),
                returnType,
                arity,
                false,
                false,
                xmlDoc,
                None,
                None,
                range0,
                {
                    EqualsRange = None
                    WithKeyword = None
                    InlineKeyword = None
                    LeadingKeyword = SynLeadingKeyword.Abstract range0
                }
            )

        SynMemberDefn.AbstractSlot (
            slot,
            interfaceMemberSlotFlags,
            range0,
            {
                GetSetKeywords = None
            }
        )

    /// `static member Foo = ...`
    let staticMember (binding : SynBinding) : SynMemberDefn =
        let binding = SynBinding.makeStaticMember binding
        SynMemberDefn.Member (binding, range0)

    /// `member this.Foo = ...`
    ///
    /// You need to make sure the `this` is present in the name of the binding.
    let memberImplementation (binding : SynBinding) : SynMemberDefn =
        let binding = SynBinding.makeInstanceMember binding
        SynMemberDefn.Member (binding, range0)
