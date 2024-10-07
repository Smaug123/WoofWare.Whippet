namespace WoofWare.Whippet.Plugin.Json.Test

open NUnit.Framework
open WoofWare.Whippet.Plugin.Json
open ApiSurface

[<TestFixture>]
module TestAttributeSurface =
    let assembly = typeof<JsonParseAttribute>.Assembly

    [<Test>]
    let ``Ensure API surface has not been modified`` () = ApiSurface.assertIdentical assembly

    (*
    [<Test>]
    let ``Check version against remote`` () =
        MonotonicVersion.validate assembly "WoofWare.Whippet.Plugin.Json.Attributes"
        *)

    [<Test ; Explicit>]
    let ``Update API surface`` () =
        ApiSurface.writeAssemblyBaseline assembly

    [<Test>]
    let ``Ensure public API is fully documented`` () =
        DocCoverage.assertFullyDocumented assembly
