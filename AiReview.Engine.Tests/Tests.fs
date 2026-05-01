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
    let ``analyzeDiff flags console log as info`` () =
        let diff = "console.log(user);"

        let result = ReviewEngine.analyzeDiff diff

        Assert.Single(result) |> ignore
        Assert.Equal(ReviewEngine.Info, result.Head.Severity)
        Assert.Equal(ReviewEngine.BugRisk, result.Head.Category)
        Assert.Contains("console.log", result.Head.Message)

    [<Fact>]
    let ``analyzeDiff flags todo comments as architecture warning`` () =
        let diff = "// TODO: refactor this later"

        let result = ReviewEngine.analyzeDiff diff

        Assert.Single(result) |> ignore
        Assert.Equal(ReviewEngine.Warning, result.Head.Severity)
        Assert.Equal(ReviewEngine.Architecture, result.Head.Category)
        Assert.Contains("TODO", result.Head.Message)

    [<Fact>]
    let ``finding includes file and line metadata`` () =
        let diff = "const password = \"123\";"

        let result = ReviewEngine.analyzeDiff diff
        let finding = result.Head

        Assert.Equal("unknown", finding.File)
        Assert.Equal(None, finding.Line)

    [<Fact>]
    let ``analyzeFile includes real file name in finding`` () =
        let input: ReviewEngine.ReviewInput =
            { File = "src/auth.ts"
              Diff = "const password = \"123\";" }

        let result = ReviewEngine.analyzeFile input
        let finding = result.Head

        Assert.Equal("src/auth.ts", finding.File)
        Assert.Equal(ReviewEngine.Critical, finding.Severity)
        Assert.Equal(ReviewEngine.Security, finding.Category)