using AiReview.Engine;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "API is running");

app.MapPost("/review-files", (List<ReviewFileRequest> requests) =>
{
    var inputs = requests
        .Select(r => new Domain.ReviewInput(r.File, r.Diff))
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

app.MapPost("/webhooks/github", async (HttpRequest request) =>
{
    using var reader = new StreamReader(request.Body);
    var body = await reader.ReadToEndAsync();

    Console.WriteLine("Received GitHub webhook:");
    Console.WriteLine(body);

    return Results.Ok();
});

app.Run();

record ReviewFileRequest(string File, string Diff);