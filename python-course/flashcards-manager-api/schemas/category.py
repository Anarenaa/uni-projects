from pydantic import BaseModel, Field, PositiveInt
from pydantic_settings import SettingsConfigDict

class CategoryDTO(BaseModel):
    id: PositiveInt | None = None
    name: str = Field(max_length=255)

    model_config = SettingsConfigDict(from_attributes=True)