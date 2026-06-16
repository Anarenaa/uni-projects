<?php

namespace App\Http\Requests;

use Illuminate\Contracts\Validation\ValidationRule;
use Illuminate\Foundation\Http\FormRequest;
use Illuminate\Validation\Rule;

class BlogCategoryUpdateRequest extends FormRequest
{
    /**
     * Determine if the user is authorized to make this request.
     */
    public function authorize(): bool
    {
        return true;
    }

    /**
     * Get the validation rules that apply to the request.
     *
     * @return array<string, ValidationRule|array<mixed>|string>
     */
    public function rules(): array
    {
        $categoryId = $this->route('category'); 

        return [
            'title' => [
                'required',
                'min:5',
                'max:200',
                Rule::unique('blog_categories', 'title')
                    ->ignore($categoryId) // Ігноруємо поточну категорію
            ],
            'slug' => [
                'nullable',
                'max:200',
                Rule::unique('blog_categories', 'slug')
                    ->ignore($categoryId)
            ],
            'description' => 'nullable|string|max:500|min:3',
            'parent_id' => [
                'required',
                'integer',
                'exists:blog_categories,id',
                Rule::notIn($categoryId)
            ]
        ];
    }

    public function messages()
    {
        return [
            'title.required' => 'Введіть назву категорії для оновлення.',
            'title.min' => 'Назва категорії має містити не менше :min символів.',
            'title.max' => 'Назва категорії не повинна перевищувати :max символів.',
            'title.unique' => 'Категорія з такою назвою вже існує в базі даних.',
            'slug.max' => 'Максимальна довжина slug — :max символів.',
            'slug.unique' => 'Цей slug вже використовується іншою категорією.',
            'description.string' => 'Опис має бути текстовим рядком.',
            'description.min' => 'Опис має містити не менше :min символів.',
            'description.max' => 'Опис не повинен перевищувати :max символів.',
            'parent_id.required' => 'Батьківська категорія обов\'язкова для вказання',
            'parent_id.integer' => 'Ідентифікатор батьківської категорії має бути числом.',
            'parent_id.exists' => 'Обрана батьківська категорія не існує в базі.',
            'parent_id.not_in' => 'Категорія не може бути батьківською сама для себе.'
        ];
    }
}
