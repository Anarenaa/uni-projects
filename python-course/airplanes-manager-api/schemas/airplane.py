from pydantic import BaseModel, Field, PositiveInt
from datetime import datetime
from pydantic_settings import SettingsConfigDict

class AirplaneDto(BaseModel):
    id: PositiveInt | None = None
    model: str = Field(max_length=255)
    manufacturer: str = Field(max_length=255)
    year: int | None = Field(ge=1500, le=datetime.now().year, default=None)
    tail_number: str | None = Field(max_length=255, default=None)

    model_config = SettingsConfigDict(from_attributes=True)