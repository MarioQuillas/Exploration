#I @"../packages/FSharp.Data.2.3.3/lib/net40"
#r "FSharp.Data.dll"
//#load some file
//#time 

open FSharp.Data
open System.Text.RegularExpressions

type TotalEuronext = HtmlProvider<"https://www.euronext.com/fr/products/equities/FR0000120271-XPAR">
