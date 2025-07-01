from typing import Annotated

from fastapi import Depends
from sqlalchemy import select, exists

from db import DatabaseContext
from models.manufacturer import Manufacturer
from schemas.manufacturer import ManufacturerDto


class ManufacturerRepository:
    def __init__(self, db_context: DatabaseContext):
        self.db_context = db_context

    def get_manufacturers(self):
        query = select(Manufacturer)

        return self.db_context.execute(query).scalars().all()

    def get_manufacturer_by_id(self, m_id: int) -> Manufacturer | None:
        query = select(Manufacturer).where(Manufacturer.id == m_id)
        manufacturer = self.db_context.execute(query).scalar_one_or_none()

        return manufacturer

    def create_manufacturer(self, manufacturer: ManufacturerDto) -> Manufacturer | None:
        query = select(exists().where(Manufacturer.name == manufacturer.name)) # повертає True або False
        if self.db_context.execute(query).scalar():
            return None

        new_manufacturer = Manufacturer(
            name=manufacturer.name,
            headquarters_location=manufacturer.headquarters_location
        )

        self.db_context.add(new_manufacturer)
        self.db_context.commit()

        return new_manufacturer

    def delete_manufacturer(self, manufacturer_id: int) -> Manufacturer | None:
        deleted_manufacturer = self.get_manufacturer_by_id(manufacturer_id)
        if not deleted_manufacturer:
            return None

        self.db_context.delete(deleted_manufacturer)
        self.db_context.commit()

        return deleted_manufacturer

ManufacturerRepositoryDependency = Annotated[ManufacturerRepository, Depends(ManufacturerRepository)]