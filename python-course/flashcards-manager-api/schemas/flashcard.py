from datetime import datetime

from pydantic import BaseModel, PositiveInt, Field
from pydantic_settings import SettingsConfigDict


class FlashcardDTO(BaseModel):
    id: PositiveInt | None = None
    word: str = Field(max_length=255)
    translation: str = Field(max_length=255)
    created_at: datetime = Field(default=datetime.now(), le=datetime.now())
    set_id: PositiveInt

    model_config = SettingsConfigDict(from_attributes=True)
