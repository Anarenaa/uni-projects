from sqlalchemy import Integer, String
from sqlalchemy.orm import mapped_column, Mapped
from models.base import Base

class Manufacturer(Base):
    __tablename__ = 'manufacturers'

    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    name: Mapped[str] = mapped_column(String(255), nullable=False)
    headquarters_location: Mapped[str] = mapped_column(String(255), nullable=True)