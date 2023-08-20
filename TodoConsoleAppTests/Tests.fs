namespace TodoConsoleAppTests

open System
open Microsoft.VisualStudio.TestTools.UnitTesting
open TodoConsoleApp.CvsTodoRepository
open TodoConsoleApp.Models

[<TestClass>]
type TestClass () =

    let getTodo () : Result<Todo, string> =
        result {
            let! todoId = TodoId.create "todoId" 1
            let! title = Title.create "title" "Do the dishes"
            let! description = Description.create "description" "Wash the dished with warm soapy water, then rinse them, then dry them"
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
    member this.CanInsertTodo () =
        let todo = getTodo ()
            
        match todo with
        | Ok todoValue ->
            insertTodo todoValue |> Async.RunSynchronously
            Assert.IsTrue(false)
        | Error errorMsg ->
            Assert.Fail(errorMsg)

        Assert.IsTrue(false)
        