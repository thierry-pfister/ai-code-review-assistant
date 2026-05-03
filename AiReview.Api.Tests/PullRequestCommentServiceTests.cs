using AiReview.Api.Contracts;
using AiReview.Api.Formatting;
using AiReview.Api.Services;
using Moq;

namespace AiReview.Api.Tests;

public class PullRequestCommentServiceTests
{
    [Fact]
    public async Task CreatesCommentWhenNoExistingBotCommentExists()
    {
        var gitHubService = new Mock<IGitHubService>();

        gitHubService
            .Setup(service => service.GetPullRequestComments(
                "thierry-pfister",
                "ai-code-review-assistant",
                1))
            .ReturnsAsync([]);

        var service = new PullRequestCommentService(gitHubService.Object);

        var body = $"{PullRequestReviewFormatter.CommentMarker}\nNew review body";

        await service.UpsertReviewComment(
            "thierry-pfister",
            "ai-code-review-assistant",
            1,
            body
        );

        gitHubService.Verify(service => service.CreatePullRequestComment(
            "thierry-pfister",
            "ai-code-review-assistant",
            1,
            body
        ), Times.Once);

        gitHubService.Verify(service => service.UpdatePullRequestComment(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<long>(),
            It.IsAny<string>()
        ), Times.Never);
    }

    [Fact]
    public async Task UpdatesExistingCommentWhenBotCommentExists()
    {
        var gitHubService = new Mock<IGitHubService>();

        gitHubService
            .Setup(service => service.GetPullRequestComments(
                "thierry-pfister",
                "ai-code-review-assistant",
                1))
            .ReturnsAsync([
                new GitHubIssueCommentResponse
                {
                    Id = 123,
                    Body = $"{PullRequestReviewFormatter.CommentMarker}\nOld review body"
                }
            ]);

        var service = new PullRequestCommentService(gitHubService.Object);

        var body = $"{PullRequestReviewFormatter.CommentMarker}\nUpdated review body";

        await service.UpsertReviewComment(
            "thierry-pfister",
            "ai-code-review-assistant",
            1,
            body
        );

        gitHubService.Verify(service => service.UpdatePullRequestComment(
            "thierry-pfister",
            "ai-code-review-assistant",
            123,
            body
        ), Times.Once);

        gitHubService.Verify(service => service.CreatePullRequestComment(
            It.IsAny<string>(),
            It.IsAny<string>(),
            It.IsAny<int>(),
            It.IsAny<string>()
        ), Times.Never);
    }
}