using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FoodResource
{
    public EFoodTheme _foodType;
    public FoodResourceData[] _arrFoodResourceData;
}

[System.Serializable]
public struct FoodResourceData
{
    public Sprite _foodImage;
    public string _foodName;
}

public class FoodResourceManager : MonoBehaviour
{
    public static FoodResourceManager   instance = null;
    public FoodResource[]               _arrFoodResource;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        
    }

    public FoodResourceData GetFood()
    {
        EFoodTheme eFoodType = StageSystem.instance.GetFoodTheme();
        int nLength = _arrFoodResource[(int)eFoodType]._arrFoodResourceData.Length;
        int randIndex = Random.Range(0, nLength);

        return _arrFoodResource[(int)eFoodType]._arrFoodResourceData[randIndex];
    }
}