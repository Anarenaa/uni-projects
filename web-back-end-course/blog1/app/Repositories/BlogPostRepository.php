<?php

namespace App\Repositories;

use App\Models\BlogPost as Model;
use Illuminate\Database\Eloquent\Collection;

/**
 * Class BlogСategoryRepository.
 */
class BlogPostRepository extends CoreRepository
{
    protected function getModelClass()
    {
        return Model::class; //абстрагування моделі BlogCategory, для легшого створення іншого репозиторія
    }
 
     /**
     * Отримати список статей
     * 
     * @return \Illuminate\Contracts\Pagination\LengthAwarePaginator
     */
    public function getAllWithPaginate($perPage = 25, $search = null)
    {
        $columns = ['id', 'title', 'slug', 'is_published', 'published_at', 'user_id', 'category_id',];

        $result = $this->startConditions()
                    ->select($columns)
                    ->orderBy('id','DESC')
                    ->with([
                        'category' => function ($query) {
                            $query->select(['id', 'title']);
                        },
                        //'category:id,title',
                        'user:id,name',
                    ]);

        if (!empty($search)) {
            $result->where('title', 'LIKE', "%{$search}%");
        }

        return $result->paginate($perPage);
    }
    /**
     *  Отримати модель для редагування в адмінці
     *  @param int $id
     *  @return Model
     */
    public function getEdit($id)
    {
        return $this->startConditions()->find($id);
    }

    /**
     *  Отримати модель з підвантаженою категорію
     *  @param int $id
     *  @return Model
     */
    public function getWithCategory($id){
        return $this->startConditions()
            ->with('category')
            ->find($id);
    }
}