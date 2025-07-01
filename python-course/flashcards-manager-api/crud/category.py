from typing import Annotated

from fastapi import Depends
from sqlalchemy import select
from db import DatabaseContext
from models import Category, SetsCategories, Set
from schemas.category import CategoryDTO

class CategoryRepository:
    def __init__(self, db: DatabaseContext):
        self.db = db

    def get_all(self) -> list[Category]:
        query = select(Category)
        return self.db.execute(query).scalars().all()

    def get_by_id(self, category_id: int) -> Category | None:
        query = select(Category).where(Category.id == category_id)
        existing_category = self.db.execute(query).scalar_one_or_none()
        if not existing_category:
            return None
        return existing_category

    def get_category_sets(self, category_id: int) -> list[Set] | None:
        existing_category = self.get_by_id(category_id)
        if not existing_category:
            return None
        query = select(Set).join(SetsCategories).where(SetsCategories.category_id == category_id)
        return self.db.execute(query).scalars().all()

    def create(self, category_object: CategoryDTO, set_ids: list[int] = []) -> Category | str:
        category = Category(name=category_object.name)
        self.db.add(category)
        self.db.flush()  # Assigns category.id

        for set_id in set_ids:
            exists_query = select(Set).where(Set.id == set_id)
            if self.db.execute(exists_query).scalar_one_or_none() is None:
                return "set-not-found"
            self.db.add(SetsCategories(set_id=set_id, category_id=category.id))
        self.db.commit()
        return category

    def update(self, category_id: int, new_category_object: CategoryDTO, new_set_ids: list[int] = []) -> Category | None:
        category = self.get_by_id(category_id)
        if not category:
            return None

        category.name = new_category_object.name

        self.db.query(SetsCategories).filter_by(category_id=category_id).delete()
        for set_id in new_set_ids:
            self.db.add(SetsCategories(set_id=set_id, category_id=category_id))
        self.db.commit()
        return category

    def delete(self, category_id: int) -> Category | None:
        category = self.get_by_id(category_id)
        if not category:
            return None

        self.db.delete(category)
        self.db.commit()
        return category

CategoryRepositoryDependency = Annotated[CategoryRepository, Depends(CategoryRepository)]
