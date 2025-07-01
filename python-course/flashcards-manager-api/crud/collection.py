from typing import Annotated

from fastapi import Depends
from sqlalchemy import select

from db import DatabaseContext
from models import Collection
from schemas.collection import CollectionDTO


class CollectionRepository:
    def __init__(self, db: DatabaseContext):
        self.db = db

    def get_all(self) -> list[Collection]:
        query = select(Collection)

        return self.db.execute(query).scalars().all()

    def get_by_id(self, collection_id: int) -> Collection | None:
        query = select(Collection).where(Collection.id == collection_id)
        existing_collection = self.db.execute(query).scalar_one_or_none()
        if not existing_collection:
            return None

        return existing_collection

    def create(self, collection: CollectionDTO) -> Collection:
        created_collection = Collection(
            name=collection.name,
            description=collection.description
        )

        self.db.add(created_collection)
        self.db.commit()

        return created_collection

    def update(self, collection_id: int, collection: CollectionDTO) -> Collection | None:
        existing_collection = self.get_by_id(collection_id)
        if not existing_collection:
            return None

        existing_collection.name = collection.name
        existing_collection.description = collection.description

        self.db.commit()

        return existing_collection

    def delete(self, collection_id: int) -> Collection | None:
        existing_collection = self.get_by_id(collection_id)
        if not existing_collection:
            return None

        self.db.delete(existing_collection)
        self.db.commit()

        return existing_collection

CollectionRepositoryDependency = Annotated[CollectionRepository, Depends(CollectionRepository)]