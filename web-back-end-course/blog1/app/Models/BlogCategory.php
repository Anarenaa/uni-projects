<?php

namespace App\Models;

use App\Observers\BlogCategoryObserver;
use Illuminate\Database\Eloquent\Attributes\ObservedBy;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\SoftDeletes;
use Illuminate\Database\Eloquent\Relations\HasMany;

#[ObservedBy([BlogCategoryObserver::class])]
class BlogCategory extends Model
{
    use SoftDeletes;
    use HasFactory;

    const ROOT = 1;

    protected $fillable
        = [
            'title',
            'slug',
            'parent_id',
            'description',
        ];
    
    /**
     * Батьківська категорія
     * 
     * @return BlogCategory
     */
    public function parentCategory()
    {
        //належить категорії
        return $this->belongsTo(BlogCategory::class, 'parent_id', 'id');
    }

     /**
     * Приклад аксесуара (Accessor)
     * 
     * @url https://laravel.com/docs/13.x/eloquent-mutator
     * 
     * @return string
     */
    public function getParentTitleAttribute()
    {
        $title = $this->parentCategory->title
            ?? ($this->isRoot()
                ? 'Корінь'
                : '???');
        
        return $title;
    }

    /**
     * Перевірка чи об'єкт є кореневим
     * 
     * @return bool
     */
    public function isRoot()
    {
        return $this->id === BlogCategory::ROOT;
    }

    public function posts(): HasMany
    {
        return $this->hasMany(BlogPost::class, 'category_id', 'id');
    }
    public function children(): HasMany
    {
        return $this->hasMany(BlogCategory::class, 'parent_id', 'id');
    }
}