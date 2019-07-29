module App.Types

open Shared

type Page =
  | AllRecipes
  | Recipe of slug : string

type AppMsg =
  | LoadRecipes
  | LoadRecipesFinished of RecipeList
  | LoadRecipesError of error : string

type AppState = {
  Recipes: RemoteCall<RecipeList>
  CurrentPage : Page
}
