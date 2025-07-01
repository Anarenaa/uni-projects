from typing import Annotated

from fastapi import Depends

from crud.airplane import AirplaneRepositoryDependency
from models.airplane import Airplane
from schemas.airplane import AirplaneDto


class AirplaneService:
    def __init__(self, airplane_repository: AirplaneRepositoryDependency):
        self.airplane_repository = airplane_repository

    def get_airplanes(self) -> list[AirplaneDto]:
        airplanes = self.airplane_repository.get_airplanes()

        return [AirplaneDto.model_validate(a) for a in airplanes]

    def get_airplane_by_id(self, a_id: int) -> AirplaneDto | None:
        airplane = self.airplane_repository.get_airplane_by_id(a_id)
        if not airplane:
            return None

        return AirplaneDto.model_validate(airplane)

    def create_airplane(self, airplane: Airplane) -> AirplaneDto | None:
        new_airplane = self.airplane_repository.create_airplane(airplane)
        if not new_airplane:
            return None

        return AirplaneDto.model_validate(new_airplane)

    def update_airplane(self, a_id: int, airplane: Airplane) -> AirplaneDto | str:
        updated_airplane = self.airplane_repository.update_airplane(a_id, airplane)
        if isinstance(updated_airplane, str):
            return updated_airplane

        return AirplaneDto.model_validate(updated_airplane)

    def delete_airplane(self, a_id: int) -> AirplaneDto | None:
        deleted_airplane = self.airplane_repository.delete_airplane(a_id)
        if not deleted_airplane:
            return None

        return AirplaneDto.model_validate(deleted_airplane)

AirplaneServiceDependency = Annotated[AirplaneService, Depends(AirplaneService)]