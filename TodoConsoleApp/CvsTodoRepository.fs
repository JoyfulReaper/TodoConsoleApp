module TodoConsoleApp.CvsTodoRepository

open TodoConsoleApp.Models
open System.IO
open System

let csvFile = Path.Combine(__SOURCE_DIRECTORY__, "resources", "todos.csv")

let insertTodo (todo : Todo) =
    async {
        File.AppendAllText(csvFile, Environment.NewLine + "TEST")
    }
    