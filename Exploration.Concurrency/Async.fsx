open System

let userTimerWithCallback = 
  // create an event to wait on
  let event = new System.Threading.AutoResetEvent(false)

  // create a timer and add an event handler that will signal the event
  let timer = new System.Timers.Timer(2000.0)
  timer.Elapsed.Add (fun _ -> event.Set() |> ignore )

  //start
  printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
  timer.Start()

  // keep working
  printfn "Doing something useful while waiting for event"

  // block on the timer via the AutoResetEvent
  event.WaitOne() |> ignore

  //done
  printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

let userTimerWithAsync = 

  // create a timer and associated async event
  let timer = new System.Timers.Timer(2000.0)
  let timerEvent = Async.AwaitEvent (timer.Elapsed) |> Async.Ignore

  // start
  printfn "Waiting for timer at %O" DateTime.Now.TimeOfDay
  timer.Start()

  // keep working
  printfn "Doing something useful while waiting for event"

  // block on the timer event now by waiting for the async to complete
  Async.RunSynchronously timerEvent

  // done
  printfn "Timer ticked at %O" DateTime.Now.TimeOfDay

let fileWriteWithAsync = 

  // create a stream to write to
  use stream = new System.IO.FileStream("test.txt",System.IO.FileMode.Create)

  // start
  printfn "Starting async write"
  let asyncResult = stream.BeginWrite(Array.empty,0,0,null,null)

  // create an async wrapper around an IAsyncResult
  let async = Async.AwaitIAsyncResult(asyncResult) |> Async.Ignore

  // keep working
  printfn "Doing something useful while waiting for write to complete"

  // block on the timer now by waiting for the async to complete
  Async.RunSynchronously async 

  // done
  printfn "Async write completed"

let sleepWorkflow  = async{
  printfn "Starting sleep workflow at %O" DateTime.Now.TimeOfDay
  do! Async.Sleep 2000
  printfn "Finished sleep workflow at %O" DateTime.Now.TimeOfDay }

Async.RunSynchronously sleepWorkflow 

let nestedWorkflow  = async{

  printfn "Starting parent"
  let! childWorkflow = Async.StartChild sleepWorkflow

  // give the child a chance and then keep working
  do! Async.Sleep 100
  printfn "Doing something useful while waiting "

  // block on the child
  let! result = childWorkflow

  // done
  printfn "Finished parent" }

// run the whole workflow
Async.RunSynchronously nestedWorkflow 

let testLoop = async {
  for i in [1..100] do
      // do something
      printf "%i before.." i
        
      // sleep a bit 
      do! Async.Sleep 10  
      printfn "..after" }

Async.RunSynchronously testLoop

open System
open System.Threading

// create a cancellation source
let cancellationSource = new CancellationTokenSource()

// start the task, but this time pass in a cancellation token
Async.Start (testLoop,cancellationSource.Token)

// wait a bit
Thread.Sleep(200)  

// cancel after 200ms
cancellationSource.Cancel()

// create a workflow to sleep for a time
let sleepWorkflowMs ms = async {
  printfn "%i ms workflow started" ms
  do! Async.Sleep ms
  printfn "%i ms workflow finished" ms }

let workflowInSeries = async {
  let! sleep1 = sleepWorkflowMs 1000
  printfn "Finished one" 
  let! sleep2 = sleepWorkflowMs 2000
  printfn "Finished two" }

#time
Async.RunSynchronously workflowInSeries 
#time

// Create them
let sleep1 = sleepWorkflowMs 1000
let sleep2 = sleepWorkflowMs 2000

// run them in parallel
#time
[sleep1; sleep2] 
  |> Async.Parallel
  |> Async.RunSynchronously 
#time

open System.Net
open System
open System.IO

let fetchUrl url =        
  let req = WebRequest.Create(Uri(url)) 
  use resp = req.GetResponse() 
  use stream = resp.GetResponseStream() 
  use reader = new IO.StreamReader(stream) 
  let html = reader.ReadToEnd() 
  printfn "finished downloading %s" url 

// a list of sites to fetch
let sites = ["http://www.bing.com";
             "http://www.google.com";
             "http://www.microsoft.com";
             "http://www.amazon.com";
             "http://www.yahoo.com"]

#time                     // turn interactive timer on
sites                     // start with the list of sites
|> List.map fetchUrl      // loop through each site and download
#time                     // turn timer off

open Microsoft.FSharp.Control.CommonExtensions   
                                        // adds AsyncGetResponse

// Fetch the contents of a web page asynchronously
let fetchUrlAsync url =        
  async {                             
    let req = WebRequest.Create(Uri(url)) 
    use! resp = req.AsyncGetResponse()  // new keyword "use!"  
    use stream = resp.GetResponseStream() 
    use reader = new IO.StreamReader(stream) 
    let html = reader.ReadToEnd() 
    printfn "finished downloading %s" url 
    }

#time                      // turn interactive timer on
sites 
|> List.map fetchUrlAsync  // make a list of async tasks
|> Async.Parallel          // set up the tasks to run in parallel
|> Async.RunSynchronously  // start them off
#time      


let childTask() = 
  // chew up some CPU. 
  for i in [1..1000] do 
    for i in [1..1000] do 
      do "Hello".Contains("H") |> ignore 
        // we don't care about the answer!

// Test the child task on its own.
// Adjust the upper bounds as needed
// to make this run in about 0.2 sec
#time
childTask()
#time

let parentTask = 
  childTask
  |> List.replicate 20
  |> List.reduce (>>)

//test
#time
parentTask()
#time

let asyncChildTask = async { return childTask() }

let asyncParentTask = 
  asyncChildTask
  |> List.replicate 20
  |> Async.Parallel

//test
#time
asyncParentTask 
|> Async.RunSynchronously
#time


