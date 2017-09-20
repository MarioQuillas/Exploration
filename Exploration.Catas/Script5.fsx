//type FileSystemItem =
//  | File of FileInfo
//  | Directory of DirectoryInfo
//and FileInfo = {name:string; fileSize:int}
//and DirectoryInfo = {name:string; dirSize:int; subitems:FileSystemItem list}

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
      
let fromFile (fileInfo:FileInfo) = 
  LeafNode fileInfo 

let fromDir (dirInfo:DirectoryInfo) subitems = 
  InternalNode (dirInfo,subitems)

let readme = fromFile {name="readme.txt"; fileSize=1}
let config = fromFile {name="config.xml"; fileSize=2}
let build  = fromFile {name="build.bat"; fileSize=3}
let src = fromDir {name="src"; dirSize=10} [readme; config; build]
let bin = fromDir {name="bin"; dirSize=10} []
let root = fromDir {name="root"; dirSize=5} [src; bin]

let totalSize fileSystemItem =
  let fFile acc (file:FileInfo) = 
    acc + file.fileSize
  let fDir acc (dir:DirectoryInfo)= 
    acc + dir.dirSize
  Tree.fold fFile fDir 0 fileSystemItem


let largestFile fileSystemItem =
  let fFile (largestSoFarOpt:FileInfo option) (file:FileInfo) = 
    match largestSoFarOpt with
    | None -> 
      Some file                
    | Some largestSoFar -> 
      if largestSoFar.fileSize > file.fileSize then
        Some largestSoFar
      else
        Some file

  let fDir largestSoFarOpt dirInfo = 
    largestSoFarOpt

  // call the fold
  Tree.fold fFile fDir None fileSystemItem




