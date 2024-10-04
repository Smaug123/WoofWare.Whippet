namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Xml
open Fantomas.FCS.Text.Range

/// Methods for manipulating `SynComponentInfo`, which is like the "front matter" for a type definition.
/// It specifies type parameters, docstrings, name, etc.
[<RequireQualifiedAccess>]
module SynComponentInfo =
    /// Create the most basic possible SynComponentInfo, specifying only its name.
    let inline createLong (name : LongIdent) =
        SynComponentInfo.SynComponentInfo ([], None, [], name, PreXmlDoc.Empty, false, None, range0)

    /// Create the most basic possible SynComponentInfo, specifying only its name.
    /// (Don't put full stops in this `name`; use `createLong` if you want to qualify the name further.)
    let inline create (name : Ident) = createLong [ name ]

    /// Add a docstring to this type definition front matter.
    let inline withDocString (doc : PreXmlDoc) (i : SynComponentInfo) : SynComponentInfo =
        match i with
        | SynComponentInfo.SynComponentInfo (attrs, typars, constraints, name, _, postfix, access, range) ->
            SynComponentInfo (attrs, typars, constraints, name, doc, postfix, access, range)

    /// Specify that this type definition has generic type parameters.
    /// This is the fully general method; you might find it more ergonomic to use the more specialised `withGenerics`
    /// instead.
    let inline setGenerics (typars : SynTyparDecls option) (i : SynComponentInfo) : SynComponentInfo =
        match i with
        | SynComponentInfo.SynComponentInfo (attrs, _, constraints, name, doc, postfix, access, range) ->
            SynComponentInfo (attrs, typars, constraints, name, doc, postfix, access, range)

    /// Specify that this type definition has these generic type parameters.
    let inline withGenerics (typars : SynTyparDecl list) (i : SynComponentInfo) : SynComponentInfo =
        let inner =
            if typars.IsEmpty then
                None
            else
                Some (SynTyparDecls.PostfixList (typars, [], range0))

        setGenerics inner i

    /// Specify that this type definition has this accessibility modifier (or no accessibility modifier).
    let inline setAccessibility (acc : SynAccess option) (i : SynComponentInfo) : SynComponentInfo =
        match i with
        | SynComponentInfo.SynComponentInfo (attrs, typars, constraints, name, doc, postfix, _, range) ->
            SynComponentInfo.SynComponentInfo (attrs, typars, constraints, name, doc, postfix, acc, range)

    /// Specify that this type definition has this accessibility modifier.
    let inline withAccessibility (acc : SynAccess) (i : SynComponentInfo) : SynComponentInfo =
        setAccessibility (Some acc) i

    /// Prepend these attributes to the type definition: `[<Foo>] type Blah = ...`.
    let inline addAttributes (attrs : SynAttribute list) (i : SynComponentInfo) : SynComponentInfo =
        match i with
        | SynComponentInfo.SynComponentInfo (oldAttrs, typars, constraints, name, doc, postfix, acc, range) ->
            let attrs =
                {
                    SynAttributeList.Attributes = attrs
                    SynAttributeList.Range = range0
                }

            SynComponentInfo.SynComponentInfo ((attrs :: oldAttrs), typars, constraints, name, doc, postfix, acc, range)
