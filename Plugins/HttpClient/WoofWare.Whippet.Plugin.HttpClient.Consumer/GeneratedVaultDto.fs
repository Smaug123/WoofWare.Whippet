namespace ConsumePlugin

/// Module containing JSON parsing extension members for the JwtVaultAuthResponse type
[<AutoOpen>]
module JwtVaultAuthResponseJsonParseExtension =
    /// Extension methods for JSON parsing
    type JwtVaultAuthResponse with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : JwtVaultAuthResponse =
            let arg_10 =
                (match node.["num_uses"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("num_uses")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_9 =
                (match node.["orphan"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("orphan")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_8 =
                (match node.["entity_id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("entity_id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_7 =
                (match node.["token_type"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("token_type")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_6 =
                (match node.["renewable"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("renewable")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_5 =
                (match node.["lease_duration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lease_duration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_4 =
                (match node.["identity_policies"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("identity_policies")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.String> ())
                |> List.ofSeq

            let arg_3 =
                (match node.["token_policies"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("token_policies")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.String> ())
                |> List.ofSeq

            let arg_2 =
                (match node.["policies"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("policies")
                         )
                     )
                 | v -> v)
                    .AsArray ()
                |> Seq.map (fun elt -> elt.AsValue().GetValue<System.String> ())
                |> List.ofSeq

            let arg_1 =
                (match node.["accessor"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("accessor")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_0 =
                (match node.["client_token"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("client_token")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                ClientToken = arg_0
                Accessor = arg_1
                Policies = arg_2
                TokenPolicies = arg_3
                IdentityPolicies = arg_4
                LeaseDuration = arg_5
                Renewable = arg_6
                TokenType = arg_7
                EntityId = arg_8
                Orphan = arg_9
                NumUses = arg_10
            }
namespace ConsumePlugin

/// Module containing JSON parsing extension members for the JwtVaultResponse type
[<AutoOpen>]
module JwtVaultResponseJsonParseExtension =
    /// Extension methods for JSON parsing
    type JwtVaultResponse with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : JwtVaultResponse =
            let arg_4 =
                JwtVaultAuthResponse.jsonParse (
                    match node.["auth"] with
                    | null ->
                        raise (
                            System.Collections.Generic.KeyNotFoundException (
                                sprintf "Required key '%s' not found on JSON object" ("auth")
                            )
                        )
                    | v -> v
                )

            let arg_3 =
                (match node.["lease_duration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lease_duration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_2 =
                (match node.["renewable"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("renewable")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_1 =
                (match node.["lease_id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lease_id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_0 =
                (match node.["request_id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("request_id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                RequestId = arg_0
                LeaseId = arg_1
                Renewable = arg_2
                LeaseDuration = arg_3
                Auth = arg_4
            }
namespace ConsumePlugin

/// Module containing JSON parsing extension members for the JwtSecretResponse type
[<AutoOpen>]
module JwtSecretResponseJsonParseExtension =
    /// Extension methods for JSON parsing
    type JwtSecretResponse with

        /// Parse from a JSON node.
        static member jsonParse (node : System.Text.Json.Nodes.JsonNode) : JwtSecretResponse =
            let arg_11 =
                (match node.["data8"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data8")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key)
                    let value = (kvp.Value).AsValue().GetValue<string> () |> System.Uri
                    key, value
                )
                |> Seq.map System.Collections.Generic.KeyValuePair
                |> System.Collections.Generic.Dictionary

            let arg_10 =
                (match node.["data7"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data7")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key)
                    let value = (kvp.Value).AsValue().GetValue<System.Int32> ()
                    key, value
                )
                |> Map.ofSeq

            let arg_9 =
                (match node.["data6"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data6")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key) |> System.Uri
                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                    key, value
                )
                |> dict

            let arg_8 =
                (match node.["data5"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data5")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key) |> System.Uri
                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                    key, value
                )
                |> readOnlyDict

            let arg_7 =
                (match node.["data4"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data4")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key)
                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                    key, value
                )
                |> Map.ofSeq

            let arg_6 =
                (match node.["data3"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data3")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key)
                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                    key, value
                )
                |> Seq.map System.Collections.Generic.KeyValuePair
                |> System.Collections.Generic.Dictionary

            let arg_5 =
                (match node.["data2"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data2")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key)
                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                    key, value
                )
                |> dict

            let arg_4 =
                (match node.["data"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("data")
                         )
                     )
                 | v -> v)
                    .AsObject ()
                |> Seq.map (fun kvp ->
                    let key = (kvp.Key)
                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                    key, value
                )
                |> readOnlyDict

            let arg_3 =
                (match node.["lease_duration"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lease_duration")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Int32> ()

            let arg_2 =
                (match node.["renewable"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("renewable")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.Boolean> ()

            let arg_1 =
                (match node.["lease_id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("lease_id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            let arg_0 =
                (match node.["request_id"] with
                 | null ->
                     raise (
                         System.Collections.Generic.KeyNotFoundException (
                             sprintf "Required key '%s' not found on JSON object" ("request_id")
                         )
                     )
                 | v -> v)
                    .AsValue()
                    .GetValue<System.String> ()

            {
                RequestId = arg_0
                LeaseId = arg_1
                Renewable = arg_2
                LeaseDuration = arg_3
                Data = arg_4
                Data2 = arg_5
                Data3 = arg_6
                Data4 = arg_7
                Data5 = arg_8
                Data6 = arg_9
                Data7 = arg_10
                Data8 = arg_11
            }
