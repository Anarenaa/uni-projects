<?php

use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\Blog\PostController;
use App\Http\Controllers\Api\Blog\Admin\CategoryController;

Route::get('/user', function (Request $request) {
    return $request->user();
})->middleware('auth:sanctum');

Route::group([ 'namespace' => 'App\Http\Controllers\Api\Blog', 'prefix' => 'blog'], function () {
    Route::apiResource('posts', PostController::class)->names('blog.posts');
});

//Адмінка
$groupData = [
    'namespace' => 'App\Http\Controllers\Api\Blog\Admin',
    'prefix' => 'blog/admin',
];
Route::group($groupData, function () {
    //BlogCategory
    $methods = ['index','store','update',];
    Route::apiResource('categories', "CategoryController")
    ->only($methods)
    ->names('blog.admin.categories'); 

    //BlogPost
    Route::apiResource('posts', 'PostController')
    ->except(['show'])                               //не робити маршрут для метода show
    ->names('blog.admin.posts');
 });