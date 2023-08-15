open System
open TodoConsoleApp.Models
open Todo


let showHelp () =
    printfn "ConsoleTodoApp Help"
    printfn "Add Todo: /a [Title] [Description] [user]"
    printfn "Edit Todo: /e [Id] [title] {description} [user]"
    printfn "Complete Todo: /m [Id]"
    printfn "Clear All Todos: /c"
    printfn "Show Todo: /s [Id]"
    printfn "Show All Todos /s"
    printfn "Help: /h"

let validateTodo (unvalidatedTodo : UnvalidatedTodo) =
    result {
        let! todoId = 
            unvalidatedTodo.TodoId
            |> TodoId.create "TodoId"
        let! title =
            unvalidatedTodo.Title
            |> Title.create "Title"
        let! desc =
            unvalidatedTodo.Description
            |> Description.create "Description"
        let! user =
            unvalidatedTodo.User
            |> User.create "User"
        
        return {
            TodoId = todoId
            Title = title
            Description = desc
            User = user
            DateCompleted = None
        }
    }


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
            addTodo todo
        | Error _ ->
            showHelp ()
    | CommandLineParser.Edit ->
        let todo = 
            commandLineOptions
            |> CommandLineParser.toUnvalidatedTodo
            |> validateTodo
            |> Result.mapError ValidationError

        match todo with
        | Ok todo ->
            editTodo todo
        | Error _ ->
            showHelp ()
    | CommandLineParser.MarkDone ->
        let todoId = 
            commandLineOptions.TodoId
            |> TodoId.create "TodoId"
            |> Result.mapError ValidationError

        match todoId with
        | Ok todoId ->
            markDone todoId
        | Error _ ->
            showHelp ()

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
        | Error _ ->
            showHelp ()
    | CommandLineParser.ShowAll ->
        showAll()
    | CommandLineParser.Help ->
        showHelp()


    exitcode