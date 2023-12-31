﻿module CommandLineParser 
    open System
    open TodoConsoleApp.Models

    type Action =
        | Add
        | Edit
        | MarkDone
        | ClearAll
        | Show
        | ShowAll
        | Help

    type CommandLineOptions = {
        TodoId : int
        Title : string
        Description : string
        User : string
        Action: Action
    }

    let toUnvalidatedTodo (commandLineOptions : CommandLineOptions) : UnvalidatedTodo =
        {
            TodoId = commandLineOptions.TodoId
            Title = commandLineOptions.Title
            Description = commandLineOptions.Description
            User = commandLineOptions.User
            DateCompleted = Nullable()
        }

    let parseCommandLine args =
        match args with
        | "/a"::title::description::user::_ ->
            let commandLineOptions = {
                TodoId = 0
                Action = Add
                Title = title
                Description = description
                User = user
            }
            commandLineOptions

        | "/e"::todoId::title::description::user::_ ->
            let commandLineOptions = {
                    TodoId = int todoId
                    Action = Edit
                    Title = title
                    Description = description
                    User = user
            }
            commandLineOptions

        | "/m"::todoId::_  ->
            let commandLineOptions = {
                    TodoId = int todoId
                    Action = MarkDone
                    Title = ""
                    Description = ""
                    User = ""
            }
            commandLineOptions

        | "/c"::_  ->
            let commandLineOptions = {
                    TodoId = 0
                    Action = ClearAll
                    Title = ""
                    Description = ""
                    User = ""
            }
            commandLineOptions

        | "/s"::todoId::_  ->
            let commandLineOptions = {
                    TodoId = int todoId
                    Action = Show
                    Title = ""
                    Description = ""
                    User = ""
            }
            commandLineOptions

        | "/s"::_  ->
            let commandLineOptions = {
                    TodoId = 0
                    Action = ShowAll
                    Title = ""
                    Description = ""
                    User = ""
            }
            commandLineOptions

        | _ -> 
            let commandLineOptions = {
                    TodoId = 0
                    Action = Help
                    Title = ""
                    Description = ""
                    User = ""
            }
            commandLineOptions
