from fastapi import Depends
from typing import Annotated, List

from crud.category import CategoryRepositoryDependency
from schemas.category import CategoryDTO
from schemas.set import SetDTO


class CategoryService:
    def __init__(self, category_repository: CategoryRepositoryDependency):
        self.category_repository = category_repository

    def get_all(self) -> List[CategoryDTO]:
        categories = self.category_repository.get_all()
        return [CategoryDTO.model_validate(category) for category in categories]

    def get_by_id(self, category_id: int) -> CategoryDTO | None:
        category = self.category_repository.get_by_id(category_id)
        if not category:
            return None
        return CategoryDTO.model_validate(category)

    def get_category_sets(self, category_id: int) -> List[SetDTO] | None:
        sets = self.category_repository.get_category_sets(category_id)
        if not sets:
            return None
        return [SetDTO.model_validate(s) for s in sets]

    def create(self, category_dto: CategoryDTO, set_ids: list[int]) -> CategoryDTO | str:
        category = self.category_repository.create(category_dto, set_ids)
        if category == "set-not-found":
            return category
        return CategoryDTO.model_validate(category)

    def update(self, category_id: int, category: CategoryDTO, set_ids: list[int]) -> CategoryDTO | None:
        category = self.category_repository.update(category_id, category, set_ids)
        if not category:
            return None
        return CategoryDTO.model_validate(category)

    def delete(self, category_id: int) -> CategoryDTO | None:
        category = self.category_repository.delete(category_id)
        if not category:
            return None
        return CategoryDTO.model_validate(category)

CategoryServiceDependency = Annotated[CategoryService, Depends(CategoryService)]
