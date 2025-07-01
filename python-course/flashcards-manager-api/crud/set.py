from typing import Annotated

from fastapi import Depends
from sqlalchemy import select

from db import DatabaseContext
from models import Set, Category, Collection
from schemas.set import SetDTO


class SetRepository:
    def __init__(self, db: DatabaseContext):
        self.db = db

    def get_all(self) -> list[Set]:
        query = select(Set)

        return self.db.execute(query).scalars().all()

    def get_by_id(self, set_id: int) -> Set | None:
        query = select(Set).where(Set.id == set_id)
        existing_set = self.db.execute(query).scalar_one_or_none()
        if not existing_set:
            return None

        return existing_set

    def __get_collection_by_id(self, collection_id: int) -> Collection | str:
        query = select(Collection).where(Collection.id == collection_id)
        existing_collection = self.db.execute(query).scalar_one_or_none()
        if not existing_collection:
            return "collection-not-found"

        return existing_collection

    def get_all_by_collection_id(self, collection_id: int) -> list[Set] | str:
        existing_collection = self.__get_collection_by_id(collection_id)
        if existing_collection == "collection-not-found":
            return existing_collection

        query = select(Set).where(Set.collection_id == collection_id)
        return self.db.execute(query).scalars().all()

    def __new_categories_in_set(self, created_set: Set, category_ids: list[int] = []) -> str | None:
        for category_id in category_ids:
            existing_category = self.db.execute(
                select(Category).where(Category.id == category_id)
            ).scalar_one_or_none()

            if not existing_category:
                return "category-not-found"

            created_set.categories.append(existing_category) # запис в SetCategories створиться автоматично

        return None

    def create(self, set_object: SetDTO, category_ids: list[int] = []) -> Set | str:
        if set_object.collection_id:
            existing_collection = self.__get_collection_by_id(set_object.collection_id)
            if existing_collection == "collection-not-found":
                return existing_collection

        created_set = Set(
            name=set_object.name,
            description=set_object.description,
            collection_id=set_object.collection_id
        )
        self.db.add(created_set)
        self.db.flush()

        if self.__new_categories_in_set(created_set, category_ids):
            return "category-not-found"

        self.db.commit()
        return created_set

    def update(self, set_id: int, set_object: SetDTO, category_ids: list[int] = []) -> Set | str:
        existing_set = self.get_by_id(set_id)
        if not existing_set:
            return "set-not-found"

        if set_object.collection_id:
            existing_collection = self.__get_collection_by_id(set_object.collection_id)
            if existing_collection == "collection-not-found":
                return existing_collection

        existing_set.name = set_object.name
        existing_set.description = set_object.description
        existing_set.collection_id = set_object.collection_id

        existing_set.categories.clear()
        if self.__new_categories_in_set(existing_set, category_ids):
            return "category-not-found"

        self.db.commit()

        return existing_set

    def delete(self, set_id: int) -> Set | None:
        existing_set = self.get_by_id(set_id)
        if not existing_set:
            return None

        self.db.delete(existing_set)
        self.db.commit()

        return existing_set

SetRepositoryDependency = Annotated[SetRepository, Depends(SetRepository)]