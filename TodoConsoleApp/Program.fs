open System
open TodoConsoleApp.Models

let debug = true

let showHelp () =
    printfn "ConsoleTodoApp Help"
    printfn "Add [title] {description} [user]"
    printfn "Edit [Id] [title] {description} [user]"
    printfn "Complete [Id]"
    printfn "ClearAll"
    printfn "Show [Id]"
    printfn "ShowAll"

let parseArgs (args : string array) =
    if debug then
        Array.iter (fun a -> printf "%O " a) args

    match args.Length with
    | 0 ->
        printfn "No option was given. Use help for help!"
    | _ ->
        match args.[0].ToUpper() with
        | "ADD" ->
            //let todo =
            //    {
            //        TodoId = TodoId.create "TodoId" 0
            //        Title = args.[1]
            //        Description = args.[2]
            //        User = args.[3]
            //        DateCompleted = None
            //    }
            //let action = Add 
            //action
            showHelp ()
        | "EDIT" ->
            showHelp ()
        | "MARKDONE" ->
            showHelp ()
        | "CLEARALL" ->
            showHelp ()
        | "SHOW" ->
            showHelp ()
        | "HELP" ->
            showHelp ()
        | _ ->
            printfn "Invalid Option!"

[<EntryPoint>]
let main args =
    parseArgs args
    0