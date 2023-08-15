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

type ValidationError = ValidationError of string