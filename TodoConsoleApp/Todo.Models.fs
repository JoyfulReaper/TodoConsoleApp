namespace TodoConsoleApp.Models

open System

type Todo =
    {
        TodoId : TodoId
        Title : Title
        Description : Description option
        User: User
        DateCompleted : DateTime option
    }

type TodoAction =
    | Add of Todo
    | Edit of Todo
    | MarkDone of TodoId
    | ClearAll
    | Show of TodoId
    | ShowAll
    | Help

type ValidationError = ValidationError of string