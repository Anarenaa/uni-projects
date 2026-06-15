<?php

namespace App\Http\Controllers\Api\Blog\Admin;

//use App\Http\Controllers\Controller;

use App\Http\Requests\BlogCategoryCreateRequest;
use App\Models\BlogCategory;
use App\Repositories\BlogCategoryRepository;
use Illuminate\Support\Str;
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
    public function index()
    {
        //dd(__METHOD__);

        //$paginator = BlogCategory::paginate(5);
        $paginator = $this->blogCategoryRepository->getAllWithPaginate(5);
        
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
        //dd(__METHOD__);
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
    }
}
