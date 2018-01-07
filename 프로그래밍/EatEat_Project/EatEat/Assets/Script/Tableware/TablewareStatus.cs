using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalFunction;

public class TablewareStatus : MonoBehaviour
{
    public int[]    _arrLevelReq;

    //Store Ui 정보
    private StoreItem _storeItem;

    private Animator        _animator;
    private SpriteRenderer  _spriteRenderer;
    private TablewareType   _tablewareType;

    private int     _level = 0;

    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        Transform animator = transform.Find("Animation");
        _spriteRenderer = animator.gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public void SetTablewareType(TablewareType tablewareType)
    {
        _tablewareType = tablewareType;
        _spriteRenderer.sprite = tablewareType.SpriteImage;
    }

    public void SetStoreItem(StoreItem storeItem)
    {
        _storeItem = storeItem;
    }

    public void LoadData()
    {
        int nData;
        if (SaveDataManager.instance.Skill.TryGetValue(name, out nData)) { _level = (int)nData; }

        if (_level != 0)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void SaveData()
    {
        SaveDataManager.instance.Skill[name] = _level;
    }

    public void DealDamage()
    {
        double damage = _tablewareType._arrDamage[_level] * SkillManager.instance._magicalTableWare.DamageIncreaseRate;
        MainFoodManager.instance.AddHp(-damage);

        bool bCritical = PlayerStatus.instance.IsCritical;
        DamageEmiter.instance.EmitDamageText(damage, bCritical);
    }

    public virtual void LevelUp()
    {
        _level++;
        PlayerStatus.instance.AddCalorie(-PlayerStatus.instance.GetLevelUpCost(_arrLevelReq[Level]) * 4);
    }

    public void CheckLevelUpAble()
    {
        if (_storeItem == null)
        {
            return;
        }

        _storeItem.Notify("update_status", _level.ToString());

        if (_arrLevelReq.Length-1 <= _level)
        {
            _storeItem.Notify("max_level", "Max Level");
            return;
        }

        double reqCalorie = PlayerStatus.instance.GetLevelUpCost(_arrLevelReq[Level + 1]) * 4;
        string reqCalorieString = Func.NumToABC(reqCalorie);
        if (PlayerStatus.instance.Calorie < reqCalorie) {
            _storeItem.Notify("calorie_lack", "레벨업\n" + "캐릭터 레벨 " + _arrLevelReq[Level + 1] + "\n칼로리 " + reqCalorieString);
            return;
        }

        if (PlayerStatus.instance.Level < _arrLevelReq[Level + 1])
        {
            _storeItem.Notify("level_lack", "레벨업\n" + "캐릭터 레벨 " + _arrLevelReq[Level + 1] + "\n칼로리 " + reqCalorieString);
            return;
        }

        _storeItem.Notify("level_up_enable", "레벨업\n" + "캐릭터 레벨 " + _arrLevelReq[Level + 1] + "\n칼로리 " + reqCalorieString);
    }

    public void SetActive(bool bActive)
    {
        gameObject.active = bActive;
    }

    public bool IsLevelUpAble
    {
        get
        {
            //스킬 최대 레벨 제한
            if (_arrLevelReq.Length - 1 <= _level)
            {
                return false;
            }
            //캐릭터 레벨 제한
            if (PlayerStatus.instance.Level < _arrLevelReq[_level + 1])
            {
                return false;
            }
            return true;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
        set
        {
            _level = value;
        }
    }

    public float Damage
    {
        get
        {
            _animator.Play("Attack", -1, 0);
            return _tablewareType.GetDamage(_level);
        }
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return _spriteRenderer;
    }
}