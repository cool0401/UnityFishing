<?php

use App\Http\Controllers\DataController;
use App\Http\Controllers\UserController;
use Illuminate\Http\Request;
use Illuminate\Support\Facades\Route;

/*
|--------------------------------------------------------------------------
| API Routes
|--------------------------------------------------------------------------
|
| Here is where you can register API routes for your application. These
| routes are loaded by the RouteServiceProvider within a group which
| is assigned the "api" middleware group. Enjoy building your API!
|
*/

Route::middleware('auth:sanctum')->get('/user', function (Request $request) {
    return $request->user(); // request for users info in the database
});
// Uniy will send the request to the backend database 
Route::post('/signup', [UserController::class, 'signup']); // function sign up from userController
Route::post('/login', [UserController::class, 'login']); // function login from userController
Route::post('/getUserData', [DataController::class, 'getUserData']); // function getUserData from dataController
Route::post('/catch', [DataController::class, 'catch']);  // catch a new fish from dataController
Route::post('/sell', [DataController::class, 'sell']);  // sell fishes from dataController
