<?php

namespace App\Http\Controllers\Api\Blog\Admin;

//use App\Http\Controllers\Controller;

use App\Http\Requests\BlogCategoryCreateRequest;
use App\Models\BlogCategory;
use App\Repositories\BlogCategoryRepository;
use Illuminate\Support\Str;
use Illuminate\Http\Request;
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
     * Display the specified resource.
     */
    public function show(string $id)
    {
        $category = BlogCategory::with('parentCategory')->findOrFail($id);
    
        return new CategoryResource($category);
    }

    /**
     * Update the specified resource in storage.
     */
    public function update(BlogCategoryUpdateRequest $request, string $id)
    {
        //dd(__METHOD__);

        $item = BlogCategory::find($id);
        
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
        //dd(__METHOD__);
        $result = BlogCategory::destroy($id); //софт деліт, запис лишається
        if ($result) {
            return [
                'success' => true,
                'message' => "Успішне видалення"
            ];
        } else {
            return ['message' => 'Помилка збереження'];
        }
    }
}
