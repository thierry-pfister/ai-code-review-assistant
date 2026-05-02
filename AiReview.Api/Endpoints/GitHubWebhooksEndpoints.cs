namespace AiReview.Api.Endpoints;

public static class GitHubWebhookEndpoints
{
    public static void MapGitHubWebhookEndpoints(this WebApplication app)
    {
        app.MapPost("/webhooks/github", async (HttpRequest request) =>
        {
            using var reader = new StreamReader(request.Body);
            var body = await reader.ReadToEndAsync();

            Console.WriteLine("Received GitHub webhook:");
            Console.WriteLine(body);

            return Results.Ok();
        });
    }
}