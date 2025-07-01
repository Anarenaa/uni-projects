from typing import Annotated

from fastapi import Depends
from sqlalchemy import create_engine, text
from sqlalchemy.orm import sessionmaker
from sqlalchemy.exc import OperationalError

class DatabaseNotEnabledError(Exception):
    pass

engine = create_engine("postgresql://postgres:postgres@localhost:5432/airplanes-db")

LocalSession = sessionmaker(bind=engine)

def check_database_enabled():
    try:
        with engine.connect() as connection:
            connection.execute(text("SELECT 1"))
    except OperationalError:
        raise DatabaseNotEnabledError("Database is not enabled.")

def get_db() -> LocalSession:
    check_database_enabled()
    db = LocalSession()
    try:
        yield db
    finally:
        db.close()

DatabaseContext = Annotated[LocalSession, Depends(get_db)]