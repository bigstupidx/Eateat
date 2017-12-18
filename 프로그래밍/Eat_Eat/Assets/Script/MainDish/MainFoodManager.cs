using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFoodManager : MonoBehaviour {

    public static MainFoodManager instance;
    public FatEmitter   _fatEmiter;
    public GameObject   _mainFoodPrefab;
    public GameObject   _positionObject;

    private GameObject  _mainFoodObj;
    private MainFood    _mainFood;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    void Start () {
        CreateMainFood();
    }

    public void CreateMainFood()
    {
        if (_mainFoodObj)
        {
            Destroy(_mainFoodObj);
        }

        _mainFoodObj = Instantiate(_mainFoodPrefab, _positionObject.transform.position, Quaternion.identity) as GameObject;
        _mainFoodObj.transform.SetParent(transform, true);
        _mainFood = _mainFoodObj.GetComponent<MainFood>();
        _mainFood.CreateMainFood();
    }

    public void AddHp(double fDamage)
    {
        _mainFood.AddHp(fDamage);

        //Main Food를 다 먹었을 때.
        if (_mainFood.IsDead())
        {
            double fCalorie = _mainFood.GetCalorie(ECalorieType.Normal);
            _fatEmiter.EmitFat(fCalorie);
            StageSystem.instance.NextStage();
            TimerSystem.instance.ResetTimer();
            CreateMainFood();
        }
    }

    public void HpRecovery()
    {
        _mainFood.HpRecovery();
    }

    public double GetCalorie(ECalorieType eCalorieType)
    {
        return _mainFood.GetCalorie(eCalorieType);
    }
}
