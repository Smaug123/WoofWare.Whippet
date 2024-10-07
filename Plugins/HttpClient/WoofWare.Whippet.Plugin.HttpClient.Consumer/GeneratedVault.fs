namespace ConsumePlugin

open System
open System.Collections.Generic
open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module VaultClientHttpClientExtension =
    /// Extension methods for HTTP clients
    type IVaultClient with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IVaultClient =
            { new IVaultClient with
                member _.GetSecret
                    (jwt : JwtVaultResponse, path : string, mountPoint : string, ct : CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null ->
                                     raise (
                                         System.ArgumentNullException (
                                             nameof (client.BaseAddress),
                                             "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                         )
                                     )
                                 | v -> v),
                                System.Uri (
                                    "v1/{mountPoint}/{path}"
                                        .Replace("{path}", path.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{mountPoint}", mountPoint.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return JwtSecretResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetJwt (role : string, jwt : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null ->
                                     raise (
                                         System.ArgumentNullException (
                                             nameof (client.BaseAddress),
                                             "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                         )
                                     )
                                 | v -> v),
                                System.Uri ("v1/auth/jwt/login", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return JwtVaultResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
            }
namespace ConsumePlugin

open System
open System.Collections.Generic
open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Module for constructing a REST client.
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix) ; RequireQualifiedAccess>]
module VaultClientNonExtensionMethod =
    /// Create a REST client.
    let make (client : System.Net.Http.HttpClient) : IVaultClientNonExtensionMethod =
        { new IVaultClientNonExtensionMethod with
            member _.GetSecret
                (jwt : JwtVaultResponse, path : string, mountPoint : string, ct : CancellationToken option)
                =
                async {
                    let! ct = Async.CancellationToken

                    let uri =
                        System.Uri (
                            (match client.BaseAddress with
                             | null ->
                                 raise (
                                     System.ArgumentNullException (
                                         nameof (client.BaseAddress),
                                         "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                     )
                                 )
                             | v -> v),
                            System.Uri (
                                "v1/{mountPoint}/{path}"
                                    .Replace("{path}", path.ToString () |> System.Uri.EscapeDataString)
                                    .Replace ("{mountPoint}", mountPoint.ToString () |> System.Uri.EscapeDataString),
                                System.UriKind.Relative
                            )
                        )

                    let httpMessage =
                        new System.Net.Http.HttpRequestMessage (
                            Method = System.Net.Http.HttpMethod.Get,
                            RequestUri = uri
                        )

                    let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                    let response = response.EnsureSuccessStatusCode ()
                    let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                    let! jsonNode =
                        System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                        |> Async.AwaitTask

                    return JwtSecretResponse.jsonParse jsonNode
                }
                |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

            member _.GetJwt (role : string, jwt : string, ct : CancellationToken option) =
                async {
                    let! ct = Async.CancellationToken

                    let uri =
                        System.Uri (
                            (match client.BaseAddress with
                             | null ->
                                 raise (
                                     System.ArgumentNullException (
                                         nameof (client.BaseAddress),
                                         "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                     )
                                 )
                             | v -> v),
                            System.Uri ("v1/auth/jwt/login", System.UriKind.Relative)
                        )

                    let httpMessage =
                        new System.Net.Http.HttpRequestMessage (
                            Method = System.Net.Http.HttpMethod.Get,
                            RequestUri = uri
                        )

                    let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                    let response = response.EnsureSuccessStatusCode ()
                    let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                    let! jsonNode =
                        System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                        |> Async.AwaitTask

                    return JwtVaultResponse.jsonParse jsonNode
                }
                |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
        }
namespace ConsumePlugin

open System
open System.Collections.Generic
open System.Text.Json.Serialization
open System.Threading
open System.Threading.Tasks
open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module VaultClientExtensionMethodHttpClientExtension =
    /// Extension methods for HTTP clients
    type IVaultClientExtensionMethod with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IVaultClientExtensionMethod =
            { new IVaultClientExtensionMethod with
                member _.GetSecret
                    (jwt : JwtVaultResponse, path : string, mountPoint : string, ct : CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null ->
                                     raise (
                                         System.ArgumentNullException (
                                             nameof (client.BaseAddress),
                                             "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                         )
                                     )
                                 | v -> v),
                                System.Uri (
                                    "v1/{mountPoint}/{path}"
                                        .Replace("{path}", path.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{mountPoint}", mountPoint.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return JwtSecretResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetJwt (role : string, jwt : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null ->
                                     raise (
                                         System.ArgumentNullException (
                                             nameof (client.BaseAddress),
                                             "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                         )
                                     )
                                 | v -> v),
                                System.Uri ("v1/auth/jwt/login", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return JwtVaultResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
            }
