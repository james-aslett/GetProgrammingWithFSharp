module printfn

open System

[<EntryPoint>]
let main argv =

    // %d - int

    // %f - float

    // %b - boolean

    // %s - string

    // %0 - the ToString() representation of the argument

    // %A - an F# pretty-print represenation of the argument that falls back to %0 if none exists

    let items = argv.Length
    //supply the args, space-separated, after the raw string
    printfn "Passed in %d items: %A" items argv
    0 // return an integer exit code
