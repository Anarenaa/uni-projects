<?php

namespace App\Http\Controllers\Api\Blog\Admin;

use App\Repositories\BlogPostRepository;
use App\Repositories\BlogCategoryRepository;
use App\Http\Requests\BlogPostUpdateRequest;
use App\Models\BlogPost;
use App\Http\Requests\BlogPostCreateRequest;
use App\Jobs\BlogPostAfterCreateJob;
use App\Jobs\BlogPostAfterDeleteJob;
use Illuminate\Http\Request;
use App\Http\Resources\Api\Blog\Admin\PostResource;

class PostController extends BaseController
{
    public function __construct(private BlogPostRepository $blogPostRepository, private BlogCategoryRepository $blogCategoryRepository)
    {
        //parent::__construct();
    }

    public function index(Request $request)
    {
        $perPage = $request->query('per_page', 25);
        $search = $request->query('search');

        $paginator = $this->blogPostRepository->getAllWithPaginate($perPage, $search);

        return PostResource::collection($paginator);
    }
    public function show(string $id)
    {
        return BlogPost::with('category')->findOrFail($id);
        
        // $item = $this->blogPostRepository->getEdit($id);
        // if (empty($item)) {
        //     return ['message' => "Запис id=[{$id}] не знайдено"];
        // }
        //return $item;
    }

    public function store(BlogPostCreateRequest $request)
    {
        $data = $request->input(); //отримаємо масив даних, які надійшли з форми

        $item = (new BlogPost())->create($data); //створюємо об'єкт і додаємо в БД

        if ($item) {
            BlogPostAfterCreateJob::dispatch($item);
            return [
                'success' => true,
                'message' => 'Успішно збережено',
                "item" => $item
            ];
        } else {
            return ['msg' => 'Помилка збереження'];
        }
    }

    public function update(BlogPostUpdateRequest $request, string $id)
    {
          $item = $this->blogPostRepository->getEdit($id);
        if (empty($item)) { //якщо ід не знайдено
            return ['message' => "Запис id=[{$id}] не знайдено"];
        }

        $data = $request->all(); //отримаємо масив даних, які надійшли з форми
        
        $result = $item->update($data); //оновлюємо дані об'єкта і зберігаємо в БД

        if ($result) {
            return [
                'success' => true,
                'message' => 'Успішно збережено',
                "item" => $item
            ];
        } else {
            return ['message' => 'Помилка збереження'];
        }
    }
    public function destroy(string $id){
        $result = BlogPost::destroy($id); //софт деліт, запис лишається

        //$result = BlogPost::find($id)->forceDelete(); //повне видалення з БД

        // Або
        // $item = BlogPost::find($id);
        // if (empty($item)) {
        //     return ['message' => "Запис id=[{$id}] не знайдено"];
        // }
        // $result = $item->delete();

        if ($result) {
            BlogPostAfterDeleteJob::dispatch($id)->delay(20);
            return [
                'success' => true,
                'message' => "Успішне видалення"
            ];
        } else {
            return ['message' => 'Помилка збереження'];
        }
    }
}
