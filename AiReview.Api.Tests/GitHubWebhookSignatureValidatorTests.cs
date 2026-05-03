using System.Security.Cryptography;
using System.Text;
using AiReview.Api.Services;
using Microsoft.Extensions.Configuration;

namespace AiReview.Api.Tests;

public class GitHubWebhookSignatureValidatorTests
{
    [Fact]
    public void AcceptsValidSignature()
    {
        var secret = "test-secret";
        var payload = """{"action":"opened"}""";
        var signature = CreateGitHubSignature(payload, secret);

        var validator = CreateValidator(secret);

        var result = validator.IsValid(payload, signature);

        Assert.True(result);
    }

    [Fact]
    public void RejectsInvalidSignature()
    {
        var secret = "test-secret";
        var payload = """{"action":"opened"}""";

        var validator = CreateValidator(secret);

        var result = validator.IsValid(payload, "sha256=invalid");

        Assert.False(result);
    }

    [Fact]
    public void RejectsMissingSignature()
    {
        var validator = CreateValidator("test-secret");

        var result = validator.IsValid("""{"action":"opened"}""", null);

        Assert.False(result);
    }

    [Fact]
    public void RejectsMissingSecret()
    {
        var configuration = new ConfigurationBuilder().Build();
        var validator = new GitHubWebhookSignatureValidator(configuration);

        var result = validator.IsValid("""{"action":"opened"}""", "sha256=abc");

        Assert.False(result);
    }

    private static GitHubWebhookSignatureValidator CreateValidator(string secret)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["GitHub:WebhookSecret"] = secret
            })
            .Build();

        return new GitHubWebhookSignatureValidator(configuration);
    }

    private static string CreateGitHubSignature(string payload, string secret)
    {
        var secretBytes = Encoding.UTF8.GetBytes(secret);
        var payloadBytes = Encoding.UTF8.GetBytes(payload);

        using var hmac = new HMACSHA256(secretBytes);
        var hash = hmac.ComputeHash(payloadBytes);

        return $"sha256={Convert.ToHexString(hash).ToLowerInvariant()}";
    }
}