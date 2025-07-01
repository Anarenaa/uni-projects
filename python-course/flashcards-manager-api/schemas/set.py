from typing import List

from pydantic import BaseModel, PositiveInt, Field
from pydantic_settings import SettingsConfigDict

from schemas.category import CategoryDTO
from schemas.flashcard import FlashcardDTO


class SetDTO(BaseModel):
    id: PositiveInt | None = None
    name: str = Field(max_length=255)
    description: str | None = Field(max_length=255, default=None)
    flashcards: List[FlashcardDTO] = []
    categories: List[CategoryDTO] = []
    collection_id: PositiveInt | None = None

    model_config = SettingsConfigDict(from_attributes=True)