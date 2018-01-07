using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalFunction;

//추상 클래스
public abstract class Skill : MonoBehaviour
{
    public int          _maxCoolTime = 10;
    private float       _coolTime;
    public int          _coolTimeAdvance = 30;
    public int[]        _arrLevelReq;

    private StoreItem   _storeItem;

    protected string    _skillName;
    protected int       _level = 0;

    private float       _coolTimeCounter = 0;
    private bool        _buttonActive = true;

    protected Image     _image = null;
    private Text        _text;

    // Use this for initialization
    public void Initialize()
    {
        _image = Func.GetChildComponent<Image>(gameObject, "SkillButton");
        Func.GetChildComponent<Image>(gameObject, "SkillButtonBackground").sprite = _image.sprite;
        _text = Func.GetChildComponent<Text>(gameObject, "SkillText");
        _coolTime = _maxCoolTime;
    }

    public void SetStoreItem(StoreItem storeItem)
    {
        _storeItem = storeItem;
    }

    public void LoadData()
    {
        int nData;
        if (SaveDataManager.instance.Skill.TryGetValue(_skillName, out nData)) { _level = nData; }

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
        SaveDataManager.instance.Skill[_skillName] = _level;
    }

    private IEnumerator UpdateCooltime()
    {
        while (true)
        {
            _image.fillMethod = Image.FillMethod.Radial360;
            _coolTimeCounter -= Time.deltaTime;
            _image.fillAmount = 1 - (_coolTimeCounter / _coolTime);
            if (60 < _coolTimeCounter)
            {
                SetText(string.Format("{0:D2}", (int)(_coolTimeCounter / 60)) + " : " + string.Format("{0:D2}", ((int)_coolTimeCounter % 60)));
            }
            else{
                SetText(string.Format("{0:F2}", _coolTimeCounter));
            }

            if (_coolTimeCounter <= 0)
            {
                SetText("");
                _buttonActive = true;
                break;
            }
            yield return null;
        }
    }

    public virtual void LevelUp()
    {
        _level++;

        if (1 == _level) {
            SetActive(true);
        }

        _coolTime = _maxCoolTime - ((_level  - 1)* _coolTimeAdvance);
        if (_coolTime < 0)
        {
            _coolTime = 0;
        }
        
        double reqCalorie = PlayerStatus.instance.GetLevelUpCost(_arrLevelReq[Level]) * 5;
        PlayerStatus.instance.AddCalorie(-reqCalorie);
    }

    public void CheckLevelUpAble()
    {
        if (_storeItem == null)
        {
            return;
        }

        _storeItem.Notify("update_status");

        if (_arrLevelReq.Length - 1 <= _level)
        {
            _storeItem.Notify("max_level", "Max Level");
            return;
        }

        double reqCalorie = PlayerStatus.instance.GetLevelUpCost(_arrLevelReq[Level + 1]) * 5;
        string reqCalorieString = Func.NumToABC(reqCalorie);
        if (PlayerStatus.instance.Calorie < reqCalorie)
        {
            _storeItem.Notify("level_up_disable", "레벨업\n" + "캐릭터 레벨 " + _arrLevelReq[Level + 1] + "\n칼로리 " + reqCalorieString);
            return;
        }

        if (PlayerStatus.instance.Level < _arrLevelReq[Level + 1])
        {
            _storeItem.Notify("level_up_disable", "레벨업\n" + "캐릭터 레벨 " + _arrLevelReq[Level + 1] + "\n칼로리 " + reqCalorieString);
            return;
        }

        _storeItem.Notify("level_up_enable", "레벨업\n" + "캐릭터 레벨 " + _arrLevelReq[Level + 1] + "\n칼로리 " + reqCalorieString);
    }

    public void SetActive(bool bActive)
    {
        gameObject.active = bActive;
    }

    public void SetText(string data)
    {
        _text.text = data;
    }

    public abstract void OnButtonClick();

    protected void ButtonClick()
    {
        _buttonActive = false;
        _coolTimeCounter = Cooltime;
        StartCoroutine(UpdateCooltime());
    }

    protected bool IsButtonActive
    {
        get
        {
            return _buttonActive;
        }
    }

    public int Level
    {
        get
        {
            return _level;
        }
    }

    public int Cooltime
    {
        get
        {
            if (_level == 0)
            {
                return 0;
            }

            int coolTime = _maxCoolTime - (_coolTimeAdvance * (_level - 1));
            return coolTime;
        }
    }

    public Sprite SkillImage
    {
        get
        {
            return _image.sprite;
        }
    }
}
