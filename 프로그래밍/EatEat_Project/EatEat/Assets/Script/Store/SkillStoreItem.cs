using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalFunction;

public class SkillStoreItem : StoreItem
{
    private Button _levelUpButton;
    private Text _levelUpButtonText;
    private Text _levelText;
    private Text _damageText;
    private Text _cooltimeText;
    private Image _image;

    public Skill _skill;

    public override void Initialize()
    {
        _skill.SetStoreItem(this);

        _levelUpButton = Func.GetChildComponent<Button>(gameObject, "LvUpButton");
        _levelUpButtonText = Func.GetChildComponent<Text>(_levelUpButton.gameObject, "Text");
        _levelText = Func.GetChildComponent<Text>(gameObject, "LevelText");
        _damageText = Func.GetChildComponent<Text>(gameObject, "DamageText");
        _cooltimeText = Func.GetChildComponent<Text>(gameObject, "CooltimeText");
        _image = Func.GetChildComponent<Image>(gameObject, "ItemImage");
        _image.sprite = _skill.SkillImage;

        _levelUpButton.onClick.AddListener(OnButtonClick);
    }

    public override void LoadData()
    {
        _skill.LoadData();
    }

    public override void SaveData()
    {
        _skill.SaveData();
    }

    public void OnButtonClick()
    {
        _skill.LevelUp();
    }

    public override void CheckLevelUpAble()
    {
        _skill.CheckLevelUpAble();
    }

    //iObserver
    public override void Notify(string type, string data = "")
    {
        switch (type)
        {
            //레벨업 불가
            case "max_level":
            case "level_up_disable":
                _levelUpButtonText.text = data;
                _levelUpButton.interactable = false;
                break;
            //레벨업 가능
            case "level_up_enable":
                _levelUpButtonText.text = data;
                _levelUpButton.interactable = true;
                break;
            //정보 갱신
            case "update_status":
                _levelText.text = "Level : " + _skill.Level.ToString();
                string cooltimeString = (_skill.Cooltime/60).ToString() + "분 " + (_skill.Cooltime % 60).ToString() + "초";
                _cooltimeText.text = "쿨타임 : " + cooltimeString;
                //_damageText.text = "데미지 : " + Func.NumToABC(_skill.OriginalDamage);
                break;
            default:
                Debug.Log("wrong type : " + type);
                break;
        }
    }
}
