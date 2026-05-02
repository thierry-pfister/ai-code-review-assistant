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

    type ReviewInput =
        { File: string
          Diff: string }

    type ReviewFinding =
        { Message: string
          Severity: Severity
          Category: Category
          File: string
          Line: int option }

    let analyzeFile (input: ReviewInput) =
        [
            if input.Diff.Contains("password") then
                { Message = "Hardcoded password detected"
                  Severity = Critical
                  Category = Security
                  File = input.File
                  Line = None }

            if input.Diff.Contains("console.log") then
                { Message = "console.log statement left in code"
                  Severity = Info
                  Category = BugRisk
                  File = input.File
                  Line = None }

            if input.Diff.Contains("TODO") then
                { Message = "TODO comment left in code"
                  Severity = Warning
                  Category = Architecture
                  File = input.File
                  Line = None }
        ]

    let analyzeFiles (inputs: ReviewInput list) =
        inputs
        |> List.collect analyzeFile

    let analyzeDiff (diff: string) =
        analyzeFile { File = "unknown"; Diff = diff }