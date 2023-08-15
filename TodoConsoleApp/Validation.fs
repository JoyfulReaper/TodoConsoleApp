module Validation 
    open TodoConsoleApp.Models

    let validateTodo (unvalidatedTodo : UnvalidatedTodo) : Result<Todo,string> =
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