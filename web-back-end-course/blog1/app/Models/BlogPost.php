<?php

namespace App\Models;

use App\Observers\BlogPostObserver;
use Illuminate\Database\Eloquent\Attributes\ObservedBy;
use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;
use Illuminate\Database\Eloquent\SoftDeletes;

#[ObservedBy([BlogPostObserver::class])]
class BlogPost extends Model
{
    use HasFactory;
    use SoftDeletes;

    const UNKNOWN_USER = 1;
    
    protected $fillable
        = [
            'title',
            'slug',
            'category_id',
            'excerpt',
            'content_raw',
            'is_published',
            'published_at'
        ];
    
    // Додаємо кастинг типів для правильної роботи API ресурсу
    protected $casts = [
        'is_published' => 'boolean',
        'published_at' => 'datetime',
    ];

    /**
     * Категорія статті
     * 
     * @return \Illuminate\Database\Eloquent\Relations\BelongsTo
     */
    public function category()
    {
        //стаття належить категорії
        return $this->belongsTo(BlogCategory::class);
    }

    /**
     * Автор статті
     * 
     * @return \Illuminate\Database\Eloquent\Relations\BelongsTo
     */
    public function user()
    {
        //стаття належить користувачу
        return $this->belongsTo(User::class);
    }
}
