# WoofWare.Whippet.Plugin.InterfaceMock

This is a [Whippet](https://github.com/Smaug123/WoofWare.Whippet) plugin for defining mocks for interfaces.

It is a copy of the corresponding [Myriad](https://github.com/MoiraeSoftware/myriad) HttpClient plugin in [WoofWare.Myriad](https://github.com/Smaug123/WoofWare.Myriad), taken from commit d59ebdfccb87a06579fb99008a15f58ea8be394e.

## Usage

Define a file like `Client.fs`:

```fsharp
type IPublicType =
    abstract Mem1 : string * int -> string list
    abstract Mem2 : string -> int
```

In your fsproj:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Client.fs" />
        <Compile Include="GeneratedClient.fs">
            <WhippetFile>Client.fs</WhippetFile>
            <WhippetParamIPublicType>InterfaceMock</WhippetParamIPublicType>
        </Compile>
    </ItemGroup>

    <ItemGroup>
        <!-- Development dependencies, hence PrivateAssets="all". Note `WhippetPlugin="true"`. -->
        <PackageReference Include="WoofWare.Whippet.Plugin.InterfaceMock" WhippetPlugin="true" Version="" />
        <PackageReference Include="WoofWare.Whippet" Version="" PrivateAssets="all" />
    </ItemGroup>
</Project>
```

(This plugin follows a standard convention taken by `WoofWare.Whippet.Plugin` plugins,
where you use Whippet parameters with the same name as each input type,
whose contents are a `!`-delimited list of the generators which you wish to apply to that input type.)

The generator produces a type like this:

```fsharp
/// Mock record type for an interface
type internal PublicTypeMock =
    {
        Mem1 : string * int -> string list
        Mem2 : string -> int
    }

    static member Empty : PublicTypeMock =
        {
            Mem1 = (fun x -> raise (System.NotImplementedException "Unimplemented mock function"))
            Mem2 = (fun x -> raise (System.NotImplementedException "Unimplemented mock function"))
        }

    interface IPublicType with
        member this.Mem1 (arg0, arg1) = this.Mem1 (arg0, arg1)
        member this.Mem2 (arg0) = this.Mem2 (arg0)
```

### What's the point?

Reflective mocking libraries like [Foq](https://github.com/fsprojects/Foq) in my experience are a rich source of flaky tests.
The [Grug-brained developer](https://grugbrain.dev/) would prefer to do this without reflection, and this reduces the rate of strange one-in-ten-thousand "failed to generate IL" errors.
But since F# does not let you partially update an interface definition, we instead stamp out a record,
thereby allowing the programmer to use F#'s record-update syntax.

### Features

You may supply an `isInternal : bool` argument:

```xml
<Compile Include="GeneratedClient.fs">
    <WhippetFile>Client.fs</WhippetFile>
    <WhippetParamIPublicType>InterfaceMock(false)</WhippetParamIPublicType>
</Compile>
```

By default, we make the resulting record type at most internal (never public),
since this is intended only to be used in tests;
but you can instead make it public by setting the `false` boolean.

Instead of configuring the client with `<WhippetParamMyType>InterfaceMock</WhippetParamMyType>`,
you may choose to add an attribute called `InterfaceMock` (with an optional "isInternal" argument)
to any type you wish to use as an input.
You may use `WoofWare.Whippet.Plugin.InterfaceMock.Attributes` to provide this attribute, or you may define it yourself.
