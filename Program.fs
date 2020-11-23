open System

module FizzBuzz =
    let getNumber = function
        | x when x % 3 = 0 && x % 5 = 0 -> "FizzBuzz"
        | x when x % 3 = 0 -> "Fizz"
        | x when x % 5 = 0 -> "Buzz"
        | x -> string x

    let getSeq x = 
        [1 .. x]
        |> Seq.map getNumber 

module Parser =
    let tryParse (x: string) = 
        match Int32.TryParse x with
        | true, d -> Some d
        | _ -> None

module Validate =
    let checkInputRange = function
        | x when x >= 1 && x <= 4000 -> Some x
        | _ -> None

module Result =
    let fromOption err = function
        | Some x -> Ok x
        | None -> Error err 

module Domain =
    type ParseNumber = string -> int option
    type ValidateNumber = int -> int option
    type GetFizzBuzzNumber = int -> string
    type GetFizzBuzzSeq = int -> seq<string>

    type ParserError = NotNumber 
    type ValidationError = InvalidNumber 

    type Error =
        | ParserError of ParserError
        | ValidationError of ValidationError

    type ExecuteWorkflow = string -> Result<string, Error>

    let execute 
        (parseNumber : ParseNumber)
        (validateNumber : ValidateNumber)
        (getFizzBuzzSeq: GetFizzBuzzSeq) =
        let parseNumber x =
            x
            |> parseNumber
            |> Result.fromOption NotNumber
            |> Result.mapError ParserError
        let validateNumber x =
            x
            |> validateNumber
            |> Result.fromOption InvalidNumber
            |> Result.mapError ValidationError
        fun x ->
            x
            |> parseNumber
            |> Result.bind validateNumber
            |> Result.map getFizzBuzzSeq

module Application =
    open Domain

    type Input = unit -> string
    type Output = string -> unit

    let execute =
        Domain.execute
            Parser.tryParse
            Validate.checkInputRange
            FizzBuzz.getSeq

    let application (input: Input) (output: Output) =
        fun () ->
            output "Please enter a digit greater than 0 and less or equal than 4000:"
            input ()
            |> execute
            |> function
                | Ok s -> 
                    sprintf "Output:\n%s" (s |> String.concat "\n")
                | Error (ParserError NotNumber) -> 
                    sprintf "Enter a digit!"
                | Error (ValidationError InvalidNumber) -> 
                    sprintf "Enter a digit from 1 to 3999!"
            |> output

[<EntryPoint>]
let main argv =
    Application.application Console.ReadLine Console.WriteLine ()
    0 // return an integer exit code
