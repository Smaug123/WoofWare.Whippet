{
  description = "Source generators for F#";

  inputs = {
    flake-utils.url = "github:numtide/flake-utils";
    nixpkgs.url = "github:NixOS/nixpkgs/nixpkgs-unstable";
  };

  outputs = {
    nixpkgs,
    flake-utils,
    ...
  }:
    flake-utils.lib.eachDefaultSystem (system: let
      pkgs = nixpkgs.legacyPackages.${system};
      pname = "WoofWare.Whippet";
      dotnet-sdk = pkgs.dotnetCorePackages.sdk_10_0;
      dotnet-runtime = pkgs.dotnetCorePackages.runtime_10_0;
      version = "0.1";
      dotnetTool = dllOverride: toolName: toolVersion: hash:
        pkgs.stdenvNoCC.mkDerivation rec {
          name = toolName;
          version = toolVersion;
          nativeBuildInputs = [pkgs.makeWrapper];
          src = pkgs.fetchNuGet {
            pname = name;
            version = version;
            hash = hash;
            installPhase = ''mkdir -p $out/bin && cp -r tools/net*/any/* $out/bin'';
          };
          installPhase = let
            dll =
              if isNull dllOverride
              then name
              else dllOverride;
          in
            # fsharp-analyzers requires the .NET SDK at runtime, so we use that instead of dotnet-runtime.
            ''
              runHook preInstall
              mkdir -p "$out/lib"
              cp -r ./bin/* "$out/lib"
              makeWrapper "${dotnet-sdk}/bin/dotnet" "$out/bin/${name}" --set DOTNET_HOST_PATH "${dotnet-sdk}/bin/dotnet" --add-flags "$out/lib/${dll}.dll"
              runHook postInstall
            '';
        };
    in {
      packages = let
        deps = builtins.fromJSON (builtins.readFile ./nix/deps.json);
      in {
        fantomas = dotnetTool null "fantomas" (builtins.fromJSON (builtins.readFile ./.config/dotnet-tools.json)).tools.fantomas.version (builtins.head (builtins.filter (elem: elem.pname == "fantomas") deps)).hash;
        fsharp-analyzers = dotnetTool "FSharp.Analyzers.Cli" "fsharp-analyzers" (builtins.fromJSON (builtins.readFile ./.config/dotnet-tools.json)).tools.fsharp-analyzers.version (builtins.head (builtins.filter (elem: elem.pname == "fsharp-analyzers") deps)).hash;
        default = pkgs.buildDotnetModule {
          inherit pname version dotnet-sdk dotnet-runtime;
          name = "WoofWare.Whippet";
          src = ./.;
          projectFile = "./WoofWare.Whippet/WoofWare.Whippet.csproj";
          testProjectFile = "./WoofWare.Whippet.Test/WoofWare.Whippet.Test.fsproj";
          # The `local` NuGet source in NuGet.config points at ./WoofWare.Whippet/bin/Debug/, which
          # only exists once the main project has been packed. During `fetch-deps`, nothing is built,
          # so that directory is absent; nixpkgs' nuget-to-json then treats the (enabled, non-directory)
          # source as a remote HTTP source and curls it, giving `curl: (3) URL rejected: No host part
          # in the URL`. Making the directory exist means nuget-to-json skips it as a local source.
          postConfigure = "mkdir -p WoofWare.Whippet/bin/Debug";
          disabledTests = ["WoofWare.Whippet.Test.TestSurface.CheckVersionAgainstRemote"];
          nugetDeps = ./nix/deps.json; # `nix build .#default.fetch-deps && ./result nix/deps.json`
          doCheck = true;
        };
      };
      devShell = pkgs.mkShell {
        buildInputs = [dotnet-sdk];
        packages = [
          pkgs.alejandra
          pkgs.lychee
          pkgs.shellcheck
          pkgs.xmlstarlet
        ];
      };
    });
}
