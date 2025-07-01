from fastapi import Depends
from typing import Annotated

from crud.collection import CollectionRepositoryDependency
from models import Collection
from schemas.collection import CollectionDTO

class CollectionService:
    def __init__(self, collection_repository: CollectionRepositoryDependency):
        self.collection_repository = collection_repository

    def get_all(self) -> list[CollectionDTO]:
        collections = self.collection_repository.get_all()

        return [CollectionDTO.model_validate(collection) for collection in collections]

    def get_by_id(self, collection_id: int) -> CollectionDTO | None:
        existing_collection = self.collection_repository.get_by_id(collection_id)
        if not existing_collection:
            return None

        return CollectionDTO.model_validate(existing_collection)

    def create(self, collection: Collection) -> CollectionDTO | None:
        created_collection = self.collection_repository.create(collection)

        return CollectionDTO.model_validate(created_collection)

    def update(self, collection_id: int, collection: Collection) -> CollectionDTO | None:
        updated_collection = self.collection_repository.update(collection_id, collection)
        if not updated_collection:
            return None

        return CollectionDTO.model_validate(updated_collection)

    def delete(self, collection_id: int) -> CollectionDTO | None:
        deleted_collection = self.collection_repository.delete(collection_id)
        if not deleted_collection:
            return None

        return CollectionDTO.model_validate(deleted_collection)

CollectionServiceDependency = Annotated[CollectionService, Depends(CollectionService)]