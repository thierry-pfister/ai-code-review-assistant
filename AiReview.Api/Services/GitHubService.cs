using System.Text.Json;

namespace AiReview.Api.Services;

public class GitHubService
{
    private readonly HttpClient _httpClient;

    public GitHubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("AiReviewBot");
    }

    public async Task<List<GitHubFile>> GetPullRequestFiles(
        string owner,
        string repo,
        int pullRequestNumber)
    {
        var url = $"https://api.github.com/repos/{owner}/{repo}/pulls/{pullRequestNumber}/files";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var files = JsonSerializer.Deserialize<List<GitHubFile>>(json);

        return files ?? new List<GitHubFile>();
    }
}

public class GitHubFile
{
    public string Filename { get; set; } = default!;
    public string? Patch { get; set; }
}