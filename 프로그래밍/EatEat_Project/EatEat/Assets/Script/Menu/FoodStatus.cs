using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System;

using GlobalFunction;

public enum ECalorieType
{
    Small,
    Normal,
    Boss,
}

public class FoodStatus : MonoBehaviour
{
    public float        _normalInitHp = 35;
    public float        _bossInitHp = 70;
    public float        _hpIncreaseRate = 1.73f;
    public float        _bossHpIncreaseRate = 1.94f;

    public float        _smallCaloriePercent = 0.000002f;
    public float        _normalCaloriePercent = 0.0007f;
    public float        _bossCaloriePercent = 0.0011f;

    private double      _maxHp;
    private double      _hp;
    private Text        _nameText;

    private BarUiAnimation _hpBarAnimation;
    private Text           _hpText;

    public void InitFoodStatus()
    {
        _hpBarAnimation = UiManager.instance.GetChildComponent<BarUiAnimation>("Hp Group");
        _hpText = UiManager.instance.GetChildComponent<Text>("Hp Group");
        _nameText = UiManager.instance.GetChildComponent<Text>("Food Group");

        if (true == StageSystem.instance.IsBossStage())
        {
            _maxHp = _bossInitHp * Math.Pow(_bossHpIncreaseRate, StageSystem.instance.GetBossStage());
        }
        else
        {
            _maxHp = _normalInitHp * Math.Pow(_hpIncreaseRate, StageSystem.instance.GetBossStage());
        }
        _hp = _maxHp;

        _hpText.text = Func.NumToABC(_hp) + " HP";
        _hpBarAnimation.SetPercentage(_hp / _maxHp);
    }

    public void SetName(string name)
    {
        _nameText.text = name;
    }

    public double GetHp()
    {
        return _hp;
    }

    public void AddHp(double fHp)
    {
        _hp += fHp;

        if (_hp < 0)
        {
            _hp = 0;
        }
        else if (_maxHp < _hp)
        {
            _hp = _maxHp;
        }

        _hpText.text = Func.NumToABC(_hp) + " HP";
        _hpBarAnimation.SetPercentage(_hp / _maxHp);
        //BiteMarkManager.instance.UpdateBiteMark((float)(_hp / _maxHp));
    }

    public void HpRecovery()
    {
        _hp += (PlayerStatus.instance.GetDamage() * 2);
        if (_maxHp < _hp)
        {
            _hp = _maxHp;
        }
        _hpText.text = Func.NumToABC(_hp) + " HP";
        _hpBarAnimation.SetPercentage(_hp / _maxHp);
        //BiteMarkManager.instance.UpdateBiteMark((float)(_hp / _maxHp));
    }

    public double GetCalorie(ECalorieType eCalorieType)
    {
        switch (eCalorieType) {
            case ECalorieType.Small:
                return _maxHp * _smallCaloriePercent * SkillManager.instance._doubleCalorie.CalorieIncreaseRate;
            case ECalorieType.Normal:
                return _maxHp * _normalCaloriePercent * SkillManager.instance._doubleCalorie.CalorieIncreaseRate;
            case ECalorieType.Boss:
                return _maxHp * _bossCaloriePercent * SkillManager.instance._doubleCalorie.CalorieIncreaseRate;
            default:
                return 0;
        }
    }

    public bool IsDead()
    {
        return (_hp == 0) ? true : false;
    }

    public double MaxHp
    {
        get
        {
            return _maxHp;
        }
    }

    public double HpPercentage
    {
        get
        {
            return _hp / _maxHp;
        }
    }
}
