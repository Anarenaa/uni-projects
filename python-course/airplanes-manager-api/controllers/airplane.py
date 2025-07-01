from fastapi import APIRouter, HTTPException

from core.airplane import AirplaneServiceDependency
from schemas.airplane import AirplaneDto

airplane_router = APIRouter(
    prefix="/airplanes"
)

@airplane_router.get("")
def get_airplanes(airplane_service: AirplaneServiceDependency):
    return airplane_service.get_airplanes()

@airplane_router.post("")
def create_airplane(airplane: AirplaneDto, airplane_service: AirplaneServiceDependency):
    new_airplane = airplane_service.create_airplane(airplane)

    if new_airplane is None:
        raise HTTPException(
            status_code=400,
            detail=f"Check your data: manufacturer is not found, or tail number is not unique."
        )

    return [
        new_airplane,
        {"message": "Airplane was created"}
    ]

@airplane_router.get("/{airplane_id}")
def get_airplane_by_id(airplane_id: int, airplane_service: AirplaneServiceDependency):
    airplane = airplane_service.get_airplane_by_id(airplane_id)
    if airplane is None:
        raise HTTPException(
            status_code=404,
            detail="Airplane not found"
        )
    return airplane

@airplane_router.put("/{airplane_id}")
def update_airplane(airplane_id:int , airplane: AirplaneDto, airplane_service: AirplaneServiceDependency):
    airplane = airplane_service.update_airplane(airplane_id, airplane)

    if airplane == "not_found":
        raise HTTPException(
            status_code=404,
            detail="Airplane is not found"
        )
    if airplane == "invalid_manufacturer":
        raise HTTPException(
            status_code=400,
            detail="Manufacturer is not found"
        )
    if airplane == "tail_number_not_unique":
        raise HTTPException(
            status_code=404,
            detail="Tail number is not unique"
        )

    return [
        airplane,
        {"message": "Airplane was updated"}
    ]

@airplane_router.delete("/{airplane_id}")
def delete_airplane(airplane_id:int , airplane_service: AirplaneServiceDependency):
    airplane = airplane_service.delete_airplane(airplane_id)
    if airplane is None:
        raise HTTPException(
            status_code=404,
            detail="Airplane not found"
        )
    return [
        airplane,
        {"message": "Airplane was deleted"}
    ]