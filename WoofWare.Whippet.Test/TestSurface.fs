namespace WoofWare.Whippet.Test

open NUnit.Framework
open WoofWare.Whippet.Core
open ApiSurface

[<TestFixture>]
module TestSurface =

    let coreAssembly = typeof<RawSourceGenerationArgs>.Assembly

    [<Test>]
    let ``Ensure API surface has not been modified`` () = ApiSurface.assertIdentical coreAssembly

    (*
    [<Test>]
    // https://github.com/nunit/nunit3-vs-adapter/issues/876
    let CheckVersionAgainstRemote () =
        MonotonicVersion.validate assembly "WoofWare.Myriad.Core"
    *)

    [<Test ; Explicit>]
    let ``Update API surface: core`` () =
        ApiSurface.writeAssemblyBaseline coreAssembly

    [<Test>]
    let ``Ensure public API is fully documented: core`` () =
        DocCoverage.assertFullyDocumented coreAssembly
