open System.IO
open System.Threading.Tasks

open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open FSharp.Control.Tasks.V2
open Giraffe
open Saturn
open Shared

open Npgsql
open Dapper

let connectionString = "Server=127.0.0.1;Port=5445;Database=recipedb;User Id=recipeapp;
Password=recipeapp;"
let tryGetEnv = System.Environment.GetEnvironmentVariable >> function null | "" -> None | x -> Some x

let publicPath = Path.GetFullPath "../Client/public"

let port =
    "SERVER_PORT"
    |> tryGetEnv |> Option.map uint16 |> Option.defaultValue 8085us

[<CLIMutable>]
type RecipesDto = {
    Id : int
    Name : string
    DurationMin : int
    DurationMax : int option
    Steps : array<string>
    Quantity : int option
    Modifier : string option
    Unit : string option
    IngredientName : string
}

let getRecipes =
    async {
        use conn = new NpgsqlConnection(connectionString)

        let! recipeDtos = (conn.QueryAsync<RecipesDto>("""
SELECT
    r_id AS Id,
    r_name AS Name,
    r_duration_min AS DurationMin,
    r_duration_max AS DurationMax,
    r_steps AS Steps,
    ri_quantity AS Quantity,
    ri_modifier AS Modifier,
    ri_unit AS Unit,
    i_name AS IngredientName
FROM recipe
JOIN recipe_ingredient ON r_id = ri_recipe_id
JOIN ingredient ON ri_ingredient_id = i_id
        """) |> Async.AwaitTask)

        let firstRow = recipeDtos |> Seq.head
        let ingredients =
            recipeDtos
            |> Seq.map (fun dto -> {
                Ingredient = dto.IngredientName
                Qty =
                    match dto.Quantity with
                    | Some qty -> Some { Qty = qty; Modifier = dto.Modifier; Unit = dto.Unit }
                    | None -> None
            })
            |> Seq.toList

        let recipe = {
            Id = firstRow.Id
            Name = firstRow.Name
            DurationMin = firstRow.DurationMin
            DurationMax = firstRow.DurationMax
            Tags = List.Empty
            Ingredients = ingredients
            Steps = firstRow.Steps
        }

        return [recipe]
    }

let webApp = router {
    get "/api/recipe" (fun next ctx ->
        task {
            let! recipes = getRecipes

            return! json recipes next ctx
        })
}

let app = application {
    url ("http://0.0.0.0:" + port.ToString() + "/")
    use_router webApp
    memory_cache
    use_static publicPath
    use_json_serializer(Thoth.Json.Giraffe.ThothSerializer())
    use_gzip
}

run app
