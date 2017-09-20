type Vector3 = {X:float; Y:float; Z:float} with
  static member create x y z = {X=x; Y=y; Z=z}
  static member (+) (a,b)    = Vector3.create (a.X + b.X) (a.Y + b.Y) (a.Z + b.Z)

let inline add a b = a + b

let vec1 = Vector3.create 1.0 1.0 1.0
let vec2 = Vector3.create 2.0 2.0 2.0
let vec3 = add vec1 vec2

//-----------------------------------------------------------------------

type Book = {title: string; price: decimal}

type ChocolateType = Dark | Milk | SeventyPercent
type Chocolate = {chocType: ChocolateType ; price: decimal}

type WrappingPaperStyle = 
  | HappyBirthday
  | HappyHolidays
  | SolidColor

type Gift =
  | Book of Book
  | Chocolate of Chocolate 
  | Wrapped of Gift * WrappingPaperStyle
  | Boxed of Gift 
  | WithACard of Gift * message:string

let wolfHall = {title="Wolf Hall"; price=20m}
let yummyChoc = {chocType=SeventyPercent; price=5m}
let birthdayPresent = WithACard (Wrapped (Book wolfHall, HappyBirthday), "Happy Birthday")
let christmasPresent = Wrapped (Boxed (Chocolate yummyChoc), HappyHolidays)

let rec cataGift fBook fChocolate fWrapped fBox fCard gift :'r =
  let recurse = cataGift fBook fChocolate fWrapped fBox fCard
  match gift with 
  | Book book -> 
    fBook book
  | Chocolate choc -> 
    fChocolate choc
  | Wrapped (gift,style) -> 
    fWrapped (recurse gift,style)
  | Boxed gift -> 
    fBox (recurse gift)
  | WithACard (gift,message) -> 
    fCard (recurse gift,message)

//type GiftMinusChocolate =
//  | Book of Book
//  | Apology of string
//  | Wrapped of GiftMinusChocolate * WrappingPaperStyle

//let removeChocolate gift =
//  let fBook (book:Book) = 
//    Book book
//  let fChocolate (choc:Chocolate) = 
//    Apology "sorry I ate your chocolate"
//  let fWrapped (innerGiftResult,style) = 
//    Wrapped (innerGiftResult,style) 
//  let fBox innerGiftResult = 
//    innerGiftResult
//  let fCard (innerGiftResult,message) = 
//    innerGiftResult
//  // call the catamorphism
//  cataGift fBook fChocolate fWrapped fBox fCard gift

let deepCopy gift =
  let fBook = Book 
  let fChocolate = Chocolate 
  let fWrapped = Wrapped 
  let fBox = Boxed 
  let fCard = WithACard 
  // call the catamorphism
  cataGift fBook fChocolate fWrapped fBox fCard gift

let upgradeChocolate gift =
  let fBook = Book 
  let fChocolate (choc:Chocolate) = 
    Chocolate {choc with chocType = SeventyPercent}
  let fWrapped = Wrapped 
  let fBox = Boxed 
  let fCard = WithACard 
  // call the catamorphism
  cataGift fBook fChocolate fWrapped fBox fCard gift

//-------------------------------------------------------------------------------

type FileSystemItem =
  | File of File
  | Directory of Directory
and File = {name:string; fileSize:int}
and Directory = {name:string; dirSize:int; subitems:FileSystemItem list}

let readme = File {name="readme.txt"; fileSize=1}
let config = File {name="config.xml"; fileSize=2}
let build  = File {name="build.bat"; fileSize=3}
let src = Directory {name="src"; dirSize=10; subitems=[readme; config; build]}
let bin = Directory {name="bin"; dirSize=10; subitems=[]}
let root = Directory {name="root"; dirSize=5; subitems=[src; bin]}

let rec cataFS fFile fDir item :'r = 
  let recurse = cataFS fFile fDir 
  match item with
  | File file -> 
    fFile file
  | Directory dir -> 
    let listOfRs = dir.subitems |> List.map recurse 
    fDir (dir.name,dir.dirSize,listOfRs)

let totalSize fileSystemItem =
  let fFile (file:File) = 
    file.fileSize
  let fDir (name,size,subsizes) = 
    (List.sum subsizes) + size
  cataFS fFile fDir fileSystemItem

let largestFile fileSystemItem =
  
  // helper to provide a default if missing
  let ifNone deflt opt =
    defaultArg opt deflt 

  // helper to get the size of a File option
  let fileSize fileOpt = 
    fileOpt 
    |> Option.map (fun file -> file.fileSize)
    |> ifNone 0

  // handle File case  
  let fFile (file:File) = 
    Some file

  // handle Directory case  
  let fDir (name,size,subfiles) = 
    match subfiles with
    | [] -> 
      None  // empty directory
    | subfiles -> 
      // find the biggest File option using the helper
      subfiles 
      |> List.maxBy fileSize  

  // call the catamorphism
  cataFS fFile fDir fileSystemItem

//--------------------------------------------------

type Product =
  | Bought of BoughtProduct 
  | Made of MadeProduct 
and BoughtProduct = {
  name : string 
  weight : int 
  vendor : string option }
and MadeProduct = {
  name : string 
  weight : int 
  components:Component list }
and Component = {
  qty : int
  product : Product }

let label = 
  Bought {name="label"; weight=1; vendor=Some "ACME"}
let bottle = 
  Bought {name="bottle"; weight=2; vendor=Some "ACME"}
let formulation = 
  Bought {name="formulation"; weight=3; vendor=None}

let shampoo = 
  Made {name="shampoo"; weight=10; components=
  [
  {qty=1; product=formulation}
  {qty=1; product=bottle}
  {qty=2; product=label}
  ]}

let twoPack = 
  Made {name="twoPack"; weight=5; components=
  [
  {qty=2; product=shampoo}
  ]}

let rec cataProduct fBought fMade product :'r = 
  let recurse = cataProduct fBought fMade 

  // Converts a Component into a (int * 'r) tuple
  let convertComponentToTuple comp =
    (comp.qty,recurse comp.product)

  match product with
  | Bought bought -> 
    fBought bought 
  | Made made -> 
    let componentTuples =  // (int * 'r) list
      made.components 
      |> List.map convertComponentToTuple 
    fMade (made.name,made.weight,componentTuples) 

let productWeight product =

  // handle Bought case
  let fBought (bought:BoughtProduct) = 
    bought.weight

  // handle Made case
  let fMade (name,weight,componentTuples) = 
    // helper to calculate weight of one component tuple
    let componentWeight (qty,weight) =
      qty * weight
    // add up the weights of all component tuples
    let totalComponentWeight = 
      componentTuples 
      |> List.sumBy componentWeight 
    // and add the weight of the Made case too
    totalComponentWeight + weight
  // call the catamorphism
  cataProduct fBought fMade product

type VendorScore = {vendor:string; score:int}

let vendor vs = vs.vendor
let score vs = vs.score

let mostUsedVendor product =

  let fBought (bought:BoughtProduct) = 
    // set score = 1 if there is a vendor
    bought.vendor
    |> Option.map (fun vendor -> {vendor = vendor; score = 1} )
    // => a VendorScore option
    |> Option.toList
    // => a VendorScore list

  let fMade (name,weight,subresults) = 
    // subresults are a list of (qty * VendorScore list)

    // helper to get sum of scores
    let totalScore (vendor,vendorScores) =
      let totalScore = vendorScores |> List.sumBy score
      {vendor=vendor; score=totalScore}

    subresults 
    // => a list of (qty * VendorScore list)
    |> List.collect snd  // ignore qty part of subresult
    // => a list of VendorScore 
    |> List.groupBy vendor 
    // second item is list of VendorScore, reduce to sum
    |> List.map totalScore 
    // => list of VendorScores 

  // call the catamorphism
  cataProduct fBought fMade product
  |> List.sortByDescending score  // find highest score
  // return first, or None if list is empty
  |> List.tryHead

//---------------------------------------------------------------------------------
let totalCostUsingCata gift =
  let fBook (book:Book) = 
    book.price
  let fChocolate (choc:Chocolate) = 
    choc.price
  let fWrapped  (innerCost,style) = 
    innerCost + 0.5m
  let fBox innerCost = 
    innerCost + 1.0m
  let fCard (innerCost,message) = 
    innerCost + 2.0m
  // call the catamorphism
  cataGift fBook fChocolate fWrapped fBox fCard gift

let deeplyNestedBox depth =
  let rec loop depth boxSoFar =
    match depth with
    | 0 -> boxSoFar 
    | n -> loop (n-1) (Boxed boxSoFar)
  loop depth (Book wolfHall)

let rec totalCostUsingAcc costSoFar gift =
  match gift with 
  | Book book -> 
    costSoFar + book.price  // final result
  | Chocolate choc -> 
    costSoFar + choc.price  // final result
  | Wrapped (innerGift,style) -> 
    let newCostSoFar = costSoFar + 0.5m
    totalCostUsingAcc newCostSoFar innerGift 
  | Boxed innerGift -> 
    let newCostSoFar = costSoFar + 1.0m
    totalCostUsingAcc newCostSoFar innerGift 
  | WithACard (innerGift,message) -> 
    let newCostSoFar = costSoFar + 2.0m
    totalCostUsingAcc newCostSoFar innerGift 

deeplyNestedBox 1000000 |> totalCostUsingAcc 0.0m    

let rec foldGift fBook fChocolate fWrapped fBox fCard acc gift :'r =
  let recurse = foldGift fBook fChocolate fWrapped fBox fCard 
  match gift with 
  | Book book -> 
    let finalAcc = fBook acc book
    finalAcc     // final result
  | Chocolate choc -> 
    let finalAcc = fChocolate acc choc
    finalAcc     // final result
  | Wrapped (innerGift,style) -> 
    let newAcc = fWrapped acc style
    recurse newAcc innerGift 
  | Boxed innerGift -> 
    let newAcc = fBox acc 
    recurse newAcc innerGift 
  | WithACard (innerGift,message) -> 
    let newAcc = fCard acc message 
    recurse newAcc innerGift

// the recursive cases don't use the 'r type at all, only the accumulator 'a type

let rec foldbackGift fBook fChocolate fWrapped fBox fCard generator gift :'r =
  let recurse = foldbackGift fBook fChocolate fWrapped fBox fCard 
  match gift with 
  | Book book -> 
    generator (fBook book)
  | Chocolate choc -> 
    generator (fChocolate choc)
  | Wrapped (innerGift,style) -> 
    let newGenerator innerVal =
      let newInnerVal = fWrapped innerVal style
      generator newInnerVal 
    recurse newGenerator innerGift 
  | Boxed innerGift -> 
    let newGenerator innerVal =
      let newInnerVal = fBox innerVal 
      generator newInnerVal 
    recurse newGenerator innerGift 
  | WithACard (innerGift,message) -> 
    let newGenerator innerVal =
      let newInnerVal = fCard innerVal message 
      generator newInnerVal 
    recurse newGenerator innerGift 

let rec foldbackGiftWithAccLast fBook fChocolate fWrapped fBox fCard gift generator :'r =
//swapped => ~~~~~~~~~~~~~~ 

  let recurse = foldbackGiftWithAccLast fBook fChocolate fWrapped fBox fCard 

  match gift with 
  | Book book -> 
    generator (fBook book)
  | Chocolate choc -> 
    generator (fChocolate choc)

  | Wrapped (innerGift,style) -> 
    let newGenerator innerVal =
      let newInnerVal = fWrapped style innerVal 
//swapped => ~~~~~~~~~~~~~~ 
      generator newInnerVal 
    recurse innerGift newGenerator  
//swapped => ~~~~~~~~~~~~~~~~~~~~~~ 

  | Boxed innerGift -> 
    let newGenerator innerVal =
        let newInnerVal = fBox innerVal 
        generator newInnerVal 
    recurse innerGift newGenerator  
//swapped => ~~~~~~~~~~~~~~~~~~~~~~ 

  | WithACard (innerGift,message) -> 
    let newGenerator innerVal =
      let newInnerVal = fCard message innerVal 
//swapped => ~~~~~~~~~~~~~~~~ 
      generator newInnerVal 
    recurse innerGift newGenerator 
//swapped => ~~~~~~~~~~~~~~~~~~~~~~ 

let descriptionUsingFoldBack gift =
  let fBook (book:Book) = 
    sprintf "'%s'" book.title 
  let fChocolate (choc:Chocolate) = 
    sprintf "%A chocolate" choc.chocType
  let fWrapped innerText style = 
    sprintf "%s wrapped in %A paper" innerText style
  let fBox innerText = 
    sprintf "%s in a box" innerText 
  let fCard innerText message = 
    sprintf "%s with a card saying '%s'" innerText message 
  // initial DescriptionGenerator
  let initialAcc = fun innerText -> innerText 
  // main call
  foldbackGift fBook fChocolate fWrapped fBox fCard initialAcc gift 


let descriptionUsingFold gift =
  let fBook descriptionSoFar (book:Book) = 
    sprintf "'%s' %s" book.title descriptionSoFar

  let fChocolate descriptionSoFar (choc:Chocolate) = 
    sprintf "%A chocolate %s" choc.chocType descriptionSoFar

  let fWrapped descriptionSoFar style = 
    sprintf "%s wrapped in %A paper" descriptionSoFar style

  let fBox descriptionSoFar = 
    sprintf "%s in a box" descriptionSoFar 

  let fCard descriptionSoFar message = 
    sprintf "%s with a card saying '%s'" descriptionSoFar message

  // initial accumulator
  let initialAcc = ""

  // main call
  foldGift fBook fChocolate fWrapped fBox fCard initialAcc gift 

let descriptionUsingFoldWithGenerator gift =

  let fBook descriptionGenerator (book:Book) = 
    descriptionGenerator (sprintf "'%s'" book.title)

  let fChocolate descriptionGenerator (choc:Chocolate) = 
    descriptionGenerator (sprintf "%A chocolate" choc.chocType)

  let fWrapped descriptionGenerator style = 
    fun innerText ->
      let newInnerText = sprintf "%s wrapped in %A paper" innerText style
      descriptionGenerator newInnerText 

  let fBox descriptionGenerator = 
    fun innerText ->
      let newInnerText = sprintf "%s in a box" innerText 
      descriptionGenerator newInnerText 

  let fCard descriptionGenerator message = 
    fun innerText ->
      let newInnerText = sprintf "%s with a card saying '%s'" innerText message 
      descriptionGenerator newInnerText 

  // initial DescriptionGenerator
  let initialAcc = fun innerText -> innerText 

  // main call
  foldGift fBook fChocolate fWrapped fBox fCard initialAcc gift 


let totalCostUsingFold gift =  

  let fBook costSoFar (book:Book) = 
    costSoFar + book.price
  let fChocolate costSoFar (choc:Chocolate) = 
    costSoFar + choc.price
  let fWrapped costSoFar style = 
    costSoFar + 0.5m
  let fBox costSoFar = 
    costSoFar + 1.0m
  let fCard costSoFar message = 
    costSoFar + 2.0m

  // initial accumulator
  let initialAcc = 0m

  // call the fold
  foldGift fBook fChocolate fWrapped fBox fCard initialAcc gift 
//---------------------------------------------------------------------------------

let rec foldFS fFile fDir acc item :'r = 
  let recurse = foldFS fFile fDir 
  match item with
  | File file -> 
    fFile acc file
  | Directory dir -> 
    let newAcc = fDir acc (dir.name,dir.dirSize) 
    dir.subitems |> List.fold recurse newAcc

let totalSizeWithFoldFS fileSystemItem =
  let fFile acc (file:File) = 
    acc + file.fileSize
  let fDir acc (name,size) = 
    acc + size
  foldFS fFile fDir 0 fileSystemItem 

let largestFileWithFoldFS fileSystemItem =
  let fFile (largestSoFarOpt:File option) (file:File) = 
    match largestSoFarOpt with
    | None -> 
      Some file                
    | Some largestSoFar -> 
      if largestSoFar.fileSize > file.fileSize then
        Some largestSoFar
      else
        Some file

  let fDir largestSoFarOpt (name,size) = 
    largestSoFarOpt

  // call the fold
  foldFS fFile fDir None fileSystemItem


