module Todo

open TodoConsoleApp.Models

let addTodo (todo : Todo ) =
    printfn "Add Todo: %A" todo

let editTodo (todo : Todo ) =
    printfn "Edit Todo: %A" todo

let markDone (todoId : TodoId ) =
    printfn "Mark Done Todo: %i" (todoId |> TodoId.value)

let clearAll () =
    printfn "clearAll"

let show (todoId : TodoId ) =
    printfn "Show Todo: %i" (todoId |> TodoId.value)

let showAll () =
    printfn "Show All"