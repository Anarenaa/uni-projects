from sqlalchemy.orm import Mapped, mapped_column, relationship
from sqlalchemy import Integer, String, ARRAY
from .base import Base

class Recipe(Base):
    __tablename__ = 'recipes'

    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    title: Mapped[str] = mapped_column(String(255), nullable=False)
    ingredients: Mapped[list["Ingredient"]] = relationship("Ingredient", back_populates="recipe")
    steps: Mapped[list[str]] = mapped_column(ARRAY(String(255)), nullable=False, default=list)