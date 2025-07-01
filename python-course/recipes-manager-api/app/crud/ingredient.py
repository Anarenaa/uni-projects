from typing import Annotated

from fastapi import Depends
from sqlalchemy import select, delete

from app.db import DatabaseContext
from app.models.ingredient import Ingredient
from app.models.recipe import Recipe
from app.schemas.ingredient import IngredientDTO, IngredientBase


class IngredientRepository:
    def __init__(self, db_context: DatabaseContext):
        self.db_context = db_context

    def get_ingredients(self):
        query = select(Ingredient)
        return self.db_context.execute(query).scalars().all()

    def get_ingredient_by_id(self, ingredient_id: int):
        query = select(Ingredient).where(Ingredient.id == ingredient_id)
        return self.db_context.execute(query).scalar_one_or_none()
    
    def get_ingredients_by_recipe_id(self, recipe_id: int):
        query = select(Ingredient).where(Ingredient.recipe_id == recipe_id)
        return self.db_context.execute(query).scalars().all()

    #def delete_ingredients_by_recipe_id(self, recipe_id: int):
    #    query = delete(Ingredient).where(Ingredient.recipe_id == recipe_id)
    #    self.db_context.execute(query)
    #    self.db_context.commit()

    def create_ingredient(self, ingredient: IngredientDTO) -> Ingredient | None:
        query = select(Recipe).where(Recipe.id == ingredient.recipe_id)
        recipe = self.db_context.execute(query).scalar_one_or_none()

        if not recipe:
            return None

        new_ingredient = Ingredient(
            name=ingredient.name,
            quantity=ingredient.quantity,
            unit=ingredient.unit,
            recipe_id=ingredient.recipe_id,
        )
        self.db_context.add(new_ingredient)
        self.db_context.commit()
        return new_ingredient

    def update_ingredient(self, ingredient_id: int, ingredient: IngredientDTO) -> Ingredient | None:
        updated_ingredient = self.get_ingredient_by_id(ingredient_id)
        if updated_ingredient is None:
            return None

        updated_ingredient.name = ingredient.name
        updated_ingredient.quantity = ingredient.quantity
        updated_ingredient.unit = ingredient.unit
        updated_ingredient.recipe_id = ingredient.recipe_id

        self.db_context.add(updated_ingredient)
        self.db_context.commit()
        return updated_ingredient

    def delete_ingredient(self, ingredient_id: int) -> Ingredient | None:
        deleted_ingredient = self.get_ingredient_by_id(ingredient_id)
        if deleted_ingredient is None:
            return None

        self.db_context.delete(deleted_ingredient)
        self.db_context.commit()
        return deleted_ingredient

IngredientRepositoryDependency = Annotated[IngredientRepository, Depends(IngredientRepository)]