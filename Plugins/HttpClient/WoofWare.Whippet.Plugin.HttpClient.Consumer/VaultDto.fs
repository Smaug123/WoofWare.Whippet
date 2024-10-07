namespace ConsumePlugin

open System
open System.Collections.Generic
open System.Text.Json.Serialization
open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient

[<JsonParse>]
type JwtVaultAuthResponse =
    {
        [<JsonPropertyName "client_token">]
        ClientToken : string
        Accessor : string
        Policies : string list
        [<JsonPropertyName "token_policies">]
        TokenPolicies : string list
        [<JsonPropertyName "identity_policies">]
        IdentityPolicies : string list
        [<JsonPropertyName "lease_duration">]
        LeaseDuration : int
        Renewable : bool
        [<JsonPropertyName "token_type">]
        TokenType : string
        [<JsonPropertyName "entity_id">]
        EntityId : string
        Orphan : bool
        [<JsonPropertyName "num_uses">]
        NumUses : int
    }

[<JsonParse>]
type JwtVaultResponse =
    {
        [<JsonPropertyName "request_id">]
        RequestId : string
        [<JsonPropertyName "lease_id">]
        LeaseId : string
        Renewable : bool
        [<JsonPropertyName "lease_duration">]
        LeaseDuration : int
        Auth : JwtVaultAuthResponse
    }

[<JsonParse>]
type JwtSecretResponse =
    {
        [<JsonPropertyName "request_id">]
        RequestId : string
        [<JsonPropertyName "lease_id">]
        LeaseId : string
        Renewable : bool
        [<JsonPropertyName "lease_duration">]
        LeaseDuration : int
        Data : IReadOnlyDictionary<string, string>
        // These ones aren't actually part of the Vault response, but are here for tests
        Data2 : IDictionary<string, string>
        Data3 : Dictionary<string, string>
        Data4 : Map<string, string>
        Data5 : IReadOnlyDictionary<System.Uri, string>
        Data6 : IDictionary<Uri, string>
        Data7 : Map<string, int>
        Data8 : Dictionary<string, Uri>
    }
