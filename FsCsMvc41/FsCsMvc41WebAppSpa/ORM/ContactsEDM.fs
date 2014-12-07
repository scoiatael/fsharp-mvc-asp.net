namespace FsWeb.ORM

module Contacts =

  open System.Data.Linq
  open System.Data.Entity
  open Microsoft.FSharp.Data.TypeProviders

  type internal EntityConnection = SqlEntityConnection<ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\etern_000\Documents\My DBs\FsCsMvc41\database.mdf';Integrated Security=True;Connect Timeout=30",
                                                       Pluralize = true>

  let internal context = EntityConnection.GetDataContext