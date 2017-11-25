let rec subsets list k = 
  match list with
  | [] -> []
  | head::tail -> 
    (subsets tail k)
    |> List.append
       (subsets tail (k-1)) 
       |> List.map (fun item -> [head] |> List.append item)

let r = subsets [1;2] 1
printfn "%A" r 