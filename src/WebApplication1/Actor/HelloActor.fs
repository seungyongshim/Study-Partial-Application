namespace WebApplication1.Actor

open WebApplication1.Domain
open FSharp.Core
open Proto

type HelloActor() =
    interface IActor with
        member _.ReceiveAsync(ctx) = task {
            match ctx.Message with
            | :? string as m ->
                printfn($"{m}")
                ctx.Respond("world")
            | :? Cell as Bomb -> ctx.Respond("*")
            | _ -> ()
        }
        
