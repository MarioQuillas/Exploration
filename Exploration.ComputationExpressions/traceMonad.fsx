//let f(x ) =
//    match x with
//    |Some y -> printfn "%A" y
//    |None -> printfn "none"
//
//let y = Some 42
//let z = f y

type TraceBuilder() =
  member this.Bind(m, f) = 
    match m with 
    | None -> 
      printfn "Binding with None. Exiting."
    | Some a -> 
      printfn "Binding with Some(%A). Continuing" a
    Option.bind f m

  member this.Return(x) = 
      printfn "Returning a unwrapped %A as an option" x
      Some x

  member this.ReturnFrom(m) = 
    printfn "Returning an option (%A) directly" m
    m

//    member this.Zero() = 
//        printfn "Zero"
//        this.Return ()

  member this.Zero() = 
    printfn "Zero"
    None

  member this.Yield(x) = 
    printfn "Yield an unwrapped %A as an option" x
    Some x

  member this.YieldFrom(m) = 
    printfn "Yield an option (%A) directly" m
    m

  member this.Combine (a,b) = 
    match a,b with
    | Some a', Some b' ->
      printfn "combining %A and %A" a' b' 
      Some (a' + b')
    | Some a', None ->
      printfn "combining %A with None" a' 
      Some a'
    | None, Some b' ->
      printfn "combining None with %A" b' 
      Some b'
    | None, None ->
      printfn "combining None with None"
      None

  member this.Combine (a,b) = 
    printfn "Combining %A with %A" a b
    match a with
    | Some _ -> a  // a succeeds -- use it
    | None -> b    // a fails -- use b instead

  member this.Combine (a,b) = 
    printfn "Combining %A with %A" a b
    this.Bind( a, fun ()-> b )

//    member this.Combine (a,b) = 
//        printfn "Returning early with %A. Ignoring second part: %A" a b 
//        a

  member this.Delay(f) = 
    printfn "Delay"
    f()

//    member this.Delay(funcToDelay) = 
//        let delayed = fun () ->
//            printfn "%A - Starting Delayed Fn." funcToDelay
//            let delayedResult = funcToDelay()
//            printfn "%A - Finished Delayed Fn. Result is %A" funcToDelay delayedResult
//            delayedResult  // return the result 
//
//        printfn "%A - Delaying using %A" funcToDelay delayed
//        delayed // return the new function

// make an instance of the workflow  
let trace = new TraceBuilder()

let g = trace {
  let! b = Option.Some 1
  return b}

trace { 
  if true then printfn "hello......."
  if false then printfn ".......world"
  return 1 } |> printfn "Result for sequential combine: %A" 

type IntOrBool = I of int | B of bool

let parseInt s = 
    match System.Int32.TryParse(s) with
    | true,i -> Some (I i)
    | false,_ -> None

let parseBool s = 
    match System.Boolean.TryParse(s) with
    | true,i -> Some (B i)
    | false,_ -> None

trace { 
    return! parseBool "42"  // fails
    return! parseInt "42"
    } |> printfn "Result for parsing: %A" 


trace { 
    if true then printfn "hello"  //expression 1
    return 1                      //expression 2
    } |> printfn "Result for combine: %A" 

trace { 
    yield 1
    let! x = None
    yield 2
    } |> printfn "Result for yield then None: %A" 

trace { 
    yield 1
    yield 2
    } |> printfn "Result for yield then yield: %A" 

// test
trace { 
    printfn "hello world"
    } |> printfn "Result for simple expression: %A" 

let f = trace { 
    if false then return 1
    }// |> printfn "Result for if without else: %A" 

