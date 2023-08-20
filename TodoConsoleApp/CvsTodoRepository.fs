module TodoConsoleApp.CsvTodoRepository

open TodoConsoleApp.Models
open System.IO
open System

[<Literal>]
let commaReplacement = "|;|"

let csvFile = Path.Combine(__SOURCE_DIRECTORY__, "resources", "todos.csv")

type CvsRepositoryError = 
    | CvsRepositoryError of string
    | NotFound of int


/// Replace all commas with a placeholder value
let replaceCommas (input: string) (replacement : string) : string =
    input.Replace(",", replacement)

/// Replace the placeholder value with commas
let restoreCommas (input : string) (placeHolder : string) : string =
    input.Replace(placeHolder, ",")

let parseTodoRow (csvRow : string) : Result<UnvalidatedTodo, CvsRepositoryError> =
    match csvRow.Split(",") with
    | [| todoId; title; desc; user; dateCompleted |] ->
        let (idSuccess, intId) =
            Int32.TryParse todoId

        let (dateSuccess, dateDateCompleted) = 
            DateTime.TryParse dateCompleted

        if not idSuccess then
            Error <| CvsRepositoryError "Failed to parse TodoId"
        elif not dateSuccess && dateCompleted <> String.Empty then
            Error <| CvsRepositoryError "Failed to parse DateCompleted"
        else
            Ok
                {
                    TodoId = intId
                    Title = restoreCommas title commaReplacement
                    Description = restoreCommas desc commaReplacement
                    User = replaceCommas user commaReplacement
                    DateCompleted = 
                        if dateSuccess then
                            dateDateCompleted |> Nullable
                        else
                            Nullable()
                }
    | _ -> 
        Error <| CvsRepositoryError "Failed to parse row"
    

/// Insert a new todo
let insertTodo (todo : Todo) =
    async {
        let todoId = TodoId.value todo.TodoId

        let title =
            replaceCommas (Title.value todo.Title) commaReplacement
       
        let user =
            replaceCommas (User.value todo.User) commaReplacement

        let dateCompleted =
            match todo.DateCompleted with
            | None ->
                ""
            | Some date ->
                date.ToString()

        let desc =
            match todo.Description with
            | None -> 
                String.Empty
            | Some desc ->
                replaceCommas (Description.value desc) commaReplacement

        let todoString =
            String.Join(",", string todoId, title, desc, user, dateCompleted)

        let row =
            todoString.Substring(0, todoString.Length - 1)

        File.AppendAllText(csvFile, Environment.NewLine + row)
    }
    
let getTodo (id : int) : Todo option =
    use sr = new StreamReader (csvFile)

    let rec loop () =
        match sr.ReadLine() with
        | null ->
            None
        | line ->
            match parseTodoRow line with
            | Ok row when row.TodoId = id ->
                let validatedTodo = 
                    Validation.validateTodo row

                match validatedTodo with
                | Ok todo ->
                    Some todo
                | Error _ ->
                    None
            | _ ->
                loop ()

    loop ()