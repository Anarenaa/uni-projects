from fastapi import APIRouter, HTTPException

from core.manufacturer import ManufacturerServiceDependency
from schemas.manufacturer import ManufacturerDto

manufacturer_router = APIRouter(
    prefix="/manufacturers"
)

@manufacturer_router.get("")
def get_manufacturers(manufacturer_service: ManufacturerServiceDependency):
    return manufacturer_service.get_manufacturers()

@manufacturer_router.post("")
def create_manufacturer(manufacturer: ManufacturerDto, manufacturer_service: ManufacturerServiceDependency):
    new_manufacturer = manufacturer_service.create_manufacturer(manufacturer)
    if new_manufacturer is None:
        raise HTTPException(
            status_code=400,
            detail=f"Manufacturer is already exist."
        )
    return [
        new_manufacturer,
        {"message": "Manufacturer was created"}
    ]

@manufacturer_router.delete("/{manufacturer_id}")
def delete_manufacturer(manufacturer_id:int , manufacturer_service: ManufacturerServiceDependency):
    manufacturer = manufacturer_service.delete_manufacturer(manufacturer_id)
    if manufacturer is None:
        raise HTTPException(
            status_code=404,
            detail="Manufacturer not found"
        )
    return [
        manufacturer,
        {"message": "Manufacturer was deleted"}
    ]