from fastapi import FastAPI, HTTPException
from fastapi.requests import Request

from app.controllers.recipe import recipe_router
from app.controllers.ingredient import ingredient_router
from app.db import DatabaseNotEnabledError

app = FastAPI()

@app.exception_handler(DatabaseNotEnabledError)
async def db_not_enabled_exception_handler(request: Request, exc: DatabaseNotEnabledError):
    raise HTTPException(status_code=503, detail=str(exc))
app.include_router(recipe_router)
app.include_router(ingredient_router)