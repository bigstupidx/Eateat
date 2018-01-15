using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainFoodManager : MonoBehaviour
{
    public static MainFoodManager instance;
    public FatEmitter   _fatEmiter;

    //Object Pooling 구현 해야됨.(아직 미 구현)
    public GameObject   _mainFoodPrefab;
    public GameObject   _positionObject;
    public GameObject   _positionObjectBoss;

    private GameObject  _mainFoodObj;
    private MainMenu    _mainFood;

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

    //메인 음식 생성(오브젝트 풀링 미 구현됨)
    public void CreateMainFood()
    {
        if (_mainFoodObj)
        {
            Destroy(_mainFoodObj);
        }

        if (true == StageSystem.instance.IsBossStage())
        {
            _mainFoodObj = Instantiate(_mainFoodPrefab, _positionObjectBoss.transform.position, Quaternion.identity) as GameObject;
            _mainFoodObj.transform.SetParent(transform, true);
            _mainFood = _mainFoodObj.GetComponent<MainMenu>();
            _mainFood.CreateMainFood();
        }
        else
        {
            _mainFoodObj = Instantiate(_mainFoodPrefab, _positionObject.transform.position, Quaternion.identity) as GameObject;
            _mainFoodObj.transform.SetParent(transform, true);
            _mainFood = _mainFoodObj.GetComponent<MainMenu>();
            _mainFood.CreateMainFood();
        }
    }

    //메인 음식에 데미지 부여
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
            PlayerStatus.instance.PlayAnimation("success");
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
