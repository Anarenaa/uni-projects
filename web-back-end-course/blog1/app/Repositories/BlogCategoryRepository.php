<?php

namespace App\Repositories;

use App\Models\BlogCategory as Model;
use Illuminate\Database\Eloquent\Collection;

/**
 * Class BlogСategoryRepository.
 */
class BlogCategoryRepository extends CoreRepository
{
    protected function getModelClass()
    {
        return Model::class; //абстрагування моделі BlogCategory, для легшого створення іншого репозиторія
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
     *  Отримати список категорій для виводу в випадаючий список
     *  @return Collection
     */
    public function getForComboBox()
    {
        //return $this->startConditions()->all();
        $columns = implode(', ', [
            'id',
            'CONCAT (id, ". ", title) AS id_title',  //додаємо поле id_title 
        ]);

        $result = $this              
            ->startConditions()
            ->selectRaw($columns)
            ->toBase()
            ->get();

        //dd($result);

        return $result;
    }

    /**
     * Отримати категорію для виводу пагінатором
     * 
     * @param int|null $perPage
     * 
     * @return \Illuminate\Contracts\Pagination\LengthAwarePaginator
     */
    public function getAllWithPaginate($perPage = 5, $search = null)
    {
        $columns = ['id', 'title', 'slug', 'parent_id', 'description'];

        $result = $this
            ->startConditions()
            ->select($columns)
            ->where('id', '<>', 1)
            ->with(['parentCategory:id,title',]);

        if (!empty($search)) {
            $result->where('title', 'LIKE', "%{$search}%");
        }

        return $result->paginate($perPage);
    }
    /**
     *  Отримати модель з батьківською категорію
     *  @param int $id
     *  @return Model
     */
    public function getWithParent($id)
    {
        return $this->startConditions()
            ->where('id', $id)
            ->with(['parentCategory:id,title'])
            ->first();
    }
}
