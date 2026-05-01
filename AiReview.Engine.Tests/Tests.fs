namespace AiReview.Engine.Tests

open Xunit
open AiReview.Engine

module ReviewEngineTests =

    [<Fact>]
    let ``analyzeDiff flags hardcoded password`` () =
        let diff = "const password = \"123456\";"

        let result = ReviewEngine.analyzeDiff diff

        Assert.Contains("hardcoded password", result)

    [<Fact>]
    let ``analyzeDiff returns no obvious issues for safe diff`` () =
        let diff = "const username = \"thierry\";"

        let result = ReviewEngine.analyzeDiff diff

        Assert.Contains("No obvious issues", result)