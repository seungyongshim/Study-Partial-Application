namespace WebApplication1.Actor

open FSharp.Core
open Proto

type HelloActor() = 
    interface IActor with
        member this.ReceiveAsync(ctx) = task {
            match ctx.Message with
            | :? string as m ->
                printfn($"{m}")
                ctx.Respond("world")
            | _ -> ()
        }
        
