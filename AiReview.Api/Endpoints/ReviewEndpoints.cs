using AiReview.Api.Contracts;
using AiReview.Api.Mapping;
using AiReview.Engine;

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

            var response = findings.Select(ReviewFindingMapper.ToResponse);

            return Results.Ok(response);
        });
    }
}