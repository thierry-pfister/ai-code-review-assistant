using System.Text.Json;
using AiReview.Api.Contracts;

namespace AiReview.Api.Endpoints;

public static class GitHubWebhookEndpoints
{
    public static void MapGitHubWebhookEndpoints(this WebApplication app)
    {
        app.MapPost("/webhooks/github", async (HttpRequest request) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            var webhook = JsonSerializer.Deserialize<GitHubWebhookRequest>(body);

            if (webhook == null)
                return Results.BadRequest();

            Console.WriteLine("GitHub Event Received:");
            Console.WriteLine($"Action: {webhook.Action}");
            Console.WriteLine($"Repo: {webhook.Repository?.FullName}");
            Console.WriteLine($"PR: {webhook.PullRequest?.Number}");

            return Results.Ok();
        });
    }
}