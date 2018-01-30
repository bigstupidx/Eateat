using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public FoodStatus       _status = null;
    public SpriteRenderer   _foodSpriteRenderer = null;
    private Sprite[]        _arrFoodImage;

    private Food _food;

// Use this for initialization
    public void CreateMainFood()
    {
        //음식 이름 Set
        FoodResourceData foodData = ImageManager.instance.GetFood();
        _status.InitFoodStatus();
        _status.SetName(foodData._foodName);

        _food = GetComponentInChildren<Food>();
        if (_food)
        {
            _food.Initialize();
            transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        }

        if (true == StageSystem.instance.IsBossStage())
        {
            transform.localScale = new Vector3(1.2f, 1.1f, 1.1f);
        }
    }

    public double GetCalorie(ECalorieType eCalorieType)
    {
        return _status.GetCalorie(eCalorieType);
    }

    public bool IsDead()
    {
        return _status.IsDead();
    }

    public void AddHp(double damage)
    {
        _status.AddHp(damage);
        _food.FoodDamaged((float)_status.HpPercentage);
    }

    public void HpRecovery()
    {
        _status.HpRecovery();
        _food.FoodDamaged((float)_status.HpPercentage);
    }

    public double MaxHp
    {
        get
        {
            return _status.MaxHp;
        }
    }
}