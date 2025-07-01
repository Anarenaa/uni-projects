from fastapi import APIRouter, HTTPException
from core.set import SetServiceDependency
from core.flashcard import FlashcardServiceDependency

from schemas.set import SetDTO

set_router = APIRouter(
    prefix="/sets"
)

@set_router.get("")
def get_all_sets(set_service: SetServiceDependency):
    return set_service.get_all()

@set_router.get("/{set_id}")
def get_set_by_id(set_id: int, set_service: SetServiceDependency):
    existing_set = set_service.get_by_id(set_id)
    if not existing_set:
        raise HTTPException(status_code=404, detail="Set not found")

    return existing_set

@set_router.get("/{set_id}/flashcards")
def get_flashcards_by_set_id(set_id: int, flashcard_service: FlashcardServiceDependency):
    existing_set = flashcard_service.get_by_set_id(set_id)
    if not existing_set:
        raise HTTPException(status_code=404, detail="Set not found")

    return flashcard_service.get_by_set_id(set_id)

@set_router.get("/{set_id}/categories")
def get_categories_by_set_id(set_id: int, set_service: SetServiceDependency):
    existing_set = set_service.get_by_id(set_id)
    if not existing_set:
        raise HTTPException(status_code=404, detail="Set not found")

    return set_service.get_categories(set_id)

@set_router.get("/{set_id}/flashcards/filter-by-alphabet")
def filter_flashcards_by_alphabet(set_id: int, flashcard_service: FlashcardServiceDependency):
    return flashcard_service.filter_by_alphabet_in_set(set_id)

@set_router.get("/{set_id}/flashcards/filter-from-new")
def filter_flashcards_from_new(set_id: int, flashcard_service: FlashcardServiceDependency):
    return flashcard_service.filter_from_new_in_set(set_id)

@set_router.post("")
def create_set(s: SetDTO, set_service: SetServiceDependency, category_ids: list[int] = []):
    created_set = set_service.create(s, category_ids)
    if isinstance(created_set, str):
        if created_set == "collection-not-found":
            raise HTTPException(status_code=404, detail="Collection not found")
        if created_set == "category-not-found":
            raise HTTPException(status_code=404, detail="Category not found")

    return created_set

@set_router.put("/{set_id}")
def update_set(set_id: int, s: SetDTO, category_ids: list[int], set_service: SetServiceDependency):
    updated_set = set_service.update(set_id, s, category_ids)
    if isinstance(updated_set, str):
        if updated_set == "set-not-found":
            raise HTTPException(status_code=404, detail="Set not found")
        if updated_set == "collection-not-found":
            raise HTTPException(status_code=404, detail="Collection not found")
        if updated_set == "category-not-found":
            raise HTTPException(status_code=404, detail="Category not found")

    return updated_set

@set_router.delete("/{set_id}")
def delete_set(set_id: int, set_service: SetServiceDependency):
    deleted_set = set_service.delete(set_id)
    if not deleted_set:
        raise HTTPException(status_code=404, detail="Set not found")

    return deleted_set