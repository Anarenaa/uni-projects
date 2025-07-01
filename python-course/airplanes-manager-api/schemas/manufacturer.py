from pydantic import BaseModel, Field, PositiveInt
from pydantic_settings import SettingsConfigDict


class ManufacturerDto(BaseModel):
    id: PositiveInt | None = None
    name: str = Field(max_length=255)
    headquarters_location: str | None = Field(max_length=255, default=None)

    model_config = SettingsConfigDict(from_attributes=True)