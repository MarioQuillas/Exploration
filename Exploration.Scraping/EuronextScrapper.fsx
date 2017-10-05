#I @"../packages/FSharp.Data.2.4.1/lib/net40"
#r "FSharp.Data.dll"
//#load some file
//#time 

open FSharp.Data
open System.Text.RegularExpressions

#load "../packages/FSharp.Charting.0.91.1/lib/net45/FSharp.Charting.fsx"
open FSharp.Charting

//type TotalEuronext_1 =
//  HtmlProvider<"https://www.euronext.com/fr/products/equities/FR0000120271-XPAR">

let array1 = [| 1; 2; 3; 4; 5; 6 |]
let list1 = [ 2; 4; 6; 8; 10; 12; 14; 16 ]

type TotalEuronext =
  HtmlProvider<"https://www.euronext.com/fr/nyx_eu_listings/real-time/quote?isin=FR0000120271&mic=XPAR">

let scrapRealTime = TotalEuronext.GetSample()

let divs = scrapRealTime.Html.Descendants["div"]
let spans = scrapRealTime.Html.Descendants["span"]

let totalDivs = Seq.length divs
let totalSpans = Seq.length spans

let priceToto = 
  spans 
  |> Seq.choose (fun x -> 
    x.TryGetAttribute("id") 
    |> Option.map(fun a ->  a.Value(), x.InnerText())
    )

let price =
  spans
  |> Seq.filter (fun node -> 
    let optionIdValue = 
      node.TryGetAttribute("id") 
      |> Option.map(fun attribute -> attribute.Value() )

    if optionIdValue.IsNone then
      false
    else
      System.String.Equals(optionIdValue.Value ,"price")
    )
  |> Seq.map(fun node -> node.InnerText())
  |> Seq.tryHead

scrapRealTime.Tables.Table6.Rows

scrapRealTime.Tables.Table1.Headers
scrapRealTime.Tables.Table1.Rows
scrapRealTime.Tables.Table1.Name

scrapRealTime.Tables.Table2.Headers
scrapRealTime.Tables.Table2.Rows
scrapRealTime.Tables.Table2.Name

scrapRealTime.Tables.Table3.Headers
scrapRealTime.Tables.Table3.Rows
scrapRealTime.Tables.Table3.Name

scrapRealTime.Tables.Table4.Headers
scrapRealTime.Tables.Table4.Rows
scrapRealTime.Tables.Table4.Name

scrapRealTime.Tables.Table5.Headers
scrapRealTime.Tables.Table5.Rows
scrapRealTime.Tables.Table5.Name

scrapRealTime.Tables.Table6.Headers
scrapRealTime.Tables.Table6.Rows
scrapRealTime.Tables.Table6.Name

//scrapRealTime.Tables.Table7.Headers
//scrapRealTime.Tables.Table7.Rows
//scrapRealTime.Tables.Table7.Name

//let htmlTotalEuronext =
//  HtmlDocument.Load("https://www.euronext.com/fr/nyx_eu_listings/real-time/quote?isin=FR0000120271&mic=XPAR")

//let toto = htmlTotalEuronext.Descendants

//let toto = TotalEuronext.GetSample()
//toto.Lists.List10.Html

//let qsdf = TotalEuronext().Lists.TOTAL.Values

type NugetStats = 
  HtmlProvider<"https://www.nuget.org/packages/FSharp.Data">
// load the live package stats, you get intellisense for this
let rawStats = NugetStats().Lists.Dependencies.Values
let rawStats2 = NugetStats().Tables.``Version History``
// note the backticks (``) for writing natural text in F#!

//let parsedList =
//  (lovelyPageDoc.CssSelect ".portlet.clearfix.reorderableModule td > font") // using a css selector on a .Parse()ed webpage
//              |> Seq.map (fun (x:HtmlNode) -> x.InnerText()) //get text of each element
//              |> Seq.skip 26 //skip some elements
//              |> Seq.pairwise //convert to pairs (in tuples)
//              |> Seq.indexed //pair with indicies
//              |> Seq.filter (fun (i, x) -> i % 2 <> 0) //get rid of every second element
//              |> Seq.map snd //get the second element of every pairing
//              |> Seq.toList //convert to list