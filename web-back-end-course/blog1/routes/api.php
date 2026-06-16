<?php


use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;
use App\Http\Controllers\Api\Blog\PostController;
use App\Http\Controllers\Api\Blog\Admin\CategoryController;
use App\Http\Controllers\DiggingDeeperController;

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
    Route::get('categories/list-all','CategoryController@listAll');
    Route::apiResource('categories', "CategoryController")
    ->names('blog.admin.categories'); 

    //BlogPost
    Route::apiResource('posts', 'PostController')
    ->names('blog.admin.posts');
 });

Route::get('process-video', 'App\Http\Controllers\DiggingDeeperController@processVideo')
->name('digging_deeper.processVideo');

Route::get('prepare-catalog', 'App\Http\Controllers\DiggingDeeperController@prepareCatalog')
->name('digging_deeper.prepareCatalog'); 