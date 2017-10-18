module FizzBuzz_RailwayOriented_UsingAppend

  open RailwayCombinatorModule 

  let (|Uncarbonated|Carbonated|) =
    function 
    | Choice1Of2 u -> Uncarbonated u
    | Choice2Of2 c -> Carbonated c

  /// convert a single value into a two-track result
  let uncarbonated x = Choice1Of2 x
  let carbonated x = Choice2Of2 x

  // concat two carbonation functions
  let (<+>) switch1 switch2 x = 
    match (switch1 x),(switch2 x) with
    | Carbonated s1,Carbonated s2 -> carbonated (s1 + s2)
    | Uncarbonated f1,Carbonated s2  -> carbonated s2
    | Carbonated s1,Uncarbonated f2 -> carbonated s1
    | Uncarbonated f1,Uncarbonated f2 -> uncarbonated f1

  // carbonate a value
  let carbonate factor label i = 
    if i % factor = 0 then
      carbonated label
    else
      uncarbonated i

  let fizzBuzz = 
    let carbonateAll = 
       carbonate 3 "Fizz" <+> carbonate 5 "Buzz"

    carbonateAll 
    >> either (printf "%i; ") (printf "%s; ") 

  // test
  [1..100] |> List.iter fizzBuzz

  let fizzBuzzPrimes rules = 
    let carbonateAll  = 
      rules
      |> List.map (fun (factor,label) -> carbonate factor label)
      |> List.reduce (<+>)
        
    carbonateAll 
    >> either (printf "%i; ") (printf "%s; ") 

  // test
  let rules = [ (3,"Fizz"); (5,"Buzz"); (7,"Baz") ]
  [1..105] |> List.iter (fizzBuzzPrimes rules)