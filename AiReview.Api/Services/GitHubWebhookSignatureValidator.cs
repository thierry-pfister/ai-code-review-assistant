using System.Security.Cryptography;
using System.Text;

namespace AiReview.Api.Services;

public class GitHubWebhookSignatureValidator : IGitHubWebhookSignatureValidator
{
    private const string SignaturePrefix = "sha256=";

    private readonly IConfiguration _configuration;

    public GitHubWebhookSignatureValidator(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool IsValid(string payload, string? signatureHeader)
    {
        var secret = _configuration["GitHub:WebhookSecret"];

        if (string.IsNullOrWhiteSpace(secret))
            return false;

        if (string.IsNullOrWhiteSpace(signatureHeader))
            return false;

        if (!signatureHeader.StartsWith(SignaturePrefix))
            return false;

        var expectedSignature = CreateSignature(payload, secret);

        var expectedBytes = Encoding.UTF8.GetBytes(expectedSignature);
        var actualBytes = Encoding.UTF8.GetBytes(signatureHeader);

        return CryptographicOperations.FixedTimeEquals(expectedBytes, actualBytes);
    }

    private static string CreateSignature(string payload, string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(secretBytes);
        var hash = hmac.ComputeHash(payloadBytes);

        return $"{SignaturePrefix}{Convert.ToHexString(hash).ToLowerInvariant()}";
    }
}