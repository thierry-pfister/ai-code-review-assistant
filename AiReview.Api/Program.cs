using AiReview.Engine;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running");

app.MapPost("/review-files", (List<ReviewFileRequest> requests) =>
{
    var inputs = requests
        .Select(r => new ReviewEngine.ReviewInput(r.File, r.Diff))
        .ToList();

    var findings = ReviewEngine.analyzeFiles(inputs);

    var response = findings.Select(finding => new
    {
        message = finding.Message,
        severity = finding.Severity.ToString(),
        category = finding.Category.ToString(),
        file = finding.File,
        line = finding.Line
    });

    return Results.Ok(response);
});

app.Run();

record ReviewFileRequest(string File, string Diff);