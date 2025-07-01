from pydantic import BaseModel, Field, PositiveInt
from typing import List
from .ingredient import IngredientDTO

from pydantic_settings import SettingsConfigDict

class RecipeInputDTO(BaseModel):
    title: str = Field(max_length=255)
    ingredients: List[IngredientDTO] = []
    steps: List[str] = []

    model_config = SettingsConfigDict(from_attributes=True)

class RecipeOutputDTO(BaseModel):
    id: PositiveInt
    title: str = Field(max_length=255)
    ingredients: List[IngredientDTO] = []
    steps: List[str] = []

    model_config = SettingsConfigDict(from_attributes=True)