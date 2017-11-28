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
//-----------------------------
let t = seq {
  for i in 1 .. 10 do
    yield i}



 
//---------------------------------------------------------------------
//BAD
//let initialize(f) = f()
//let rec num = initialize (fun _ -> num + 1)

//GOOD
let memoize(f) =
  let cache = new System.Collections.Generic.Dictionary<_, _>()
  (fun x ->
    match cache.TryGetValue(x) with
    | true, v -> v
    | _ -> 
      let v = f(x)
      cache.Add(x, v)
      v)

let rec factorial = memoize(fun x ->
  printfn "Calculating factorial(%d)" x
  if (x <= 0) then 1 else x * factorial(x - 1))
//---------------------------------------------------------------------
let loop sequence iterations =
  let rec loop sequence i iterations =
    if i < iterations then
      printfn "%A" (sequence |> Seq.tryHead)
    loop (sequence |> Seq.skip(1)) (i+1) iterations
  loop sequence 0 iterations

let ttt:int = loop (seq{1..1..5}) 44

//-------------------------------------------
module Option =
  
  // The apply function for Options
  let apply fOpt xOpt = 
    match fOpt,xOpt with
    | Some f, Some x -> Some (f x)
    | _ -> None

  let (<*>) = apply 
  let (<!>) = Option.map

  let lift2 f x y = 
    f <!> x <*> y
        
  let lift3 f x y z = 
    f <!> x <*> y <*> z
        
  let lift4 f x y z w = 
    f <!> x <*> y <*> z <*> w

module Result =
  let apply fResult xResult = 
    match fResult,xResult with
    | Success f, Success x ->
      Ok (f x)
    | Failure errs, Success x ->
      Error errs
    | Success f, Failure errs ->
      Error errs
    | Failure errs1, Failure errs2 ->
      // concat both lists of errors
      Error (List.concat [errs1; errs2])
// Signature: Result<('a -> 'b)> -> Result<'a> -> Result<'b>

module List =
 
  // The apply function for lists
  // [f;g] apply [x;y] becomes [f x; f y; g x; g y]
  let apply (fList: ('a->'b) list) (xList: 'a list)  = 
    [ for f in fList do
      for x in xList do
        yield f x ]

  let (<!>) = List.map
  let (<*>) = apply

  let lift2 f x y = 
    f <!> x <*> y
        
  let lift3 f x y z = 
    f <!> x <*> y <*> z
        
  let lift4 f x y z w = 
    f <!> x <*> y <*> z <*> w
  
  let ( <* ) x y = 
    lift2 (fun left right -> left) x y 

  let ( *> ) x y = 
    lift2 (fun left right -> right) x y 
  
  /// Map a Result producing function over a list to get a new Result 
  /// using applicative style
  /// ('a -> Result<'b>) -> 'a list -> Result<'b list>
  let rec traverseResultA (f: 'a -> Result<'b, _>) list =
    // define the applicative functions
    let (<*>) = Result.apply
    let retn = Result.Ok

    // define a "cons" function
    let cons head tail = head :: tail

    // loop through the list
    match list with
    | [] -> 
      // if empty, lift [] to a Result
      retn []
    | head::tail ->
      // otherwise lift the head to a Result using f
      // and cons it with the lifted version of the remaining list
      retn cons <*> (f head) <*> (traverseResultA f tail)


  /// Map a Result producing function over a list to get a new Result 
  /// using monadic style
  /// ('a -> Result<'b>) -> 'a list -> Result<'b list>
  let rec traverseResultM f list =

    // define the monadic functions
    let (>>=) x f = Result.bind f x
    let retn = Result.Ok

    // define a "cons" function
    let cons head tail = head :: tail

    // loop through the list
    match list with
    | [] -> 
      // if empty, lift [] to a Result
      retn []
    | head::tail ->
      // otherwise lift the head to a Result using f
      // then lift the tail to a Result using traverse
      // then cons the head and tail and return it
      f head                 >>= (fun h -> 
      traverseResultM f tail >>= (fun t ->
      retn (cons h t) ))



let add x y = x + y
let toto = (Some 3) |> Option.apply ((Some 2)|> Option.map add)
toto
let resultOption2 =  
  let (<!>) = Option.map
  let (<*>) = Option.apply

  add <!> (Some 2) <*> (Some 3)
// resultOption2 = Some 5

let resultList2 =  
  let (<!>) = List.map
  let (<*>) = List.apply

  add <!> [1;2] <*> [10;20]

let batman = 
  let (<!>) = List.map
  let (<*>) = List.apply

  // string concatenation using +
  (+) <!> ["bam"; "kapow"; "zap"] <*> ["!"; "!!"]  

// resultList2 = [11; 21; 12; 22]    
// result =
// ["bam!"; "bam!!"; "kapow!"; "kapow!!"; "zap!"; "zap!!"]

module Async = 

  let map f xAsync = async {
    // get the contents of xAsync 
    let! x = xAsync 
    // apply the function and lift the result
    return f x
    }

  let retn x = async {
    // lift x to an Async
    return x
    }

  let apply fAsync xAsync = async {
    // start the two asyncs in parallel
    let! fChild = Async.StartChild fAsync
    let! xChild = Async.StartChild xAsync

    // wait for the results
    let! f = fChild
    let! x = xChild 

    // apply the function to the results
    return f x 
    }

  let bind f xAsync = async {
    // get the contents of xAsync 
    let! x = xAsync 
    // apply the function but don't lift the result
    // as f will return an Async
    return! f x
    }