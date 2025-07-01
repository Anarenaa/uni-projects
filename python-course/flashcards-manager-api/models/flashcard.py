from sqlalchemy import Integer, String, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column, relationship
from models.base import Base
from datetime import datetime
from sqlalchemy import DateTime

class Flashcard(Base):
    __tablename__ = 'flashcards'

    id: Mapped[int] = mapped_column(Integer, primary_key=True, autoincrement=True)
    word: Mapped[str] = mapped_column(String(255), nullable=False)
    translation: Mapped[str] = mapped_column(String(255), nullable=False)
    created_at: Mapped[datetime] = mapped_column(DateTime, nullable=False, default=datetime.now())
    set_id: Mapped[int] = mapped_column(Integer, ForeignKey("sets.id"), nullable=False)
    set: Mapped["Set"] = relationship("Set", back_populates="flashcards")
