using AiReview.Api.Contracts;

namespace AiReview.Api.Services;

public interface IGitHubService
{
    Task<List<GitHubPullRequestFileResponse>> GetPullRequestFiles(
        string owner,
        string repo,
        int pullRequestNumber);

    Task<List<GitHubIssueCommentResponse>> GetPullRequestComments(
        string owner,
        string repo,
        int pullRequestNumber);

    Task CreatePullRequestComment(
        string owner,
        string repo,
        int pullRequestNumber,
        string body);

    Task UpdatePullRequestComment(
        string owner,
        string repo,
        long commentId,
        string body);
}