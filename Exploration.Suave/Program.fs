open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators

// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

let qsdf =  path "/hello" >=> OK "Hello, folks"
let titi =
  choose
    [ qsdf
      path "/goodbye" >=> OK "GoodByeeeee!!!"]

let routes =
  GET >=> titi

//let some1 url =
//  let some2 url headers =  [url, headers] |> String.concat ""
//  some2 url

//let some3 = some1 ""

[<EntryPoint>]
let main argv = 
  startWebServer defaultConfig routes
  0 // return an integer exit code
