using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeverSystem : MonoBehaviour
{
    public static FeverSystem instance = null;

    //피버 모드를 위한 콤보 기록
    private int     _nComboCount = 0;
    public int      _nMaxCombo = 30;

    private float   _feverTimer;
    public float    _feverMaxTime = 10;
    private float   _feverGage = 0;

    private int     _nFeverCount = 0;
    public int      _nMaxFever = 5;
    private float   _superFeverTimer = 0;
    public float    _superFeverMaxTime = 15;

    //Ui
    private BarUiAnimation _feverGauge;
    private BarUiAnimation _superfeverGauge;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        InitFeverSystem();
    }

    void InitFeverSystem()
    {
        _feverGauge = UiManager.instance.GetChildComponent<BarUiAnimation>("FeverGauge");
        _feverGauge.SetBeginPercentage(0);
        _feverGauge.SetPercentage(0);

        _superfeverGauge = UiManager.instance.GetChildComponent<BarUiAnimation>("SuperFeverGauge");
        _superfeverGauge.SetBeginPercentage(0);
        _superfeverGauge.SetPercentage(0);
    }
	
	// Update is called once per frame
	void Update () {
        switch (PlayerStatus.instance.GetState())
        {
            case EPlayerState.normal:
                break;
            case EPlayerState.fever:
                _feverTimer += Time.deltaTime;

                _feverGauge.SetPercentage((_feverMaxTime - _feverTimer) / _feverMaxTime);
                if (_feverTimer > _feverMaxTime)
                {
                    _feverTimer = 0;
                    PlayerStatus.instance.SetState(EPlayerState.normal);
                    FoodConveyor.instance.SetDishSpeed(EDishSpeed.min);
                }
                break;
            case EPlayerState.super_fever:
                _superFeverTimer += Time.deltaTime;

                _superfeverGauge.SetPercentage((_superFeverMaxTime - _superFeverTimer) / _superFeverMaxTime);
                if (_superFeverTimer > _superFeverMaxTime)
                {
                    _superFeverTimer = 0;
                    PlayerStatus.instance.SetState(EPlayerState.normal);
                    FoodConveyor.instance.SetDishSpeed(EDishSpeed.min);
                }
                break;
        }
    }

    public void AddFeverGauge()
    {
        //피버 상태가 아닐때에만 피버를 위한 콤보 카운터를 증가시킨다.
        EPlayerState state = PlayerStatus.instance.GetState();
        if (state == EPlayerState.normal)
        {
            _nComboCount++;
            _feverGauge.SetPercentage((float)_nComboCount / _nMaxCombo);

            //피버 상태를 발생시킨다.
            if (_nComboCount == _nMaxCombo)
            {
                _nFeverCount++;
                _superfeverGauge.SetPercentage((float)_nFeverCount / _nMaxFever);

                if (_nFeverCount == _nMaxFever)
                {
                    _nFeverCount = 0;
                    _nComboCount = 0;
                    _feverGauge.SetPercentage(0);
                    PlayerStatus.instance.SetState(EPlayerState.super_fever);
                    FoodConveyor.instance.SetDishSpeed(EDishSpeed.max);
                }
                else
                {
                    _nComboCount = 0;
                    PlayerStatus.instance.SetState(EPlayerState.fever);
                    FoodConveyor.instance.SetDishSpeed(EDishSpeed.max);
                }
            }
        }
    }

    public void SetSuperFever()
    {
        _superFeverTimer = 0;
        _superfeverGauge.SetPercentage(1);
        PlayerStatus.instance.SetState(EPlayerState.super_fever);
        FoodConveyor.instance.SetDishSpeed(EDishSpeed.max);
    }

    public void ResetFeverGauge()
    {
        _nComboCount = 0;
        _feverGauge.SetPercentage(0);
    }
}
