open System
open TodoConsoleApp.Models

let debug = true

let showHelp () =
    printfn "ConsoleTodoApp Help"
    printfn "Add Todo: /a [Title] [Description] [user]"
    printfn "Edit Todo: /e [Id] [title] {description} [user]"
    printfn "Complete Todo: /m [Id]"
    printfn "Clear All Todos: /c"
    printfn "Show Todo: /s [Id]"
    printfn "Show All Todos /s"

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


//let parseTodo (args : string array) =
//    result {
//        let unvalidatedTodo : UnvalidatedTodo =
//            {
//                TodoId = 0
//                Title = args[1]
//                Description = args[2]
//                User = args[3]
//                DateCompleted = Nullable()
//            }

//        let! todo =
//            unvalidatedTodo 
//            |> validateTodo
//            |> Result.mapError ValidationError

//        let action = Add todo
//        return action
//    }


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
    | Ok _ ->
        printfn "Ok"
    | Error (ValidationError error) ->
        printfn "Error: %s" error

    //parseArgs args
    0