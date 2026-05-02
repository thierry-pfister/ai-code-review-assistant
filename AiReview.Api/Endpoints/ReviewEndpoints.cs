using AiReview.Engine;
using AiReview.Api.Contracts;

namespace AiReview.Api.Endpoints;

public static class ReviewEndpoints
{
    public static void MapReviewEndpoints(this WebApplication app)
    {
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
    }
}