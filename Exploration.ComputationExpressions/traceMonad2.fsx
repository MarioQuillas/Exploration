type TraceBuilder() =
    member this.Bind(m, f) = 
        match m with 
        | None -> 
            printfn "Binding with None. Exiting."
        | Some a -> 
            printfn "Binding with Some(%A). Continuing" a
        Option.bind f m

    member this.Return(x) = 
        printfn "Return an unwrapped %A as an option" x
        Some x

    member this.Zero() = 
        printfn "Zero"
        None

    member this.Combine (a,b) = 
        printfn "Returning early with %A. Ignoring second part: %A" a b 
        a

//    member this.Delay(f) = 
//        printfn "Delay"
//        f()
    member this.Delay(funcToDelay) = 
        let delayed = fun () ->
            printfn "%A - Starting Delayed Fn." funcToDelay
            let delayedResult = funcToDelay()
            printfn "%A - Finished Delayed Fn. Result is %A" funcToDelay delayedResult
            delayedResult  // return the result 

        printfn "%A - Delaying using %A" funcToDelay delayed
        delayed // return the new function

    member this.Run(funcToRun) = 
        printfn "%A - Run Start." funcToRun
        let runResult = funcToRun()
        printfn "%A - Run End. Result is %A" funcToRun runResult
        runResult // return the result of running the delayed function
// make an instance of the workflow  
let trace = new TraceBuilder()

trace { 
    printfn "Part 1: about to return 1"
    return 1
    printfn "Part 2: after return has happened"
    } |> printfn "Result for Part1 without Part2: %A" 
//
//let f = trace { 
//    printfn "Part 1: about to return 1"
//    return 1
//    printfn "Part 2: after return has happened"
//    } 
//f() |> printfn "Result for Part1 without Part2: %A"  