module App.View

open Components
open App.Types
open Fable.React
open Fable.React.Props
open Shared
open Fulma

let formatDuration (recipe : Recipe) =
  match recipe.DurationMax with
  | Some maxDuration -> sprintf "%d - %d min" recipe.DurationMin maxDuration
  | None -> sprintf "%d min" recipe.DurationMin

let recipesList (recipes : RecipeList) dispatch =
  let recipeRows =
    recipes
    |> Seq.map (fun recipe -> tr [] [
      td [] [ a [ Href (sprintf "#/recipes/%s" recipe.Name) ] [ str recipe.Name ] ]
      td [] [ str (formatDuration recipe) ] ])

  Table.table [ Table.IsStriped ]
    [ thead []
        [ tr []
            [ th [] [ str "Nimi" ]
              th [] [ str "Kesto" ] ] ]
      tbody [] recipeRows ]

let render (state: AppState) dispatch =
  match state.CurrentPage with
  | AllRecipes ->
    match state.Recipes with
    | Uninitialized -> div [] []
    | Loading -> loadingIndicator
    | LoadError error -> div [] [ str (sprintf "Error: %s" error) ]
    | Body recipes -> recipesList recipes dispatch
  | Recipe slug -> div [] [ str "Todo" ]
