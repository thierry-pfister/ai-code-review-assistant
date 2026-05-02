namespace AiReview.Engine

open AiReview.Engine.Domain

module Rules =

    let passwordRule (input: ReviewInput) =
        if input.Diff.Contains("password") then
            Some
                { Message = "Hardcoded password detected"
                  Severity = Critical
                  Category = Security
                  File = input.File
                  Line = None }
        else
            None

    let consoleLogRule (input: ReviewInput) =
        if input.Diff.Contains("console.log") then
            Some
                { Message = "console.log statement left in code"
                  Severity = Info
                  Category = BugRisk
                  File = input.File
                  Line = None }
        else
            None

    let todoRule (input: ReviewInput) =
        if input.Diff.Contains("TODO") then
            Some
                { Message = "TODO comment left in code"
                  Severity = Warning
                  Category = Architecture
                  File = input.File
                  Line = None }
        else
            None

    let all =
        [ passwordRule
          consoleLogRule
          todoRule ]