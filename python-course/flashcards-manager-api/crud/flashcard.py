from typing import Annotated

from sqlalchemy import select
from fastapi import Depends

from db import DatabaseContext
from models import Flashcard, Set
from schemas.flashcard import FlashcardDTO

from .set import SetRepositoryDependency


class FlashcardRepository:
    def __init__(self, db: DatabaseContext, set_repository: SetRepositoryDependency):
        self.db = db
        self.set_repository = set_repository

    def get_all(self) -> list[Flashcard]:
        query = select(Flashcard)

        return self.db.execute(query).scalars().all()

    def get_by_set_id(self, set_id: int) -> list[Flashcard] | None:
        existing_set = self.set_repository.get_by_id(set_id)
        if not existing_set:
            return None
        query = select(Flashcard).where(Flashcard.set_id == set_id)

        return self.db.execute(query).scalars().all()

    def get_by_id(self, flashcard_id: int) -> Flashcard | None:
        query = select(Flashcard).where(Flashcard.id == flashcard_id)
        existing_flashcard = self.db.execute(query).scalar_one_or_none()
        if not existing_flashcard:
            return None

        return existing_flashcard

    def create(self, flashcard: FlashcardDTO) -> Flashcard | None:
        existing_set = self.set_repository.get_by_id(flashcard.set_id)
        if not existing_set:
            return None

        flashcard = Flashcard(
            word=flashcard.word,
            translation=flashcard.translation,
            created_at=flashcard.created_at,
            set_id=existing_set.id
        )
        self.db.add(flashcard)
        self.db.commit()

        return flashcard

    def update(self, flashcard_id: int, flashcard: FlashcardDTO) -> Flashcard | str:
        existing_flashcard = self.get_by_id(flashcard_id)
        if not existing_flashcard:
            return "flashcard-not-found"

        existing_set = self.set_repository.get_by_id(flashcard.set_id)
        if not existing_set:
            return "set-not-found"

        existing_flashcard.word = flashcard.word
        existing_flashcard.translation = flashcard.translation
        existing_flashcard.created_at = flashcard.created_at
        existing_flashcard.set_id = existing_set.id

        self.db.commit()

        return existing_flashcard

    def delete(self, flashcard_id: int) -> Flashcard | None:
        existing_flashcard = self.get_by_id(flashcard_id)
        if not existing_flashcard:
            return None

        self.db.delete(existing_flashcard)
        self.db.commit()

        return existing_flashcard

    def filter_by_alphabet_in_set(self, set_id: int) -> list[Flashcard]:
        query = (
            select(Flashcard)
            .where(Flashcard.set_id == set_id)
            .order_by(Flashcard.word)
        )
        return self.db.execute(query).scalars().all()

    def filter_from_new_in_set(self, set_id: int) -> list[Flashcard]:
        query = (
            select(Flashcard)
            .where(Flashcard.set_id == set_id)
            .order_by(Flashcard.created_at.desc())
        )
        return self.db.execute(query).scalars().all()

    def search_by_word(self, word) -> list[Flashcard]:
        query = select(Flashcard).where(Flashcard.word.contains(word))
        return self.db.execute(query).scalars().all()

FlashcardRepositoryDependency = Annotated[FlashcardRepository, Depends(FlashcardRepository)]