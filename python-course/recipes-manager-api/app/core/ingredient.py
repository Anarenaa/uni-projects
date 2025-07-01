from app.schemas.ingredient import IngredientDTO, IngredientBase
from app.models.ingredient import Ingredient

from app.crud.ingredient import IngredientRepositoryDependency
from fastapi import Depends
from typing import Annotated

class IngredientService:
    def __init__(self, ingredient_repository: IngredientRepositoryDependency):
        self.ingredient_repository = ingredient_repository

    def get_ingredients(self) -> list[IngredientDTO] | None:
        ingredients = self.ingredient_repository.get_ingredients()

        return [IngredientDTO.model_validate(ingredient) for ingredient in ingredients]

    def get_ingredient_by_id(self, ingredient_id: int) -> IngredientDTO | None:
        ingredient = self.ingredient_repository.get_ingredient_by_id(ingredient_id)
        if not ingredient:
            return None

        return IngredientDTO.model_validate(ingredient)
    
    def get_ingredients_by_recipe_id(self, recipe_id: int) -> list[IngredientBase] | None:
        ingredients = self.ingredient_repository.get_ingredients_by_recipe_id(recipe_id)
        if not ingredients:
            return None

        return [IngredientBase.model_validate(ingredient) for ingredient in ingredients]

    def create_ingredient(self, ingredient: Ingredient) -> IngredientDTO | None:
        created_ingredient = self.ingredient_repository.create_ingredient(ingredient)  
        if not created_ingredient:
            return None

        return IngredientDTO.model_validate(created_ingredient)

    def update_ingredient(self, ingredient_id: int, ingredient: Ingredient) -> IngredientDTO | None:
        updated_ingredient = self.ingredient_repository.update_ingredient(ingredient_id, ingredient)
        if not updated_ingredient:
            return None
        
        return IngredientDTO.model_validate(updated_ingredient)

    def delete_ingredient(self, ingredient_id: int) -> IngredientDTO | None:
        deleted_ingredient = self.ingredient_repository.delete_ingredient(ingredient_id)
        if not deleted_ingredient:
            return None
        
        return IngredientDTO.model_validate(deleted_ingredient)

IngredientServiceDependency = Annotated[IngredientService, Depends(IngredientService)]        