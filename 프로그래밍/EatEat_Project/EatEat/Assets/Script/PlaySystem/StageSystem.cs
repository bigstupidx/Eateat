﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EStageMode
{
    Normal,
    Boss,
    Infinite
}

public class StageSystem : MonoBehaviour
{
    public static StageSystem instance = null;

    //보스방 탈출 & 재입장 버튼
    public GameObject   _stageButton = null;
    public Text         _buttonText = null;

    private EStageMode _eStageMode = EStageMode.Normal;

    //지나온 Boss Stage
    private int         _stage = 0;
    //1~10 사이 Stage
    private int         _miniStage = 1;
    private int         _maxMiniStage = 10;

    //State Theme(서양식, 한식, 일식 등...)
    private EFoodTheme  _foodTheme = EFoodTheme.western;
    private Text        _stageText;

    void Awake()
    {
        if (null != instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        _stageButton.SetActive(false);
    }

    public void InitStageSystem()
    {
        LoadData();
        _stageText = UiManager.instance.GetChildComponent<Text>("StageText");
        Verify();
    }

    public void LoadData()
    {
        if (SaveDataManager.instance != null)
        {
            double dData;
            if (SaveDataManager.instance.Status.TryGetValue("STG", out dData)) { _stage = (int)dData; }
        }
    }

    public void SaveData()
    {
        if (SaveDataManager.instance != null)
        {
            SaveDataManager.instance.Status["STG"] = _stage;
        }
    }

    EFoodTheme dat = EFoodTheme.western;

    public void NextStage()
    {
        switch (_eStageMode) {
            case EStageMode.Normal: //노말모드 일 때 스테이지 볂환
                _miniStage++;
                //Mini Stage 11 달성시 보스모드 입장
                if (_miniStage == _maxMiniStage + 1)
                {
                    _eStageMode = EStageMode.Boss;
                    _miniStage = 1;
                }
                break;
            case EStageMode.Boss:   //보스모드일 때 스테이지 변환
                _stage++;
                _eStageMode = EStageMode.Normal;
                _stageButton.SetActive(false);

                _foodTheme++;
                if (System.Enum.GetValues(typeof(EFoodTheme)).Length-1 < (int)_foodTheme)
                {
                    _foodTheme = 0;
                }

                TableManager.instance.ValidateTheme();

                break;
            case EStageMode.Infinite:   //무한모드일 때 스테이지 변환
                break;
        }
        Verify();
    }

    //스테이지 변환시 현재 스테이지 상태(UI) 최신화
    private void Verify()
    {
        switch (_eStageMode) {
            case EStageMode.Normal:
                _stageText.text = _miniStage.ToString() + " / " + _maxMiniStage.ToString();
                break;
            case EStageMode.Boss:
                _stageText.text = "BOSS!!";
                _stageButton.SetActive(true);
                _buttonText.text = "무한방";
                break;
            case EStageMode.Infinite:
                _stageText.text = "Infinite";
                _buttonText.text = "보스방";
                break;
        }
    }

    public EFoodTheme FoodTheme
    {
        get
        {
            return _foodTheme;
        }
    }

    public int GetBossStage()
    {
        return _stage;
    }

    public bool IsBossStage()
    {
        return _eStageMode == EStageMode.Boss;
    }

    public void OnButtonClick()
    {
        switch (_eStageMode)
        {
            case EStageMode.Boss:
                _eStageMode = EStageMode.Infinite;
                break;
            case EStageMode.Infinite:
                _eStageMode = EStageMode.Boss;
                break;
        }
        Verify();

        MainFoodManager.instance.CreateMainFood();
    }
}
