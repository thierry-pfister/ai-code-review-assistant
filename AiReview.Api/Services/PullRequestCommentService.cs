using AiReview.Api.Formatting;

namespace AiReview.Api.Services;

public class PullRequestCommentService
{
    private readonly IGitHubService _gitHubService;

    public PullRequestCommentService(IGitHubService gitHubService)
    {
        _gitHubService = gitHubService;
    }

    public async Task UpsertReviewComment(
        string owner,
        string repo,
        int pullRequestNumber,
        string body)
    {
        var comments = await _gitHubService.GetPullRequestComments(
            owner,
            repo,
            pullRequestNumber
        );

        var existingComment = comments.FirstOrDefault(comment =>
            comment.Body.Contains(PullRequestReviewFormatter.CommentMarker));

        if (existingComment is null)
        {
            await _gitHubService.CreatePullRequestComment(
                owner,
                repo,
                pullRequestNumber,
                body
            );

            return;
        }

        await _gitHubService.UpdatePullRequestComment(
            owner,
            repo,
            existingComment.Id,
            body
        );
    }
}