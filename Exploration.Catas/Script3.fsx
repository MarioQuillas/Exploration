type Book = {title: string; price: decimal}

type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {chocType: ChocolateType ; price: decimal}

type WrappingPaperStyle = 
  | HappyBirthday
  | HappyHolidays
  | SolidColor

// unified data for non-recursive cases
type GiftContents = 
  | Book of Book
  | Chocolate of Chocolate 

// unified data for recursive cases
type GiftDecoration = 
  | Wrapped of WrappingPaperStyle
  | Boxed 
  | WithACard of string

//type Gift =
//  // non-recursive case
//  | Contents of GiftContents
//  // recursive case
//  | Decoration of Gift * GiftDecoration

type Container<'ContentData,'DecorationData> =
  | Contents of 'ContentData
  | Decoration of 'DecorationData * Container<'ContentData,'DecorationData> 

module Container = 

  let rec cata fContents fDecoration (container:Container<'ContentData,'DecorationData>) :'r = 
    let recurse = cata fContents fDecoration 
    match container with
    | Contents contentData -> 
      fContents contentData 
    | Decoration (decorationData,subContainer) -> 
      fDecoration decorationData (recurse subContainer)
            
  (* val cata : // function parameters fContents:('ContentData -> 'r) -> fDecoration:('DecorationData -> 'r -> 'r) -> // input container:Container<'ContentData,'DecorationData> -> // return value 'r *)
            
  let rec fold fContents fDecoration acc (container:Container<'ContentData,'DecorationData>) :'r = 
    let recurse = fold fContents fDecoration 
    match container with
    | Contents contentData -> 
      fContents acc contentData 
    | Decoration (decorationData,subContainer) -> 
      let newAcc = fDecoration acc decorationData
      recurse newAcc subContainer
            
  (* val fold : // function parameters fContents:('a -> 'ContentData -> 'r) -> fDecoration:('a -> 'DecorationData -> 'a) -> // accumulator acc:'a -> // input container:Container<'ContentData,'DecorationData> -> // return value 'r *)
            
  let foldBack fContents fDecoration (container:Container<'ContentData,'DecorationData>) :'r = 
    let fContents' generator contentData =
      generator (fContents contentData)
    let fDecoration' generator decorationData =
      let newGenerator innerValue =
        let newInnerValue = fDecoration decorationData innerValue 
        generator newInnerValue 
      newGenerator 
    fold fContents' fDecoration' id container
            
  (* val foldBack : // function parameters fContents:('ContentData -> 'r) -> fDecoration:('DecorationData -> 'r -> 'r) -> // input container:Container<'ContentData,'DecorationData> -> // return value 'r *)

type Gift = Container<GiftContents,GiftDecoration>

let fromBook book = 
  Contents (Book book)

let fromChoc choc = 
  Contents (Chocolate choc)

let wrapInPaper paperStyle innerGift = 
  let container = Wrapped paperStyle 
  Decoration (container, innerGift)

let putInBox innerGift = 
  let container = Boxed
  Decoration (container, innerGift)

let withCard message innerGift = 
  let container = WithACard message
  Decoration (container, innerGift)

let wolfHall = {title="Wolf Hall"; price=20m}
let yummyChoc = {chocType=SeventyPercent; price=5m}

let birthdayPresent = 
  wolfHall 
  |> fromBook
  |> wrapInPaper HappyBirthday
  |> withCard "Happy Birthday"
 
let christmasPresent = 
  yummyChoc
  |> fromChoc
  |> putInBox
  |> wrapInPaper HappyHolidays

let totalCost gift =  

  let fContents costSoFar contentData = 
    match contentData with
    | Book book ->
      costSoFar + book.price
    | Chocolate choc ->
      costSoFar + choc.price

  let fDecoration costSoFar decorationInfo = 
    match decorationInfo with
    | Wrapped style ->
      costSoFar + 0.5m
    | Boxed ->
      costSoFar + 1.0m
    | WithACard message ->
      costSoFar + 2.0m

  // initial accumulator
  let initialAcc = 0m

  // call the fold
  Container.fold fContents fDecoration initialAcc gift 

let description gift =

  let fContents contentData = 
    match contentData with
    | Book book ->
      sprintf "'%s'" book.title
    | Chocolate choc ->
      sprintf "%A chocolate" choc.chocType

  let fDecoration decorationInfo innerText = 
    match decorationInfo with
    | Wrapped style ->
      sprintf "%s wrapped in %A paper" innerText style
    | Boxed ->
      sprintf "%s in a box" innerText 
    | WithACard message ->
      sprintf "%s with a card saying '%s'" innerText message 

  // main call
  Container.foldBack fContents fDecoration gift  

