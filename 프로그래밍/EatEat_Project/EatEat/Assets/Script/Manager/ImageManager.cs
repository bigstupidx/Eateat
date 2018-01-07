using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct FoodResourceData
{
    //음식의 3가지 상태 이미지(새것 상태, 조금 먹은 상태, 많이 먹은 상태)
    public Sprite[] _arrFoodImage;
    //음식 이름
    public string _foodName;
}

[System.Serializable]
public struct FoodResource
{
    //음식 테마(서양식, 일식, 한식, 중식)
    public EFoodTheme _foodTheme;
    //음식 테마에 대한 음식 List
    public FoodResourceData[] _arrFoodResourceData;
}

public enum EResourceType
{
    eBackground,
    eTouchTable,
    eMainTable,
    eFirstTable,
    eConveyorBelt,
    eConveyorBeltTail
}

public class ImageManager : MonoBehaviour
{
    public static ImageManager  instance = null;
    public FoodResource[]   _arrFoodResource;

    //배열 내용 순서는, EFoodTheme를 따른다.
    //western = 0
    //korean = 1
    //chinese = 2
    //... etc...
    public Sprite[]     _arrBackgroundResource;
    public Sprite[]     _arrTouchTableResource;
    public Sprite[]     _arrMainTableResource;
    public Sprite[]     _arrFirstTableResource;
    public Sprite[]     _arrConveyorBeltResource;
    public Sprite[]     _arrConveyorBeltTailResource;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //현재 Stage의 Theme에 대한 음식을 반환한다.
    public FoodResourceData GetFood()
    {
        EFoodTheme eFoodType = StageSystem.instance.FoodTheme;
        int nLength = _arrFoodResource[(int)eFoodType]._arrFoodResourceData.Length;
        int randIndex = Random.Range(0, nLength);

        return _arrFoodResource[(int)eFoodType]._arrFoodResourceData[randIndex];
    }

    public Sprite GetThemeSprite(EResourceType type)
    {
        EFoodTheme eFoodType = StageSystem.instance.FoodTheme;

        switch (type) {
            case EResourceType.eBackground:
                return _arrBackgroundResource[(int)eFoodType];
            case EResourceType.eTouchTable:
                return _arrTouchTableResource[(int)eFoodType];
            case EResourceType.eMainTable:
                return _arrMainTableResource[(int)eFoodType];
            case EResourceType.eFirstTable:
                return _arrFirstTableResource[(int)eFoodType];
            case EResourceType.eConveyorBelt:
                return _arrConveyorBeltResource[(int)eFoodType];
            case EResourceType.eConveyorBeltTail:
                return _arrConveyorBeltTailResource[(int)eFoodType];
            default:
                return null;
        }
    }
}
