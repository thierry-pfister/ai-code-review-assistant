using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AiReview.Api.Contracts;

namespace AiReview.Api.Services;

public class GitHubService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public GitHubService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;

        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("AiReviewBot");

        var token = _configuration["GitHub:Token"];

        if (!string.IsNullOrWhiteSpace(token))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);
        }
    }

    public async Task<List<GitHubPullRequestFileResponse>> GetPullRequestFiles(
        string owner,
        string repo,
        int pullRequestNumber)
    {
        var url = $"https://api.github.com/repos/{owner}/{repo}/pulls/{pullRequestNumber}/files";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();

        var files = JsonSerializer.Deserialize<List<GitHubPullRequestFileResponse>>(json);

        return files ?? new List<GitHubPullRequestFileResponse>();
    }

    public async Task PostPullRequestComment(
        string owner,
        string repo,
        int pullRequestNumber,
        string body)
    {
        var url = $"https://api.github.com/repos/{owner}/{repo}/issues/{pullRequestNumber}/comments";

        var payload = JsonSerializer.Serialize(new
        {
            body
        });

        var content = new StringContent(payload, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, content);
        response.EnsureSuccessStatusCode();
    }
}