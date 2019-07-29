namespace Shared

type RemoteCall<'a> =
    | Uninitialized
    | Loading
    | LoadError of string
    | Body of 'a

type Ingredient = {
  Id : string
  Name : string
  Type : string
  Tags : string list
}

type RecipeIngredientQty = {
  Qty : int
  Modifier : string option
  Unit : string option
}

type RecipeIngredient = {
  Ingredient : string
  Qty : RecipeIngredientQty option
}

type RecipeStep = string

type Recipe = {
  Id : int
  Name : string
  DurationMin : int
  DurationMax : int option
  Tags : string list
  Ingredients : RecipeIngredient list
  Steps : array<RecipeStep>
}

type RecipeList = Recipe list