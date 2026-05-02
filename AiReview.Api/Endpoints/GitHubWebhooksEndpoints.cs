using System.Text.Json;
using AiReview.Api.Contracts;
using AiReview.Api.Services;

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
                return Results.BadRequest();

            if (webhook.PullRequest is null || webhook.Repository is null)
                return Results.Ok(new { ignored = true });

            if (!SupportedPullRequestActions.Contains(webhook.Action))
                return Results.Ok(new { ignored = true });

            var repoParts = webhook.Repository.FullName.Split('/');
            var owner = repoParts[0];
            var repo = repoParts[1];
            var prNumber = webhook.PullRequest.Number;

            var files = await gitHubService.GetPullRequestFiles(owner, repo, prNumber);

            Console.WriteLine("Fetched files:");
            foreach (var file in files)
            {
                Console.WriteLine($"{file.Filename}");
            }

            return Results.Ok(new { files = files.Count });
        });
    }
}