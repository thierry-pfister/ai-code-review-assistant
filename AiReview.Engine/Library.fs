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
          Category: Category
          File: string
          Line: int option }

    let analyzeDiff (diff: string) =
        [
            if diff.Contains("password") then
                { Message = "Hardcoded password detected"
                  Severity = Critical
                  Category = Security
                  File = "unknown"
                  Line = None }

            if diff.Contains("console.log") then
                { Message = "console.log statement left in code"
                  Severity = Info
                  Category = BugRisk
                  File = "unknown"
                  Line = None }

            if diff.Contains("TODO") then
                { Message = "TODO comment left in code"
                  Severity = Warning
                  Category = Architecture
                  File = "unknown"
                  Line = None }
        ]