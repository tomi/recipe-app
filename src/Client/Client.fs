module App

open Elmish
open Elmish.Navigation
open Elmish.React
open Elmish.UrlParser
open Fable.React
open Fable.React.Props
open Fetch.Types
open Thoth.Fetch
open Fulma
open Thoth.Json

open Shared

// type Page =
//   | AllRecipes
//   | Recipe of slug : string

// let pageHash =
//   function
//   | AllRecipes -> Urls.Recipes
//   | Recipe slug -> Urls.combine [ Urls.Recipes; slug ]

// let pageParser =
//   oneOf [
//     UrlParser.map AllRecipes (UrlParser.s Urls.Recipes)
//     UrlParser.map Recipe (UrlParser.s Urls.Recipes </> UrlParser.str) ]

// The model holds data that you want to keep track of while the application is running
// in this case, we are keeping track of a counter
// we mark it as optional, because initially it will not be available from the client
// the initial value will be requested from server
// type Model = {
//   Recipes: RecipeList option
//   CurrentPage: Page
// }

// The Msg type defines what events/actions can occur while the application is running
// the state of the application changes *only* in reaction to these events
// type Msg =
// // | Increment
// // | Decrement
// // | InitialCountLoaded of Counter
// | InitialRecipesLoaded of RecipeList
// | NavigateTo of Page

// let initialCounter () = Fetch.fetchAs<RecipeList> "/api/recipe"

// let initialRecipes () = Fetch.fetchAs<RecipeList> "/api/recipe"

// let urlUpdate (result: Option<Page>) state =
//   match result with
//   | Some page ->
//     { state with CurrentPage = page }, Cmd.none
//   | None ->
//     state, Navigation.modifyUrl "recipes"

// defines the initial state and initial command (= side-effect) of the application
// let init (result) : Model * Cmd<Msg> =
//   let initialModel = {
//     Recipes = None
//     CurrentPage = AllRecipes
//   }

//   let (model, cmd) = urlUpdate result initialModel
//   model, cmd
//   // let loadCountCmd =
//   //   Cmd.OfPromise.perform initialRecipes () InitialRecipesLoaded

//   // initialModel, loadCountCmd

// The update function computes the next state of the application based on the current state and the incoming events/messages
// It can also run side-effects (encoded as commands) like calling the server via Http.
// these commands in turn, can dispatch messages to which the update function will react.
// let update (msg : Msg) (state : Model) : Model * Cmd<Msg> =
//   match msg with
//   // | Some counter, Increment ->
//   //   let nextModel = { currentModel with Counter = Some { Value = counter.Value + 1 } }
//   //   nextModel, Cmd.none
//   // | Some counter, Decrement ->
//   //   let nextModel = { currentModel with Counter = Some { Value = counter.Value - 1 } }
//   //   nextModel, Cmd.none
//   // | _, InitialCountLoaded initialCount->
//   //   let nextModel = { currentModel with Counter = Some initialCount }
//   //   nextModel, Cmd.none
//   | InitialRecipesLoaded initialRecipes ->
//     let nextState = { state with Recipes = Some initialRecipes }
//     nextState, Cmd.none
//   | _ -> state, Cmd.none
//   // | NavigateTo page ->
//   //   let nextUrl = Urls.hashPrefix (pageHash page)
//   //   state, Urls.newUrl nextUrl

let safeComponents =
    let components =
        span [ ]
           [ a [ Href "https://github.com/SAFE-Stack/SAFE-template" ]
               [ str "SAFE  "
                 str Version.template ]
             str ", "
             a [ Href "https://saturnframework.github.io" ] [ str "Saturn" ]
             str ", "
             a [ Href "http://fable.io" ] [ str "Fable" ]
             str ", "
             a [ Href "https://elmish.github.io" ] [ str "Elmish" ]
             str ", "
             a [ Href "https://fulma.github.io/Fulma" ] [ str "Fulma" ]
           ]

    span [ ]
        [ str "Version "
          strong [ ] [ str Version.app ]
          str " powered by: "
          components ]

// let show = function
// // | { Counter = Some counter } -> string counter.Value
// // | { Counter = None   } -> "Loading..."
// | { Recipes = Some recipes } -> "Ready"
// | { Recipes = None } -> "Loading..."

// let button txt onClick =
//     Button.button
//         [ Button.IsFullWidth
//           Button.Color IsPrimary
//           Button.OnClick onClick ]
//         [ str txt ]

// let formatDuration (recipe : Recipe) =
//   match recipe.DurationMax with
//   | Some maxDuration -> sprintf "%d - %d min" recipe.DurationMin maxDuration
//   | None -> sprintf "%d min" recipe.DurationMin

// let recipesList (recipes : RecipeList) (dispatch : Msg -> unit) =
//   let recipeRows =
//     recipes
//     |> Seq.map (fun recipe -> tr [] [
//       td [] [ a [ OnClick(fun ev -> dispatch (NavigateTo (Recipe recipe.Name))) ] [ str recipe.Name ] ]
//       td [] [ str (formatDuration recipe) ] ])

//   Table.table [ Table.IsStriped ]
//     [ thead []
//         [ tr []
//             [ th [] [ str "Nimi" ]
//               th [] [ str "Kesto" ] ] ]
//       tbody [] recipeRows ]

// let view (model : Model) (dispatch : Msg -> unit) =
//     div []
//         [ Navbar.navbar [ Navbar.Color IsPrimary ]
//             [ Navbar.Item.div [ ]
//                 [ Heading.h2 [ ]
//                     [ str "SAFE Template" ] ] ]

//           Container.container []
//               [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
//                     [
//                         match model.Recipes with
//                         | Some recipes -> yield recipesList recipes dispatch
//                         | None -> yield div [] [ str "Loading" ]
//                     ]
//                         // Heading.h3 []
//                         // [ str ("Press buttons to manipulate counter: " + show model) ]
//                 // Columns.columns []
//                 //     [ Column.column [] [ button "-" (fun _ -> dispatch Decrement) ]
//                 //       Column.column [] [ button "+" (fun _ -> dispatch Increment) ] ]
//                 ]

//           Footer.footer [ ]
//                 [ Content.content [ Content.Modifiers [ Modifier.TextAlignment (Screen.All, TextAlignment.Centered) ] ]
//                     [ safeComponents ] ] ]

#if DEBUG
open Elmish.Debug
open Elmish.HMR
#endif

Program.mkProgram App.State.init App.State.update App.View.render
|> Program.toNavigable (parseHash App.State.pageParser) App.State.urlUpdate
#if DEBUG
|> Program.withConsoleTrace
#endif
|> Program.withReactBatched "elmish-app"
#if DEBUG
|> Program.withDebugger
#endif
|> Program.run
