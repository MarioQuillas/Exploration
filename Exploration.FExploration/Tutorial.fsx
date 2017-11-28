open System
open System.IO
open System.Text

let coolFileWriter targetFile maxQueueMessages messageHandler =
  if not (File.Exists(targetFile)) then
    use file = File.Create(targetFile) 
    file |> ignore

  let queue = new System.Collections.Generic.Queue<string>()
  (fun message -> 
    queue.Enqueue(messageHandler message)
    if queue.Count = maxQueueMessages then
      let sb = new System.Text.StringBuilder()
      while (queue.Count > 0) do
        sb.Append(queue.Dequeue()) |> ignore
        sb.Append(Environment.NewLine) |> ignore // or appendline

      //File.AppendAllText(targetFile, sb.ToString())) // this is working
      use fileStream = new FileStream(targetFile, FileMode.Append, FileAccess.Write)
      use streamWriter = new StreamWriter(fileStream, Encoding.UTF8)
        
      //let encoder = new UTF8Encoding(true)
      //let bytes = encoder.GetBytes(sb.ToString())
      //fileStream.Write(bytes, 0, bytes.Length))
      streamWriter.Write(sb.ToString()))