namespace WoofWare.Whippet.Fantomas

open Fantomas.FCS.Syntax
open Fantomas.FCS.Text.Range

/// Module for manipulating the AST `SynAttributes` type. This represents a collection of `[<FooAttribute>]` objects:
/// if you have `[<A>] [<B ; C>] foo` then the SynAttributes contains two elements, `[A]` and `[B ; C]`, each of which
/// is a list of attributes.
[<RequireQualifiedAccess>]
module SynAttributes =
    /// Build a SynAttributes of the form `[<A>] [<B>] [<C>]`, i.e. one `[<>]` for each input attribute.
    let ofAttrs (attrs : SynAttribute list) : SynAttributes =
        attrs
        |> List.map (fun a ->
            {
                Attributes = [ a ]
                Range = range0
            }
        )

    /// Get all the attributes out of this SynAttributes, as a list of individual attributes.
    let toAttrs (attrs : SynAttributes) : SynAttribute list =
        attrs |> List.collect (fun attr -> attr.Attributes)
