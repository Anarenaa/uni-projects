from typing import Annotated

from fastapi import Depends

from crud.manufacturer import ManufacturerRepositoryDependency
from models.manufacturer import Manufacturer
from schemas.manufacturer import ManufacturerDto


class ManufacturerService:
    def __init__(self, manufacturer_repository: ManufacturerRepositoryDependency):
        self.manufacturer_repository = manufacturer_repository

    def get_manufacturers(self) -> list[ManufacturerDto]:
        manufacturers = self.manufacturer_repository.get_manufacturers()

        return [ManufacturerDto.model_validate(m) for m in manufacturers]

    def create_manufacturer(self, manufacturer: Manufacturer) -> ManufacturerDto | None:
        new_manufacturer = self.manufacturer_repository.create_manufacturer(manufacturer)
        if not new_manufacturer:
            return None

        return ManufacturerDto.model_validate(new_manufacturer)

    def delete_manufacturer(self, m_id: int) -> ManufacturerDto | None:
        deleted_manufacturer = self.manufacturer_repository.delete_manufacturer(m_id)
        if not deleted_manufacturer:
            return None

        return ManufacturerDto.model_validate(deleted_manufacturer)

ManufacturerServiceDependency = Annotated[ManufacturerService, Depends(ManufacturerService)]