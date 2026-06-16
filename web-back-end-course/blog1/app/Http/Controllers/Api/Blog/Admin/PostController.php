<?php

namespace App\Http\Controllers\Api\Blog\Admin;

use App\Models\BlogPost;
use App\Repositories\BlogPostRepository;
use App\Http\Requests\BlogPostUpdateRequest;
use App\Http\Requests\BlogPostCreateRequest;
use App\Jobs\BlogPostAfterCreateJob;
use App\Jobs\BlogPostAfterDeleteJob;
use Illuminate\Http\Request;
use App\Http\Resources\Api\Blog\Admin\PostResource;

class PostController extends BaseController
{
    public function __construct(private BlogPostRepository $blogPostRepository)
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
        $item = $this->blogPostRepository->getWithCategory($id);
        if (empty($item)) {
            return response()->json(['message' => "Запис id=[{$id}] не знайдено"], 404);
        }
        return $item;
    }

    public function store(BlogPostCreateRequest $request)
    {
        $data = $request->input(); //отримаємо масив даних, які надійшли з форми

        $item = BlogPost::create($data); //створюємо об'єкт і додаємо в БД

        if ($item) {
            BlogPostAfterCreateJob::dispatch($item);
            return [
                'success' => true,
                'message' => 'Успішно збережено',
                "item" => $item
            ];
        } else {
            return ['message' => 'Помилка збереження'];
        }
    }

    public function update(BlogPostUpdateRequest $request, string $id)
    {
        $item = $this->blogPostRepository->getEdit($id);
        if (empty($item)) { //якщо ід не знайдено
            return response()->json(['message' => "Запис id=[{$id}] не знайдено"], 404);
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
        //$result = BlogPost::find($id)->forceDelete(); //повне видалення з БД

        $item = $this->blogPostRepository->getEdit($id);
        if (empty($item)) {
            return response()->json(['message' => "Запис id=[{$id}] не знайдено"], 404);
        }
        $result = $item->delete();

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
