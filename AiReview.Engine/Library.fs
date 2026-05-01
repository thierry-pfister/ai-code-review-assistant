namespace AiReview.Engine

module ReviewEngine =

    type Severity =
        | Info
        | Warning
        | Critical

    type Category =
        | Security
        | BugRisk
        | Architecture

    type ReviewFinding =
        { Message: string
          Severity: Severity
          Category: Category }

    let analyzeDiff (diff: string) =
        [
            if diff.Contains("password") then
                { Message = "Hardcoded password detected"
                  Severity = Critical
                  Category = Security }

            if diff.Contains("console.log") then
                { Message = "console.log statement left in code"
                  Severity = Info
                  Category = BugRisk }
        ]