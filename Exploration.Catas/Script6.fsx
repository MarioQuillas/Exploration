type Tree<'LeafData,'INodeData> =
  | LeafNode of 'LeafData
  | InternalNode of 'INodeData * Tree<'LeafData,'INodeData> seq

type FileInfo = {name:string; fileSize:int}
type DirectoryInfo = {name:string; dirSize:int}

type FileSystemItem = Tree<FileInfo,DirectoryInfo>

module Tree = 

  let rec cata fLeaf fNode (tree:Tree<'LeafData,'INodeData>) :'r = 
    let recurse = cata fLeaf fNode  
    match tree with
    | LeafNode leafInfo -> 
      fLeaf leafInfo 
    | InternalNode (nodeInfo,subtrees) -> 
      fNode nodeInfo (subtrees |> Seq.map recurse)

  let rec fold fLeaf fNode acc (tree:Tree<'LeafData,'INodeData>) :'r = 
    let recurse = fold fLeaf fNode  
    match tree with
    | LeafNode leafInfo -> 
      fLeaf acc leafInfo 
    | InternalNode (nodeInfo,subtrees) -> 
      // determine the local accumulator at this level
      let localAccum = fNode acc nodeInfo
      // thread the local accumulator through all the subitems using Seq.fold
      let finalAccum = subtrees |> Seq.fold recurse localAccum 
      // ... and return it
      finalAccum
   
  let rec map fLeaf fNode (tree:Tree<'LeafData,'INodeData>) = 
    let recurse = map fLeaf fNode  
    match tree with
    | LeafNode leafInfo -> 
      let newLeafInfo = fLeaf leafInfo
      LeafNode newLeafInfo 
    | InternalNode (nodeInfo,subtrees) -> 
      let newNodeInfo = fNode nodeInfo
      let newSubtrees = subtrees |> Seq.map recurse 
      InternalNode (newNodeInfo, newSubtrees)

  let rec iter fLeaf fNode (tree:Tree<'LeafData,'INodeData>) = 
      let recurse = iter fLeaf fNode  
      match tree with
      | LeafNode leafInfo -> 
        fLeaf leafInfo
      | InternalNode (nodeInfo,subtrees) -> 
        subtrees |> Seq.iter recurse 
        fNode nodeInfo

open System
open System.IO

type FileSystemTree = Tree<IO.FileInfo,IO.DirectoryInfo>

let fromFile (fileInfo:IO.FileInfo) = 
  LeafNode fileInfo 

let rec fromDir (dirInfo:IO.DirectoryInfo) = 
  let subItems = seq{
    yield! dirInfo.EnumerateFiles() |> Seq.map fromFile
    yield! dirInfo.EnumerateDirectories() |> Seq.map fromDir
    }
  InternalNode (dirInfo,subItems)

let totalSize fileSystemItem =
  let fFile acc (file:FileInfo) = 
    acc + file.Length
  let fDir acc (dir:DirectoryInfo)= 
    acc 
  Tree.fold fFile fDir 0L fileSystemItem 

let largestFile fileSystemItem =
  let fFile (largestSoFarOpt:FileInfo option) (file:FileInfo) = 
    match largestSoFarOpt with
    | None -> 
      Some file                
    | Some largestSoFar -> 
      if largestSoFar.Length > file.Length then
        Some largestSoFar
      else
        Some file

  let fDir largestSoFarOpt dirInfo = 
    largestSoFarOpt
  
  // call the fold
  Tree.fold fFile fDir None fileSystemItem

let dirListing fileSystemItem =
  let printDate (d:DateTime) = d.ToString()
  let mapFile (fi:FileInfo) = 
    sprintf "%10i %s %-s"  fi.Length (printDate fi.LastWriteTime) fi.Name
  let mapDir (di:DirectoryInfo) = 
    di.FullName 
  Tree.map mapFile mapDir fileSystemItem

/// Fold over the lines in a file asynchronously
/// passing in the current line and line number to the folder function.
///
/// Signature:
/// folder:('a -> int -> string -> 'a) -> 
/// acc:'a -> 
/// fi:FileInfo -> 
/// Async<'a>
let foldLinesAsync folder acc (fi:FileInfo) = 
  async {
    let mutable acc = acc
    let mutable lineNo = 1
    use sr = new System.IO.StreamReader(path=fi.FullName)
    while not sr.EndOfStream do
      let! lineText = sr.ReadLineAsync() |> Async.AwaitTask
      acc <- folder acc lineNo lineText 
      lineNo <- lineNo + 1
    return acc
  }

let asyncMap f asyncX = async { 
  let! x = asyncX
  return (f x)  }

let matchPattern textPattern (fi:FileInfo) = 
  // set up the regex
  let regex = Text.RegularExpressions.Regex(pattern=textPattern)
    
  // set up the function to use with "fold"
  let folder results lineNo lineText =
    if regex.IsMatch lineText then
      let result = sprintf "%40s:%-5i %s" fi.Name lineNo lineText
      result :: results
    else
      // pass through
      results
    
  // main flow
  fi
  |> foldLinesAsync folder []
  // the fold output is in reverse order, so reverse it
  |> asyncMap List.rev

let grep filePattern textPattern fileSystemItem =
  let regex = Text.RegularExpressions.Regex(pattern=filePattern)

  /// if the file matches the pattern
  /// do the matching and return Some async, else None
  let matchFile (fi:FileInfo) =
    if regex.IsMatch fi.Name then
      Some (matchPattern textPattern fi)
    else
      None

  /// process a file by adding its async to the list
  let fFile asyncs (fi:FileInfo) = 
    // add to the list of asyncs
    (matchFile fi) :: asyncs 

  // for directories, just pass through the list of asyncs
  let fDir asyncs (di:DirectoryInfo)  = 
    asyncs 

  fileSystemItem
  |> Tree.fold fFile fDir []    // get the list of asyncs
  |> Seq.choose id              // choose the Somes (where a file was processed)
  |> Async.Parallel             // merge all asyncs into a single async
  |> asyncMap (Array.toList >> List.collect id)  // flatten array of lists into a single list
