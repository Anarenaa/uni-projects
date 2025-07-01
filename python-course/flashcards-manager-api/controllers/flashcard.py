from fastapi import APIRouter, HTTPException, Query
from core.flashcard import FlashcardServiceDependency

from schemas.flashcard import FlashcardDTO

flashcard_router = APIRouter(
    prefix="/flashcards"
)

@flashcard_router.get("")
def get_flashcards(flashcard_service: FlashcardServiceDependency):
    return flashcard_service.get_all()

@flashcard_router.get("/search")
def search_flashcards(flashcard_service: FlashcardServiceDependency, word: str = Query(min_length=1, max_length=50)):
    return flashcard_service.search_by_word(word)

@flashcard_router.get("/{flashcard_id}")
def get_flashcard(flashcard_id: int, flashcard_service: FlashcardServiceDependency):
    existing_flashcard = flashcard_service.get_by_id(flashcard_id)
    if not existing_flashcard:
        raise HTTPException(status_code=404, detail="Flashcard not found")

    return existing_flashcard

@flashcard_router.post("")
def create_flashcard(flashcard: FlashcardDTO, flashcard_service: FlashcardServiceDependency):
    created_flashcard = flashcard_service.create(flashcard)
    if not created_flashcard:
        raise HTTPException(status_code=500, detail="Create a set first")

    return created_flashcard

@flashcard_router.put("/{flashcard_id}")
def update_flashcard(flashcard_id: int, flashcard: FlashcardDTO, flashcard_service: FlashcardServiceDependency):
    updated_flashcard = flashcard_service.update(flashcard_id, flashcard)
    if updated_flashcard == "flashcard-not-found":
        raise HTTPException(status_code=404, detail="Flashcard not found")
    elif updated_flashcard == "set-not-found":
        raise HTTPException(status_code=404, detail="Set not found")

    return updated_flashcard

@flashcard_router.delete("/{flashcard_id}")
def delete_flashcard(flashcard_id: int, flashcard_service: FlashcardServiceDependency):
    deleted_flashcard = flashcard_service.delete(flashcard_id)
    if not deleted_flashcard:
        raise HTTPException(status_code=404, detail="Flashcard not found")

    return deleted_flashcard