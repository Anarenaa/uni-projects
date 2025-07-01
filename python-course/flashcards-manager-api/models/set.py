from sqlalchemy import Integer, String, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from models.base import Base

class Set(Base):
    __tablename__ = 'sets'

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    name: Mapped[str] = mapped_column(String(255), nullable=False)
    description: Mapped[str] = mapped_column(String(255), nullable=True)
    flashcards: Mapped[list["Flashcard"]] = relationship("Flashcard", back_populates="set", cascade="all, delete-orphan")
    categories: Mapped[list["Category"]] = relationship("Category", secondary="sets_categories_association", back_populates="sets")
    collection_id: Mapped[int] = mapped_column(Integer, ForeignKey("collections.id"), nullable=True)
    collection: Mapped["Collection"] = relationship("Collection", back_populates="sets")
