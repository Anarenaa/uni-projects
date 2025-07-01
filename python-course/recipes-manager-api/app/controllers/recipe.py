from fastapi import APIRouter, HTTPException

from app.core.recipe import RecipeServiceDependency
from app.schemas.recipe import RecipeInputDTO

recipe_router = APIRouter(
    prefix="/recipes"
)

@recipe_router.get("")
def get_recipes(recipe_service: RecipeServiceDependency = RecipeServiceDependency):
    return recipe_service.get_recipes()

@recipe_router.get("/{recipe_id}")
def get_recipe_by_id(recipe_id: int, recipe_service: RecipeServiceDependency = RecipeServiceDependency):
    recipe = recipe_service.get_recipe_by_id(recipe_id)
    if not recipe:
        raise HTTPException(status_code=404, detail="Recipe not found")
    return recipe

@recipe_router.post("")
def create_recipe(recipe: RecipeInputDTO, recipe_service: RecipeServiceDependency = RecipeServiceDependency):
    created_recipe = recipe_service.create_recipe(recipe)

    return created_recipe

@recipe_router.put("/{recipe_id}")
def update_recipe(recipe_id: int, recipe: RecipeInputDTO, recipe_service: RecipeServiceDependency = RecipeServiceDependency):
    updated_recipe = recipe_service.update_recipe(recipe_id, recipe)
    if not updated_recipe:
        raise HTTPException(status_code=404, detail="Recipe not found")
    
    return updated_recipe

@recipe_router.delete("/{recipe_id}")
def delete_recipe(recipe_id: int, recipe_service: RecipeServiceDependency = RecipeServiceDependency):
    deleted_recipe = recipe_service.delete_recipe(recipe_id)
    if not deleted_recipe:
        raise HTTPException(status_code=404, detail="Recipe not found")
    
    return deleted_recipe

