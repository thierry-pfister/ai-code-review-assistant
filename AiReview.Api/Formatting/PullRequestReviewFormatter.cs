using AiReview.Api.Contracts;

namespace AiReview.Api.Formatting;

public static class PullRequestReviewFormatter
{
    public const string CommentMarker = "<!-- ai-code-review-assistant -->";

    public static string Format(IEnumerable<ReviewFindingResponse> findings)
    {
        var findingsList = findings.ToList();

        if (findingsList.Count == 0)
        {
            return $"""
            {CommentMarker}
            ## 🤖 AI Code Review Assistant

            No rule-based issues were found in this pull request.

            ✅ Looks good from the current automated checks.
            """;
        }

        var groupedByFile = findingsList
            .GroupBy(finding => finding.File)
            .OrderBy(group => group.Key);

        var lines = new List<string>
        {
            CommentMarker,
            "## 🤖 AI Code Review Assistant",
            "",
            $"Found **{findingsList.Count}** potential issue(s).",
            ""
        };

        foreach (var fileGroup in groupedByFile)
        {
            lines.Add($"### `{fileGroup.Key}`");
            lines.Add("");

            foreach (var finding in fileGroup)
            {
                var lineText = finding.Line is null
                    ? ""
                    : $" Line {finding.Line}.";

                lines.Add($"- **{finding.Severity} / {finding.Category}**: {finding.Message}{lineText}");
            }

            lines.Add("");
        }

        return string.Join(Environment.NewLine, lines);
    }
}