namespace WebApplication1

open Boost.Proto.Actor.DependencyInjection
open WebApplication1.Actor

#nowarn "20"
open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.HttpsPolicy
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Boost.Proto.Actor.Hosting.Local
open FSharp.Core


module Program =
    [<EntryPoint>]
    let main args =
        let builder = WebApplication.CreateBuilder(args)

        builder.Services.AddControllers()
        builder.Host.UseProtoActor(Action<_,_>(fun o -> fun sp ->
            o.FuncActorSystemStart <- fun root ->
                root.SpawnNamed(sp.GetRequiredService<IPropsFactory<HelloActor>>().Create(), "hello")
                root
            ))

        let app = builder.Build()

        app.UseHttpsRedirection()
        app.UseAuthorization()
        app.MapControllers()

        app.Run()

        0
