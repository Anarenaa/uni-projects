from fastapi import APIRouter, HTTPException

from app.core.ingredient import IngredientServiceDependency
from app.schemas.ingredient import IngredientDTO

ingredient_router = APIRouter(
    prefix="/ingredients"
)

@ingredient_router.get("")
def get_ingredients(ingredient_service: IngredientServiceDependency = IngredientServiceDependency):
    return ingredient_service.get_ingredients()

@ingredient_router.get("/recipe-{recipe_id}")
def get_ingredients_by_recipe_id(recipe_id: int, ingredient_service: IngredientServiceDependency = IngredientServiceDependency):
    return ingredient_service.get_ingredients_by_recipe_id(recipe_id)

@ingredient_router.get("/{ingredient_id}")
def get_ingredient_by_id(ingredient_id: int, ingredient_service: IngredientServiceDependency = IngredientServiceDependency):
    ingredient = ingredient_service.get_ingredient_by_id(ingredient_id)
    if not ingredient:
        raise HTTPException(status_code=404, detail="Ingredient not found")
    return ingredient

@ingredient_router.post("")
def create_ingredient(ingredient: IngredientDTO, ingredient_service: IngredientServiceDependency = IngredientServiceDependency):
    created_ingredient = ingredient_service.create_ingredient(ingredient)
    if not created_ingredient:
        raise HTTPException(status_code=404, detail="Recipe not found")

    return created_ingredient

@ingredient_router.put("/{ingredient_id}")
def update_ingredient(ingredient_id: int, ingredient: IngredientDTO, ingredient_service: IngredientServiceDependency = IngredientServiceDependency):
    updated_ingredient = ingredient_service.update_ingredient(ingredient_id, ingredient)
    if not updated_ingredient:
        raise HTTPException(status_code=404, detail="Ingredient not found")
    
    return updated_ingredient

@ingredient_router.delete("/{ingredient_id}")
def delete_ingredient(ingredient_id: int, ingredient_service: IngredientServiceDependency = IngredientServiceDependency):
    deleted_ingredient = ingredient_service.delete_ingredient(ingredient_id)
    if not deleted_ingredient:
        raise HTTPException(status_code=404, detail="Ingredient not found")
    
    return deleted_ingredient
