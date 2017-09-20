// Mimic type classes with additional param

type 'a Num = {
    zero: 'a
    add: 'a -> 'a -> 'a
    mult: 'a -> 'a -> 'a
    fromInteger: int -> 'a }

let intNum = {
    zero = 0
    add = fun (a:int) b -> a + b
    mult = fun (a:int) b -> a * b
    fromInteger = id } 

let floatNum = {
    zero = 0.
    add = fun (a:float) b -> a + b
    mult = fun (a:float) b -> a * b
    fromInteger = float } 

let inc (d: 'a Num) x = d.add x (d.fromInteger 1)
let square (d: 'a Num) x = d.mult x x

let sum (d: 'a Num) = Seq.fold d.add d.zero

let sumofSquares (d: 'a Num) = Seq.map (square d) >> sum d

sumofSquares intNum [1;2;3] |> printfn "SumofSquares: %A"
sumofSquares floatNum [1.;2.;3.] |> printfn "SumofSquares: %A"
// sumofSquares intNum [1.;2.;3.] |> printfn "SumofSquares: %A" doesn't compile


// trying nested type classes

type 'a Eq = { equals : 'a -> 'a -> bool }


let intEq = { equals = fun (a:int) b -> a = b }
let floatEq = { equals = fun (a:float) b -> a = b }
let listEq (elementEq: 'a Eq) = { 
  equals = 
    let rec compare l1 l2 =
      match l1,l2 with
      | [],[] -> true
      | x::xs,y::ys -> elementEq.equals x y && compare xs ys
      | _ -> false
    compare } 

let l1 = [1;2;3]
let l2 = [1;2;4]
let l3 = [1;2]
let l4 = [1.;2.;3.]

printfn "%A = %A ? %b" l1 l1 ((listEq intEq).equals l1 l1)
printfn "%A = %A ? %b" l1 l2 ((listEq intEq).equals l1 l2)
printfn "%A = %A ? %b" l1 l3 ((listEq intEq).equals l1 l3)
// printfn "%A = %A ? %b" l1 l4 ((listEq intEq).equals l1 l4)  doesn't compile
 