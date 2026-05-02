using AiReview.Api.Endpoints;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running");

app.MapReviewEndpoints();
app.MapGitHubWebhookEndpoints();

app.Run();