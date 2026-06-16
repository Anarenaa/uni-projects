<?php

namespace App\Http\Controllers\Api\Blog\Admin;

use App\Http\Requests\BlogCategoryCreateRequest;
use App\Models\BlogCategory;
use App\Repositories\BlogCategoryRepository;
use Illuminate\Support\Str;
use Illuminate\Http\Request;
use Illuminate\Http\Response;
use App\Http\Requests\BlogCategoryUpdateRequest;
use App\Http\Resources\Api\Blog\Admin\CategoryResource;

class CategoryController extends BaseController
{
    public function __construct(private BlogCategoryRepository $blogCategoryRepository)
    {
        //parent::__construct();
     
    }
    /**
     * Display a listing of the resource.
     */
    public function index(Request $request)
    {
        //dd(__METHOD__);

        $perPage = $request->query('per_page', 5);
        $search = $request->query('search');
        $paginator = $this->blogCategoryRepository->getAllWithPaginate($perPage, $search);
        
        return CategoryResource::collection($paginator);
    }
    public function listAll() {
        $categories = $this->blogCategoryRepository->getForComboBox();
        return $categories;
    }

    /**
     * Display the specified resource.
     */
    public function show(string $id)
    {
        $category = $this->blogCategoryRepository->getWithParent($id);
    
        return new CategoryResource($category);
    }

    /**
     * Store a newly created resource in storage.
     */
    public function store(BlogCategoryCreateRequest $request)
    {
        //dd(__METHOD__);

        $data = $request->input();;

        // Створюємо об'єкт у базі даних
        $item = BlogCategory::create($data);

        if ($item) {
            return [
                'success' => true,
                'message' => 'Успішно збережено',
                'item' => $item
            ];
        } else {
            return [
                'success' => false,
                'message' => 'Помилка збереження',
            ];
        }
    }

    /**
     * Update the specified resource in storage.
     */
    public function update(BlogCategoryUpdateRequest $request, string $id)
    {
        //dd(__METHOD__);

        $item = $this->blogCategoryRepository->getEdit($id);
        
        $data = $request->all(); //отримаємо масив даних, які надійшли з форми

        if (empty($data['slug'])) { //якщо псевдонім порожній
            $data['slug'] = Str::slug($data['title']); //генеруємо псевдонім
        }
        
        $result = $item->update($data);  //оновлюємо дані об'єкта і зберігаємо в БД

        if ($result) {
            return [
            'success' => true,
            'message' => 'Успішно збережено',
            'item' => $item
            ];
        } else {
            return ['message' => 'Помилка збереження'];
        }
    }

    /**
     * Remove the specified resource from storage.
     */
    public function destroy(string $id)
    {
        if ((int)$id === 1) {
            return response()->json([
                'message' => 'Категорію "Без категорії" видалити неможливо, вона є системною.'
            ], Response::HTTP_FORBIDDEN); // 403
        }

        $category = $this->blogCategoryRepository->getEdit($id);
        if(empty($category)){
            return response()->json(['message' => "Запис id=[{$id}] не знайдено"], 404);
        }

        $category->posts()->update(['category_id' => 1]);
        $category->children()->update(['parent_id' => 1]);

        $isDeleted = $category->delete(); //софт деліт, запис лишається
        
        if ($isDeleted) {
            return [
                'success' => true,
                'message' => "Успішне видалення"
            ];
        } else {
            return ['message' => 'Помилка збереження'];
        }
    }
}
