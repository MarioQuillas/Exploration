type LinkedList<'a> = 
  | Empty
  | Cons of head:'a * tail:LinkedList<'a>

module LinkedList = 

  let rec cata fCons fEmpty list :'r=
    let recurse = cata fCons fEmpty 
    match list with
    | Empty -> 
      fEmpty
    | Cons (element,list) -> 
      fCons element (recurse list)

  let rec foldWithEmpty fCons fEmpty acc list :'r=
    let recurse = foldWithEmpty fCons fEmpty 
    match list with
    | Empty -> 
      fEmpty acc 
    | Cons (element,list) -> 
      let newAcc = fCons acc element 
      recurse newAcc list
    
  let rec fold fCons acc list :'r=
    let recurse = fold fCons 
    match list with
    | Empty -> 
      acc 
    | Cons (element,list) -> 
      let newAcc = fCons acc element 
      recurse newAcc list
    
  let foldBack fCons list acc :'r=
    let fEmpty' generator = 
      generator acc 
    let fCons' generator element= 
      fun innerResult -> 
        let newResult = fCons element innerResult 
        generator newResult 
    let initialGenerator = id
    foldWithEmpty fCons' fEmpty' initialGenerator  list 

  let toList linkedList = 
    let fCons head tail = head::tail
    let initialState = [] 
    foldBack fCons linkedList initialState

  let ofList list = 
    let fCons head tail = Cons(head,tail)
    let initialState = Empty
    List.foldBack fCons list initialState 

  /// map a function "f" over all elements
  let map f list = 
    // helper function  
    let folder head tail =
      Cons(f head,tail)     
    foldBack folder list Empty

  /// return a new list of elements for which "pred" is true
  let filter pred list = 
    // helper function
    let folder head tail =
      if pred head then 
        Cons(head,tail)
      else
        tail
    foldBack folder list Empty
  
  let rev list = 
    // helper function
    let folder tail head =
      Cons(head,tail)

    fold folder Empty list 

  let foldBack_ViaRev fCons list acc :'r=
    let fCons' acc element = 
      // just swap the params!
      fCons element acc 
    list
    |> rev
    |> fold fCons' acc 
  




