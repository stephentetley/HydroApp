// Copyright (c) Stephen Tetley 2018
// License: BSD 3 Clause

namespace HydroApp

open SQLite

module Database = 
    
    [<Table("app_user")>]
    type AppUser () = 
        [<PrimaryKey>][<Column("user_name")>]
        member val UserName : string = ""  with get, set


    let connect dbPath = async {
        let db = new SQLiteAsyncConnection(dbPath)
        do! db.CreateTableAsync<AppUser>() |> Async.AwaitTask |> Async.Ignore
        return db
    }

    let loadAllUser dbPath = async {
        let! database = connect dbPath
        let! objs = database.Table<AppUser>().ToListAsync() |> Async.AwaitTask
        return objs |> Seq.toList
    }

    let insertUser dbPath (user:string) = async {
        let! database = connect dbPath
        let aUser : AppUser = AppUser(UserName = user)
        do! database.InsertAsync(aUser) |> Async.AwaitTask |> Async.Ignore
        return aUser
    }