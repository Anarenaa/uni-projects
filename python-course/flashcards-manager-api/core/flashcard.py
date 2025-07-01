from fastapi import Depends
from typing import Annotated

from crud.flashcard import FlashcardRepositoryDependency
from crud.set import SetRepositoryDependency
from models import Flashcard
from schemas.flashcard import FlashcardDTO

class FlashcardService:
    def __init__(self, flashcard_repository: FlashcardRepositoryDependency):
        self.flashcard_repository = flashcard_repository

    def get_all(self) -> list[FlashcardDTO]:
        flashcards = self.flashcard_repository.get_all()

        return [FlashcardDTO.model_validate(card) for card in flashcards]

    def get_by_set_id(self, set_id: int) -> list[FlashcardDTO] | None:
        flashcards = self.flashcard_repository.get_by_set_id(set_id)
        if not flashcards:
            return None

        return [FlashcardDTO.model_validate(card) for card in flashcards]

    def get_by_id(self, flashcard_id: int) -> FlashcardDTO | None:
        flashcard = self.flashcard_repository.get_by_id(flashcard_id)
        if not flashcard:
            return None

        return FlashcardDTO.model_validate(flashcard)

    def create(self, flashcard: Flashcard) -> FlashcardDTO | None:
        created_flashcard = self.flashcard_repository.create(flashcard)
        if not created_flashcard:
            return None

        return FlashcardDTO.model_validate(created_flashcard)

    def update(self, flashcard_id: int, flashcard: Flashcard) -> FlashcardDTO | str:
        updated_flashcard = self.flashcard_repository.update(flashcard_id, flashcard)
        if isinstance(updated_flashcard, str):
            return updated_flashcard

        return FlashcardDTO.model_validate(updated_flashcard)

    def delete(self, flashcard_id: int) -> FlashcardDTO | None:
        deleted_flashcard = self.flashcard_repository.delete(flashcard_id)
        if not deleted_flashcard:
            return None

        return FlashcardDTO.model_validate(deleted_flashcard)

    def filter_by_alphabet_in_set(self, set_id: int) -> list[FlashcardDTO]:
        flashcards = (self.flashcard_repository.filter_by_alphabet_in_set(set_id))

        return [FlashcardDTO.model_validate(card) for card in flashcards]

    def filter_from_new_in_set(self, set_id: int) -> list[FlashcardDTO]:
        flashcards = self.flashcard_repository.filter_from_new_in_set(set_id)

        return [FlashcardDTO.model_validate(card) for card in flashcards]

    def search_by_word(self, word: str) -> list[FlashcardDTO]:
        flashcards = self.flashcard_repository.search_by_word(word)

        return [FlashcardDTO.model_validate(card) for card in flashcards]

FlashcardServiceDependency = Annotated[FlashcardService, Depends(FlashcardService)]