open System.Threading.Tasks


[<Interface>]
type ILogger =
    abstract Debug: string -> unit
    abstract Error: string -> unit 

[<Interface>] type ILog = abstract Logger: ILogger

module Log =
    let debug (env: #ILog) fmt = Printf.kprintf env.Logger.Debug fmt
    let error (env: #ILog) fmt = Printf.kprintf env.Logger.Error fmt

[<Interface>]
type IDatabase =
    abstract Query: string * 'i -> Task<'o>
    abstract Execute: string * 'i -> Task
	
[<Interface>] type IDb = abstract Database: IDatabase

module Db = 
    let fetchUser (env: #IDb) userId = 
        env.Database.Query(Sql.FetchUser, {| userId = userId |})
    let updateUser (env: #IDb) user = env.Database.Execute(Sql.UpdateUser, user)

let changePass env = fun req -> task {
    let! user = Db.fetchUser env req.UserId
    if user.Hash = bcrypt user.Salt req.OldPass then
        let salt = Random.bytes env 32
        do! Db.updateUser env { user with Salt = salt; Hash = bcrypt salt req.NewPass }
        Log.info env "Changed password for user %i" user.Id
        return Ok ()
    else 
        Log.error env "Password change unauthorized: user %i" user.Id
        return Error "Old password is invalid"
}

module Log =
    let live : ILogger = ?? // create logger interface

[<Struct>]
type AppEnv = 
    interface ILog with member _.Logger = Log.live
    interface IDb with member _.Database = Db.live connectionString
	
foo (AppEnv())
