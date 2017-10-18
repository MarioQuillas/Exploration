type toto<'a> = Tt of 'a

type ff = toto<string>
type dd = string toto

let dodo (f:ff) (g:dd) =
    printfn "%s" "qsdf"
//
//let x = Tt "AE"
//let y = 

type Suit = Club | Diamond | Spade | Heart
type Rank = Two | Three | Four | Five | Six | Seven | Eight 
            | Nine | Ten | Jack | Queen | King | Ace

let aceHearts = Heart, Ace
let twoHearts = Heart, Two
let aceSpades = Spade, Ace

let evens qsdf=
  qsdf |> List.filter(fun x -> x % 2 = 0)


let printRandomNumbersUntilMatched matchValue maxValue =
  let randomNumberGenerator = new System.Random()
  let sequenceGenerator _ = randomNumberGenerator.Next(maxValue)
  let isNotMatch = (<>) matchValue

  //create and process the sequence of rands
  Seq.initInfinite sequenceGenerator 
    |> Seq.takeWhile isNotMatch
    |> Seq.iter (printf "%d ")

  // done
  printfn "\nFound a %d!" matchValue

//test
printRandomNumbersUntilMatched 10 20

//----------------------------------------------------------------
let (|Even|Odd|) input = if input % 2 = 0 then Even else Odd

let TestNumber input =
  match input with
  | Even -> printfn "%d is even" input
  | Odd -> printfn "%d is odd" input

TestNumber 7
TestNumber 11
TestNumber 32
//----------------------------------------------------------------

let (|Success|Failure|) =
  function 
  | Choice1Of2 s -> Success s
  | Choice2Of2 f -> Failure f

/// convert a single value into a two-track result
let succeed x = Choice1Of2 x

/// convert a single value into a two-track result
let fail x = Choice2Of2 x

// apply either a success function or failure function
let either successFunc failureFunc twoTrackInput =
  match twoTrackInput with
  | Success s -> successFunc s
  | Failure f -> failureFunc f

// convert a switch function into a two-track function
let bind f = 
  either f fail

let carbonate factor label i = 
  if i % factor = 0 then
    succeed label
  else
    fail i

let connect f = 
  function
  | Success x -> succeed x 
  | Failure i -> f i

let connect' f = 
  either succeed f

let fizzBuzz = 
  carbonate 15 "FizzBuzz"      // need the 15-FizzBuzz rule because of short-circuit
  >> connect (carbonate 3 "Fizz")
  >> connect (carbonate 5 "Buzz")
  >> either (printf "%s; ") (printf "%i; ")

// test
[1..100] |> List.iter fizzBuzz