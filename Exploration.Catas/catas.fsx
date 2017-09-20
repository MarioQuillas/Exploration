//catamorphismes
type Nat =
  |Zero
  |Succ of Nat

type StepAlgebra<'b> = StepAlgebra of 'b * ('b -> 'b) //(nil, next)

//let ff (a:int StepAlgebra) = 12

let rec cata step nat =
  match step, nat with
  |StepAlgebra(nil, next), Zero-> nil
  |StepAlgebra(nil, next), Succ(prec) -> next (cata step prec)
        
let algebra = StepAlgebra("go!", fun s -> "wait... " + s)

let numb = Succ(Succ(Succ(Zero)))
let tot = cata algebra numb

type ContainerAlgebra<'a, 'b> = ContainerAlgebra of 'b*('a -> 'b -> 'b)//(nil, merge)

type MyList<'a> =
  |Nil
  |Cons of 'a*MyList<'a>

let rec cata2 container list =
  match container, list with
  |ContainerAlgebra(nil, merge), Nil -> nil
  |ContainerAlgebra(nil, merge), Cons(x, xs) -> merge(x)(cata2 container xs)

let aa = ContainerAlgebra(3, fun x -> fun y -> x*y)
let ll = Cons(10,Cons(100,Cons(1000,Nil)))
let bb = cata2 aa ll

type TreeAlgebra<'a, 'b> = TreeAlgebra of ('a -> 'b)*('b -> 'b -> 'b)//(f,g)

type 'a Tree =
  |Leaf of 'a
  |Branch of Tree<'a>*Tree<'a>

let rec cata3 treeAlgebra tree =
  match treeAlgebra, tree with
  |TreeAlgebra(f, g), Leaf(x) -> f(x)
  |TreeAlgebra(f, g), Branch(left, right) -> g(cata3 treeAlgebra left)(cata3 treeAlgebra right)

let totoTree = Branch(Branch(Leaf(12), Leaf(2)), Leaf(1))

let bbb = Leaf(fun x->x)

let treeDepth = TreeAlgebra((fun x -> 1), fun x -> fun y -> (1 + max x y) )

let treeSum = TreeAlgebra((fun x -> x), fun x -> fun y -> x + y)

let tt = cata3 treeDepth totoTree
let ttt = cata3 treeSum totoTree

//higher kinds and catamorphisms

//type 'a Functor 
//type Algebra 'f 'a = Algebra of ('f 'a -> 'a)
//type Fix 'f = Iso of 'f (Fix 'f)

type Peano = 
  |Zero
  |Succ of Peano
  //|Succ of System.Func<Peano, Peano>
  with 
    static member op_Explicit(source: Peano) : int =
      let rec desugar = 
        function
          | Zero   -> 0
          | Succ x -> 1 + desugar x
      desugar source
    override this.ToString() =       
      sprintf "%d" (int this)

let rec add a b = 
  match a with
  | Zero -> b
  | Succ (aa) -> Succ(add aa b)

let rec mult a b =
  match a with
  |Zero -> Zero
  |Succ(aa) -> add b (mult aa b)

let five = add (Succ(Succ(Succ(Zero)))) (Succ(Succ(Zero)))