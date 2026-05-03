using System.Text.Json.Serialization;

namespace AiReview.Api.Contracts;

public class GitHubIssueCommentResponse
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("body")]
    public string Body { get; set; } = default!;
}