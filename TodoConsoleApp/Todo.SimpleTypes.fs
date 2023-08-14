namespace TodoConsoleApp.Models

open System

type UnvalidatedTodo = {
    TodoId : int
    Title : string
    Description : string
    User : string
    DateCompleted : Nullable<DateTime>
}

// Simple Types and constrained types

type TodoId = private TodoId of int

type Title = private Title of string

type Description = private Description of string

type User = private User of string

/////////////////////////////////////////////////////

module ConstrainedType =
    let createString fieldName ctor maxLen str =
        if String.IsNullOrEmpty(str) then
            let msg = sprintf "%s must not be null or empty" fieldName
            Error msg
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i characters" fieldName maxLen
            Error msg
        else
            Ok (ctor str)

    let createStringOption fieldName ctor maxLen str = 
        if String.IsNullOrEmpty(str) then
            Ok None
        elif str.Length > maxLen then
            let msg = sprintf "%s must not be more than %i characters" fieldName maxLen 
            Error msg 
        else
            Ok (ctor str |> Some)

    let createInt fieldName ctor minVal maxVal i =
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %i" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greater than %i" fieldName maxVal
            Error msg
        else
            Ok (ctor i)

/////////////////////////////////////////////////////

module TodoId =
    let value (TodoId id) = id

    let create fieldName v =
        ConstrainedType.createInt fieldName TodoId 0 Int32.MaxValue v

module Description =
    let value (Description desc) = desc

    let create fieldName v =
        ConstrainedType.createStringOption fieldName Description 100 v

module User =
    let value (User user) = User

    let create fieldName v =
        ConstrainedType.createString fieldName User 50 v

module Title =
    let value (Title title) = Title

    let create fieldName v =
        ConstrainedType.createString fieldName Title 3 v // TODO set back to 30 when done testing