namespace AiReview.Engine

open AiReview.Engine.Domain

module ReviewEngine =

    let analyzeFile (input: ReviewInput) =
        Rules.all
        |> List.choose (fun rule -> rule input)

    let analyzeFiles (inputs: seq<ReviewInput>) =
        inputs
        |> Seq.collect analyzeFile
        |> Seq.toList

    let analyzeDiff (diff: string) =
        analyzeFile { File = "unknown"; Diff = diff }