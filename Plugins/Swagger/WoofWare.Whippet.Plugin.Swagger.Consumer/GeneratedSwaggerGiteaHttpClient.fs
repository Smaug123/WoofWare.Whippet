namespace Gitea

open WoofWare.Whippet.Plugin.Json
open WoofWare.Whippet.Plugin.HttpClient

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module GiteaHttpClientExtension =
    /// Extension methods for HTTP clients
    type IGitea with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IGitea =
            { new IGitea with
                member _.ActivitypubPerson (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "activitypub/user/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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

                        return ActivityPub.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.ActivitypubPersonInbox (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "activitypub/user/{username}/inbox"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCronList (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("admin/cron"
                                     + (if "admin/cron".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Cron.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCronRun (task : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/cron/{task}"
                                        .Replace ("{task}", task.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminListHooks (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("admin/hooks"
                                     + (if "admin/hooks".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Hook.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCreateHook (body : CreateHookOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("admin/hooks", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminGetHook (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/hooks/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminEditHook
                    (id : int, body : EditHookOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/hooks/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminGetAllOrgs (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("admin/orgs"
                                     + (if "admin/orgs".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Organization.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminUnadoptedList
                    (page : int, limit : int, pattern : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("admin/unadopted"
                                     + (if "admin/unadopted".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&pattern="
                                     + ((pattern.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> elt.AsValue().GetValue<System.String> ())
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminAdoptRepository
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/unadopted/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminDeleteUnadoptedRepository
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/unadopted/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminGetAllUsers (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("admin/users"
                                     + (if "admin/users".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCreateUser (body : CreateUserOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("admin/users", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateUserOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return User.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminDeleteUser
                    (username : string, purge : bool, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("admin/users/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "admin/users/{username}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "purge="
                                     + ((purge.ToString ()) |> System.Uri.EscapeDataString)),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminEditUser
                    (username : string, body : EditUserOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/users/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditUserOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return User.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCreatePublicKey
                    (username : string, key : CreateKeyOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/users/{username}/keys"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                key
                                |> CreateKeyOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PublicKey.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminDeleteUserPublicKey
                    (username : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/users/{username}/keys/{id}"
                                        .Replace("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCreateOrg
                    (username : string, organization : CreateOrgOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/users/{username}/orgs"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                organization
                                |> CreateOrgOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Organization.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminCreateRepo
                    (username : string, repository : CreateRepoOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "admin/users/{username}/repos"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                repository
                                |> CreateRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AdminDeleteHook (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "amdin/hooks/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RenderMarkdown (body : MarkdownOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("markdown", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> MarkdownOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "text/html"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RenderMarkdownRaw (body : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("markdown/raw", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams = new System.Net.Http.StringContent (body, null, "text/html")
                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetNodeInfo (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("nodeinfo", System.UriKind.Relative)
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

                        return NodeInfo.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyGetList
                    (
                        all : bool,
                        status_types : string list,
                        subject_type : string list,
                        since : string,
                        before : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("notifications"
                                     + (if "notifications".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "all="
                                     + ((all.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&status-types="
                                     + ((status_types.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&subject-type="
                                     + ((subject_type.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> NotificationThread.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyReadList
                    (
                        last_read_at : string,
                        all : string,
                        status_types : string list,
                        to_status : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("notifications"
                                     + (if "notifications".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "last_read_at="
                                     + ((last_read_at.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&all="
                                     + ((all.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&status-types="
                                     + ((status_types.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&to-status="
                                     + ((to_status.ToString ()) |> System.Uri.EscapeDataString)),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> NotificationThread.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyNewAvailable (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("notifications/new", System.UriKind.Relative)
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

                        return NotificationCount.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyGetThread (id : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "notifications/threads/{id}"
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return NotificationThread.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyReadThread
                    (id : string, to_status : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("notifications/threads/{id}"
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                     + (if "notifications/threads/{id}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "to-status="
                                     + ((to_status.ToString ()) |> System.Uri.EscapeDataString)),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return NotificationThread.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateOrgRepoDeprecated
                    (org : string, body : CreateRepoOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "org/{org}/repos".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgGetAll (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs"
                                     + (if "orgs".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Organization.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgCreate (organization : CreateOrgOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("orgs", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                organization
                                |> CreateOrgOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Organization.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgGet (org : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
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

                        return Organization.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgDelete (org : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgEdit (org : string, body : EditOrgOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditOrgOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Organization.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListHooks
                    (org : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/hooks"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/hooks".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Hook.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgCreateHook
                    (org : string, body : CreateHookOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/hooks".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgGetHook (org : string, id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/hooks/{id}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgDeleteHook (org : string, id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/hooks/{id}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgEditHook
                    (org : string, id : int, body : EditHookOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/hooks/{id}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListLabels
                    (org : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/labels"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/labels".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Label.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgCreateLabel
                    (org : string, body : CreateLabelOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/labels"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateLabelOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Label.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgGetLabel (org : string, id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/labels/{id}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Label.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgDeleteLabel (org : string, id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/labels/{id}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgEditLabel
                    (org : string, id : int, body : EditLabelOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/labels/{id}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditLabelOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Label.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListMembers
                    (org : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/members"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/members".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgIsMember (org : string, username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/members/{username}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgDeleteMember
                    (org : string, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/members/{username}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListPublicMembers
                    (org : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/public_members"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/public_members".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgIsPublicMember
                    (org : string, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/public_members/{username}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgConcealMember
                    (org : string, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/public_members/{username}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgPublicizeMember
                    (org : string, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/public_members/{username}"
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListRepos
                    (org : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/repos"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/repos".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateOrgRepo
                    (org : string, body : CreateRepoOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/repos".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListTeams
                    (org : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/teams"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/teams".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Team.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgCreateTeam
                    (org : string, body : CreateTeamOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "orgs/{org}/teams".Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateTeamOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Team.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.TeamSearch
                    (
                        org : string,
                        q : string,
                        include_desc : bool,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("orgs/{org}/teams/search"
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                     + (if "orgs/{org}/teams/search".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&include_desc="
                                     + ((include_desc.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.ListPackages
                    (
                        owner : string,
                        page : int,
                        limit : int,
                        type' : string,
                        q : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("packages/{owner}"
                                        .Replace ("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                     + (if "packages/{owner}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&type="
                                     + ((type'.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Package.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetPackage
                    (
                        owner : string,
                        type' : string,
                        name : string,
                        version : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "packages/{owner}/{type}/{name}/{version}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{type}", type'.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{name}", name.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{version}", version.ToString () |> System.Uri.EscapeDataString),
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

                        return Package.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.DeletePackage
                    (
                        owner : string,
                        type' : string,
                        name : string,
                        version : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "packages/{owner}/{type}/{name}/{version}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{type}", type'.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{name}", name.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{version}", version.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.ListPackageFiles
                    (
                        owner : string,
                        type' : string,
                        name : string,
                        version : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "packages/{owner}/{type}/{name}/{version}/files"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{type}", type'.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{name}", name.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{version}", version.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PackageFile.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueSearchIssues
                    (
                        state : string,
                        labels : string,
                        milestones : string,
                        q : string,
                        priority_repo_id : int,
                        type' : string,
                        since : string,
                        before : string,
                        assigned : bool,
                        created : bool,
                        mentioned : bool,
                        review_requested : bool,
                        owner : string,
                        team : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/issues/search"
                                     + (if "repos/issues/search".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "state="
                                     + ((state.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&labels="
                                     + ((labels.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&milestones="
                                     + ((milestones.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&priority_repo_id="
                                     + ((priority_repo_id.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&type="
                                     + ((type'.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&assigned="
                                     + ((assigned.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&created="
                                     + ((created.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&mentioned="
                                     + ((mentioned.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&review_requested="
                                     + ((review_requested.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&owner="
                                     + ((owner.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&team="
                                     + ((team.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Issue.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoMigrate (body : MigrateRepoOptions, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("repos/migrate", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> MigrateRepoOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoSearch
                    (
                        q : string,
                        topic : bool,
                        includeDesc : bool,
                        uid : int,
                        priority_owner_id : int,
                        team_id : int,
                        starredBy : int,
                        private' : bool,
                        is_private : bool,
                        template : bool,
                        archived : bool,
                        mode : string,
                        exclusive : bool,
                        sort : string,
                        order : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/search"
                                     + (if "repos/search".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&topic="
                                     + ((topic.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&includeDesc="
                                     + ((includeDesc.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&uid="
                                     + ((uid.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&priority_owner_id="
                                     + ((priority_owner_id.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&team_id="
                                     + ((team_id.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&starredBy="
                                     + ((starredBy.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&private="
                                     + ((private'.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&is_private="
                                     + ((is_private.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&template="
                                     + ((template.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&archived="
                                     + ((archived.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&mode="
                                     + ((mode.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&exclusive="
                                     + ((exclusive.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&sort="
                                     + ((sort.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&order="
                                     + ((order.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return SearchResults.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGet (owner : string, repo : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDelete (owner : string, repo : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEdit
                    (
                        owner : string,
                        repo : string,
                        body : EditRepoOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetArchive
                    (owner : string, repo : string, archive : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/archive/{archive}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{archive}", archive.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetAssignees
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/assignees"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListBranchProtection
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branch_protections"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> BranchProtection.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateBranchProtection
                    (
                        owner : string,
                        repo : string,
                        body : CreateBranchProtectionOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branch_protections"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateBranchProtectionOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return BranchProtection.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetBranchProtection
                    (owner : string, repo : string, name : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branch_protections/{name}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{name}", name.ToString () |> System.Uri.EscapeDataString),
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

                        return BranchProtection.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteBranchProtection
                    (owner : string, repo : string, name : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branch_protections/{name}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{name}", name.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditBranchProtection
                    (
                        owner : string,
                        repo : string,
                        name : string,
                        body : EditBranchProtectionOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branch_protections/{name}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{name}", name.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditBranchProtectionOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return BranchProtection.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListBranches
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/branches"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/branches".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Branch.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateBranch
                    (
                        owner : string,
                        repo : string,
                        body : CreateBranchRepoOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branches"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateBranchRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Branch.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetBranch
                    (owner : string, repo : string, branch : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branches/{branch}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{branch}", branch.ToString () |> System.Uri.EscapeDataString),
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

                        return Branch.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteBranch
                    (owner : string, repo : string, branch : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/branches/{branch}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{branch}", branch.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListCollaborators
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/collaborators"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/collaborators".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCheckCollaborator
                    (
                        owner : string,
                        repo : string,
                        collaborator : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/collaborators/{collaborator}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{collaborator}",
                                            collaborator.ToString () |> System.Uri.EscapeDataString
                                        ),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteCollaborator
                    (
                        owner : string,
                        repo : string,
                        collaborator : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/collaborators/{collaborator}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{collaborator}",
                                            collaborator.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoAddCollaborator
                    (
                        owner : string,
                        repo : string,
                        collaborator : string,
                        body : AddCollaboratorOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/collaborators/{collaborator}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{collaborator}",
                                            collaborator.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> AddCollaboratorOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetRepoPermissions
                    (
                        owner : string,
                        repo : string,
                        collaborator : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/collaborators/{collaborator}/permission"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{collaborator}",
                                            collaborator.ToString () |> System.Uri.EscapeDataString
                                        ),
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

                        return RepoCollaboratorPermission.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetAllCommits
                    (
                        owner : string,
                        repo : string,
                        sha : string,
                        path : string,
                        stat : bool,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/commits"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/commits".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "sha="
                                     + ((sha.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&path="
                                     + ((path.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&stat="
                                     + ((stat.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Commit.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetCombinedStatusByRef
                    (
                        owner : string,
                        repo : string,
                        ref : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/commits/{ref}/status"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{ref}", ref.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/commits/{ref}/status".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return CombinedStatus.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListStatusesByRef
                    (
                        owner : string,
                        repo : string,
                        ref : string,
                        sort : string,
                        state : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/commits/{ref}/statuses"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{ref}", ref.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/commits/{ref}/statuses".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "sort="
                                     + ((sort.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&state="
                                     + ((state.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> CommitStatus.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetContentsList
                    (owner : string, repo : string, ref : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/contents"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/contents".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "ref="
                                     + ((ref.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> ContentsResponse.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetContents
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        ref : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/contents/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/contents/{filepath}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "ref="
                                     + ((ref.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return ContentsResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateFile
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        body : CreateFileOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/contents/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateFileOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return FileResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteFile
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        body : DeleteFileOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/contents/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> DeleteFileOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return FileDeleteResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoUpdateFile
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        body : UpdateFileOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/contents/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> UpdateFileOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return FileResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoApplyDiffPatch
                    (
                        owner : string,
                        repo : string,
                        body : UpdateFileOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/diffpatch"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> UpdateFileOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return FileResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetEditorConfig
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        ref : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/editorconfig/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/editorconfig/{filepath}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "ref="
                                     + ((ref.ToString ()) |> System.Uri.EscapeDataString)),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.ListForks
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/forks"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/forks".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateFork
                    (
                        owner : string,
                        repo : string,
                        body : CreateForkOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/forks"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateForkOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetBlob
                    (owner : string, repo : string, sha : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/blobs/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString),
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

                        return GitBlobResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetSingleCommit
                    (owner : string, repo : string, sha : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/commits/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString),
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

                        return Commit.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDownloadCommitDiffOrPatch
                    (
                        owner : string,
                        repo : string,
                        sha : string,
                        diffType : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/commits/{sha}.{diffType}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{sha}", sha.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{diffType}", diffType.ToString () |> System.Uri.EscapeDataString),
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
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetNote
                    (owner : string, repo : string, sha : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/notes/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString),
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

                        return Note.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListAllGitRefs
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/refs"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Reference.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListGitRefs
                    (owner : string, repo : string, ref : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/refs/{ref}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{ref}", ref.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Reference.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetAnnotatedTag
                    (owner : string, repo : string, sha : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/git/tags/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString),
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

                        return AnnotatedTag.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetTree
                    (
                        owner : string,
                        repo : string,
                        sha : string,
                        recursive : bool,
                        page : int,
                        per_page : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/git/trees/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/git/trees/{sha}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "recursive="
                                     + ((recursive.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&per_page="
                                     + ((per_page.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return GitTreeResponse.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListHooks
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/hooks"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/hooks".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Hook.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateHook
                    (
                        owner : string,
                        repo : string,
                        body : CreateHookOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListGitHooks
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/git"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> GitHook.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetGitHook
                    (owner : string, repo : string, id : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/git/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return GitHook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteGitHook
                    (owner : string, repo : string, id : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/git/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditGitHook
                    (
                        owner : string,
                        repo : string,
                        id : string,
                        body : EditGitHookOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/git/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditGitHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return GitHook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetHook
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteHook
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditHook
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        body : EditHookOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/hooks/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditHookOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Hook.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoTestHook
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        ref : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/hooks/{id}/tests"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/hooks/{id}/tests".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "ref="
                                     + ((ref.ToString ()) |> System.Uri.EscapeDataString)),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetIssueTemplates
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issue_templates"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> IssueTemplate.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueListIssues
                    (
                        owner : string,
                        repo : string,
                        state : string,
                        labels : string,
                        q : string,
                        type' : string,
                        milestones : string,
                        since : string,
                        before : string,
                        created_by : string,
                        assigned_by : string,
                        mentioned_by : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/issues".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "state="
                                     + ((state.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&labels="
                                     + ((labels.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&type="
                                     + ((type'.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&milestones="
                                     + ((milestones.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&created_by="
                                     + ((created_by.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&assigned_by="
                                     + ((assigned_by.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&mentioned_by="
                                     + ((mentioned_by.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Issue.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueCreateIssue
                    (
                        owner : string,
                        repo : string,
                        body : CreateIssueOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateIssueOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Issue.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetRepoComments
                    (
                        owner : string,
                        repo : string,
                        since : string,
                        before : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues/comments"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/issues/comments".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Comment.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteComment
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueListIssueCommentAttachments
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}/assets"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Attachment.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetIssueCommentAttachment
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        attachment_id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
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

                        return Attachment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteIssueCommentAttachment
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        attachment_id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueEditIssueCommentAttachment
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        attachment_id : int,
                        body : EditAttachmentOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditAttachmentOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Attachment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetCommentReactions
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}/reactions"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Reaction.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteCommentReaction
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        content : EditReactionOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/comments/{id}/reactions"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                content
                                |> EditReactionOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetIssue
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
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

                        return Issue.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDelete
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueEditIssue
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : EditIssueOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditIssueOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Issue.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueListIssueAttachments
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/assets"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Attachment.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetIssueAttachment
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        attachment_id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
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

                        return Attachment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteIssueAttachment
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        attachment_id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueEditIssueAttachment
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        attachment_id : int,
                        body : EditAttachmentOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditAttachmentOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Attachment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetComments
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        since : string,
                        before : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues/{index}/comments"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/issues/{index}/comments".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Comment.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueCreateComment
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : CreateIssueCommentOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/comments"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateIssueCommentOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Comment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteCommentDeprecated
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/comments/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueEditIssueDeadline
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : EditDeadlineOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/deadline"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditDeadlineOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return IssueDeadline.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetLabels
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/labels"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Label.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueAddLabel
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : IssueLabelsOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/labels"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> IssueLabelsOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Label.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueClearLabels
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/labels"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueReplaceLabels
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : IssueLabelsOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/labels"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> IssueLabelsOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Label.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueRemoveLabel
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/labels/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetIssueReactions
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues/{index}/reactions"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/issues/{index}/reactions".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Reaction.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteIssueReaction
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        content : EditReactionOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/reactions"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                content
                                |> EditReactionOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteStopWatch
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/stopwatch/delete"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueStartStopWatch
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/stopwatch/start"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueStopStopWatch
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/stopwatch/stop"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueSubscriptions
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues/{index}/subscriptions"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if
                                            "repos/{owner}/{repo}/issues/{index}/subscriptions".IndexOf (char 63) >= 0
                                        then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueCheckSubscription
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/subscriptions/check"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
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

                        return WatchInfo.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetCommentsAndTimeline
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        since : string,
                        page : int,
                        limit : int,
                        before : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues/{index}/timeline"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/issues/{index}/timeline".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> TimelineComment.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueTrackedTimes
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        user : string,
                        since : string,
                        before : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/issues/{index}/times"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/issues/{index}/times".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "user="
                                     + ((user.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> TrackedTime.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueAddTime
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : AddTimeOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/times"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> AddTimeOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return TrackedTime.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueResetTime
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/times"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteTime
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/issues/{index}/times/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListKeys
                    (
                        owner : string,
                        repo : string,
                        key_id : int,
                        fingerprint : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/keys"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/keys".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "key_id="
                                     + ((key_id.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&fingerprint="
                                     + ((fingerprint.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> DeployKey.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateKey
                    (
                        owner : string,
                        repo : string,
                        body : CreateKeyOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/keys"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateKeyOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return DeployKey.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetKey
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/keys/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return DeployKey.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteKey
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/keys/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueListLabels
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/labels"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/labels".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Label.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueCreateLabel
                    (
                        owner : string,
                        repo : string,
                        body : CreateLabelOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/labels"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateLabelOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Label.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetLabel
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/labels/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Label.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteLabel
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/labels/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueEditLabel
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        body : EditLabelOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/labels/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditLabelOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Label.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetLanguages
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/languages"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return LanguageStatistics.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetRawFileOrLFS
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        ref : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/media/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/media/{filepath}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "ref="
                                     + ((ref.ToString ()) |> System.Uri.EscapeDataString)),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetMilestonesList
                    (
                        owner : string,
                        repo : string,
                        state : string,
                        name : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/milestones"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/milestones".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "state="
                                     + ((state.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&name="
                                     + ((name.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Milestone.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueCreateMilestone
                    (
                        owner : string,
                        repo : string,
                        body : CreateMilestoneOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/milestones"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateMilestoneOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Milestone.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueGetMilestone
                    (owner : string, repo : string, id : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/milestones/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Milestone.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueDeleteMilestone
                    (owner : string, repo : string, id : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/milestones/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.IssueEditMilestone
                    (
                        owner : string,
                        repo : string,
                        id : string,
                        body : EditMilestoneOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/milestones/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditMilestoneOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Milestone.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoMirrorSync
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/mirror-sync"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyGetRepoList
                    (
                        owner : string,
                        repo : string,
                        all : bool,
                        status_types : string list,
                        subject_type : string list,
                        since : string,
                        before : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/notifications"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/notifications".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "all="
                                     + ((all.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&status-types="
                                     + ((status_types.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&subject-type="
                                     + ((subject_type.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> NotificationThread.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.NotifyReadRepoList
                    (
                        owner : string,
                        repo : string,
                        all : string,
                        status_types : string list,
                        to_status : string,
                        last_read_at : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/notifications"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/notifications".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "all="
                                     + ((all.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&status-types="
                                     + ((status_types.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&to-status="
                                     + ((to_status.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&last_read_at="
                                     + ((last_read_at.ToString ()) |> System.Uri.EscapeDataString)),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> NotificationThread.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListPullRequests
                    (
                        owner : string,
                        repo : string,
                        state : string,
                        sort : string,
                        milestone : int,
                        labels : int list,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/pulls"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/pulls".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "state="
                                     + ((state.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&sort="
                                     + ((sort.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&milestone="
                                     + ((milestone.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&labels="
                                     + ((labels.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PullRequest.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreatePullRequest
                    (
                        owner : string,
                        repo : string,
                        body : CreatePullRequestOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreatePullRequestOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PullRequest.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetPullRequest
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
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

                        return PullRequest.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditPullRequest
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : EditPullRequestOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditPullRequestOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PullRequest.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDownloadPullDiffOrPatch
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        diffType : string,
                        binary : bool,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/pulls/{index}.{diffType}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{diffType}", diffType.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/pulls/{index}.{diffType}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "binary="
                                     + ((binary.ToString ()) |> System.Uri.EscapeDataString)),
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
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetPullRequestCommits
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/pulls/{index}/commits"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/pulls/{index}/commits".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Commit.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetPullRequestFiles
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        skip_to : string,
                        whitespace : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/pulls/{index}/files"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/pulls/{index}/files".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "skip-to="
                                     + ((skip_to.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&whitespace="
                                     + ((whitespace.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> ChangedFile.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoPullRequestIsMerged
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/merge"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoMergePullRequest
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : MergePullRequestOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/merge"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> MergePullRequestOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCancelScheduledAutoMerge
                    (owner : string, repo : string, index : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/merge"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreatePullReviewRequests
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : PullReviewRequestOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/requested_reviewers"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> PullReviewRequestOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PullReview.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeletePullReviewRequests
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : PullReviewRequestOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/requested_reviewers"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> PullReviewRequestOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListPullReviews
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/pulls/{index}/reviews"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/pulls/{index}/reviews".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PullReview.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreatePullReview
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        body : CreatePullReviewOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreatePullReviewOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PullReview.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetPullReview
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return PullReview.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoSubmitPullReview
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        body : SubmitPullReviewOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> SubmitPullReviewOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PullReview.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeletePullReview
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetPullReviewComments
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews/{id}/comments"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PullReviewComment.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDismissPullReview
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        body : DismissPullReviewOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews/{id}/dismissals"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> DismissPullReviewOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PullReview.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoUnDismissPullReview
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/pulls/{index}/reviews/{id}/undismissals"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PullReview.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoUpdatePullRequest
                    (
                        owner : string,
                        repo : string,
                        index : int,
                        style : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/pulls/{index}/update"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{index}", index.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/pulls/{index}/update".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "style="
                                     + ((style.ToString ()) |> System.Uri.EscapeDataString)),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListPushMirrors
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/push_mirrors"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/push_mirrors".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PushMirror.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoAddPushMirror
                    (
                        owner : string,
                        repo : string,
                        body : CreatePushMirrorOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/push_mirrors"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreatePushMirrorOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PushMirror.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoPushMirrorSync
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/push_mirrors-sync"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetPushMirrorByRemoteName
                    (owner : string, repo : string, name : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/push_mirrors/{name}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{name}", name.ToString () |> System.Uri.EscapeDataString),
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

                        return PushMirror.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeletePushMirror
                    (owner : string, repo : string, name : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/push_mirrors/{name}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{name}", name.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetRawFile
                    (
                        owner : string,
                        repo : string,
                        filepath : string,
                        ref : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/raw/{filepath}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{filepath}", filepath.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/raw/{filepath}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "ref="
                                     + ((ref.ToString ()) |> System.Uri.EscapeDataString)),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListReleases
                    (
                        owner : string,
                        repo : string,
                        draft : bool,
                        pre_release : bool,
                        per_page : int,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/releases"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/releases".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "draft="
                                     + ((draft.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&pre-release="
                                     + ((pre_release.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&per_page="
                                     + ((per_page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Release.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateRelease
                    (
                        owner : string,
                        repo : string,
                        body : CreateReleaseOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateReleaseOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Release.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetLatestRelease
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/latest"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return Release.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetReleaseByTag
                    (owner : string, repo : string, tag : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/tags/{tag}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{tag}", tag.ToString () |> System.Uri.EscapeDataString),
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

                        return Release.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteReleaseByTag
                    (owner : string, repo : string, tag : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/tags/{tag}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{tag}", tag.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetRelease
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Release.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteRelease
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditRelease
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        body : EditReleaseOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditReleaseOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Release.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListReleaseAttachments
                    (owner : string, repo : string, id : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}/assets"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Attachment.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetReleaseAttachment
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        attachment_id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
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

                        return Attachment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteReleaseAttachment
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        attachment_id : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditReleaseAttachment
                    (
                        owner : string,
                        repo : string,
                        id : int,
                        attachment_id : int,
                        body : EditAttachmentOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/releases/{id}/assets/{attachment_id}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace (
                                            "{attachment_id}",
                                            attachment_id.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditAttachmentOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Attachment.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetReviewers
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/reviewers"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoSigningKey
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/signing-key.gpg"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListStargazers
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/stargazers"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/stargazers".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListStatuses
                    (
                        owner : string,
                        repo : string,
                        sha : string,
                        sort : string,
                        state : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/statuses/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/statuses/{sha}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "sort="
                                     + ((sort.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&state="
                                     + ((state.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> CommitStatus.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateStatus
                    (
                        owner : string,
                        repo : string,
                        sha : string,
                        body : CreateStatusOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/statuses/{sha}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{sha}", sha.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateStatusOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return CommitStatus.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListSubscribers
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/subscribers"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/subscribers".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentCheckSubscription
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/subscription"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return WatchInfo.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentDeleteSubscription
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/subscription"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentPutSubscription
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/subscription"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return WatchInfo.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListTags
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/tags"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/tags".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Tag.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateTag
                    (
                        owner : string,
                        repo : string,
                        body : CreateTagOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/tags"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateTagOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Tag.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetTag
                    (owner : string, repo : string, tag : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/tags/{tag}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{tag}", tag.ToString () |> System.Uri.EscapeDataString),
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

                        return Tag.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteTag
                    (owner : string, repo : string, tag : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/tags/{tag}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{tag}", tag.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListTeams (owner : string, repo : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/teams"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Team.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCheckTeam
                    (owner : string, repo : string, team : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/teams/{team}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{team}", team.ToString () |> System.Uri.EscapeDataString),
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

                        return Team.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteTeam
                    (owner : string, repo : string, team : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/teams/{team}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{team}", team.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoAddTeam
                    (owner : string, repo : string, team : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/teams/{team}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{team}", team.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoTrackedTimes
                    (
                        owner : string,
                        repo : string,
                        user : string,
                        since : string,
                        before : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/times"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/times".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "user="
                                     + ((user.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> TrackedTime.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserTrackedTimes
                    (owner : string, repo : string, user : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/times/{user}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{user}", user.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> TrackedTime.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoListTopics
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/topics"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/topics".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return TopicName.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoUpdateTopics
                    (
                        owner : string,
                        repo : string,
                        body : RepoTopicOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/topics"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> RepoTopicOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteTopic
                    (owner : string, repo : string, topic : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/topics/{topic}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{topic}", topic.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoAddTopic
                    (owner : string, repo : string, topic : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/topics/{topic}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{topic}", topic.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoTransfer
                    (
                        owner : string,
                        repo : string,
                        body : TransferRepoOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/transfer"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> TransferRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.AcceptRepoTransfer
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/transfer/accept"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RejectRepoTransfer
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/transfer/reject"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoCreateWikiPage
                    (
                        owner : string,
                        repo : string,
                        body : CreateWikiPageOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/wiki/new"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateWikiPageOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return WikiPage.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetWikiPage
                    (owner : string, repo : string, pageName : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/wiki/page/{pageName}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{pageName}", pageName.ToString () |> System.Uri.EscapeDataString),
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

                        return WikiPage.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoDeleteWikiPage
                    (owner : string, repo : string, pageName : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/wiki/page/{pageName}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{pageName}", pageName.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoEditWikiPage
                    (
                        owner : string,
                        repo : string,
                        pageName : string,
                        body : CreateWikiPageOptions,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{owner}/{repo}/wiki/page/{pageName}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{pageName}", pageName.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateWikiPageOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return WikiPage.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetWikiPages
                    (
                        owner : string,
                        repo : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/wiki/pages"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/wiki/pages".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> WikiPageMetaData.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetWikiPageRevisions
                    (
                        owner : string,
                        repo : string,
                        pageName : string,
                        page : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("repos/{owner}/{repo}/wiki/revisions/{pageName}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{repo}", repo.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{pageName}", pageName.ToString () |> System.Uri.EscapeDataString)
                                     + (if "repos/{owner}/{repo}/wiki/revisions/{pageName}".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return WikiCommitList.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GenerateRepo
                    (
                        template_owner : string,
                        template_repo : string,
                        body : GenerateRepoOption,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repos/{template_owner}/{template_repo}/generate"
                                        .Replace(
                                            "{template_owner}",
                                            template_owner.ToString () |> System.Uri.EscapeDataString
                                        )
                                        .Replace (
                                            "{template_repo}",
                                            template_repo.ToString () |> System.Uri.EscapeDataString
                                        ),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> GenerateRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.RepoGetByID (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "repositories/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGeneralAPISettings (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("settings/api", System.UriKind.Relative)
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

                        return GeneralAPISettings.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGeneralAttachmentSettings (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("settings/attachment", System.UriKind.Relative)
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

                        return GeneralAttachmentSettings.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGeneralRepositorySettings (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("settings/repository", System.UriKind.Relative)
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

                        return GeneralRepoSettings.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGeneralUISettings (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("settings/ui", System.UriKind.Relative)
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

                        return GeneralUISettings.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetSigningKey (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("signing-key.gpg", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgGetTeam (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return Team.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgDeleteTeam (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgEditTeam (id : int, body : EditTeamOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> EditTeamOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Team.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListTeamMembers
                    (id : int, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("teams/{id}/members"
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                     + (if "teams/{id}/members".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListTeamMember
                    (id : int, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}/members/{username}"
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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

                        return User.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgRemoveTeamMember
                    (id : int, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}/members/{username}"
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgAddTeamMember
                    (id : int, username : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}/members/{username}"
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListTeamRepos
                    (id : int, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("teams/{id}/repos".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                     + (if "teams/{id}/repos".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListTeamRepo
                    (id : int, org : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}/repos/{org}/{repo}"
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgRemoveTeamRepository
                    (id : int, org : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}/repos/{org}/{repo}"
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgAddTeamRepository
                    (id : int, org : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "teams/{id}/repos/{org}/{repo}"
                                        .Replace("{id}", id.ToString () |> System.Uri.EscapeDataString)
                                        .Replace("{org}", org.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.TopicSearch
                    (q : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("topics/search"
                                     + (if "topics/search".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> TopicResponse.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGetCurrent (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user", System.UriKind.Relative)
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

                        return User.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGetOauth2Application
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/applications/oauth2"
                                     + (if "user/applications/oauth2".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> OAuth2Application.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCreateOAuth2Application
                    (body : CreateOAuth2ApplicationOptions, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/applications/oauth2", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateOAuth2ApplicationOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return OAuth2Application.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGetOAuth2Application (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/applications/oauth2/{id}"
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return OAuth2Application.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserDeleteOAuth2Application (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/applications/oauth2/{id}"
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserUpdateOAuth2Application
                    (id : int, body : CreateOAuth2ApplicationOptions, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/applications/oauth2/{id}"
                                        .Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateOAuth2ApplicationOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return OAuth2Application.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListEmails (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/emails", System.UriKind.Relative)
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Email.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserAddEmail (body : CreateEmailOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/emails", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateEmailOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Email.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserDeleteEmail (body : DeleteEmailOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/emails", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> DeleteEmailOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentListFollowers
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/followers"
                                     + (if "user/followers".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentListFollowing
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/following"
                                     + (if "user/following".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentCheckFollowing (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/following/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentDeleteFollow (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/following/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentPutFollow (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/following/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetVerificationToken (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/gpg_key_token", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentDeleteGPGKey (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/gpg_keys/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentListKeys
                    (fingerprint : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/keys"
                                     + (if "user/keys".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "fingerprint="
                                     + ((fingerprint.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PublicKey.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentPostKey (body : CreateKeyOption, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/keys", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateKeyOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return PublicKey.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentGetKey (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/keys/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
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

                        return PublicKey.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentDeleteKey (id : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/keys/{id}".Replace ("{id}", id.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListCurrentUserOrgs
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/orgs"
                                     + (if "user/orgs".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Organization.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentListRepos
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/repos"
                                     + (if "user/repos".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateCurrentUserRepo
                    (body : CreateRepoOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/repos", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateRepoOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return Repository.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetUserSettings (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/settings", System.UriKind.Relative)
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> UserSettings.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UpdateUserSettings
                    (body : UserSettingsOptions, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("user/settings", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> UserSettingsOptions.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> UserSettings.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentListStarred
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/starred"
                                     + (if "user/starred".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentCheckStarring
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/starred/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentDeleteStar
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/starred/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentPutStar
                    (owner : string, repo : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "user/starred/{owner}/{repo}"
                                        .Replace("{owner}", owner.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{repo}", repo.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Put,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGetStopWatches (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/stopwatches"
                                     + (if "user/stopwatches".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> StopWatch.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentListSubscriptions
                    (page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/subscriptions"
                                     + (if "user/subscriptions".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListTeams (page : int, limit : int, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/teams"
                                     + (if "user/teams".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Team.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCurrentTrackedTimes
                    (
                        page : int,
                        limit : int,
                        since : string,
                        before : string,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("user/times"
                                     + (if "user/times".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&since="
                                     + ((since.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&before="
                                     + ((before.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> TrackedTime.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserSearch
                    (q : string, uid : int, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/search"
                                     + (if "users/search".IndexOf (char 63) >= 0 then "&" else "?")
                                     + "q="
                                     + ((q.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&uid="
                                     + ((uid.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGet (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "users/{username}"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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

                        return User.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListFollowers
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/followers"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/followers".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListFollowing
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/following"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/following".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> User.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCheckFollowing
                    (username : string, target : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "users/{username}/following/{target}"
                                        .Replace("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{target}", target.ToString () |> System.Uri.EscapeDataString),
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
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGetHeatmapData (username : string, ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "users/{username}/heatmap"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> UserHeatmapData.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListKeys
                    (
                        username : string,
                        fingerprint : string,
                        page : int,
                        limit : int,
                        ct : System.Threading.CancellationToken option
                    )
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/keys"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/keys".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "fingerprint="
                                     + ((fingerprint.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> PublicKey.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgListUserOrgs
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/orgs"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/orgs".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Organization.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.OrgGetUserPermissions
                    (username : string, org : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "users/{username}/orgs/{org}/permissions"
                                        .Replace("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{org}", org.ToString () |> System.Uri.EscapeDataString),
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

                        return OrganizationPermissions.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListRepos
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/repos"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/repos".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListStarred
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/starred"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/starred".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserListSubscriptions
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/subscriptions"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/subscriptions".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> Repository.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserGetTokens
                    (username : string, page : int, limit : int, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    ("users/{username}/tokens"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                     + (if "users/{username}/tokens".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "page="
                                     + ((page.ToString ()) |> System.Uri.EscapeDataString)
                                     + "&limit="
                                     + ((limit.ToString ()) |> System.Uri.EscapeDataString)),
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

                        return
                            jsonNode.AsArray ()
                            |> Seq.map (fun elt -> AccessToken.jsonParse elt)
                            |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserCreateToken
                    (username : string, body : CreateAccessTokenOption, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "users/{username}/tokens"
                                        .Replace ("{username}", username.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                body
                                |> CreateAccessTokenOption.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ()),
                                null,
                                "application/json"
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return AccessToken.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.UserDeleteAccessToken
                    (username : string, token : string, ct : System.Threading.CancellationToken option)
                    =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "users/{username}/tokens/{token}"
                                        .Replace("{username}", username.ToString () |> System.Uri.EscapeDataString)
                                        .Replace ("{token}", token.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Delete,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return ()
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetVersion (ct : System.Threading.CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
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
                                    System.Uri ("/api/v1/", System.UriKind.Relative)
                                ),
                                System.Uri ("version", System.UriKind.Relative)
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

                        return ServerVersion.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
            }
