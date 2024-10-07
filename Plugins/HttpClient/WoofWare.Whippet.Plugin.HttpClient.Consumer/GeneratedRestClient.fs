namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module PureGymApiHttpClientExtension =
    /// Extension methods for HTTP clients
    type IPureGymApi with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IPureGymApi =
            { new IPureGymApi with
                member _.GetGyms (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (("v1/gyms/"), System.UriKind.Relative)
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

                        return jsonNode.AsArray () |> Seq.map (fun elt -> Gym.jsonParse elt) |> List.ofSeq
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGymAttendance (gymId : int, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (
                                    "v1/gyms/{gym_id}/attendance"
                                        .Replace ("{gym_id}", gymId.ToString () |> System.Uri.EscapeDataString),
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

                        return GymAttendance.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGymAttendance' (gymId : int, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (
                                    "v1/gyms/{gym_id}/attendance"
                                        .Replace ("{gym_id}", gymId.ToString () |> System.Uri.EscapeDataString),
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

                        return GymAttendance.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetMember (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("v1/member", System.UriKind.Relative)
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

                        return Member.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetGym (gym : int, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (
                                    "v1/gyms/{gym}"
                                        .Replace ("{gym}", gym.ToString () |> System.Uri.EscapeDataString),
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

                        return Gym.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetMemberActivity (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("v1/member/activity", System.UriKind.Relative)
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

                        return MemberActivityDto.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetUrl (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("some/url", System.UriKind.Relative)
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

                        return UriThing.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.PostStringToString (foo : Map<string, string> option, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("some/url", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                foo
                                |> (fun field ->
                                    match field with
                                    | None -> null :> System.Text.Json.Nodes.JsonNode
                                    | Some field ->
                                        ((fun field ->
                                            let ret = System.Text.Json.Nodes.JsonObject ()

                                            for (KeyValue (key, value)) in field do
                                                ret.Add (
                                                    key.ToString (),
                                                    System.Text.Json.Nodes.JsonValue.Create<string> value
                                                )

                                            ret
                                        )
                                            field)
                                        :> System.Text.Json.Nodes.JsonNode
                                )
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ())
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            match jsonNode with
                            | null -> None
                            | v ->
                                v.AsObject ()
                                |> Seq.map (fun kvp ->
                                    let key = (kvp.Key)
                                    let value = (kvp.Value).AsValue().GetValue<System.String> ()
                                    key, value
                                )
                                |> Map.ofSeq
                                |> Some
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetSessions (fromDate : DateOnly, toDate : DateOnly, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (
                                    ("/v2/gymSessions/member"
                                     + (if "/v2/gymSessions/member".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "fromDate="
                                     + ((fromDate.ToString "yyyy-MM-dd") |> System.Uri.EscapeDataString)
                                     + "&toDate="
                                     + ((toDate.ToString "yyyy-MM-dd") |> System.Uri.EscapeDataString)),
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

                        return Sessions.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetSessionsWithQuery (fromDate : DateOnly, toDate : DateOnly, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (
                                    ("/v2/gymSessions/member?foo=1"
                                     + (if "/v2/gymSessions/member?foo=1".IndexOf (char 63) >= 0 then
                                            "&"
                                        else
                                            "?")
                                     + "fromDate="
                                     + ((fromDate.ToString "yyyy-MM-dd") |> System.Uri.EscapeDataString)
                                     + "&toDate="
                                     + ((toDate.ToString "yyyy-MM-dd") |> System.Uri.EscapeDataString)),
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

                        return Sessions.jsonParse jsonNode
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserString (user : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams = new System.Net.Http.StringContent (user)
                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserStream (user : System.IO.Stream, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams = new System.Net.Http.StreamContent (user)
                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserByteArr (user : byte[], ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams = new System.Net.Http.ByteArrayContent (user)
                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserByteArr' (user : array<byte>, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams = new System.Net.Http.ByteArrayContent (user)
                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserByteArr'' (user : byte array, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams = new System.Net.Http.ByteArrayContent (user)
                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserSerialisedBody (user : PureGym.Member, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                user
                                |> PureGym.Member.toJsonNode
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ())
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserSerialisedUrlBody (user : Uri, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                user
                                |> System.Text.Json.Nodes.JsonValue.Create<Uri>
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ())
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserSerialisedIntBody (user : int, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        let queryParams =
                            new System.Net.Http.StringContent (
                                user
                                |> System.Text.Json.Nodes.JsonValue.Create<int>
                                |> (fun node -> if isNull node then "null" else node.ToJsonString ())
                            )

                        do httpMessage.Content <- queryParams
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.CreateUserHttpContent (user : System.Net.Http.HttpContent, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("users/new", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Post,
                                RequestUri = uri
                            )

                        do httpMessage.Content <- user
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetPathParam (parameter : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri (
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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

                member _.GetStream (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetStream' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetStream'' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask
                        return responseStream
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponseMessage (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return response
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponseMessage' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return response
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponseMessage'' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return response
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponseMessage''' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return response
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponse (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            new RestEase.Response<_> (
                                responseString,
                                response,
                                (fun () -> (MemberActivityDto.jsonParse jsonNode))
                            )
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponse' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            new RestEase.Response<_> (
                                responseString,
                                response,
                                (fun () -> (MemberActivityDto.jsonParse jsonNode))
                            )
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponse'' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            new RestEase.Response<_> (
                                responseString,
                                response,
                                (fun () -> (MemberActivityDto.jsonParse jsonNode))
                            )
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetResponse''' (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        let! responseStream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                        let! jsonNode =
                            System.Text.Json.Nodes.JsonNode.ParseAsync (responseStream, cancellationToken = ct)
                            |> Async.AwaitTask

                        return
                            new RestEase.Response<_> (
                                responseString,
                                response,
                                (fun () -> (MemberActivityDto.jsonParse jsonNode))
                            )
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetWithAnyReturnCode (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        return response
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

                member _.GetWithoutAnyReturnCode (ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                (match client.BaseAddress with
                                 | null -> System.Uri "https://whatnot.com/"
                                 | v -> v),
                                System.Uri ("endpoint", System.UriKind.Relative)
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        return response
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module internal ApiWithoutBaseAddressHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithoutBaseAddress with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithoutBaseAddress =
            { new IApiWithoutBaseAddress with
                member _.GetPathParam (parameter : string, ct : CancellationToken option) =
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
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithBasePathHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithBasePath with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithBasePath =
            { new IApiWithBasePath with
                member _.GetPathParam (parameter : string, cancellationToken : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null ->
                                         raise (
                                             System.ArgumentNullException (
                                                 nameof (client.BaseAddress),
                                                 "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                             )
                                         )
                                     | v -> v),
                                    System.Uri ("foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = cancellationToken))
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithBasePathAndAddressHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithBasePathAndAddress with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithBasePathAndAddress =
            { new IApiWithBasePathAndAddress with
                member _.GetPathParam (parameter : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null -> System.Uri "https://whatnot.com/thing/"
                                     | v -> v),
                                    System.Uri ("foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithAbsoluteBasePathHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithAbsoluteBasePath with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithAbsoluteBasePath =
            { new IApiWithAbsoluteBasePath with
                member _.GetPathParam (parameter : string, cancellationToken : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null ->
                                         raise (
                                             System.ArgumentNullException (
                                                 nameof (client.BaseAddress),
                                                 "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                             )
                                         )
                                     | v -> v),
                                    System.Uri ("/foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = cancellationToken))
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithAbsoluteBasePathAndAddressHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithAbsoluteBasePathAndAddress with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithAbsoluteBasePathAndAddress =
            { new IApiWithAbsoluteBasePathAndAddress with
                member _.GetPathParam (parameter : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null -> System.Uri "https://whatnot.com/thing/"
                                     | v -> v),
                                    System.Uri ("/foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithBasePathAndAbsoluteEndpointHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithBasePathAndAbsoluteEndpoint with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithBasePathAndAbsoluteEndpoint =
            { new IApiWithBasePathAndAbsoluteEndpoint with
                member _.GetPathParam (parameter : string, cancellationToken : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null ->
                                         raise (
                                             System.ArgumentNullException (
                                                 nameof (client.BaseAddress),
                                                 "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                             )
                                         )
                                     | v -> v),
                                    System.Uri ("foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "/endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = cancellationToken))
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithBasePathAndAddressAndAbsoluteEndpointHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithBasePathAndAddressAndAbsoluteEndpoint with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithBasePathAndAddressAndAbsoluteEndpoint =
            { new IApiWithBasePathAndAddressAndAbsoluteEndpoint with
                member _.GetPathParam (parameter : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null -> System.Uri "https://whatnot.com/thing/"
                                     | v -> v),
                                    System.Uri ("foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "/endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithAbsoluteBasePathAndAbsoluteEndpointHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithAbsoluteBasePathAndAbsoluteEndpoint with

        /// Create a REST client.
        static member make (client : System.Net.Http.HttpClient) : IApiWithAbsoluteBasePathAndAbsoluteEndpoint =
            { new IApiWithAbsoluteBasePathAndAbsoluteEndpoint with
                member _.GetPathParam (parameter : string, cancellationToken : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null ->
                                         raise (
                                             System.ArgumentNullException (
                                                 nameof (client.BaseAddress),
                                                 "No base address was supplied on the type, and no BaseAddress was on the HttpClient."
                                             )
                                         )
                                     | v -> v),
                                    System.Uri ("/foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "/endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = cancellationToken))
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithAbsoluteBasePathAndAddressAndAbsoluteEndpointHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithAbsoluteBasePathAndAddressAndAbsoluteEndpoint with

        /// Create a REST client.
        static member make
            (client : System.Net.Http.HttpClient)
            : IApiWithAbsoluteBasePathAndAddressAndAbsoluteEndpoint
            =
            { new IApiWithAbsoluteBasePathAndAddressAndAbsoluteEndpoint with
                member _.GetPathParam (parameter : string, ct : CancellationToken option) =
                    async {
                        let! ct = Async.CancellationToken

                        let uri =
                            System.Uri (
                                System.Uri (
                                    (match client.BaseAddress with
                                     | null -> System.Uri "https://whatnot.com/thing/"
                                     | v -> v),
                                    System.Uri ("/foo/", System.UriKind.Relative)
                                ),
                                System.Uri (
                                    "/endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
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
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithHeadersHttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithHeaders with

        /// Create a REST client. The input functions will be re-evaluated on every HTTP request to obtain the required values for the corresponding header properties.
        static member make
            (someHeader : unit -> string)
            (someOtherHeader : unit -> int)
            (client : System.Net.Http.HttpClient)
            : IApiWithHeaders
            =
            { new IApiWithHeaders with
                member _.SomeHeader : string = someHeader ()
                member _.SomeOtherHeader : int = someOtherHeader ()

                member this.GetPathParam (parameter : string, ct : CancellationToken option) =
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
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        do httpMessage.Headers.Add ("X-Foo", this.SomeHeader.ToString ())
                        do httpMessage.Headers.Add ("Authorization", this.SomeOtherHeader.ToString ())
                        do httpMessage.Headers.Add ("Header-Name", "Header-Value")
                        do httpMessage.Headers.Add ("Something-Else", "val")
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
            }
namespace PureGym

open System
open System.Threading
open System.Threading.Tasks
open System.IO
open System.Net
open System.Net.Http
open WoofWare.Whippet.Plugin.HttpClient
open RestEase

/// Extension methods for constructing a REST client.
[<AutoOpen>]
module ApiWithHeaders2HttpClientExtension =
    /// Extension methods for HTTP clients
    type IApiWithHeaders2 with

        /// Create a REST client. The input functions will be re-evaluated on every HTTP request to obtain the required values for the corresponding header properties.
        static member make
            (someHeader : unit -> string)
            (someOtherHeader : unit -> int)
            (client : System.Net.Http.HttpClient)
            : IApiWithHeaders2
            =
            { new IApiWithHeaders2 with
                member _.SomeHeader : string = someHeader ()
                member _.SomeOtherHeader : int = someOtherHeader ()

                member this.GetPathParam (parameter : string, ct : CancellationToken option) =
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
                                    "endpoint/{param}"
                                        .Replace ("{param}", parameter.ToString () |> System.Uri.EscapeDataString),
                                    System.UriKind.Relative
                                )
                            )

                        let httpMessage =
                            new System.Net.Http.HttpRequestMessage (
                                Method = System.Net.Http.HttpMethod.Get,
                                RequestUri = uri
                            )

                        do httpMessage.Headers.Add ("X-Foo", this.SomeHeader.ToString ())
                        do httpMessage.Headers.Add ("Authorization", this.SomeOtherHeader.ToString ())
                        do httpMessage.Headers.Add ("Header-Name", "Header-Value")
                        let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                        let response = response.EnsureSuccessStatusCode ()
                        let! responseString = response.Content.ReadAsStringAsync ct |> Async.AwaitTask
                        return responseString
                    }
                    |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))
            }
