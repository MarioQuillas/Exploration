

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