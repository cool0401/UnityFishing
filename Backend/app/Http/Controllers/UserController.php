<?php

namespace App\Http\Controllers;

use App\Models\Fish;
use App\Models\Info;
use Illuminate\Http\Request;
use App\Models\User;
use GrahamCampbell\ResultType\Success;
use Illuminate\Support\Facades\Auth;
use Illuminate\Support\Facades\Hash;
use Illuminate\Support\Facades\DB;


class UserController extends Controller
{
    //
    public function signup(Request $request)
    { 
    // if account exist
        $name = $request->name;  // initilize name

        $name_count = User::where('name', $name)->get()->count();   // search database if same user exist

        if($name_count != 0)
        {
            return response([
                'result' => '1' // already exists
            ]);
        }
    // entering new user's username and password created 
        $user = new User();  // if the current user is new, create a new user
        $user->name = $request->name; // inserts name
        $user->password = Hash::make($request->password);  // hash password created from inserted user text
        $user->save();                                           // if the pasword is not hashed others can see it in the database 

    // Create new info
        $id = User::all()->last()->id;   // sets new ID for new user 

        $info = new Info();  
        $info->ID = $id;           // sets ID
        $info->CoinAmt = 0;        // sets coin amount to zero 
        $info->position = DB::raw("(GeomFromText('POINT(0 0)'))");   // sets position
        $info->save();

        //Create fish lists for the new user
        $fish = new Fish();
        $fish->ID = $id;
        $fish->BassCaught = 0;
        $fish->MuskieCaught = 0;
        $fish->BlueGillCaught = 0;
        $fish->BassTotal = 0;
        $fish->MuskieTotal = 0;
        $fish->BlueGillTotal = 0;
        $fish->save();               // initulize all fish to 0 and save in database

        return response([
            'result' => '2' // success
        ]);
    }

    public function login(Request $request)
    {
        $name = $request->name;
        $password = $request->password;

        if(Auth::attempt(['name' => $name, 'password' => $password]))  // checks for a match 
        {
            $id = User::where('name', $name)->get()->first()->id;
            return response([
                'result' => '1', //success at log in
                'id' => $id
            ]);
        }
        else{
            $count = User::where('name', $name)->get()->count();  // checks database

            if($count == 0)
            {
                return response([
                    'result' => '2' // not registered because name doesn't exist
                ]);
            }
            else{
                return response([
                    'result' => '3' // wrong pwd- name exist but wrong password
                ]);
            }
        }
    }
}
