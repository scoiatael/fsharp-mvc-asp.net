namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open System.Collections.Generic
open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders

open FsWeb.Models

type internal EntityConnection = SqlEntityConnection<ConnectionString="Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename='C:\Users\etern_000\Documents\My DBs\FsCsMvc41\database.mdf';Integrated Security=True;Connect Timeout=30",
                                                     Pluralize = true>
type internal Contact = EntityConnection.ServiceTypes.Contact

type ContactsController() =
    inherit ApiController()

    let db = EntityConnection.GetDataContext()
    let contactsTable = db.Contacts

    let contactFrom (contact : EntityConnection.ServiceTypes.Contact ) = MyContact(
                                                                                   FirstName = contact.FirstName,
                                                                                   LastName  = contact.LastName,
                                                                                   Email     = contact.Email,
                                                                                   Avatar    = contact.Avatar
    )

    let getContacts() = contactsTable |> Seq.map contactFrom

    let setFields (contact : MyContact) = fun (ofContact : EntityConnection.ServiceTypes.Contact ) ->
      ofContact.Avatar <- contact.Avatar;
      ofContact.FirstName <- contact.FirstName;
      ofContact.LastName <- contact.LastName;
      ofContact

    let saveChanges() = try
                          db.DataContext.SaveChanges() |> ignore; new System.Net.Http.HttpResponseMessage(enum<System.Net.HttpStatusCode>200)
                           with
                             | :? System.Data.UpdateException as ex -> new System.Net.Http.HttpResponseMessage(enum<System.Net.HttpStatusCode>500)

    let insertContact (contact : MyContact) = contactsTable.CreateObject(
                                                                 Avatar    = contact.Avatar,
                                                                 FirstName = contact.FirstName,
                                                                 LastName  = contact.LastName,
                                                                 Email     = contact.Email ) |> fun contact -> db.DataContext.AddObject("Contacts", contact); saveChanges()

    let findByEmail (contact : MyContact) = query { for row in contactsTable do 
                                                                where (row.Email.Equals contact.Email)
                                                                select row }  |> seq |> Seq.exactlyOne                                                                
                                                                
    let deleteContact (contact : MyContact) = findByEmail(contact) |> contactsTable.DeleteObject; saveChanges()
    let updateContact (contact : MyContact) = findByEmail(contact) |> setFields(contact) |> contactsTable.ApplyCurrentValues |> ignore; saveChanges()

    let getAll() = getContacts()
    let createNew = insertContact
    let delete = deleteContact
    let update = updateContact


    // GET /api/contacts
    member x.Get() = getAll()
    // POST /api/contacts
    member x.Post ([<FromBody>] contact : MyContact ) = Contact.CreateContact(contact.Email) |> 
                                                        setFields(contact) |> 
                                                        contactsTable.AddObject; 
                                                        saveChanges()

    member x.Put ([<FromBody>] contact : MyContact) = update contact

    member x.Delete ([<FromBody>] contact : MyContact) = delete contact