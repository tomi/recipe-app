module Urls

open Browser.Dom

let hashPrefix = sprintf "#%s"

let combine xs = List.fold (sprintf "%s/%s") "" xs

[<Literal>]
let Recipes = "recipes"

[<Literal>]
let NavigationEvent = "NavigationEvent"

let newUrl (newUrl : string) : Elmish.Cmd<_> =
    [ fun _ ->
        history.pushState ((), "", newUrl)
        let ev = document.createEvent "Event"
        ev.initEvent (NavigationEvent, true, true)
        window.dispatchEvent ev |> ignore ]

let navigate xs : Elmish.Cmd<_> =
    let nextUrl = hashPrefix (combine xs)
    [ fun _ ->
        history.pushState ((), "", nextUrl)
        let ev = document.createEvent "Event"
        ev.initEvent (NavigationEvent, true, true)
        window.dispatchEvent ev |> ignore ]
