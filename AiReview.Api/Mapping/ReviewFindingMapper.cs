using AiReview.Api.Contracts;
using AiReview.Engine;
using Microsoft.FSharp.Core;

namespace AiReview.Api.Mapping;

public static class ReviewFindingMapper
{
    public static ReviewFindingResponse ToResponse(Domain.ReviewFinding finding)
    {
        return new ReviewFindingResponse(
            finding.Message,
            finding.Severity.ToString(),
            finding.Category.ToString(),
            finding.File,
            OptionModule.ToNullable(finding.Line)
        );
    }
}