namespace AiReview.Engine

module ReviewEngine =

    let analyzeDiff (diff: string) =
        if diff.Contains("password") then
            "⚠️ Potential security risk: hardcoded password detected."
        else
            "✅ No obvious issues found."