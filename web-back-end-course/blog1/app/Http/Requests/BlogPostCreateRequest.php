<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\Rule;

class BlogPostCreateRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        return [
            'title' => 'required|min:5|max:200|unique:blog_posts',
            'slug' => 'nullable|max:200|unique:blog_posts',
            'content_raw' => 'required|string|min:5|max:10000',
            'category_id' => 'required|integer|exists:blog_categories,id',
        ];
    }
    
    /**
     * Get the error messages for the defined validation rules.
     *
     * @return array
     */
    public function messages()
    {
        return [
            'title.required' => 'Введіть заголовок статті.',
            'title.min' => 'Заголовок статті має містити не менше :min символів.',
            'title.max' => 'Заголовок статті не повинен перевищувати :max символів.',
            'title.unique' => 'Стаття з таким заголовком вже існує.',
            'slug.max' => 'Максимальна довжина slug — :max символів.',
            'slug.unique' => 'Цей slug вже зайнятий іншою статтею.',
            'content_raw.required' => 'Ви забули написати текст статті.',
            'content_raw.string' => 'Текст статті має бути рядком.',
            'content_raw.min' => 'Мінімальна довжина статті — :min символів.',
            'content_raw.max' => 'Текст статті не повинен перевищувати :max символів.',
            'category_id.required' => 'Обов\'язково виберіть категорію для статті.',
            'category_id.integer' => 'Ідентифікатор категорії має бути цілим числом.',
            'category_id.exists' => 'Обрана категорія не існує.',
        ];
    }
}