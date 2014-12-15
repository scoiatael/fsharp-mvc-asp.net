namespace FsWeb.Models

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open System.Collections.Generic
open System.Data.Linq
open System.Data.Entity
open Microsoft.FSharp.Data.TypeProviders



type MyContact = { firstName : string; lastName: string; email: string; avatar: string }
(*
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable email = ""
    let mutable avatar = "http://placehold.it/100x100"


    member x.FirstName with get() = firstName and set v = firstName <- v
    member x.LastName with get() = lastName and set v = lastName <- v
    member x.Email with get() = email and set v = email <- v
    member x.Avatar with get() = avatar and set v = avatar <- v
    *)
