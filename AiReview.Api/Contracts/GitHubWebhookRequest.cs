using System.Text.Json.Serialization;

namespace AiReview.Api.Contracts;

public class GitHubWebhookRequest
{
    [JsonPropertyName("action")]
    public string Action { get; set; } = default!;

    [JsonPropertyName("pull_request")]
    public PullRequest? PullRequest { get; set; }

    [JsonPropertyName("repository")]
    public Repository? Repository { get; set; }
}

public class PullRequest
{
    [JsonPropertyName("number")]
    public int Number { get; set; }
}

public class Repository
{
    [JsonPropertyName("full_name")]
    public string FullName { get; set; } = default!;
}