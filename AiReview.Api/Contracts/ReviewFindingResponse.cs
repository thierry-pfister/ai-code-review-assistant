namespace AiReview.Api.Contracts;

public record ReviewFindingResponse(
    string Message,
    string Severity,
    string Category,
    string File,
    int? Line
);