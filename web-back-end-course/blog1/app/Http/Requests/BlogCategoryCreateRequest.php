<?php

namespace App\Http\Requests;

use Illuminate\Contracts\Validation\ValidationRule;
use Illuminate\Foundation\Http\FormRequest;

class BlogCategoryCreateRequest extends FormRequest
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
        return [
            'title'       => 'required|min:5|max:200|unique:blog_categories,title',
            'slug'        => 'nullable|max:200|unique:blog_categories,slug',
            'description' => 'nullable|string|max:500|min:3',
            'parent_id'   => 'required|integer|exists:blog_categories,id',
        ];
    }

    public function messages()
    {
        return [
            'title.required'       => 'Введіть назву категорії.',
            'title.min'            => 'Назва категорії має містити не менше :min символів.',
            'title.max'            => 'Назва категорії не повинна перевищувати :max символів.',
            'title.unique'         => 'Категорія з такою назвою вже існує.',
            'slug.max'             => 'Максимальна довжина slug — :max символів.',
            'slug.unique'          => 'Цей slug вже зайнятий іншою категорією.',
            'description.string'   => 'Опис має бути текстовим рядком.',
            'description.min'      => 'Опис має містити не менше :min символів.',
            'description.max'      => 'Опис не повинен перевищувати :max символів.',
            'parent_id.required'   => 'Батьківська категорія обов\'язкова для вказання.',
            'parent_id.integer'    => 'Ідентифікатор батьківської категорії має бути числом.',
            'parent_id.exists'     => 'Обрана батьківська категорія не існує в системі.',
        ];
    }
}
