using AiReview.Engine;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running");

// Test endpoint
app.MapPost("/review", (string diff) =>
{
    var result = ReviewEngine.analyzeDiff(diff);
    return Results.Ok(new { review = result });
});

app.Run();