module TodoConsoleApp.CsvTodoRepository

open TodoConsoleApp.Models
open System.IO
open System

[<Literal>]
let commaReplacement = "|;|"

let csvFile = Path.Combine(__SOURCE_DIRECTORY__, "resources", "todos.csv")

type CsvRepositoryError = 
    | CsvRepositoryError of string
    | NotFound of int
    | AlreadyExists of int


/// Replace all commas with a placeholder value
let replaceCommas (input: string) (replacement : string) : string =
    input.Replace(",", replacement)

/// Replace the placeholder value with commas
let restoreCommas (input : string) (placeHolder : string) : string =
    input.Replace(placeHolder, ",")

/// Convert an unvalidated Todo to a CSV row
let convertUnvalidatedTodoToCsvRow (todo : UnvalidatedTodo) : string =
    let dateCompleted = 
        match todo.DateCompleted.HasValue with
        | false ->
            String.Empty
        | true -> 
            todo.DateCompleted.Value.ToString()

    sprintf "%i,%s,%s,%s,%s" 
        todo.TodoId
        (replaceCommas todo.Title commaReplacement)
        (replaceCommas todo.Description commaReplacement)
        (replaceCommas todo.User commaReplacement)
        dateCompleted

let parseTodoRow (csvRow : string) : Result<UnvalidatedTodo, CsvRepositoryError> =
    match csvRow.Split(",") with
    | [| todoId; title; desc; user; dateCompleted |] ->
        let (idSuccess, intId) =
            Int32.TryParse todoId

        let (dateSuccess, dateDateCompleted) = 
            DateTime.TryParse dateCompleted

        if not idSuccess then
            Error <| CsvRepositoryError "Failed to parse TodoId"
        elif not dateSuccess && dateCompleted <> String.Empty then
            Error <| CsvRepositoryError "Failed to parse DateCompleted"
        else
            Ok
                {
                    TodoId = intId
                    Title = restoreCommas title commaReplacement
                    Description = restoreCommas desc commaReplacement
                    User = restoreCommas user commaReplacement
                    DateCompleted = 
                        if dateSuccess then
                            dateDateCompleted |> Nullable
                        else
                            Nullable()
                }
    | _ -> 
        Error <| CsvRepositoryError "Failed to parse row"
    
// Get the next id for a new todo
let getNextId () =
    let allLines = File.ReadAllLines(csvFile)

    let allTodos = 
        allLines
        |> Array.map parseTodoRow
        |> Array.choose (function
            | Ok todo -> Some todo
            | _ -> None)
        |> Array.toList

    match allTodos with
    | [] -> 1
    | _ -> 
        allTodos
        |> List.maxBy (fun t -> t.TodoId)
        |> fun t -> t.TodoId + 1
    
/// Get a todo by id
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



/// Insert a new todo
let insertTodo (todo : Todo) =
    result {
        let todoId = getNextId()

        match getTodo todoId with
            | Some _ ->
                return! Error <| AlreadyExists todoId
            | None ->
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

let editTodo (todo : Todo) =
    result {
        let todoId = todo.TodoId |> TodoId.value

        let allTodos = 
            File.ReadAllLines(csvFile)
            |> Array.map parseTodoRow
            |> Array.choose (function
                | Ok todo -> Some todo
                | _ -> None)
            |> Array.toList

        let targetTodo =
            allTodos
            |> List.tryFind (fun t -> t.TodoId = todoId)

        match targetTodo with
        | None ->
            return! Error <| NotFound todoId
        | Some _ ->
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

            let updatedTodo : UnvalidatedTodo =
                {
                    TodoId = todoId
                    Title = title
                    Description = desc
                    User = user
                    DateCompleted = todo.DateCompleted |> Option.toNullable
                }

            let! validatedTodo = 
                Validation.validateTodo updatedTodo |> Result.mapError (fun err -> CsvRepositoryError err)

            let updatedAllTodos =
                allTodos
                |> List.filter (fun t -> t.TodoId <> todoId)
                |> List.append [ updatedTodo ]
                |> List.map convertUnvalidatedTodoToCsvRow
                |> List.toArray

            File.WriteAllLines(csvFile, updatedAllTodos)
    }

        