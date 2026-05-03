namespace AiReview.Api.Services;

public interface IGitHubWebhookSignatureValidator
{
    bool IsValid(string payload, string? signatureHeader);
}