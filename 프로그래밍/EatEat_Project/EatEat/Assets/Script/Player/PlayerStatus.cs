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

    public Background   _background = null;
    //Ui
    public Text         _calorieText = null;
    public Text         _starText = null;
    private StoreItem   _storeItem;

    void Awake()
    {
        //if (instance != null)
        //{
        //    Destroy(instance._this);
        //    instance._this = gameObject;
        //    return;
        //}
        //instance = this;
        //instance._this = gameObject;

        //DontDestroyOnLoad(gameObject);

        Debug.Log(name);

        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        Debug.Log(_calorie);
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

    //Cloud로부터 받아온 데이터로부터 데이터 추출
    public void LoadData()
    {
        if (SaveDataManager.instance != null)
        {
            double dData;
            int nData;
            if (SaveDataManager.instance.Status.TryGetValue("CAL", out dData)) { _calorie = dData; }
            if (SaveDataManager.instance.Status.TryGetValue("STAR", out dData)) { _star = (int)dData; }
            if (SaveDataManager.instance.Status.TryGetValue("LEV", out dData)) { _level = (int)dData; }
            if (SaveDataManager.instance.Status.TryGetValue("RBT", out dData)) { _rebirth = (int)dData; }
        }
        UpdateDamage();
        _calorieText.text = Func.NumToABC(_calorie);
        _starText.text = _star.ToString();
    }

    //Cloud에 데이터 저장을 위하여 데이터 기록
    public void SaveData()
    {
        if (SaveDataManager.instance != null)
        {
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

    //PlayerStoreItem에서 PlayerStatus에 따라서 UI 상태를 갱신하기 위하여 사용
    public void CheckLevelUpAble()
    {
        if (_storeItem == null)
        {
            return;
        }

        _storeItem.Notify("update_status", _level.ToString());

        double reqCalorie = GetLevelUpCost(Level);
        string reqCalorieString = Func.NumToABC(reqCalorie);

        //칼로리가 부족하지 않다면
        if (PlayerStatus.instance.Calorie < reqCalorie)
        {
            _storeItem.Notify("level_up_disable", "레벨업\n" + "칼로리 " + reqCalorieString);
            return;
        }

        _storeItem.Notify("level_up_enable", "레벨업\n" + "칼로리 " + reqCalorieString);
    }

    //레벨업에 필요한 Calorie 계산
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

    //칼로리 추가
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

    //별 추가
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
        damage *= TableWareManager.instance.GetDamageBuff(dishColor);

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

    public void PlayAnimation(string name)
    {
        _animation.PlayAnimation(name);
    }

    public bool IsCritical
    {
        get
        {
            return UnityEngine.Random.Range(1, 101) <= ((_critical + SkillManager.instance._criticalUp.Critical) * 100);
        }
    }

    //노말, 피버, 슈퍼 피버 상태 변환
    public void SetState(EPlayerState state)
    {
        _state = state;
        switch (state)
        {
            case EPlayerState.normal:
                _background.OnOff(true);
                _animation.SetBool("IsFever", false);
                _animation.SetTrigger("TriggerFever");
                break;
            case EPlayerState.fever:
                SoundManager.Instance.PlayEffectSound("fever");
                _background.OnOff(false);
                _animation.SetBool("IsFever", true);
                _animation.SetBool("IsUnstopAble", true);
                Handheld.Vibrate();
                break;
            case EPlayerState.super_fever:
                SoundManager.Instance.PlayEffectSound("super_fever");
                _background.OnOff(false);
                _animation.SetBool("IsFever", true);
                _animation.SetBool("IsUnstopAble", true);
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
