namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Methods for manipulating SynTypeDefnRepr, which represents the "body" of a type definition: what actually makes up
/// the type, as opposed to e.g. its name.
[<RequireQualifiedAccess>]
module SynTypeDefnRepr =

    /// A definition like `abstract Blah : int`, as you would use to define an interface type.
    let inline interfaceType (mems : SynMemberDefns) : SynTypeDefnRepr =
        SynTypeDefnRepr.ObjectModel (SynTypeDefnKind.Unspecified, mems, range0)

    /// Indicates the body of a `type Foo with {body}` extension type declaration.
    let inline augmentation () : SynTypeDefnRepr =
        SynTypeDefnRepr.ObjectModel (SynTypeDefnKind.Augmentation range0, [], range0)

    /// An F# discriminated union with the given accessibility modifier on the implementation:
    /// `type Foo = private | Blah`.
    let inline unionWithAccess (implAccess : SynAccess option) (cases : SynUnionCase list) : SynTypeDefnRepr =
        SynTypeDefnRepr.Simple (SynTypeDefnSimpleRepr.Union (implAccess, cases, range0), range0)

    /// An F# discriminated union.
    let inline union (cases : SynUnionCase list) : SynTypeDefnRepr = unionWithAccess None cases

    /// An F# record with the given accessibility modifier on the implementation: `type Foo = private { blah }`.
    let inline recordWithAccess (implAccess : SynAccess option) (fields : SynField list) : SynTypeDefnRepr =
        SynTypeDefnRepr.Simple (SynTypeDefnSimpleRepr.Record (implAccess, fields, range0), range0)

    /// An F# record.
    let inline record (fields : SynField list) : SynTypeDefnRepr = recordWithAccess None fields
