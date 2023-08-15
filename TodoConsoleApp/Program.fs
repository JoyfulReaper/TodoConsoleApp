open System
open TodoConsoleApp.Models
open Todo

let debug = true

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
    let commandLineOptions = List.ofArray args |> CommandLineParser.parseCommandLine 

    if debug then
        match commandLineOptions.Action with
        | CommandLineParser.Add ->
            printfn "Add"
        | CommandLineParser.Edit ->
            printfn "Edit"
        | CommandLineParser.MarkDone ->
            printfn "MarkDone"
        | CommandLineParser.ClearAll ->
            printfn "ClearAll"
        | CommandLineParser.Show ->
            printfn "Show"
        | CommandLineParser.ShowAll ->
            printfn "ShowAll"
        | CommandLineParser.Help ->
            printfn "Help"

    let unvalidatedTodo = 
        CommandLineParser.toUnvalidatedTodo commandLineOptions

    let validatedTodo = 
        unvalidatedTodo
        |> validateTodo
        |> Result.mapError ValidationError

    match validatedTodo with
    | Ok todo ->
        printfn "Ok"

        match commandLineOptions.Action with
        | CommandLineParser.Add ->
            addTodo todo
        | CommandLineParser.Edit ->
            editTodo todo
        | CommandLineParser.MarkDone ->
            markDone todo.TodoId
        | CommandLineParser.ClearAll ->
            clearAll ()
        | CommandLineParser.Show ->
            show todo.TodoId
        | CommandLineParser.ShowAll ->
            showAll ()
        | _ ->
            showHelp ()

    | Error (ValidationError error) ->
        printfn "Error: %s" error
        System.Environment.Exit 1



    //parseArgs args
    0