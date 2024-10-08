namespace ConsumePlugin

open System.Text.Json.Serialization
open WoofWare.Whippet.Plugin.Json

/// Module containing JSON serializing extension members for the InternalTypeNotExtensionSerial type
[<AutoOpen>]
module internal InternalTypeNotExtensionSerialJsonSerializeExtension =
    /// Extension methods for JSON parsing
    type InternalTypeNotExtensionSerial with

        /// Serialize to a JSON node
        static member toJsonNode (input : InternalTypeNotExtensionSerial) : System.Text.Json.Nodes.JsonNode =
            let node = System.Text.Json.Nodes.JsonObject ()

            do
                node.Add (
                    (Literals.something),
                    (input.InternalThing2 |> System.Text.Json.Nodes.JsonValue.Create<string>)
                )

            node :> _
namespace ConsumePlugin

open System.Text.Json.Serialization
open WoofWare.Whippet.Plugin.Json

/// Module containing JSON serializing extension members for the InternalTypeExtension type
[<AutoOpen>]
module internal InternalTypeExtensionJsonSerializeExtension =
    /// Extension methods for JSON parsing
    type InternalTypeExtension with

        /// Serialize to a JSON node
        static member toJsonNode (input : InternalTypeExtension) : System.Text.Json.Nodes.JsonNode =
            let node = System.Text.Json.Nodes.JsonObject ()
            do node.Add ((Literals.something), (input.ExternalThing |> System.Text.Json.Nodes.JsonValue.Create<string>))
            node :> _

namespace ConsumePlugin

/// Module containing JSON parsing extension members for the InnerType type
[<AutoOpen>]
module InnerTypeJsonParseExtension =
    /// Extension methods for JSON parsing
    type InnerType with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : InnerType =
            let arg_0 =
                (match node.[(Literals.something)] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ((Literals.something))
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                Thing = arg_0
            }
namespace ConsumePlugin

/// Module containing JSON parsing extension members for the JsonRecordType type
[<AutoOpen>]
module JsonRecordTypeJsonParseExtension =
    /// Extension methods for JSON parsing
    type JsonRecordType with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : JsonRecordType =
            let arg_5 =
                (match node.["f"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("f")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.Int32> ())
                |> Array.ofSeq

            let arg_4 =
                (match node.["e"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("e")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.String> ())
                |> Array.ofSeq

            let arg_3 =
                InnerType.jsonParse (
                    match node.["d"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("d")
                            )
                        )
                    | v -> v
                )

            let arg_2 =
                (match node.["hi"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("hi")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.Int32> ())
                |> List.ofSeq

            let arg_1 =
                (match node.["another-thing"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("another-thing")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_0 =
                (match node.["a"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("a")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            {
                A = arg_0
                B = arg_1
                C = arg_2
                D = arg_3
                E = arg_4
                F = arg_5
            }
namespace ConsumePlugin

/// Module containing JSON parsing extension members for the InternalTypeNotExtension type
[<AutoOpen>]
module internal InternalTypeNotExtensionJsonParseExtension =
    /// Extension methods for JSON parsing
    type InternalTypeNotExtension with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : InternalTypeNotExtension =
            let arg_0 =
                (match node.[(Literals.something)] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ((Literals.something))
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                InternalThing = arg_0
            }
namespace ConsumePlugin

/// Module containing JSON parsing extension members for the InternalTypeExtension type
[<AutoOpen>]
module internal InternalTypeExtensionJsonParseExtension =
    /// Extension methods for JSON parsing
    type InternalTypeExtension with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : InternalTypeExtension =
            let arg_0 =
                (match node.[(Literals.something)] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ((Literals.something))
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                ExternalThing = arg_0
            }
namespace ConsumePlugin

/// Module containing JSON parsing extension members for the ToGetExtensionMethod type
[<AutoOpen>]
module ToGetExtensionMethodJsonParseExtension =
    /// Extension methods for JSON parsing
    type ToGetExtensionMethod with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : ToGetExtensionMethod =
            let arg_20 = System.Numerics.BigInteger.Parse (node.["whiskey"].ToJsonString ())

            let arg_19 =
                (match node.["victor"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("victor")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Char> ()

            let arg_18 =
                (match node.["uniform"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("uniform")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Decimal> ()

            let arg_17 =
                (match node.["tango"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("tango")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.SByte> ()

            let arg_16 =
                (match node.["quebec"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("quebec")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Byte> ()

            let arg_15 =
                (match node.["papa"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("papa")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Byte> ()

            let arg_14 =
                (match node.["oscar"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("oscar")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.SByte> ()

            let arg_13 =
                (match node.["november"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("november")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.UInt16> ()

            let arg_12 =
                (match node.["mike"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("mike")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int16> ()

            let arg_11 =
                (match node.["lima"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lima")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.UInt32> ()

            let arg_10 =
                (match node.["kilo"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("kilo")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_9 =
                (match node.["juliette"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("juliette")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.UInt32> ()

            let arg_8 =
                (match node.["india"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("india")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_7 =
                (match node.["hotel"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("hotel")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.UInt64> ()

            let arg_6 =
                (match node.["golf"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("golf")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int64> ()

            let arg_5 =
                (match node.["foxtrot"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("foxtrot")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Double> ()

            let arg_4 =
                (match node.["echo"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("echo")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Single> ()

            let arg_3 =
                (match node.["delta"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("delta")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Single> ()

            let arg_2 =
                (match node.["charlie"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("charlie")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Double> ()

            let arg_1 =
                (match node.["bravo"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("bravo")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<string> ()
                |> System.Uri

            let arg_0 =
                (match node.["alpha"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("alpha")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                Alpha = arg_0
                Bravo = arg_1
                Charlie = arg_2
                Delta = arg_3
                Echo = arg_4
                Foxtrot = arg_5
                Golf = arg_6
                Hotel = arg_7
                India = arg_8
                Juliette = arg_9
                Kilo = arg_10
                Lima = arg_11
                Mike = arg_12
                November = arg_13
                Oscar = arg_14
                Papa = arg_15
                Quebec = arg_16
                Tango = arg_17
                Uniform = arg_18
                Victor = arg_19
                Whiskey = arg_20
            }
