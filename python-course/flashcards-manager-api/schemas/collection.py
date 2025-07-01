from typing import List
from pydantic import BaseModel, Field, PositiveInt
from pydantic_settings import SettingsConfigDict

from schemas.set import SetDTO


class CollectionDTO(BaseModel):
    id: PositiveInt | None = None
    name: str = Field(max_length=255)
    description: str | None = Field(max_length=255, default=None)
    sets: List[SetDTO] = []

    model_config = SettingsConfigDict(from_attributes=True)