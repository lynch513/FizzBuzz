open System

let getFizzBuzz = function
    | x when x % 3 = 0 && x % 5 = 0 -> "FizzBuzz"
    | x when x % 3 = 0 -> "Fizz"
    | x when x % 5 = 0 -> "Buzz"
    | x -> string x

let readIntFromUser () =
    let x = Console.ReadLine ()  
    match Int32.TryParse x with
    | true, d -> Some d
    | _ -> None

let checkUserInt = function
    | x when x >= 1 && x <= 4000 -> Some x
    | _ -> None

[<EntryPoint>]
let main argv =
    printfn "Please enter a digit greater than 0 and less or equal than 4000:"
    let x = readIntFromUser ()  
    printfn "Output:"
    match x with
    | Some x ->
        match checkUserInt x with
        | Some x ->
            [1 .. x]
            |> Seq.map getFizzBuzz
            |> Seq.iter (printfn "%s")
        | _ -> printfn "Enter a digit from 1 to 3999!"
    | _ -> printfn "Enter a digit!"
    0 // return an integer exit code
