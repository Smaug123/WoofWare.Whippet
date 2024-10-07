# WoofWare.Whippet.Plugin.ArgParser

This is a [Whippet](https://github.com/Smaug123/WoofWare.Whippet) plugin defining an argument parser.

It is a copy of the corresponding [Myriad](https://github.com/MoiraeSoftware/myriad) arg parser in [WoofWare.Myriad](https://github.com/Smaug123/WoofWare.Myriad), taken from commit d59ebdfccb87a06579fb99008a15f58ea8be394e.

## Usage

Define an `Args.fs` file like the following:

```fsharp
namespace MyNamespace

[<ArgParser>]
type LoadsOfTypes =
    {
        Foo : int
        Bar : string
        Baz : bool
        SomeFile : FileInfo
        SomeDirectory : DirectoryInfo
        SomeList : DirectoryInfo list
        OptionalThingWithNoDefault : int option
        [<PositionalArgs>]
        Positionals : int list
        [<ArgumentDefaultFunction>]
        OptionalThing : Choice<bool, bool>
        [<ArgumentDefaultFunction>]
        AnotherOptionalThing : Choice<int, int>
        [<ArgumentDefaultEnvironmentVariable "CONSUMEPLUGIN_THINGS">]
        YetAnotherOptionalThing : Choice<string, string>
    }

    static member DefaultOptionalThing () = true

    static member DefaultAnotherOptionalThing () = 3
```

In your fsproj:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Args.fs" />
        <Compile Include="GeneratedArgs.fs">
            <WhippetFile>Args.fs</WhippetFile>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Runtime dependency: you use attributes to give instructions to the generator.
             Specify the `Version` appropriately by getting the latest version from NuGet.org.
         -->
        <PackageReference Include="WoofWare.Whippet.Plugin.ArgParser.Attributes" Version="" />
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.ArgParser" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

The generator will produce a file like the following:

```fsharp
[<RequireQualifiedAccess>]
module LoadsOfTypes =
    // in case you want to test it, you get one with dependencies injected
    let parse' (getEnvVar : string -> string) (args : string list) : LoadsOfTypes = ...
    // this is the one we expect you actually want to use, if you don't want to test the arg parser
    let parse (args : string list) : LoadsOfTypes = ...
```

## Features

* Default arguments are handled as `Choice<'a, 'a>`: you get a `Choice1Of2` if the user provided the input, or a `Choice2Of2` if the parser filled in your specified default value.
* Default arguments from the environment, specified with `[<ArgumentDefaultEnvironmentVariable "ENV_VAR">]`. If such an arg is not supplied on the command line, its value is parsed from the value of that env var.
* Default arguments, specified with `[<ArgumentDefaultFunction>]`. If an arg `[<ArgumentDefaultFunction>] Foo : Choice<'a, 'a>` is not supplied on the command line, the parser calls `DefaultFoo : unit -> 'a` to obtain its value.
* Positional arguments: a list with attribute `[<PositionalArgs>]` will accumulate all args which didn't match anything else. By default, the parser will fail if any of these arguments looks like an arg itself (i.e. it starts with `--`) but comes *before* a positional arg separator `--`; you can optionally give this attribute the argument `(* includeFlagLike = *) true` to instead just put such flag-like args into the accumulator.
* Positional args can also be of type `Choice<'a, 'a> list`, in which case we tell you whether the arg came before (`Choice1Of2`) or after (`Choice2Of2`) any `--` positional args separator.
* You can control TimeSpan and friends with the `[<InvariantCulture>]` and `[<ParseExact @"hh\:mm\:ss">]` attributes.
* By default, we generate F# extension methods for the type; you can instead create a module with the type's name, using `[<ArgParser (* isExtensionMethod = *) false>]`.
* If `--help` appears in a position where the parser is expecting a key (e.g. in the first position, or after a `--foo=bar`), the parser fails with help text. The parser also makes a limited effort to supply help text when encountering an invalid parse.
* "Flag DUs": if a two-case DU appears in the generator input file, you can tag its cases as in `type DryRun = | [<ArgumentFlag false>] Wet | [<ArgumentFlag true>] Dry`. Then you can consume the flag like a bool: `[<ArgParser>] type Args = { DryRun : DryRun }`, so `--dry-run` is parsed into `DryRun.Dry`.
* Control long forms of arguments with `[<ArgumentLongForm "alternative-name">] Foo : int`, so that instead of accepting the default `--foo=3`, we accept `--alternative-name=3`.
* Custom help text for individual args is supplied with `[<ArgumentHelpText "this text is displayed next to the arg when the user calls --help">]`, and similarly help text for the entire args object is supplied with `[<ArgParser>] [<ArgumentHelpText "hi!">] type Args = ...`.

## Limitations

This is very bare-bones, but do raise GitHub issues if you like (or if you find cases where the parser does the wrong thing).

* Help is signalled by throwing an exception, so you'll get an unsightly stack trace and a nonzero exit code.
* Help doesn't take into account any arguments the user has entered. Ideally you'd get contextual information like an identification of which args the user has supplied at the point where the parse failed or help was requested.
* I don't handle very many types, and in particular a real arg parser would handle DUs and records with nesting.
* I don't try very hard to find a valid parse. It may well be possible to find a case where I fail to parse despite there existing a valid parse.
* There's no subcommand support (you'll have to do that yourself).

It should work fine if you just want to compose a few primitive types, though.
