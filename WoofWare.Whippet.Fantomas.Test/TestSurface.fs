namespace WoofWare.Whippet.Fantomas.Test

open NUnit.Framework
open WoofWare.Whippet.Fantomas
open ApiSurface

[<TestFixture>]
module TestSurface =

    let coreAssembly = typeof<CompExprBinding>.Assembly

    [<Test>]
    let ``Ensure API surface has not been modified`` () = ApiSurface.assertIdentical coreAssembly

    [<Test>]
    // https://github.com/nunit/nunit3-vs-adapter/issues/876
    let CheckVersionAgainstRemote () =
        MonotonicVersion.validate coreAssembly "WoofWare.Whippet.Fantomas"

    [<Test ; Explicit>]
    let ``Update API surface`` () =
        ApiSurface.writeAssemblyBaseline coreAssembly

    [<Test>]
    let ``Ensure public API is fully documented`` () =
        DocCoverage.assertFullyDocumented coreAssembly
