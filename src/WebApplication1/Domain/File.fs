namespace WebApplication1.Domain

type Cell =
    | Covered of Cell
    | Number of int
    | Bomb

