using System.Text.Json;
using AiReview.Api.Contracts;

namespace AiReview.Api.Tests;

public class GitHubPullRequestFileResponseTests
{
    [Fact]
    public void DeserializesGitHubPullRequestFileResponse()
    {
        var json = """
        [
          {
            "filename": "AiReview.Api/Program.cs",
            "patch": "+ builder.Services.AddHttpClient<GitHubService>();"
          }
        ]
        """;

        var files = JsonSerializer.Deserialize<List<GitHubPullRequestFileResponse>>(json);

        Assert.NotNull(files);
        Assert.Single(files);

        var file = files[0];

        Assert.Equal("AiReview.Api/Program.cs", file.Filename);
        Assert.Equal("+ builder.Services.AddHttpClient<GitHubService>();", file.Patch);
    }
}