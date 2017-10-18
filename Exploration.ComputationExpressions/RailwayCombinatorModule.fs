module RailwayCombinatorModule

  let (|Success|Failure|) =
    function 
    | Choice1Of2 s -> Success s
    | Choice2Of2 f -> Failure f

  /// convert a single value into a two-track result
  let succeed x = Choice1Of2 x

  /// convert a single value into a two-track result
  let fail x = Choice2Of2 x

  // appy either a success function or failure function
  let either successFunc failureFunc twoTrackInput =
    match twoTrackInput with
    | Success s -> successFunc s
    | Failure f -> failureFunc f

  // convert a switch function into a two-track function
  let bind f = 
    either f fail