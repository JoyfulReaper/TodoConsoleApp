namespace TodoConsoleAppTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open TodoConsoleApp.CsvTodoRepository
open TodoConsoleApp.Models

[<TestClass>]
type TestClass () =

    let getSampleTodo () : Result<Todo, string> =
        result {
            let! todoId = TodoId.create "todoId" 1
            let! title = Title.create "title" "Do the dishes"
            let! description = Description.create "description" "Wash the dishes with warm soapy water, then rinse them, then dry them"
            let! user = User.create "user" "Kyle"

            return 
                {
                    TodoId = todoId
                    Title = title
                    Description = description
                    User = user
                    DateCompleted = None
                }
        }

    [<TestMethod>]
    member this.CanReplaceCommas () =
        let test =
            replaceCommas "Do the dishes, then dry them" "|;|"

        Assert.AreEqual("Do the dishes|;| then dry them", test)


    [<TestMethod>]
    member this.CanInsertTodo () =
        let todo = getSampleTodo ()
            
        match todo with
        | Ok todoValue ->
            insertTodo todoValue |> Async.RunSynchronously
            Assert.IsTrue(false)
        | Error errorMsg ->
            Assert.Fail(errorMsg)

        Assert.IsTrue(false)

    [<TestMethod>]
    member this.CanGetTodo () =
        let todo = getTodo 1

        Assert.IsTrue(todo.IsSome)
        let todoValue = todo.Value

        Assert.AreEqual(1, todoValue.TodoId |> TodoId.value)
        
        