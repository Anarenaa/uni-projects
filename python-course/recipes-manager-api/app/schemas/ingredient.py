from pydantic import BaseModel, Field, PositiveInt
from pydantic_settings import SettingsConfigDict

class IngredientDTO(BaseModel):
    id: PositiveInt | None = None
    name: str = Field(max_length=255)
    quantity: PositiveInt = Field(max_value=3000)
    unit: str = Field(max_length=25)
    recipe_id: PositiveInt

    model_config = SettingsConfigDict(from_attributes=True)

class IngredientBase(BaseModel):
    name: str = Field(max_length=255)
    quantity: PositiveInt = Field(max_value=3000)
    unit: str = Field(max_length=25)

    model_config = SettingsConfigDict(from_attributes=True)