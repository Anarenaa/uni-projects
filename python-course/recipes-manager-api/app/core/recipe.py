from app.schemas.recipe import RecipeOutputDTO
from app.models.recipe import Recipe

from app.crud.recipe import RecipeRepositoryDependency
from fastapi import Depends
from typing import Annotated

class RecipeService:
    def __init__(self, recipe_repository: RecipeRepositoryDependency):
        self.recipe_repository = recipe_repository

    def get_recipes(self):
        recipes = self.recipe_repository.get_recipes()

        return [RecipeOutputDTO.model_validate(recipe) for recipe in recipes]

    def get_recipe_by_id(self, recipe_id: int):
        recipe = self.recipe_repository.get_recipe_by_id(recipe_id)
        if not recipe:
            return None

        return RecipeOutputDTO.model_validate(recipe)

    def create_recipe(self, recipe: Recipe) -> RecipeOutputDTO:
        created_recipe = self.recipe_repository.create_recipe(recipe)
        return RecipeOutputDTO.model_validate(created_recipe)

    def update_recipe(self, recipe_id: int, recipe: Recipe) -> RecipeOutputDTO | None:
        updated_recipe = self.recipe_repository.update_recipe(recipe_id, recipe)
        if not updated_recipe:
            return None
        
        return RecipeOutputDTO.model_validate(updated_recipe)

    def delete_recipe(self, recipe_id: int) -> RecipeOutputDTO | None:
        deleted_recipe = self.recipe_repository.delete_recipe(recipe_id)
        if not deleted_recipe:
            return None
        
        return RecipeOutputDTO.model_validate(deleted_recipe)

RecipeServiceDependency = Annotated[RecipeService, Depends(RecipeService)]        