
type ListBuilder() =
    member this.Bind(m, f) = 
        // List.collect is the bind method for lists
        m |> List.collect f

    member this.Zero() = 
        printfn "Zero"
        []
        
    member this.Yield(x) = 
        printfn "Yield an unwrapped %A as a list" x
        [x]

    member this.YieldFrom(m) = 
        printfn "Yield a list (%A) directly" m
        m

    member this.For(m,f) =
        printfn "For %A" m
        this.Bind(m,f)
        
    member this.Combine (a,b) = 
        printfn "combining %A and %A" a b 
        List.concat [a;b]

    member this.Delay(f) = 
        printfn "Delay"
        f()

// make an instance of the workflow  
let listbuilder = new ListBuilder()

let f = listbuilder { 
    yield 1
    yield 2
    } //|> printfn "Result for yield then yield: %A" 

listbuilder { 
    yield 1
    yield! [2;3]
    } |> printfn "Result for yield then yield! : %A" 

listbuilder { 
    for i in ["red";"blue"] do
        yield i
        for j in ["hat";"tie"] do
            yield! [i + " " + j;"-"]
    } |> printfn "Result for for..in..do : %A" 

listbuilder { 
    yield 1
    yield 2
    yield 3
    yield 4
    } |> printfn "Result for yield x 4: %A" 

