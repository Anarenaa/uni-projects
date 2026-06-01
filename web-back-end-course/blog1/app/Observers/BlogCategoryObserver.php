<?php

namespace App\Observers;
use Illuminate\Support\Str;
use App\Models\BlogCategory;

class BlogCategoryObserver
{
    /**
     * Обробка перед створенням запису.
     *
     * @param  BlogCategory  $blogCategory
     * 
     */ 
    public function creating(BlogCategory $blogCategory)
    {
        $this->setSlug($blogCategory);
    }
    
    public function updating(BlogCategory $blogCategory)
    {
        $this->setSlug($blogCategory);
    }
    /**
     * якщо псевдонім порожній 
     * то генеруємо псевдонім
     * 
     * @param BlogCategory  $blogCategory
     */
    protected function setSlug(BlogCategory $blogCategory)
    {
        if (empty($blogCategory->slug)) { 
            $blogCategory->slug = Str::slug($blogCategory->title);
        }
    }
}
