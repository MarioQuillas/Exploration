//#if INTERACTIVE
//let msg = "Interactive"
//#else
//let msg = "Not Interactive"
//#endif
//
//printfn "%s" msg

//#I ".."

#I "../packages/FSharp.Charting.0.91.1/lib/net45"
#I "../packages/Deedle.1.2.5"
#load "FSharp.Charting.fsx"
#load "Deedle.fsx"

open System
open Deedle
open FSharp.Charting

(**
qsdf
=================

qsdfqsfqsdfqd
sdqdfq

ppp
---------

sdfgsf
*)
let dates  = 
  [ DateTime(2013,1,1); 
    DateTime(2013,1,4); 
    DateTime(2013,1,8) ]
let values = 
  [ 10.0; 20.0; 30.0 ]
(*** include-value:dates ***)
// Create from a single list of observations

(*** define-output: toto1 ***)
Series.ofObservations
  [ DateTime(2013,1,1) => 10.0
    DateTime(2013,1,4) => 20.0
    DateTime(2013,1,8) => 30.0 ]
(*** include-it: toto1***)

let dateRange (first:System.DateTime) count = seq {    
    for i in 0 .. (count - 1) -> first.AddDays(float i)
}
//let tto = dateRange System.DateTime.Now 10
//tto |> Seq.map (fun x -> printfn "%O" x)
//let d = for x in tto do
//            printfn "%O" x

let rand count =
    let rnd = System.Random()
    seq {
        for i in 0 .. (count - 1) do
            yield rnd.NextDouble()
    }

let first = Series(dates, values)
let second = Series (dateRange (DateTime (2013,1,1)) 10, rand 10)

let df1 = Frame(["first"; "second"], [first; second])

let sourceDirectory = "C:\MyGithub\Sniper\Sniper.DeedleTuto"

let msftCsv = Frame.ReadCsv(sourceDirectory + "/data/stocks/MSFT.csv")
let fbCsv = Frame.ReadCsv(sourceDirectory + "/data/stocks/FB.csv")

// Use the Date column as the index & order rows
let msftOrd = 
  msftCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey

// Create data frame with just Open and Close prices
let msft = msftOrd.Columns.[ ["Open"; "Close"] ]

// Add new column with the difference between Open & Close
msft?Difference <- msft?Open - msft?Close

// Do the same thing for Facebook
let fb = 
  fbCsv
  |> Frame.indexRowsDate "Date"
  |> Frame.sortRowsByKey
  |> Frame.sliceCols ["Open"; "Close"]
fb?Difference <- fb?Open - fb?Close

// Now we can easily plot the differences
Chart.Combine
  [ Chart.Line(msft?Difference |> Series.observations) 
    Chart.Line(fb?Difference |> Series.observations) ]

// Change the column names so that they are unique
let msftNames = ["MsftOpen"; "MsftClose"; "MsftDiff"]
let msftRen = msft |> Frame.indexColsWith msftNames

let fbNames = ["FbOpen"; "FbClose"; "FbDiff"]
let fbRen = fb |> Frame.indexColsWith fbNames

// Outer join (align & fill with missing values)
let joinedOut = msftRen.Join(fbRen, kind=JoinKind.Outer)

// Inner join (remove rows with missing values)
let joinedIn = msftRen.Join(fbRen, kind=JoinKind.Inner)

// Visualize daily differences on available values only
Chart.Rows
  [ Chart.Line(joinedIn?MsftDiff |> Series.observations) 
    Chart.Line(joinedIn?FbDiff |> Series.observations) ]

// Look for a row at a specific date
joinedIn.Rows.[DateTime(2013, 1, 2)]
//val it : ObjectSeries<string> =
//  FbOpen    -> 28.00            
//  FbClose   -> 27.44   
//  FbDiff    -> -0.5599 
//  MsftOpen  -> 27.62   
//  MsftClose -> 27.25    
//  MsftDiff  -> -0.3700 

// Get opening Facebook price for 2 Jan 2013
joinedIn.Rows.[DateTime(2013, 1, 2)]?FbOpen
//val it : float = 28.0

// Get values for the first three days of January 2013
let janDates = [ for d in 2 .. 4 -> DateTime(2013, 1, d) ]
let jan234 = joinedIn.Rows.[janDates]

// Calculate mean of Open price for 3 days
jan234?MsftOpen |> Stats.mean

// Get values corresponding to entire January 2013
let jan = joinedIn.Rows.[DateTime(2013, 1, 1) .. DateTime(2013, 1, 31)] 

// Calculate means over the period
jan?FbOpen |> Stats.mean
jan?MsftOpen |> Stats.mean

let daysSeries = Series(dateRange DateTime.Today 10, rand 10)
let obsSeries = Series(dateRange DateTime.Now 10, rand 10)

//The indexing operation written as daysSeries.[date] uses exact semantics so it will fail if 
//the exact date is not available.
//When using Get method, we can provide an additional parameter to 
//specify the required behaviour:

// Fails, because current time is not present
try daysSeries.[DateTime.Now] with _ -> nan
try obsSeries.[DateTime.Now] with _ -> nan

// This works - we get the value for DateTime.Today (12:00 AM)
daysSeries.Get(DateTime.Now, Lookup.ExactOrSmaller)
// This does not - there is no nearest key <= Today 12:00 AM
try obsSeries.Get(DateTime.Today, Lookup.ExactOrSmaller)
with _ -> nan

let daysFrame = [ 1 => daysSeries ] |> Frame.ofColumns
let obsFrame = [ 2 => obsSeries ] |> Frame.ofColumns

// All values in column 2 are missing (because the times do not match)
let obsDaysExact = daysFrame.Join(obsFrame, kind=JoinKind.Left)

// All values are available - for each day, we find the nearest smaller
// time in the frame indexed by later times in the day
let obsDaysPrev = 
  (daysFrame, obsFrame) 
  ||> Frame.joinAlign JoinKind.Left Lookup.ExactOrSmaller

// The first value is missing (because there is no nearest 
// value with greater key - the first one has the smallest 
// key) but the rest is available
let obsDaysNext =
  (daysFrame, obsFrame) 
  ||> Frame.joinAlign JoinKind.Left Lookup.ExactOrGreater

joinedOut?Comparison <- joinedOut |> Frame.mapRowValues (fun row -> 
  if row?MsftOpen > row?FbOpen then "MSFT" else "FB")

joinedOut.GetColumn<string>("Comparison")
|> Series.filterValues ((=) "MSFT") |> Series.countValues
//val it : int = 220

joinedOut.GetColumn<string>("Comparison")
|> Series.filterValues ((=) "FB") |> Series.countValues
//val it : int = 103

// Get data frame with only 'Open' columns
let joinedOpens = joinedOut.Columns.[ ["MsftOpen"; "FbOpen"] ]

// Get only rows that don't have any missing values
// and then we can safely filter & count
joinedOpens.RowsDense
|> Series.filterValues (fun row -> row?MsftOpen > row?FbOpen)
|> Series.countValues

let monthly =
  joinedIn
  |> Frame.groupRowsUsing (fun k _ -> DateTime(k.Year, k.Month, 1))
//Frame.ofRecords

//slicing notation
monthly.Rows.[DateTime(2013,5,1), *] |> Stats.mean

monthly 
|> Frame.getNumericCols
|> Series.mapValues (Stats.levelMean fst)
|> Frame.ofColumns

//open RProvider.graphics



//Chart.Line [ for x in 0 .. 10 -> x, x*x ]