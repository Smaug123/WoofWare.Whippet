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
      dotnet-sdk = pkgs.dotnet-sdk_8;
      dotnet-runtime = pkgs.dotnetCorePackages.runtime_8_0;
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
            installPhase = ''mkdir -p $out/bin && cp -r tools/net6.0/any/* $out/bin'';
          };
          installPhase = let
            dll =
              if isNull dllOverride
              then name
              else dllOverride;
          in ''
            runHook preInstall
            mkdir -p "$out/lib"
            cp -r ./bin/* "$out/lib"
            makeWrapper "${dotnet-runtime}/bin/dotnet" "$out/bin/${name}" --add-flags "$out/lib/${dll}.dll"
            runHook postInstall
          '';
        };
    in {
      packages = {
        fantomas = dotnetTool null "fantomas" (builtins.fromJSON (builtins.readFile ./.config/dotnet-tools.json)).tools.fantomas.version (builtins.head (builtins.filter (elem: elem.pname == "fantomas") ((import ./nix/deps.nix) {fetchNuGet = x: x;}))).hash;
        fsharp-analyzers = dotnetTool "FSharp.Analyzers.Cli" "fsharp-analyzers" (builtins.fromJSON (builtins.readFile ./.config/dotnet-tools.json)).tools.fsharp-analyzers.version (builtins.head (builtins.filter (elem: elem.pname == "fsharp-analyzers") ((import ./nix/deps.nix) {fetchNuGet = x: x;}))).hash;
        default = pkgs.buildDotnetModule {
          inherit pname version dotnet-sdk dotnet-runtime;
          name = "WoofWare.Whippet";
          src = ./.;
          projectFile = "./WoofWare.Whippet/WoofWare.Whippet.fsproj";
          testProjectFile = "./WoofWare.Whippet.Test/WoofWare.Whippet.Test.fsproj";
          disabledTests = ["WoofWare.Whippet.Test.TestSurface.CheckVersionAgainstRemote"];
          nugetDeps = ./nix/deps.nix; # `nix build .#default.passthru.fetch-deps && ./result nix/deps.nix`
          doCheck = true;
        };
      };
      devShell = pkgs.mkShell {
        buildInputs = [dotnet-sdk];
        packages = [
          pkgs.alejandra
          pkgs.nodePackages.markdown-link-check
          pkgs.shellcheck
        ];
      };
    });
}
