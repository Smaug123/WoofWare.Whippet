# WoofWare.Whippet.Plugin.ArgParser

This is a [Whippet](https://github.com/Smaug123/WoofWare.Whippet) plugin defining an argument parser.

It is a copy of the corresponding [Myriad](https://github.com/MoiraeSoftware/myriad) arg parser in [WoofWare.Myriad](https://github.com/Smaug123/WoofWare.Myriad), taken from commit d59ebdfccb87a06579fb99008a15f58ea8be394e.

## Usage

Define an `Args.fs` file like the following:

```fsharp
```

In your fsproj:

```xml
<Project>
    <ItemGroup>
        <Compile Include="Args.fs" />
    </ItemGroup>

    <ItemGroup>
    </ItemGroup>
</Project>
```
