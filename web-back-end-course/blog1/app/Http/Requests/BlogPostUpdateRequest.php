<?php

namespace App\Http\Requests;

use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\Rule;

class BlogPostUpdateRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     *
     * @return bool
     */
    public function authorize()
    {
        //return->auth()->check();
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array
     */
    public function rules()
    {
        $postId = $this->route('post'); 

        return [
            'title' => [
                'required',
                'min:5',
                'max:200',
                Rule::unique('blog_posts', 'title')
                    ->ignore($postId) // Ігноруємо поточну статтю
            ],
            'slug' => [
                'nullable',
                'max:200',
                Rule::unique('blog_posts', 'slug')
                    ->ignore($postId)
            ],
            'excerpt' => 'nullable|string|max:500',
            'content_raw' => 'required|string|min:5|max:10000',
            'category_id' => [
                'required',
                'integer',
                Rule::exists('blog_categories', 'id')
            ],
        ];
    }

    public function messages()
    {
        return [
            'title.required' => 'Заголовок статті не може бути порожнім.',
            'title.min' => 'Заголовок статті має містити не менше :min символів.',
            'title.max' => 'Заголовок статті не повинен перевищувати :max символів.',
            'title.unique' => 'Інша стаття вже має такий заголовок.',
            'slug.max' => 'Максимальна довжина slug — :max символів.',
            'slug.unique' => 'Цей slug вже зайнятий іншою статтею.',
            'excerpt.max' => 'Уривок статті (excerpt) не повинен перевищувати :max символів.',
            'content_raw.required' => 'Поле з текстом статті є обов\'язковим.',
            'content_raw.string' => 'Текст статті має бути рядком.',
            'content_raw.min' => 'Мінімальна довжина тексту статті — :min символів.',
            'content_raw.max' => 'Текст статті не повинен перевищувати :max символів.',
            'category_id.required' => 'Стаття повинна бути прив\'язана до категорії.',
            'category_id.integer' => 'Ідентифікатор категорії має бути цілим числом.',
            'category_id.exists' => 'Категорії не існує.',
        ];
    }
}