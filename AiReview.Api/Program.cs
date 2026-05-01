using AiReview.Engine;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running");

app.MapPost("/review", (ReviewRequest request) =>
{
    var findings = ReviewEngine.analyzeDiff(request.Diff);

    var response = findings.Select(finding => new
    {
        message = finding.Message,
        severity = finding.Severity.ToString(),
        category = finding.Category.ToString()
    });

    return Results.Ok(response);
});

app.Run();

record ReviewRequest(string Diff);