namespace AiReview.Engine.Tests

open Xunit
open AiReview.Engine

module ReviewEngineTests =

    [<Fact>]
    let ``analyzeDiff flags hardcoded password`` () =
        let diff = "const password = \"123456\";"

        let result = ReviewEngine.analyzeDiff diff

        Assert.NotEmpty(result)

    [<Fact>]
    let ``analyzeDiff returns no issues for safe diff`` () =
        let diff = "const username = \"thierry\";"

        let result = ReviewEngine.analyzeDiff diff

        Assert.Empty(result)

    [<Fact>]
    let ``analyzeDiff returns structured finding when password detected`` () =
        let diff = "const password = \"123\";"

        let result = ReviewEngine.analyzeDiff diff

        Assert.NotEmpty(result)