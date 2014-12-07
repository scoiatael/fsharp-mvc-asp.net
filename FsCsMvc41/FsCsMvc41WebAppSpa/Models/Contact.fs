namespace FsWeb.Models

type Contact() =
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable email = ""
    member x.FirstName with get() = firstName and set v = firstName <- v
    member x.LastName with get() = lastName and set v = lastName <- v
    member x.Email with get() = email and set v = email <- v
