open TodoConsoleApp.Models
open TodoConsoleApp
open Todo
open Validation


let showHelp () =
    printfn "ConsoleTodoApp Help"
    printfn "Add Todo: /a [Title] [Description] [user]"
    printfn "Edit Todo: /e [Id] [title] [description] [user]"
    printfn "Complete Todo: /m [Id]"
    printfn "Clear All Todos: /c"
    printfn "Show Todo: /s [Id]"
    printfn "Show All Todos /s"
    printfn "Help: /h"


[<EntryPoint>]
let main args =
    let exitcode = 0

    let commandLineOptions =
        List.ofArray args 
        |> CommandLineParser.parseCommandLine 

    match commandLineOptions.Action with
    | CommandLineParser.Add ->
        let todo =
            commandLineOptions
            |> CommandLineParser.toUnvalidatedTodo
            |> validateTodo
            |> Result.mapError ValidationError

        match todo with
        | Ok todo ->
            match addTodo todo with
            | Ok _ ->
                printfn "Todo added!"
            | Error (CsvTodoRepository.CsvRepositoryError e) ->
                printfn "Error adding todo: %s!" e
        | Error (ValidationError e) ->
            printfn "Validation error: %s\nUse /h for help" e
    | CommandLineParser.Edit ->
        let todo = 
            commandLineOptions
            |> CommandLineParser.toUnvalidatedTodo
            |> validateTodo
            |> Result.mapError ValidationError

        match todo with
        | Ok todo ->
            match editTodo todo with
            | Ok _ ->
                printfn "Todo edited!"
            | Error (CsvTodoRepository.CsvRepositoryError e) ->
                printfn "Error editing todo: %s!" e
        | Error (ValidationError e) ->
            printfn "Validation error: %s\nUse /h for help" e
    | CommandLineParser.MarkDone ->
        let todoId = 
            commandLineOptions.TodoId
            |> TodoId.create "TodoId"
            |> Result.mapError ValidationError

        match todoId with
        | Ok todoId ->
            markDone todoId
        | Error (ValidationError e) ->
            printfn "Validation error: %s\nUse /h for help" e

    | CommandLineParser.ClearAll ->
        clearAll ()
    | CommandLineParser.Show ->
        let todoId = 
            commandLineOptions.TodoId
            |> TodoId.create "TodoId"
            |> Result.mapError ValidationError

        match todoId with
        | Ok todoId ->
            show todoId
        | Error (ValidationError e) ->
            printfn "Validation error: %s\nUse /h for help" e
    | CommandLineParser.ShowAll ->
        showAll()
    | CommandLineParser.Help ->
        showHelp()

    //match commandLineOptions.Action with
    //| CommandLineParser.Add ->
    //    let todo = 
    //        commandLineOptions
    //        |> CommandLineParser.toUnvalidatedTodo
    //        |> validateTodo
    //        |> Result.mapError ValidationError

    //    match todo with
    //    | Ok todo ->
    //        addTodo todo
    //    | Error (ValidationError e) ->
    //        printfn "Validation error: %s\nUse /h for help" e
    //| CommandLineParser.Edit ->
    //    let todo = 
    //        commandLineOptions
    //        |> CommandLineParser.toUnvalidatedTodo
    //        |> validateTodo
    //        |> Result.mapError ValidationError

    //    match todo with
    //    | Ok todo ->
    //        editTodo todo
    //    | Error (ValidationError e) ->
    //        printfn "Validation error: %s\nUse /h for help" e
    //| CommandLineParser.MarkDone ->
    //    let todoId = 
    //        commandLineOptions.TodoId
    //        |> TodoId.create "TodoId"
    //        |> Result.mapError ValidationError

    //    match todoId with
    //    | Ok todoId ->
    //        markDone todoId
    //    | Error (ValidationError e) ->
    //        printfn "Validation error: %s\nUse /h for help" e

    //| CommandLineParser.ClearAll ->
    //    clearAll ()
    //| CommandLineParser.Show ->
    //    let todoId = 
    //        commandLineOptions.TodoId
    //        |> TodoId.create "TodoId"
    //        |> Result.mapError ValidationError

    //    match todoId with
    //    | Ok todoId ->
    //        show todoId
    //    | Error (ValidationError e) ->
    //        printfn "Validation error: %s\nUse /h for help" e
    //| CommandLineParser.ShowAll ->
    //    showAll()
    //| CommandLineParser.Help ->
    //    showHelp()


    exitcode