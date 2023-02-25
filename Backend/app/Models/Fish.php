<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Model;

class Fish extends Model
{
    use HasFactory;

    public $timestamps = false;

    protected $fillable = [
        'ID',
        'BassCaught',
        'MuskieCaught',
        'BlueGillCaught',
        'BassTotal',
        'MuskieTotal',
        'BlueGillCaught'
    ];
}
