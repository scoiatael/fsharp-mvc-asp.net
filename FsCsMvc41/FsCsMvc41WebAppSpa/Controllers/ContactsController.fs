namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open FsWeb.Models

type ContactsController() =
    inherit ApiController()

    // GET /api/contacts
    member x.Get() = DB.getAll()
    // POST /api/contacts
    member x.Post ([<FromBody>] contact : Contact) = DB.createNew contact