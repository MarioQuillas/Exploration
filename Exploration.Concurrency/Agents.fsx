let slowConsoleWrite msg = 
    msg |> String.iter (fun ch->
        System.Threading.Thread.Sleep(1)
        System.Console.Write ch
        )

// test in isolation
slowConsoleWrite "abc"

let makeTask logger taskId = async {
    let name = sprintf "Task%i" taskId
    for i in [1..3] do 
        let msg = sprintf "-%s:Loop%i-" name i
        logger msg 
    }

// test in isolation
let task = makeTask slowConsoleWrite 1
Async.RunSynchronously task

type UnserializedLogger() = 
    // interface
    member this.Log msg = slowConsoleWrite msg

// test in isolation
let unserializedLogger = UnserializedLogger()
unserializedLogger.Log "hello"

let unserializedExample = 
    let logger = new UnserializedLogger()
    [1..5]
        |> List.map (fun i -> makeTask logger.Log i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

type SerializedLogger() = 

    // create the mailbox processor
    let agent = MailboxProcessor.Start(fun inbox -> 

        // the message processing function
        let rec messageLoop () = async{

            // read a message
            let! msg = inbox.Receive()

            // write it to the log
            slowConsoleWrite msg

            // loop to top
            return! messageLoop ()
            }

        // start the loop
        messageLoop ()
        )

    // public interface
    member this.Log msg = agent.Post msg

// test in isolation
let serializedLogger = SerializedLogger()
serializedLogger.Log "hello"

let serializedExample = 
    let logger = new SerializedLogger()
    [1..5]
        |> List.map (fun i -> makeTask logger.Log i)
        |> Async.Parallel
        |> Async.RunSynchronously
        |> ignore

//serializedExample