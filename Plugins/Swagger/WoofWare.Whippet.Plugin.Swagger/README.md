# WoofWare.Whippet.Plugin.Swagger

This is a [Whippet](https://github.com/Smaug123/WoofWare.Whippet) plugin for defining strongly-typed HTTP clients according to [Swagger](https://swagger.io/) schemas.

It is a copy of the corresponding [Myriad](https://github.com/MoiraeSoftware/myriad) HttpClient plugin in [WoofWare.Myriad](https://github.com/Smaug123/WoofWare.Myriad), taken from commit d59ebdfccb87a06579fb99008a15f58ea8be394e.

## Usage

Save a Swagger schema as `my-swagger-schema.json`.

In your `fsproj`:

```xml
<ItemGroup>
    <None Include="my-swagger-schema.json" />
    <Compile Include="Client.fs">
        <WhippetFile>my-swagger-schema.json</WhippetFile>
        <WhippetParamClassName>GiteaClient</WhippetParamClassName>
        <!-- Optionally: -->
        <WhippetParamGenerateMock>true</WhippetParamGenerateMock>
    </Compile>
</ItemGroup>
```

(Note that you must supply `<WhippetParamClassName>SomeClassName</WhippetParamClassName>` to tell the generator what to name the type it produces.)

The generator produces code like this.
Notice that it adds `JsonParse` and `JsonSerialize` attributes (i.e. it assumes you have access to [WoofWare.Whippet.Plugin.Json.Attributes](https://www.nuget.org/packages/WoofWare.Whippet.Plugin.Json.Attributes)),
and also the `HttpClient` attribute (i.e. it assumes you have access to [WoofWare.Whippet.Plugin.HttpClient.Attributes](https://www.nuget.org/packages/WoofWare.Whippet.Plugin.HttpClient.Attributes)).

```fsharp
/// A type which was defined in the Swagger spec
[<JsonParse true ; JsonSerialize true>]
type SwaggerType1 =
    {
        [<System.Text.Json.Serialization.JsonExtensionData>]
        AdditionalProperties : System.Collections.Generic.Dictionary<string, System.Text.Json.Nodes.JsonNode>
        Message : string
    }

/// Documentation from the Swagger spec
[<HttpClient false ; RestEase.BasePath "/api/v1">]
type IGitea =
    /// Returns the Person actor for a user
    [<RestEase.Get "/activitypub/user/{username}">]
    abstract ActivitypubPerson :
        [<RestEase.Path "username">] username : string * ?ct : System.Threading.CancellationToken ->
            ActivityPub System.Threading.Tasks.Task
```

That means if you choose to, you can chain another Whippet generator off this one, to generate JSON serde methods and HTTP REST clients:

```xml
<ItemGroup>
    <None Include="my-swagger-schema.json" />
    <Compile Include="Client.fs">
        <WhippetFile>my-swagger-schema.json</WhippetFile>
        <WhippetParamClassName>GiteaClient</WhippetParamClassName>
        <!-- Optionally: -->
        <WhippetParamGenerateMock>true</WhippetParamGenerateMock>
    </Compile>
    <Compile Include="Client2.fs">
        <WhippetFile>Client.fs</WhippetFile>
    </Compile>
</ItemGroup>
```

The `<ClassName />` key tells us what to name the resulting interface (it gets an `I` prepended for you).
You can optionally also set `<GenerateMockVisibility>v</GenerateMockVisibility>` to add the `[<GenerateMock>]` attribute to the type
(where `v` should be `internal` or `public`, indicating "resulting mock type is internal" vs "is public"),
so that the following manoeuvre will result in a generated mock:

### What's the point?

[`SwaggerProvider`](https://github.com/fsprojects/SwaggerProvider) is *absolutely magical*, but it's kind of witchcraft.
I fear no man, but that thing… it scares me.

Also, builds using `SwaggerProvider` appear to be inherently nondeterministic, even if the data source doesn't change.

## Limitations

Swagger API specs appear to be pretty cowboy in the wild.
I try to cope with invalid schemas I have seen, but I can't guarantee I do so correctly.
Definitely do perform integration tests and let me know of weird specs you encounter, and bits of the (very extensive) Swagger spec I have omitted!

I have only attempted to deal with Swagger v2.0 so far.
