import json
from typing import List, Type, Optional
import uuid

from fastapi import FastAPI, HTTPException
from fastapi.responses import JSONResponse
app = FastAPI()
from pydantic import BaseModel, Field
from datetime import datetime, timezone

class FileManager:
    def __init__(self, filename: str):
        self.filename = filename
        self.__objects = []

    def read_from_json(self, dto: Type[BaseModel]) -> List[BaseModel]:
        with open(self.filename, "r", encoding="utf-8") as f:
            raw_data = json.load(f)
            self.__objects = [dto.model_validate(el, from_attributes=True) for el in raw_data]
            return self.__objects

    def save_to_json(self):
        with open(self.filename, "w", encoding="utf-8") as f:
            f.write(json.dumps([el.model_dump(mode='json') for el in self.__objects], ensure_ascii=False, indent=2))


class PetDto(BaseModel):
    id: str = Field(default_factory=lambda: str(uuid.uuid4()))
    name: str = Field(min_length=3, max_length=100)
    species: str = Field(min_length=3, max_length=100)
    hunger: int = Field(ge=0, le=100, default=50)
    happiness: int = Field(ge=0, le=100, default=50)
    last_fed: datetime = Field(le=datetime.now(timezone.utc), default_factory=lambda: datetime.now(timezone.utc))
    last_played: datetime = Field(le=datetime.now(timezone.utc), default_factory=lambda: datetime.now(timezone.utc))

class Pets:
    def __init__(self, file_manager: FileManager):
        self.file_manager = file_manager
        self.__pets = self.file_manager.read_from_json(PetDto)

    def get_all(self):
        return self.__pets

    def get_by_id(self, pet_id: str):
        pet = next((p for p in self.__pets if p.id == pet_id), None)
        return pet

    def add_pet(self, new_pet: PetDto):
        self.__pets.append(new_pet)
        self.file_manager.save_to_json()
        return new_pet

    def feed_pet(self, pet_id: str):
        hungry_pet = self.get_by_id(pet_id)
        if hungry_pet.hunger > 20:
            hungry_pet.hunger -= 20
        else:
            hungry_pet.hunger = 0

        hungry_pet.last_fed = datetime.now(timezone.utc)

        self.file_manager.save_to_json()
        return hungry_pet

    def play_with_pet(self, pet_id: str):
        happy_pet = self.get_by_id(pet_id)
        happy_pet.happiness += 20
        if happy_pet.happiness > 100:
            happy_pet.happiness = 100

        happy_pet.last_played = datetime.now(timezone.utc)

        self.file_manager.save_to_json()
        return happy_pet

    def get_pets_in_range(self, min_hunger, max_happiness):
        pets_in_range = self.__pets

        if min_hunger is not None:
            pets_in_range = [p for p in pets_in_range if p.hunger >= min_hunger]

        if max_happiness is not None:
            pets_in_range = [p for p in pets_in_range if p.happiness <= max_happiness]

        return pets_in_range


file1 = FileManager('data.json')
pets = Pets(file1)

@app.get('/pets')
def show_all_pets():
    return pets.get_all()

@app.get('/pets/needs_attention') #повинен стояти вище за pet_id
def get_pets_in_range(min_hunger: Optional[int] = None, max_happiness: Optional[int] = None):
    return pets.get_pets_in_range(min_hunger, max_happiness)

@app.get('/pets/{pet_id}')
def get_pet_by_id(pet_id: str):
    pet = pets.get_by_id(pet_id)
    if pet is None:
        raise HTTPException(status_code=404, detail="Pet not found")
    return pet

@app.post('/pets')
def add_pet(pet: PetDto):
    added_pet = pets.add_pet(pet)
    return JSONResponse(
        status_code=201,
        content={
            "data": added_pet.model_dump(mode='json'),
            "message": "virtual pet was added"
        }
    )

@app.post('/pets/{pet_id}/feed')
def feed_pet(pet_id: str):
    pets.feed_pet(pet_id)
    pet = pets.get_by_id(pet_id)
    return JSONResponse(
        status_code=200,
        content={
            "data": pet.model_dump(mode='json'),
            "message": "virtual pet was fed"
        }
    )

@app.post('/pets/{pet_id}/play')
def play_with_pet(pet_id: str):
    pets.play_with_pet(pet_id)
    pet = pets.get_by_id(pet_id)
    return JSONResponse(
        status_code=200,
        content={
            "data": pet.model_dump(mode='json'),
            "message": "virtual pet was played"
        }
    )

if __name__ == "__main__":
    import uvicorn

    uvicorn.run(app, host="localhost", port=3331)