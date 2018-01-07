using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Define the Color of small Dishes
public enum EDishColor
{
    red = 0,
    blue,
    yellow,
    green,
    white,
    black,
    none,
}

public enum EDishSpeed
{
    min,
    max
}

public enum EPlayerState
{
    normal = 0,
    fever,
    super_fever,
}

//순서 매우 중요함
public enum EFoodTheme
{
    western,
    korean,
    //chinese,
    //japanese,
}

public enum ESkill
{
    Fever = 0,
    Triple,
    Critical,
    Calorie,
    Magical,
    Damage
}