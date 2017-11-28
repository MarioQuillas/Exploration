open System
open System.IO
open System.Text

type IFileWriter =
  inherit System.IDisposable
  abstract member Write: string -> unit

let simpleFileWriter targetFile maxQueueMessages messageHandler =
  if not (File.Exists(targetFile)) then
    use file = File.Create(targetFile) 
    file |> ignore

  let queue = new System.Collections.Generic.Queue<string>()

  let flushQueue () =
    let sb = new System.Text.StringBuilder()
    while (queue.Count > 0) do
      sb.Append(queue.Dequeue()) |> ignore
      sb.Append(Environment.NewLine) |> ignore // or appendline

    File.AppendAllText(targetFile, sb.ToString())

  { 
    new IFileWriter with
    member this.Dispose() =
      flushQueue() 
    member this.Write message =
      queue.Enqueue(messageHandler message)
      if queue.Count = maxQueueMessages then
        flushQueue()
   }

let errorLogger targetFile =
  simpleFileWriter targetFile 5 (fun msg -> sprintf "Error at [%s] : %s" (System.DateTime.UtcNow.ToShortTimeString()) msg)

let writer targetFile = simpleFileWriter targetFile 20000 id

//let testWriter = writer @"D:\test2.txt"


let test (lines) =
  use testWriter = writer @"D:\test2.txt"
  [1..1..lines] |> List.map (fun item -> testWriter.Log(sprintf "%d" item)) |> ignore

#time "on"
let lines = 15//10*1000*1000
test(lines)
#time "off"