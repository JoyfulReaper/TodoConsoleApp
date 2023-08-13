namespace TodoConsoleApp

open System

module ConstraintedType =
    
    // Create a constrained integer using the constructor provided
    // Return Error if input is less than minVal or more than maxVal
    let createInt fieldName ctor minVal maxVal i =
        if i < minVal then
            let msg = sprintf "%s: Must not be less than %i" fieldName minVal
            Error msg
        elif i > maxVal then
            let msg = sprintf "%s: Must not be greated than %i" fieldName maxVal
            Error msg
        else
            Ok (ctor i)

type TodoId = private TodoId of int
module TodoId =
    let create fieldName v =
        ConstraintedType.createInt fieldName TodoId 1 Int32.MaxValue
        
    let value (TodoId todoId) = todoId