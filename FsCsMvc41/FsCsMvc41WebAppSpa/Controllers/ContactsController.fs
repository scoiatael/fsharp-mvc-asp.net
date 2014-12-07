namespace FsWeb.Controllers

open System.Collections.Generic
open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open FsWeb.Models
open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders

type internal EntityConnection = SqlEntityConnection<ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\etern_000\Documents\My DBs\FsCsMvc41\database.mdf';Integrated Security=True;Connect Timeout=30",
                                                     Pluralize = true>

type ContactsController() =
    inherit ApiController()

    let context = EntityConnection.GetDataContext

    // This is for demonstration purposes only. 
    let mutable contacts:seq<Contact> option = None

    let checkContacts() = if contacts = None then contacts <- Some(seq( query { for contact:Contact in context.Contacts do select contact }))
    let getContacts() = checkContacts();
                        match contacts with
                        | Some(T) -> T
    let setContacts (contacts') = contacts <- Some(contacts'); contacts'

    // GET /api/contacts
    member x.Get = getContacts
    // POST /api/contacts
    member x.Post ([<FromBody>] contact:Contact) = 
        getContacts() |> Seq.append [ contact ] |> setContacts
        