using AiReview.Api.Endpoints;
using AiReview.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<GitHubService>();

var app = builder.Build();

app.MapGet("/", () => "API is running");

app.MapReviewEndpoints();
app.MapGitHubWebhookEndpoints();

app.Run();