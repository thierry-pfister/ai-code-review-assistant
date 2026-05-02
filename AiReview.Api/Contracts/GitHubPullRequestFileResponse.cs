using System.Text.Json.Serialization;

namespace AiReview.Api.Contracts;

public class GitHubPullRequestFileResponse
{
    [JsonPropertyName("filename")]
    public string Filename { get; set; } = default!;

    [JsonPropertyName("patch")]
    public string? Patch { get; set; }
}