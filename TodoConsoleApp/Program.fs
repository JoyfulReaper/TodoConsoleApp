open System

[<EntryPoint>]
let main args =
    Array.iter (fun a -> printfn "%O" a) args
    0