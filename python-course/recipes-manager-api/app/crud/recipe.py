from typing import Annotated

from fastapi import Depends
from sqlalchemy import select

from app.db import DatabaseContext
from app.models.recipe import Recipe
from app.schemas.recipe import RecipeInputDTO

class RecipeRepository:
    def __init__(self, db_context: DatabaseContext):
        self.db_context = db_context

    def get_recipes(self):
        query = select(Recipe)
        return self.db_context.execute(query).scalars().all()

    def get_recipe_by_id(self, recipe_id: int):
        query = select(Recipe).where(Recipe.id == recipe_id)
        return self.db_context.execute(query).scalar_one_or_none()

    def create_recipe(self, recipe: RecipeInputDTO) -> Recipe:
        new_recipe = Recipe(
            title=recipe.title,
            steps=recipe.steps,
        )
        self.db_context.add(new_recipe)
        self.db_context.commit()
        return new_recipe

    def update_recipe(self, recipe_id: int, recipe: RecipeInputDTO) -> Recipe | None:
        updated_recipe = self.get_recipe_by_id(recipe_id)
        if updated_recipe is None:
            return None

        updated_recipe.title = recipe.title
        updated_recipe.steps = recipe.steps

        self.db_context.add(updated_recipe)
        self.db_context.commit()
        return updated_recipe

    def delete_recipe(self, recipe_id: int) -> Recipe | None:
        deleted_recipe = self.get_recipe_by_id(recipe_id)
        if deleted_recipe is None:
            return None

        self.db_context.delete(deleted_recipe)
        self.db_context.commit()
        return deleted_recipe

RecipeRepositoryDependency = Annotated[RecipeRepository, Depends(RecipeRepository)]