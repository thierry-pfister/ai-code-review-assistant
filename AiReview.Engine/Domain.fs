namespace AiReview.Engine

module Domain =

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