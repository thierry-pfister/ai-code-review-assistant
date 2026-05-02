using AiReview.Api.Contracts;
using AiReview.Api.Formatting;

namespace AiReview.Api.Tests;

public class PullRequestReviewFormatterTests
{
    [Fact]
    public void FormatsEmptyFindings()
    {
        var result = PullRequestReviewFormatter.Format([]);

        Assert.Contains(PullRequestReviewFormatter.CommentMarker, result);
        Assert.Contains("AI Code Review Assistant", result);
        Assert.Contains("No rule-based issues", result);
        Assert.Contains("Looks good", result);
    }

    [Fact]
    public void FormatsFindingsGroupedByFile()
    {
        var findings = new List<ReviewFindingResponse>
        {
            new(
                "Hardcoded password detected",
                "Critical",
                "Security",
                "src/auth.ts",
                null
            ),
            new(
                "console.log statement left in code",
                "Info",
                "BugRisk",
                "src/logger.ts",
                null
            )
        };

        var result = PullRequestReviewFormatter.Format(findings);

        Assert.Contains(PullRequestReviewFormatter.CommentMarker, result);
        Assert.Contains("Found **2** potential issue", result);
        Assert.Contains("### `src/auth.ts`", result);
        Assert.Contains("Critical / Security", result);
        Assert.Contains("Hardcoded password detected", result);
        Assert.Contains("### `src/logger.ts`", result);
        Assert.Contains("Info / BugRisk", result);
    }
}