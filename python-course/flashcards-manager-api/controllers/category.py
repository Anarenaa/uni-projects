from fastapi import APIRouter, HTTPException, Depends
from core.category import CategoryServiceDependency
from schemas.category import CategoryDTO

category_router = APIRouter(prefix="/categories")

@category_router.get("")
def get_all(service: CategoryServiceDependency):
    return service.get_all()
@category_router.get("/{category_id}")
def get_by_id(category_id: int, service: CategoryServiceDependency):
    result = service.get_by_id(category_id)
    if not result:
        raise HTTPException(status_code=404, detail="Category not found")
    return result
@category_router.get("/{category_id}/sets")
def get_category_sets(category_id: int, service: CategoryServiceDependency):
    result = service.get_category_sets(category_id)
    if not result:
        raise HTTPException(status_code=404, detail="Category not found")
    return result
@category_router.post("")
def create_category_with_sets(category: CategoryDTO, set_ids: list[int], service: CategoryServiceDependency):
    result = service.create(category, set_ids)
    if result == "set-not-found":
        raise HTTPException(status_code=404, detail="One or more sets not found")
    return result

@category_router.put("/{category_id}")
def update_category(category_id: int, category: CategoryDTO, set_ids: list[int], service: CategoryServiceDependency):
    result = service.update(category_id, category, set_ids)
    if not result:
        raise HTTPException(status_code=404, detail="Category not found")
    return result

@category_router.delete("/{category_id}")
def delete_category(category_id: int, service: CategoryServiceDependency):
    result = service.delete(category_id)
    if not result:
        raise HTTPException(status_code=404, detail="Category not found")
    return result
