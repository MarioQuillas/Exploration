open System
open System.IO
open System.Text

let coolFileWriter targetFile maxQueueMessages messageHandler =
  if not (File.Exists(targetFile)) then
    use file = File.Create(targetFile) 
    file |> ignore

  let queue = new System.Collections.Generic.Queue<string>()
  //let timer = new System.Timers.Timer(5.0f)
  let stopWatch = new System.Diagnostics.Stopwatch()

  let flushQueue () =
    let sb = new System.Text.StringBuilder()
    while (queue.Count > 0) do
      sb.Append(queue.Dequeue()) |> ignore
      sb.Append(Environment.NewLine) |> ignore // or appendline

    File.AppendAllText(targetFile, sb.ToString())

  let cronTask timeInterval =
    let timer = new System.Timers.Timer(float timeInterval)
    timer.AutoReset <- true
    async {
      while (true) do
        if (stopWatch.Elapsed.TotalSeconds > 1.0) then
          stopWatch.Stop()
          flushQueue() }

  Async.Start(cronTask 1.0)

  (fun message -> 
    if (queue.Count = 0) then 
      stopWatch.Restart()
    queue.Enqueue(messageHandler message)
    if queue.Count = maxQueueMessages then
      flushQueue())

let errorLogger targetFile =
  coolFileWriter targetFile 5 (fun msg -> sprintf "Error at [%s] : %s" (System.DateTime.UtcNow.ToShortTimeString()) msg)

let writer targetFile = coolFileWriter targetFile 2000 id

let testWriter = writer @"D:\test2.txt"
  
//let toto = new System.Diagnostics.Stopwatch()
//toto.Restart()
//toto.Elapsed.TotalSeconds
//toto.Stop()

#timer "on"
[1..1..99] |> List.map (fun item -> testWriter (sprintf "%d" item)) |> ignore
#timer "off"