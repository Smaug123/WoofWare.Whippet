namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.SyntaxTrivia
open Fantomas.FCS.Text.Range

/// Methods for manipulating `SynTypeDefn`, which represents any type definition like `type Foo = ...`.
[<RequireQualifiedAccess>]
module SynTypeDefn =

    /// Build a `SynTypeDefn` from its components:
    /// the "front matter" `SynComponentInfo`, and the "body" `SynTypeDefnRepr`.
    let inline create (componentInfo : SynComponentInfo) (repr : SynTypeDefnRepr) : SynTypeDefn =
        SynTypeDefn.SynTypeDefn (
            componentInfo,
            repr,
            [],
            None,
            range0,
            {
                LeadingKeyword = SynTypeDefnLeadingKeyword.Type range0
                EqualsRange = Some range0
                WithKeyword = None
            }
        )

    /// Add member definitions to this type: `type Foo = ... with member Blah = ...`
    let inline withMemberDefns (members : SynMemberDefn list) (r : SynTypeDefn) : SynTypeDefn =
        match r with
        | SynTypeDefn (typeInfo, typeRepr, _, ctor, range, trivia) ->
            SynTypeDefn.SynTypeDefn (typeInfo, typeRepr, members, ctor, range, trivia)

    /// Get the name of this type as it appears in the source.
    let getName (defn : SynTypeDefn) : LongIdent =
        match defn with
        | SynTypeDefn (SynComponentInfo.SynComponentInfo (_, _, _, id, _, _, _, _), _, _, _, _, _) -> id

    /// Select from this type definition the first attribute with the given name: `[<Foo>] type Blah = ...`
    let getAttribute (attrName : string) (defn : SynTypeDefn) : SynAttribute option =
        match defn with
        | SynTypeDefn (SynComponentInfo.SynComponentInfo (attrs, _, _, _, _, _, _, _), _, _, _, _, _) ->
            attrs
            |> List.collect (fun a -> a.Attributes)
            |> List.tryFind (fun i ->
                match i.TypeName with
                | SynLongIdent.SynLongIdent (id, _, _) ->
                    let name = List.last(id).idText
                    name = attrName || name + "Attribute" = attrName
            )

    /// Determine whether this type definition has an attribute with the given name: `[<Foo>] type Blah = ...`
    let hasAttribute (attrName : string) (defn : SynTypeDefn) : bool =
        getAttribute attrName defn |> Option.isSome
