from typing import Annotated

from fastapi import Depends
from sqlalchemy import select, exists

from db import DatabaseContext
from models.airplane import Airplane
from models.manufacturer import Manufacturer
from schemas.airplane import AirplaneDto

class AirplaneRepository:
    def __init__(self, db_context: DatabaseContext):
        self.db_context = db_context

    def get_airplanes(self):
        query = select(Airplane)

        return self.db_context.execute(query).scalars().all()

    def get_airplane_by_id(self, a_id: int) -> Airplane | None:
        query = select(Airplane).where(Airplane.id == a_id)
        airplane = self.db_context.execute(query).scalar_one_or_none()

        return airplane

    def __check_manufacturer(self, airplane: AirplaneDto) -> Manufacturer | None:
        query = select(Manufacturer).where(Manufacturer.name == airplane.manufacturer)
        manufacturer = self.db_context.execute(query).scalar_one_or_none()

        return manufacturer

    def __check_tail_number(self, airplane: AirplaneDto) -> None | bool:
        if airplane.tail_number is not None:
            query = select(exists().where(Airplane.tail_number == airplane.tail_number))
            return self.db_context.execute(query).scalar()

    def create_airplane(self, airplane: AirplaneDto) -> Airplane | None:

        manufacturer = self.__check_manufacturer(airplane)
        if not manufacturer:
            return None

        if self.__check_tail_number(airplane):
            return None

        new_airplane = Airplane(
            model=airplane.model,
            manufacturer=manufacturer.name,
            year=airplane.year,
            tail_number=airplane.tail_number
        )

        self.db_context.add(new_airplane)
        self.db_context.commit() #оновлення в базі даних

        return new_airplane

    def update_airplane(self, airplane_id: int, airplane: AirplaneDto) -> Airplane | str:
        updated_airplane = self.get_airplane_by_id(airplane_id)
        if not updated_airplane:
            return "not_found"

        manufacturer = self.__check_manufacturer(airplane)
        if not manufacturer:
            return "invalid_manufacturer"

        if self.__check_tail_number(airplane):
            return "tail_number_not_unique"

        updated_airplane.model = airplane.model
        updated_airplane.manufacturer = manufacturer.name
        updated_airplane.year = airplane.year
        updated_airplane.tail_number = airplane.tail_number

        self.db_context.commit()
        self.db_context.refresh(updated_airplane)

        return updated_airplane

    def delete_airplane(self, airplane_id: int) -> Airplane | None:
        deleted_airplane = self.get_airplane_by_id(airplane_id)
        if not deleted_airplane:
            return None

        self.db_context.delete(deleted_airplane)
        self.db_context.commit()

        return deleted_airplane

AirplaneRepositoryDependency = Annotated[AirplaneRepository, Depends(AirplaneRepository)]