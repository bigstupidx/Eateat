using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

using GlobalFunction;
//#if UNITY_EDITOR
//    using UnityEditor.Animations;
//#endif

public class PlayerStatus : MonoBehaviour
{
    public static PlayerStatus instance;

    private EPlayerState _state = EPlayerState.normal;
    private double  _damage;
    private double  _calorie;
    private int     _star;
    private int     _level;
    private int     _rebirth = 0;
    private float   _critical = 0.1f;

    private PlayerAnimation _animation;

    public float _superFeverIncrease = 1.3f;
    public float _criticalIncrease = 1.3f;

    public TableWareManager _tableWareManager = null;
    public Background   _background = null;
    //Ui
    public Text         _calorieText = null;
    public Text         _starText = null;
    private StoreItem _storeItem;

    void Awake()
    {
        if (null != instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void SetStoreItem(StoreItem storeItem)
    {
        _storeItem = storeItem;
    }

    public void InitPlayerStatus()
    {
        _animation = GetComponentInChildren<PlayerAnimation>();
        _animation.Initialize();
    }

    public void LoadData()
    {
        if (SaveDataManager.instance != null)
        {
            double dData;
            int nData;
            //if (SaveDataManager.instance.Status.TryGetValue("DMG", out dData)) { _damage = dData; }
            if (SaveDataManager.instance.Status.TryGetValue("CAL", out dData)) { _calorie = dData; }
            if (SaveDataManager.instance.Status.TryGetValue("STAR", out dData)) { _star = (int)dData; }
            if (SaveDataManager.instance.Status.TryGetValue("LEV", out dData)) { _level = (int)dData; }
            if (SaveDataManager.instance.Status.TryGetValue("RBT", out dData)) { _rebirth = (int)dData; }
        }
        UpdateDamage();
        _calorieText.text = Func.NumToABC(_calorie);
        _starText.text = _star.ToString();
    }

    public void SaveData()
    {
        if (SaveDataManager.instance != null)
        {
            //SaveDataManager.instance.Status["DMG"] = _damage;
            SaveDataManager.instance.Status["CAL"] = Math.Round(_calorie, 3);
            SaveDataManager.instance.Status["LEV"] = _level;
            SaveDataManager.instance.Status["RBT"] = _rebirth;
        }
    }

    public void LevelUp()
    {
        _level += 1;

        //칼로리 소비
        double reqCalorie = PlayerStatus.instance.GetLevelUpCost(Level);
        AddCalorie(-reqCalorie);
        UpdateDamage();
    }

    public void CheckLevelUpAble()
    {
        if (_storeItem == null)
        {
            return;
        }

        _storeItem.Notify("update_status", _level.ToString());

        double reqCalorie = PlayerStatus.instance.GetLevelUpCost(Level + 1);
        string reqCalorieString = Func.NumToABC(reqCalorie);
        if (PlayerStatus.instance.Calorie < reqCalorie)
        {
            _storeItem.Notify("level_up_disable", "레벨업\n" + "칼로리 " + reqCalorieString);
            return;
        }

        _storeItem.Notify("level_up_enable", "레벨업\n" + "칼로리 " + reqCalorieString);
    }

    public double GetLevelUpCost(int level)
    {
        double cost = 0.5;

        int nMultiplier = level - 1;

        if (level < 23)
        {
            cost *= Math.Pow(1.33, nMultiplier);
        }
        if (23 <= level)
        {
            cost *= Math.Pow(1.33, 21);
            cost *= Math.Pow(1.0725, level - 22);
        }

        cost = Math.Round(cost, 3);
        return cost;
    }

    public bool AddCalorie(double calorie)
    {
        _calorie += calorie;
        _calorieText.text = Func.NumToABC(_calorie);

        //구매창 UI Button 업데이트(Store UI 활성화 상태일 때에만)
        if (UiManager.instance.TouchDisable == true)
        {
            StoreManager.instance.CheckLevelUpAble();
        }
        
        return true;
    }

    public void AddStar(int star)
    {
        _star += star;
        _starText.text = _star.ToString();

        //구매창 UI Button 업데이트(Store UI 활성화 상태일 때에만)
        if (UiManager.instance.TouchDisable == true)
        {
            StoreManager.instance.CheckLevelUpAble();
        }
    }

    public void UpdateDamage()
    {
        int nMultiplier = (int)(_level / 10);
        int n = _level % 10;
        _damage = (Mathf.Pow(2, nMultiplier) * 10) - 10;
        _damage += Mathf.Pow(2, nMultiplier) * n;
    }

    public double GetDamage()
    {
        double damage = _damage;

        switch (_state) {
            case EPlayerState.normal:
                break;
            case EPlayerState.fever:
                break;
            case EPlayerState.super_fever:
                //Super Fever + Instance Super Fever Buff
                damage *= _superFeverIncrease + SkillManager.instance._instanceSuperFever.DamageIncreaseRate;
                break;
        }

        //Double Damage Buff
        damage *= SkillManager.instance._doubleDamage.IncreaseRate;

        return damage;
    }

    public void DealDamage(EDishColor dishColor)
    {
        //음식에 데미지 추가
        double damage = GetDamage();
        damage *= _tableWareManager.GetDamageBuff(dishColor);

        bool bCritical = IsCritical;

        //Critical + Critical Buff
        if (bCritical)
        {
            damage *= _criticalIncrease;
        }
        //데미지 텍스트
        DamageEmiter.instance.EmitDamageText(damage, bCritical);

        //데미지 부여
        MainFoodManager.instance.AddHp(-damage);

        //애니메이션
        _animation.PlayAnimation("eat");
    }

    public void DealFail()
    {
        _animation.PlayAnimation("fail");
    }

    public bool IsCritical
    {
        get
        {
            return UnityEngine.Random.Range(1, 101) <= ((_critical + SkillManager.instance._criticalUp.Critical) * 100);
        }
    }

    public void SetState(EPlayerState state)
    {
        _state = state;
        switch (state)
        {
            case EPlayerState.normal:
                _background.OnOff(true);
                break;
            case EPlayerState.fever:
                SoundManager.Instance.PlayEffectSound("fever");
                _background.OnOff(false);
                Handheld.Vibrate();
                break;
            case EPlayerState.super_fever:
                SoundManager.Instance.PlayEffectSound("super_fever");
                _background.OnOff(false);
                Handheld.Vibrate();
                break;
        }
    }

    public EPlayerState GetState()
    {
        return _state;
    }

    public void SetController(RuntimeAnimatorController controller)
    {
        _animation.SetController(controller);
    }

    public double OriginalDamage
    {
        get
        {
            return _damage;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
    }

    public double Calorie
    {
        get
        {
            return _calorie;
        }
    }

    public int Star
    {
        get
        {
            return _star;
        }
    }
}
