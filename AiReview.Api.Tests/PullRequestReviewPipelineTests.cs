using AiReview.Api.Contracts;
using AiReview.Api.Mapping;
using AiReview.Engine;

namespace AiReview.Api.Tests;

public class PullRequestReviewPipelineTests
{
    [Fact]
    public void AnalyzesGitHubPullRequestFilesAndReturnsFindings()
    {
        // Arrange
        var gitHubFiles = new List<GitHubPullRequestFileResponse>
        {
            new()
            {
                Filename = "src/auth.ts",
                Patch = "+ const password = \"123\";"
            },
            new()
            {
                Filename = "src/logger.ts",
                Patch = "+ console.log(user);"
            },
            new()
            {
                Filename = "src/user.ts",
                Patch = "+ const username = \"thierry\";"
            }
        };

        // Act
        var reviewInputs = gitHubFiles
            .Select(GitHubFileMapper.ToReviewInput)
            .ToList();

        var findings = ReviewEngine.analyzeFiles(reviewInputs);

        var responses = findings
            .Select(ReviewFindingMapper.ToResponse)
            .ToList();

        // Assert
        Assert.Equal(2, responses.Count);

        Assert.Contains(responses, finding =>
            finding.File == "src/auth.ts" &&
            finding.Severity == "Critical" &&
            finding.Category == "Security" &&
            finding.Message.Contains("password"));

        Assert.Contains(responses, finding =>
            finding.File == "src/logger.ts" &&
            finding.Severity == "Info" &&
            finding.Category == "BugRisk" &&
            finding.Message.Contains("console.log"));

        Assert.DoesNotContain(responses, finding =>
            finding.File == "src/user.ts");
    }
}