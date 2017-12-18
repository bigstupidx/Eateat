using UnityEngine;
using System.Collections;

public class MainFood : MonoBehaviour
{
    public FoodStatus       _status = null;
    public Transform        _renderObject = null;
    public SpriteRenderer   _foodSpriteRenderer = null;

    // Use this for initialization
    void Start()
    {

    }

    public void CreateMainFood()
    {
        //음식 이름 Set
        FoodResourceData foodData = FoodResourceManager.instance.GetFood();
        _status.InitFoodStatus();
        _status.SetName(foodData._foodName);

        //음식 Image Set
        _foodSpriteRenderer.sprite = foodData._foodImage;
        if (true == StageSystem.instance.IsBossStage())
        {
            _renderObject.localScale = new Vector3(1.5f, 1.5f, 1f);
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
    }

    public void HpRecovery()
    {
        _status.HpRecovery();
    }

    public double MaxHp
    {
        get
        {
            return _status.MaxHp;
        }
    }
}
