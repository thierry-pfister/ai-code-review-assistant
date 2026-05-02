using System.Text.Json;
using AiReview.Api.Contracts;
using AiReview.Api.Formatting;
using AiReview.Api.Mapping;
using AiReview.Api.Services;
using AiReview.Engine;

namespace AiReview.Api.Endpoints;

public static class GitHubWebhooksEndpoints
{
    private static readonly HashSet<string> SupportedPullRequestActions =
    [
        "opened",
        "synchronize",
        "reopened"
    ];

    public static void MapGitHubWebhookEndpoints(this WebApplication app)
    {
        app.MapPost("/webhooks/github", async (
            HttpRequest request,
            GitHubService gitHubService) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var webhook = JsonSerializer.Deserialize<GitHubWebhookRequest>(body);

            if (webhook is null)
                return Results.BadRequest("Invalid GitHub webhook payload.");

            if (webhook.PullRequest is null || webhook.Repository is null)
                return Results.Ok(new { ignored = true, reason = "Not a pull request event." });

            if (!SupportedPullRequestActions.Contains(webhook.Action))
                return Results.Ok(new { ignored = true, reason = $"Unsupported action: {webhook.Action}" });

            var repoParts = webhook.Repository.FullName.Split('/');

            if (repoParts.Length != 2)
                return Results.BadRequest("Invalid repository full name.");

            var owner = repoParts[0];
            var repo = repoParts[1];
            var prNumber = webhook.PullRequest.Number;

            // 1. Fetch files from GitHub
            var files = await gitHubService.GetPullRequestFiles(owner, repo, prNumber);

            // 2. Map to F# domain input
            var reviewInputs = files
                .Select(GitHubFileMapper.ToReviewInput)
                .ToList();

            // 3. Run analysis
            var findings = ReviewEngine.analyzeFiles(reviewInputs);

            // 4. Map to API response
            var response = findings
                .Select(ReviewFindingMapper.ToResponse)
                .ToList();

            // 5. Format PR comment
            var summary = PullRequestReviewFormatter.Format(response);

            // 6. Post comment to GitHub PR
            await gitHubService.PostPullRequestComment(
                owner,
                repo,
                prNumber,
                summary
            );

            // 7. Return API response
            return Results.Ok(new
            {
                accepted = true,
                repository = webhook.Repository.FullName,
                pullRequestNumber = prNumber,
                changedFiles = files.Count,
                findings = response
            });
        });
    }
}