
type Cell =
    | Covered of Cell
    | Number of int
    | Bomb

let a : obj = Bomb;


let b =
    match a with
    | :? Cell as m ->
        match m with
        | Bomb -> "*"
        | _ -> "_"
    | _ -> "?"

printfn $"{b}"
