from fastapi import FastAPI, HTTPException
from fastapi.requests import Request

from controllers.collection import collection_router
from controllers.set import set_router
from controllers.flashcard import flashcard_router
from controllers.category import category_router
from db import DatabaseNotEnabledError

app = FastAPI()

@app.exception_handler(DatabaseNotEnabledError)
async def db_not_enabled_exception_handler(request: Request, exc: DatabaseNotEnabledError):
    raise HTTPException(status_code=503, detail=str(exc))

app.include_router(collection_router)
app.include_router(set_router)
app.include_router(flashcard_router)
app.include_router(category_router)

if __name__ == "__main__":
    import uvicorn

    uvicorn.run(app, host="localhost", port=8000)