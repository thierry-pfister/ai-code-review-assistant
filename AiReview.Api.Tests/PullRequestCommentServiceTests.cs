using AiReview.Api.Contracts;
using AiReview.Api.Formatting;
using AiReview.Api.Services;

namespace AiReview.Api.Tests;

public class PullRequestCommentServiceTests
{
    [Fact]
    public async Task CreatesCommentWhenNoExistingBotCommentExists()
    {
        var fakeGitHubService = new FakeGitHubService
        {
            ExistingComments = []
        };

        var service = new PullRequestCommentService(fakeGitHubService);

        await service.UpsertReviewComment(
            "thierry-pfister",
            "ai-code-review-assistant",
            1,
            $"{PullRequestReviewFormatter.CommentMarker}\nNew review body"
        );

        Assert.Equal(1, fakeGitHubService.CreateCallCount);
        Assert.Equal(0, fakeGitHubService.UpdateCallCount);
        Assert.Equal("New review body", fakeGitHubService.CreatedBodyWithoutMarkerText());
    }

    [Fact]
    public async Task UpdatesExistingCommentWhenBotCommentExists()
    {
        var fakeGitHubService = new FakeGitHubService
        {
            ExistingComments =
            [
                new GitHubIssueCommentResponse
                {
                    Id = 123,
                    Body = $"{PullRequestReviewFormatter.CommentMarker}\nOld review body"
                }
            ]
        };

        var service = new PullRequestCommentService(fakeGitHubService);

        await service.UpsertReviewComment(
            "thierry-pfister",
            "ai-code-review-assistant",
            1,
            $"{PullRequestReviewFormatter.CommentMarker}\nUpdated review body"
        );

        Assert.Equal(0, fakeGitHubService.CreateCallCount);
        Assert.Equal(1, fakeGitHubService.UpdateCallCount);
        Assert.Equal(123, fakeGitHubService.UpdatedCommentId);
        Assert.Equal("Updated review body", fakeGitHubService.UpdatedBodyWithoutMarkerText());
    }

    private class FakeGitHubService : IGitHubService
    {
        public List<GitHubIssueCommentResponse> ExistingComments { get; init; } = [];

        public int CreateCallCount { get; private set; }

        public int UpdateCallCount { get; private set; }

        public long? UpdatedCommentId { get; private set; }

        private string? CreatedBody { get; set; }

        private string? UpdatedBody { get; set; }

        public Task<List<GitHubPullRequestFileResponse>> GetPullRequestFiles(
            string owner,
            string repo,
            int pullRequestNumber)
        {
            return Task.FromResult(new List<GitHubPullRequestFileResponse>());
        }

        public Task<List<GitHubIssueCommentResponse>> GetPullRequestComments(
            string owner,
            string repo,
            int pullRequestNumber)
        {
            return Task.FromResult(ExistingComments);
        }

        public Task CreatePullRequestComment(
            string owner,
            string repo,
            int pullRequestNumber,
            string body)
        {
            CreateCallCount++;
            CreatedBody = body;

            return Task.CompletedTask;
        }

        public Task UpdatePullRequestComment(
            string owner,
            string repo,
            long commentId,
            string body)
        {
            UpdateCallCount++;
            UpdatedCommentId = commentId;
            UpdatedBody = body;

            return Task.CompletedTask;
        }

        public string? CreatedBodyWithoutMarkerText()
        {
            return CreatedBody?.Replace(PullRequestReviewFormatter.CommentMarker, "").Trim();
        }

        public string? UpdatedBodyWithoutMarkerText()
        {
            return UpdatedBody?.Replace(PullRequestReviewFormatter.CommentMarker, "").Trim();
        }
    }
}