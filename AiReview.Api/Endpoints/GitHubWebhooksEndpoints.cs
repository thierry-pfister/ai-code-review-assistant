using System.Text.Json;
using AiReview.Api.Contracts;

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
        app.MapPost("/webhooks/github", async (HttpRequest request) =>
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

            Console.WriteLine("Supported GitHub Pull Request Event:");
            Console.WriteLine($"Action: {webhook.Action}");
            Console.WriteLine($"Repo: {webhook.Repository.FullName}");
            Console.WriteLine($"PR: {webhook.PullRequest.Number}");

            return Results.Ok(new
            {
                accepted = true,
                action = webhook.Action,
                repository = webhook.Repository.FullName,
                pullRequestNumber = webhook.PullRequest.Number
            });
        });
    }
}