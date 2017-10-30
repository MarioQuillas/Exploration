type 'a seqMsg =   
  | Die   
  | Next of AsyncReplyChannel<'a>   
  
type primes() =   
  let counter(init) =   
    MailboxProcessor.Start(fun inbox ->   
      let rec loop n =   
        async { 
          let! msg = inbox.Receive()   
          match msg with   
          | Die -> return ()   
          | Next(reply) ->   
            reply.Reply(n)   
            return! loop(n + 1) }   
      loop init)   
      
  let filter(original : MailboxProcessor<'a seqMsg>, pred) =   
    MailboxProcessor.Start(fun inbox ->   
      let rec loop() =   
        async {   
          let! msg = inbox.Receive()   
          match msg with   
          | Die ->   
            original.Post(Die)   
            return()   
          | Next(reply) ->   
            let rec filter' n =   
              if pred n then async { return n }   
              else  
                async {
                  let! m = original.PostAndAsyncReply(Next)   
                  return! filter' m }   
            let! testItem = original.PostAndAsyncReply(Next)   
            let! filteredItem = filter' testItem   
            reply.Reply(filteredItem)   
            return! loop()   
        }   
      loop()   
    )

  let processor = MailboxProcessor.Start(fun inbox ->   
    let rec loop (oldFilter : MailboxProcessor<int seqMsg>) prime =   
      async {   
        let! msg = inbox.Receive()   
        match msg with   
        | Die ->   
          oldFilter.Post(Die)   
          return()   
        | Next(reply) ->   
          reply.Reply(prime)   
          let newFilter = filter(oldFilter, (fun x -> x % prime <> 0))   
          let! newPrime = oldFilter.PostAndAsyncReply(Next)   
          return! loop newFilter newPrime   
      }   
    loop (counter(3)) 2)   
  
  member this.Next() = 
    processor.PostAndReply(
      (fun reply -> Next(reply)),
      timeout = 2000)
    
  interface System.IDisposable with
    member this.Dispose() = processor.Post(Die)

  static member upto max =   
    [ use p = new primes()
      let lastPrime = ref (p.Next())
      while !lastPrime <= max do
        yield !lastPrime
        lastPrime := p.Next() ]