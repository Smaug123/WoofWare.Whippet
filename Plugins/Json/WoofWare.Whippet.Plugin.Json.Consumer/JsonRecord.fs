namespace ConsumePlugin

open System.Text.Json.Serialization
open WoofWare.Whippet.Plugin.Json

module Literals =
    [<Literal>]
    let something = "something"

[<JsonParse>]
type InnerType =
    {
        [<JsonPropertyName(Literals.something)>]
        Thing : string
    }

/// My whatnot
[<JsonParse>]
type JsonRecordType =
    {
        /// A thing!
        A : int
        /// Another thing!
        [<JsonPropertyName "another-thing">]
        B : string
        [<System.Text.Json.Serialization.JsonPropertyName "hi">]
        C : int list
        D : InnerType
        E : string array
        F : int[]
    }

[<JsonParse>]
type internal InternalTypeNotExtension =
    {
        [<JsonPropertyName(Literals.something)>]
        InternalThing : string
    }

[<JsonSerialize>]
type internal InternalTypeNotExtensionSerial =
    {
        [<JsonPropertyName(Literals.something)>]
        InternalThing2 : string
    }

[<JsonParse true>]
[<JsonSerialize true>]
type internal InternalTypeExtension =
    {
        [<JsonPropertyName(Literals.something)>]
        ExternalThing : string
    }

[<JsonParse true>]
type ToGetExtensionMethod =
    {
        Alpha : string
        Bravo : System.Uri
        Charlie : float
        Delta : float32
        Echo : single
        Foxtrot : double
        Golf : int64
        Hotel : uint64
        India : int
        Juliette : uint
        Kilo : int32
        Lima : uint32
        Mike : int16
        November : uint16
        Oscar : int8
        Papa : uint8
        Quebec : byte
        Tango : sbyte
        Uniform : decimal
        Victor : char
        Whiskey : bigint
    }

[<RequireQualifiedAccess>]
module ToGetExtensionMethod =
    let thisModuleWouldClash = 3
