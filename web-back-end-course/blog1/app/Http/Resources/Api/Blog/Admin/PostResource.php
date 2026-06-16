<?php

namespace App\Http\Resources\Api\Blog\Admin;

use Illuminate\Http\Request;
use Illuminate\Http\Resources\Json\JsonResource;

class PostResource extends JsonResource
{
    /**
     * Трансформація ресурсу в масив.
     *
     * @return array<string, mixed>
     */
    public function toArray(Request $request): array
    {
        // $this вказує на поточний об'єкт моделі BlogPost
        return [
            'id'             => $this->id,
            'title'          => $this->title,
            'slug'           => $this->slug,
            'is_published'   => (bool) $this->is_published,
            
            'date_published' => $this->published_at ? $this->published_at->format('Y-m-d H:i:s') : "Чернетка",
            
            'user_id'        => $this->user_id,
            'category_id'    => $this->category_id,
            
            'category_title' => $this->category?->title, 
            'author_name'    => $this->user?->name,
        ];
    }
}