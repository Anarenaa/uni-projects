from fastapi import APIRouter, HTTPException

from core.collection import CollectionServiceDependency
from schemas.collection import CollectionDTO

collection_router = APIRouter(
    prefix="/collections"
)

@collection_router.get('')
def get_all_collections(collection_service: CollectionServiceDependency):
    return collection_service.get_all()

@collection_router.get('/{collection_id}')
def get_collection_by_id(collection_id: int, collection_service: CollectionServiceDependency):
    existing_collection = collection_service.get_by_id(collection_id)
    if not existing_collection:
        raise HTTPException(status_code=404, detail="Collection not found")

    return existing_collection

@collection_router.post('')
def create_collection(collection: CollectionDTO, collection_service: CollectionServiceDependency):
    return collection_service.create(collection)

@collection_router.put('/{collection_id}')
def update_collection(collection_id: int, collection: CollectionDTO, collection_service: CollectionServiceDependency):
    updated_collection = collection_service.update(collection_id, collection)
    if not updated_collection:
        raise HTTPException(status_code=404, detail="Collection not found")

    return updated_collection

@collection_router.delete('/{collection_id}')
def delete_collection(collection_id: int, collection_service: CollectionServiceDependency):
    deleted_collection = collection_service.delete(collection_id)
    if not deleted_collection:
        raise HTTPException(status_code=404, detail="Collection not found")

    return deleted_collection