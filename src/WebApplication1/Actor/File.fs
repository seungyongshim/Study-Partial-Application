namespace WebApplication1.Actor

open FSharp
open FSharp.Core
open Proto

type HelloActor = 
    interface IActor with
        member this.ReceiveAsync(ctx) = task {
            match ctx.Message with
            | string -> printfn("${ctx.Message}")
        }
        
