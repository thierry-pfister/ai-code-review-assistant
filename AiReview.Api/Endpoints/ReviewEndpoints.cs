using AiReview.Api.Contracts;
using AiReview.Engine;
using Microsoft.FSharp.Core;

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

            var response = findings.Select(finding =>
                new ReviewFindingResponse(
                    finding.Message,
                    finding.Severity.ToString(),
                    finding.Category.ToString(),
                    finding.File,
                    OptionModule.ToNullable(finding.Line)
                )
            );

            return Results.Ok(response);
        });
    }
}