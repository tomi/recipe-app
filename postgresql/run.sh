#!/bin/sh

docker run --name recipeapp-postgres --rm -p 5445:5432 -e POSTGRES_USER=recipeapp -e POSTGRES_PASSWORD=recipeapp recipeapp_postgress
