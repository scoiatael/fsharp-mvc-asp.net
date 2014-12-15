namespace FsWeb.Models

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open System.Collections.Generic
open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders


type internal EntityConnection = SqlEntityConnection<ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\etern_000\Documents\My DBs\FsCsMvc41\database.mdf';Integrated Security=True;Connect Timeout=30",
                                                     Pluralize = true>

type Contact() =
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable email = ""


    member x.FirstName with get() = firstName and set v = firstName <- v
    member x.LastName with get() = lastName and set v = lastName <- v
    member x.Email with get() = email and set v = email <- v

type DB() =
    static let db = EntityConnection.GetDataContext()
    static let contactsTable = db.Contacts

    static let contactFrom (contact : EntityConnection.ServiceTypes.Contact ) = Contact(
                                                                                     FirstName = contact.FirstName,
                                                                                     LastName  = contact.LastName,
                                                                                     Email     = contact.Email
    )

    static let getContacts() = contactsTable |> Seq.map contactFrom

    static let setFields (contact : Contact) = fun (ofContact : EntityConnection.ServiceTypes.Contact ) ->
      ofContact.Email <- contact.Email
      ofContact.FirstName <- contact.FirstName
      ofContact.LastName <- contact.LastName
      ; ofContact

    static let saveChanges() = try
                                db.DataContext.SaveChanges() |> ignore; new System.Net.Http.HttpResponseMessage(enum<System.Net.HttpStatusCode>200)
                                 with
                                   | :? System.Data.UpdateException as ex -> new System.Net.Http.HttpResponseMessage(enum<System.Net.HttpStatusCode>500)

    static let insertContact (contact : Contact) = db.Contacts.CreateObject(
                                                                 FirstName = contact.FirstName,
                                                                 LastName  = contact.LastName,
                                                                 Email     = contact.Email ) |> ignore; saveChanges()

    static member getAll() = getContacts()
    static member createNew = insertContact
