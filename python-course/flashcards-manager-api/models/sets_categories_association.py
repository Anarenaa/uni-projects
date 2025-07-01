from sqlalchemy import Integer, ForeignKey
from sqlalchemy.orm import Mapped, mapped_column
from models.base import Base

class SetsCategories(Base):
    __tablename__ = "sets_categories_association"

    set_id: Mapped[int] = mapped_column(
        Integer,
        ForeignKey("sets.id", ondelete="CASCADE"),
        primary_key=True
    )
    category_id: Mapped[int] = mapped_column(
        Integer,
        ForeignKey("categories.id", ondelete="CASCADE"),
        primary_key=True
    )