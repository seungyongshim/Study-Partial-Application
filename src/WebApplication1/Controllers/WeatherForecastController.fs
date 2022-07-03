namespace WebApplication1.Controllers

open System
open System.Collections.Generic
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore.Mvc
open Microsoft.Extensions.Logging
open WebApplication1
open Proto
open WebApplication1.Domain
open System.Threading

[<ApiController>]
[<Route("[controller]")>]
type WeatherForecastController (logger : ILogger<WeatherForecastController>, root : IRootContext) =
    inherit ControllerBase()

    let summaries =
        [|
            "Freezing"
            "Bracing"
            "Chilly"
            "Cool"
            "Mild"
            "Warm"
            "Balmy"
            "Hot"
            "Sweltering"
            "Scorching"
        |]

    [<HttpGet>]
    member _.Get(ct: CancellationToken) =
        async {
            let! res = root.RequestAsync<string>(PID("nonhost", "hello"), 1 |> Number) |> Async.AwaitTask
            logger.LogInformation("{response}", res)

            let rng = System.Random()

            return [|
                for index in 0..4 -> {
                    Date = DateTime.Now.AddDays(float index)
                    TemperatureC = rng.Next(-20,55)
                    Summary = summaries.[rng.Next(summaries.Length)]
                }
            |]
        } |> fun x -> Async.StartAsTask(x, TaskCreationOptions.None, ct)
