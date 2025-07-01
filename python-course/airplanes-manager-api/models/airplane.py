from sqlalchemy import Integer, String, SmallInteger
from sqlalchemy.orm import mapped_column, Mapped
from models.base import Base

class Airplane(Base):
    __tablename__ = 'airplanes'

    id: Mapped[int] = mapped_column(Integer, primary_key=True)
    model: Mapped[str] = mapped_column(String(255), nullable=False)
    manufacturer: Mapped[str] = mapped_column(String(255), nullable=False)
    year: Mapped[int] = mapped_column(SmallInteger, nullable=True)
    tail_number:  Mapped[str] = mapped_column(String(255), nullable=True)