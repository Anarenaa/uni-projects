from fastapi import Depends
from typing import Annotated

from crud.set import SetRepositoryDependency
from models import Set
from schemas.category import CategoryDTO
from schemas.set import SetDTO

class SetService:
    def __init__(self, set_repository: SetRepositoryDependency):
        self.set_repository = set_repository

    def get_all(self) -> list[SetDTO]:
        sets = self.set_repository.get_all()

        return [SetDTO.model_validate(s) for s in sets]

    def get_all_by_collection_id(self, collection_id: int) -> list[SetDTO] | str:
        sets = self.set_repository.get_all_by_collection_id(collection_id)
        if sets == "collection-not-found":
            return sets

        return [SetDTO.model_validate(s) for s in sets]

    def get_by_id(self, set_id: int) -> SetDTO | None:
        existing_set = self.set_repository.get_by_id(set_id)
        if not existing_set:
            return None

        return SetDTO.model_validate(existing_set)

    def get_categories(self, set_id: int) -> list[CategoryDTO] | None: #немає в crud
        existing_set = self.set_repository.get_by_id(set_id)
        if not existing_set:
            return None

        return [CategoryDTO.model_validate(c) for c in existing_set.categories]

    def create(self, s: Set, category_ids: list[int] = []) -> SetDTO | str:
        created_set = self.set_repository.create(s, category_ids)
        if isinstance(created_set, str):
            return created_set

        return SetDTO.model_validate(created_set)

    def update(self, set_id: int, s: Set, category_ids: list[int]) -> SetDTO | str:
        updated_set = self.set_repository.update(set_id, s, category_ids)
        if isinstance(updated_set, str):
            return updated_set

        return SetDTO.model_validate(updated_set)

    def delete(self, set_id: int) -> SetDTO | None:
        deleted_set = self.set_repository.delete(set_id)
        if not deleted_set:
            return None

        return SetDTO.model_validate(deleted_set)

SetServiceDependency = Annotated[SetService, Depends(SetService)]