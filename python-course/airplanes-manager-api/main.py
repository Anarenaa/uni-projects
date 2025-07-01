from fastapi import FastAPI, HTTPException
from fastapi.responses import RedirectResponse
from fastapi import Request
from db import DatabaseNotEnabledError

from controllers.airplane import airplane_router
from controllers.manufacturer import manufacturer_router

app = FastAPI()

@app.exception_handler(DatabaseNotEnabledError)
async def db_not_enabled_exception_handler(request: Request, exc: DatabaseNotEnabledError):
    raise HTTPException(status_code=503, detail=str(exc))

app.include_router(airplane_router)
app.include_router(manufacturer_router)

@app.get("/")
def root():
    return RedirectResponse(url="/docs")