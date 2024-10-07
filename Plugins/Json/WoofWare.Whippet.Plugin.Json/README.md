# WoofWare.Whippet.Plugin.Json

This is a [Whippet](https://github.com/Smaug123/WoofWare.Whippet) plugin defining JSON parse and serialise methods.

It is a copy of the corresponding [Myriad](https://github.com/MoiraeSoftware/myriad) JSON plugin in [WoofWare.Myriad](https://github.com/Smaug123/WoofWare.Myriad), taken from commit d59ebdfccb87a06579fb99008a15f58ea8be394e.

## What's the point?

`System.Text.Json`, in a `PublishAot` context, relies on C# source generators.
The default reflection-heavy implementations have the necessary code trimmed away, and result in a runtime exception.
But C# source generators [are entirely unsupported in F#](https://github.com/dotnet/fsharp/issues/14300).

These generators handle going from your strongly-typed domain objects to `System.Text.Json.Nodes.JsonNode`, and back.

## Usage: `JsonParse`

Define a `Dto.fs` file like the following:

```fsharp
namespace MyNamespace

open WoofWare.Whippet.Plugin.Json

[<JsonParse>]
type InnerType =
    {
        [<JsonPropertyName "something">]
        Thing : string
    }

/// My whatnot
[<JsonParse (* isExtensionMethod = *) false>]
type JsonRecordType =
    {
        /// A thing!
        A : int
        /// Another thing!
        B : string
        [<System.Text.Json.Serialization.JsonPropertyName "hi">]
        C : int list
        D : InnerType
    }
```

In your fsproj:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Dto.fs" />
        <Compile Include="GeneratedDto.fs">
            <WhippetFile>Dto.fs</WhippetFile>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Optional runtime dependency: you may use attributes to give instructions to the generator.
             Specify the `Version` appropriately by getting the latest version from NuGet.org.
         -->
        <PackageReference Include="WoofWare.Whippet.Plugin.Json.Attributes" Version="" />
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.Json" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

The generator will produce a file somewhat like the following:

```fsharp
/// Module containing JSON parsing methods for the InnerType type
[<RequireQualifiedAccess>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module InnerType =
    /// Parse from a JSON node.
    let jsonParse (node: System.Text.Json.Nodes.JsonNode) : InnerType =
        let Thing = node.["something"].AsValue().GetValue<string>()
        { Thing = Thing }
namespace UsePlugin

/// Module containing JSON parsing methods for the JsonRecordType type
[<AutoOpen>]
module JsonRecordTypeExtension =
    type JsonRecordType with
        /// Parse from a JSON node.
        let jsonParse (node: System.Text.Json.Nodes.JsonNode) : JsonRecordType =
            let D = InnerType.jsonParse node.["d"]

            let C =
                node.["hi"].AsArray() |> Seq.map (fun elt -> elt.GetValue<int>()) |> List.ofSeq

            let B = node.["b"].AsValue().GetValue<string>()
            let A = node.["a"].AsValue().GetValue<int>()
            { A = A; B = B; C = C; D = D }
```

You may instead choose to define attributes with the correct name yourself (if you don't want to take a dependency on the `WoofWare.Whippet.Plugin.Json.Attributes` package).
Alternatively, you may omit the attributes and the runtime dependency, and control the generator entirely through the fsproj file:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Dto.fs" />
        <Compile Include="GeneratedDto.fs">
            <WhippetFile>Dto.fs</WhippetFile>
            <WhippetParamInnerType>JsonParse</WhippetParamInnerType>
            <WhippetParamJsonRecordType>JsonParse(false)</WhippetParamJsonRecordType>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.Json" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

(This plugin follows a standard convention taken by `WoofWare.Whippet.Plugin` plugins,
where you use Whippet parameters with the same name as each input type,
whose contents are a `!`-delimited list of the generators which you wish to apply to that input type.)

## Usage: `JsonSerialize`

Define a `Dto.fs` file like the following:

```fsharp
namespace MyNamespace

open WoofWare.Whippet.Plugin.Json

[<JsonSerialize true>]
type InnerTypeWithBoth =
    {
        [<JsonPropertyName("it's-a-me")>]
        Thing : string
        ReadOnlyDict : IReadOnlyDictionary<string, Uri list>
    }
```

In your fsproj:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Dto.fs" />
        <Compile Include="GeneratedDto.fs">
            <WhippetFile>Dto.fs</WhippetFile>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Optional runtime dependency: you may use attributes to give instructions to the generator.
             Specify the `Version` appropriately by getting the latest version from NuGet.org.
         -->
        <PackageReference Include="WoofWare.Whippet.Plugin.Json.Attributes" Version="" />
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.Json" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

The generator will produce a file somewhat like the following:

```fsharp
namespace UsePlugin

/// Module containing JSON parsing methods for the JsonRecordType type
[<AutoOpen>]
module JsonRecordTypeExtension =
    type InnerTypeWithBoth with
        let toJsonNode (input : InnerTypeWithBoth) : System.Text.Json.Nodes.JsonNode =
            let node = System.Text.Json.Nodes.JsonObject ()

            do
                node.Add (("it's-a-me"), System.Text.Json.Nodes.JsonValue.Create<string> input.Thing)

                node.Add (
                    "ReadOnlyDict",
                    (fun field ->
                        let ret = System.Text.Json.Nodes.JsonObject ()

                        for (KeyValue (key, value)) in field do
                            ret.Add (key.ToString (), System.Text.Json.Nodes.JsonValue.Create<Uri> value)

                        ret
                    ) input.Map
                )

            node
```

You may instead choose to define attributes with the correct name yourself (if you don't want to take a dependency on the `WoofWare.Whippet.Plugin.Json.Attributes` package).
Alternatively, you may omit the attributes and the runtime dependency, and control the generator entirely through the fsproj file:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Dto.fs" />
        <Compile Include="GeneratedDto.fs">
            <WhippetFile>Dto.fs</WhippetFile>
            <WhippetParamInnerType>JsonSerialize</WhippetParamInnerType>
            <WhippetParamJsonRecordType>JsonSerialize(false)</WhippetParamJsonRecordType>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.Json" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

(This plugin follows a standard convention taken by `WoofWare.Whippet.Plugin` plugins,
where you use Whippet parameters with the same name as each input type,
whose contents are a `!`-delimited list of the generators which you wish to apply to that input type.)

## Notes

* The plugin includes an *opinionated* de/serializer for discriminated unions. (Any such serializer must be opinionated, because JSON does not natively model DUs.)
* Supply the optional boolean arg `false` to the `[<JsonParse>]`/`[<JsonSerialize>]` attributes, or pass it via `<WhippetParamMyType>JsonParse(false)</WhippetParamMyType>`, to get a genuine module that can be consumed from C#.
