module App.State

open Shared
open App.Types
open Elmish
open Elmish.Navigation
open Elmish.UrlParser

let pageParser : State<(Page -> Page)> -> State<Page> list =
  oneOf [
    map AllRecipes (s Urls.Recipes)
    map Recipe (s Urls.Recipes </> str) ]

let urlUpdate (result: Option<Page>) state =
  match result with
  | Some page ->
    { state with CurrentPage = page }, Cmd.none
  | None ->
    state, Navigation.modifyUrl "#/recipes"

let init result : AppState * Cmd<AppMsg> =
  let initialState = {
    Recipes = Uninitialized
    CurrentPage = AllRecipes
  }

  let (model, cmd) = urlUpdate result initialState
  model,
  Cmd.batch [ cmd
              Cmd.ofMsg LoadRecipes ]


let update (msg : AppMsg) (state : AppState) : AppState * Cmd<AppMsg> =
  match msg with
  // | Some counter, Increment ->
  //   let nextModel = { currentModel with Counter = Some { Value = counter.Value + 1 } }
  //   nextModel, Cmd.none
  // | Some counter, Decrement ->
  //   let nextModel = { currentModel with Counter = Some { Value = counter.Value - 1 } }
  //   nextModel, Cmd.none
  // | _, InitialCountLoaded initialCount->
  //   let nextModel = { currentModel with Counter = Some initialCount }
  //   nextModel, Cmd.none
  | LoadRecipes ->
    let nextState = { state with Recipes = Loading }
    nextState, Http.loadRecipes
  | LoadRecipesFinished recipes ->
    let nextState = { state with Recipes = Body recipes }
    nextState, Cmd.none
  | _ -> state, Cmd.none
  // | NavigateTo page ->
  //   let nextUrl = Urls.hashPrefix (pageHash page)
  //   state, Urls.newUrl nextUrl