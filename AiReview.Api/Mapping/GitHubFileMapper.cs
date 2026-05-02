using AiReview.Api.Contracts;
using AiReview.Engine;

namespace AiReview.Api.Mapping;

public static class GitHubFileMapper
{
    public static Domain.ReviewInput ToReviewInput(GitHubPullRequestFileResponse file)
    {
        return new Domain.ReviewInput(
            file.Filename,
            file.Patch ?? string.Empty
        );
    }
}