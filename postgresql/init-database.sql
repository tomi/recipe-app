CREATE DATABASE recipedb lc_collate 'fi_FI.UTF-8' lc_ctype 'fi_FI.UTF-8' encoding 'UTF8' template template0;
GRANT ALL PRIVILEGES ON DATABASE recipedb to recipeapp;
CREATE DATABASE recipedb_test lc_collate 'fi_FI.UTF-8' lc_ctype 'fi_FI.UTF-8' encoding 'UTF8' template template0;
GRANT ALL PRIVILEGES ON DATABASE recipedb_test to recipeapp;
