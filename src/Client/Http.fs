module App.Http

open Elmish
open Fetch.Types
open Thoth.Fetch
open App.Types

open Shared

let initialRecipes () = Fetch.fetchAs<RecipeList> "/api/recipe"

let loadRecipes : Cmd<AppMsg> =
  Cmd.OfPromise.perform initialRecipes () LoadRecipesFinished
