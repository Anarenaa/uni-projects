from typing import Annotated

from fastapi import Depends
from sqlalchemy.orm import sessionmaker
from sqlalchemy import create_engine, text
from sqlalchemy.exc import OperationalError

class DatabaseNotEnabledError(Exception):
    pass

engine = create_engine("postgresql://postgres:postgres@localhost:5432/flashcards-db")
SessionLocal = sessionmaker(bind=engine)

def __check_database_enabled():
    try:
        with engine.connect() as connection:
            connection.execute(text("SELECT 1"))
    except OperationalError:
        raise DatabaseNotEnabledError("Database is not enabled.")

def get_db() -> SessionLocal:
    __check_database_enabled()

    db = SessionLocal()
    try:
        yield db
    finally:
        db.close()

DatabaseContext = Annotated[SessionLocal, Depends(get_db)]
