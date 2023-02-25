<?php

namespace App\Http\Controllers;

use App\Models\Fish;
use App\Models\Info;
use Illuminate\Http\Request;

class DataController extends Controller
{
    //
    public function getUserData(Request $request)
    {
        // When the user logs in, a user ID is saved with this ID we can retreve the corresponding user data in the database. 
        $id = $request->id;   // users ID 

        $fish = Fish::where('ID', $id)->get()->first();
        // retrieving data 
        $BassCaught = $fish->BassCaught;
        $MuskieCaught = $fish->MuskieCaught;
        $BlueGillCaught = $fish->BlueGillCaught;
        $BassTotal = $fish->BassTotal;
        $MuskieTotal = $fish->MuskieTotal;
        $BlueGillTotal = $fish->BlueGillTotal;

        $coin = Info::where('ID', $id)->get()->first()->CoinAmt;

        return response()->json([                                // results pulled 
            'bassCaught' => $BassCaught,
            'muskieCaught' => $MuskieCaught,
            'blueGillCaught' => $BlueGillCaught,
            'bassTotal' => $BassTotal,
            'muskieTotal' => $MuskieTotal,
            'blueGillTotal' => $BlueGillTotal,
            'coin' => $coin
        ]);
    }

    // Catch a new fish
    public function catch(Request $request)
    {
        # code...
        $id = $request->id;  // users ID 
        $fish_type = $request->fish_name;   

        $fish = Fish::where('ID', $id)->get()->first();
        
        // 3 types of fish saved when caught adding totals in the inventory  
        switch($fish_type)  
        {
            case 'Bass':
                $caughtcount = $fish->BassCaught;
                $caughtcount++;                                // adds one to caught total

                Fish::where('ID', $id)->update(['BassCaught' => $caughtcount]);

                $totalcount = $fish->BassTotal;
                $totalcount++;                               // adds one to total count
                Fish::where('ID', $id)->update(['BassTotal' => $totalcount]);

                break;

            case 'Muskie':
                $caughtcount = $fish->MuskieCaught;
                $caughtcount++;                                 // adds one to caught total
                Fish::where('ID', $id)->update(['MuskieCaught' => $caughtcount]);

                $totalcount = $fish->MuskieTotal;
                $totalcount++;                                 // adds one to total count
                Fish::where('ID', $id)->update(['MuskieTotal' => $totalcount]);

                break;
            
            case 'BlueGill':
                $caughtcount = $fish->BlueGillCaught;
                $caughtcount++;                                  // adds one to caught total
                Fish::where('ID', $id)->update(['BlueGillCaught' => $caughtcount]);

                $totalcount = $fish->BlueGillTotal;
                $totalcount++;                                  // adds one to total count
                Fish::where('ID', $id)->update(['BlueGillTotal' => $totalcount]);

                break;
        }

        $fish = Fish::where('ID', $id)->get()->first();
        $BassCaught = $fish->BassCaught;
        $MuskieCaught = $fish->MuskieCaught;
        $BlueGillCaught = $fish->BlueGillCaught;
        $BassTotal = $fish->BassTotal;
        $MuskieTotal = $fish->MuskieTotal;
        $BlueGillTotal = $fish->BlueGillTotal;

        return response()->json([                       // results changed 
            'bassCaught' => $BassCaught,
            'muskieCaught' => $MuskieCaught,
            'blueGillCaught' => $BlueGillCaught,
            'bassTotal' => $BassTotal,
            'muskieTotal' => $MuskieTotal,
            'blueGillTotal' => $BlueGillTotal
        ]);     
    }

    // Sell fishes
    public function sell(Request $request)
    {
        # code...
        $id = $request->id;
        $bass = $request->bass;
        $muskie = $request->muskie;
        $bluegill = $request->blueGill;
        $coin = $request->coin;

        $fish = Fish::where('ID', $id)->get()->first();

        $BassCaught = $fish->BassCaught - $bass;
        $MuskieCaught = $fish->MuskieCaught - $muskie;
        $BlueGillCaught = $fish->BlueGillCaught - $bluegill;

        Fish::where('ID', $id)->update([
            'BassCaught' => $BassCaught,
            'MuskieCaught' => $MuskieCaught,
            'BlueGillCaught' => $BlueGillCaught
        ]); 

        $info = Info::where('ID', $id)->get()->first();
        $CoinAmt = $info->CoinAmt + $coin;                                 // coin is increase according to its set Coin amount for the fish
        
        Info::where('ID', $id)->update(['CoinAmt' => $CoinAmt]);

        return response()->json([
            'bassCaught' => $BassCaught,
            'muskieCaught' => $MuskieCaught,
            'blueGillCaught' => $BlueGillCaught,
            'coin' => $CoinAmt
        ]);

    }
}
