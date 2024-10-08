# WoofWare.Whippet

Whippet is a source generator for F#, inspired by [Myriad](https://github.com/MoiraeSoftware/myriad).

It is currently vapourware; please do not use it, because its API surface and features are liable to change completely without notice.
It currently lacks most of the future features intended to distinguish Whippet from Myriad.

Differentiating features:

* Whippet expands the range of information available to a source-generating plugin. Eventually (in the far future) we intend for it to supply type-checking information.
* Whippet will eventually support the Fantomas [Oak](https://fsprojects.github.io/fantomas/docs/end-users/GeneratingCode.html) format, rather than just a plain AST. It already does support this, in the sense that the only interface to Whippet is "we give you bytes, you give us text", so you're free to use an Oak already; but we give you no help with this.
* Whippet is intended to be more modular, providing various different helper assemblies a plugin author can optionally use depending on what features they want.

## How to use

The simplest invocation is as follows.

### Import the source generator framework

In your `.fsproj`, take the following `PackageReference`, setting the `Version=""` to the latest version available on NuGet.

```xml
<PackageReference Include="WoofWare.Whippet" PrivateAssets="all" Version="" />
```

(`PrivateAssets="all"` is necessary to prevent the Whippet dependency from flowing to the consumers of your package.)

### Import the plugin you wish to call

```xml
<PackageReference Include="WoofWare.Whippet.Plugin.ArgParser" PrivateAssets="all" WhippetPlugin="true" />
```

Note the important `WhippetPlugin="true"` which is how Whippet determines which packages to search for plugins,
and the `PrivateAssets="all"` again to prevent this dependency from flowing to your consumers.

### Configure the source generator

The simplest possible configuration is as follows:

```xml
<ItemGroup>
  <Compile Include="Args.fs" />
  <Compile Include="GeneratedArgs.fs">
    <WhippetFile>Args.fs</WhippetFile>
  </Compile>
</ItemGroup>
```

Here, you wrote the `Args.fs` file, and have specified that the `GeneratedArgs.fs` file is to be generated using `Args.fs`
as input.

### Advanced configuration of source generators

A plugin author may choose to have their plugin be configurable, by recognising parameters passed through the fsproj.

```xml
<ItemGroup>
    <Compile Include="SwaggerGitea.fs" />
    <Compile Include="GeneratedSwaggerGitea.fs">
        <WhippetFile>swagger-gitea.json</WhippetFile>
        <WhippetParamGenerateMockInternal>true</WhippetParamGenerateMockInternal>
        <WhippetParamClassName>Gitea</WhippetParamClassName>
    </Compile>
</ItemGroup>
```

Any key prefixed with `WhippetParam` will have that prefix removed and the string value passed in to the generator
through the `Parameters` field on the plugin's args.
(MSBuild only allows strings here, so the `"true"` in the above example is a string, not a boolean.
If you want more advanced inputs to your plugin, you will have to create a parser yourself.)

You can supply `<WhippetSuppressPlugin>JsonSerializeGenerator,JsonParseGenerator</WhippetSuppressPlugin>` (for example) next to `<WhippetFile>`
to suppress on that file the action of all of the specified comma-separated list of generators.
(That is, we will ignore any generator defined in a class with one of these names.)

## Standalone tool

The standalone tool takes the following arguments:

* A path to an `fsproj` file.
* (positional args) A list of DLLs from which to load plugins. (Currently I strongly recommend only using one plugin per fsproj file; it's completely untested to use more!)

The tool uses MSBuild to load the fsproj file to discover what files need to be generated.
(This duplicates a bunch of work, because you're presumably executing the tool during a build anyway!)
The tool then loads the plugins, and reflectively determines which source generators contained therein should run on each file.
