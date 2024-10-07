# WoofWare.Whippet.Plugin.HttpClient

This is a [Whippet](https://github.com/Smaug123/WoofWare.Whippet) plugin for defining [RestEase](https://github.com/canton7/RestEase)-style HTTP clients.

It is a copy of the corresponding [Myriad](https://github.com/MoiraeSoftware/myriad) HttpClient plugin in [WoofWare.Myriad](https://github.com/Smaug123/WoofWare.Myriad), taken from commit d59ebdfccb87a06579fb99008a15f58ea8be394e.

## Usage

Define a file like `Client.fs`:

```fsharp
open System.Threading.Tasks
open WoofWare.Whippet.Plugin.HttpClient

[<HttpClient>]
type IPureGymApi =
    [<Get "v1/gyms/">]
    abstract GetGyms : ?ct : CancellationToken -> Task<Gym list>

    [<Get "v1/gyms/{gym_id}/attendance">]
    abstract GetGymAttendance : [<Path "gym_id">] gymId : int * ?ct : CancellationToken -> Task<GymAttendance>

    [<Get "v1/member">]
    abstract GetMember : ?ct : CancellationToken -> Task<Member>

    [<Get "v1/gyms/{gym_id}">]
    abstract GetGym : [<Path "gym_id">] gymId : int * ?ct : CancellationToken -> Task<Gym>

    [<Get "v1/member/activity">]
    abstract GetMemberActivity : ?ct : CancellationToken -> Task<MemberActivityDto>

    [<Get "v2/gymSessions/member">]
    abstract GetSessions :
        [<Query>] fromDate : DateTime * [<Query>] toDate : DateTime * ?ct : CancellationToken -> Task<Sessions>
```

In your fsproj:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Client.fs" />
        <Compile Include="GeneratedClient.fs">
            <WhippetFile>Client.fs</WhippetFile>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Optional runtime dependency: you may use attributes to give instructions to the generator.
             Specify the `Version` appropriately by getting the latest version from NuGet.org.
             You may instead wish to take a dependency on RestEase to get the attributes;
             and if you want to use RestEase's types like `Response` then you *must* do so.
         -->
        <PackageReference Include="WoofWare.Whippet.Plugin.HttpClient.Attributes" Version="" />
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.HttpClient" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

The generator produces a type like this (here I'm showing the `isExtensionMethod = false` version):

```fsharp
/// Module for constructing a REST client.
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module PureGymApi =
    /// Create a REST client.
    let make (client : System.Net.Http.HttpClient) : IPureGymApi =
        { new IPureGymApi with
            member _.GetGyms (ct : CancellationToken option) =
                async {
                    let! ct = Async.CancellationToken

                    let httpMessage =
                        new System.Net.Http.HttpRequestMessage (
                            Method = System.Net.Http.HttpMethod.Get,
                            RequestUri = System.Uri (client.BaseAddress.ToString () + "v1/gyms/")
                        )

                    let! response = client.SendAsync (httpMessage, ct) |> Async.AwaitTask
                    let response = response.EnsureSuccessStatusCode ()
                    let! stream = response.Content.ReadAsStreamAsync ct |> Async.AwaitTask

                    let! node =
                        System.Text.Json.Nodes.JsonNode.ParseAsync (stream, cancellationToken = ct)
                        |> Async.AwaitTask

                    return node.AsArray () |> Seq.map (fun elt -> Gym.jsonParse elt) |> List.ofSeq
                }
                |> (fun a -> Async.StartAsTask (a, ?cancellationToken = ct))

            // (more methods here)
        }
```

You tell the generator to generate a client using the `[<HttpClient>]` attribute.
You may instead choose to define an attribute with the correct name yourself (if you don't want to take a dependency on the `WoofWare.Whippet.Plugin.RestEase.Attributes` package),
and use the RestEase attributes directly from RestEase by taking a dependency on RestEase.

Alternatively, you may omit the `[<HttpClient>]` attribute entirely, and control the generator through the fsproj file:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Client.fs" />
        <Compile Include="GeneratedClient.fs">
            <WhippetFile>Client.fs</WhippetFile>
            <WhippetParamClientType1>HttpClient</WhippetParamClientType1>
            <WhippetParamClientType2>HttpClient(false)</WhippetParamClientType2>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.HttpClient" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

(This plugin follows a standard convention taken by `WoofWare.Whippet.Plugin` plugins,
where you use Whippet parameters with the same name as each input type,
whose contents are a `!`-delimited list of the generators which you wish to apply to that input type.)

## Notes

* The plugin assumes access to the `WoofWare.Whippet.Plugin.Json` generators in some situations. If you find the result does not compile due to the lack of `.jsonParse` methods, you might want to generate them using that plugin.
* Supply the optional boolean arg `false` to the `[<HttpClient>]` attribute, or pass it via `<WhippetParamMyType>HttpClient(false)</WhippetParamMyType>`, to get a genuine module that can be consumed from C# (rather than an extension method).
