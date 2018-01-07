using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalFunction;

public class TablewareStoreItem : StoreItem
{
    private Button          _levelUpButton;
    private Text            _levelUpButtonText;
    private Text            _levelText;
    private Text            _damageText;
    private Image           _image;

    public TablewareStatus _status;
    private TablewareType[] _arrType;
//    private TablewareType   _equiptedType;

    public override void Initialize()
    {
        _status.SetStoreItem(this);

        _levelUpButton = Func.GetChildComponent<Button>(gameObject, "LvUpButton");
        _levelUpButtonText = Func.GetChildComponent<Text>(_levelUpButton.gameObject, "Text");
        _levelText = Func.GetChildComponent<Text>(gameObject, "LevelText");
        _damageText = Func.GetChildComponent<Text>(gameObject, "DamageText");
        _image = Func.GetChildComponent<Image>(gameObject, "ItemImage");
        _arrType = GetComponentsInChildren<TablewareType>();

        foreach (TablewareType type in _arrType) {
            type.InitializeTablewareType(_status);
        }
    }

    public override void LoadData()
    {
        _status.LoadData();
        foreach (TablewareType type in _arrType) {
            type.LoadData();
        }
    }

    public override void SaveData()
    {
        _status.SaveData();
        foreach (TablewareType type in _arrType)
        {
            type.SaveData();
        }
    }

    public void OnButtonClick()
    {
        _status.LevelUp();
        foreach (TablewareType tablewareType in _arrType)
        {
            tablewareType.UpdateData();
        }
    }

    public override void CheckLevelUpAble() {
        _status.CheckLevelUpAble();
        foreach (TablewareType tablewareType in _arrType)
        {
            tablewareType.Renewal();
        }
    }

    public override void Notify(string type, string data = "")
    {
        switch (type)
        {
        //레벨업 불가
            case "calorie_lack":
            case "level_lack":
            case "max_level":
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
                int level = int.Parse(data);
                _levelText.text = "Level : " + level;
                break;
            case "purchase":
                break;
            case "equipt":
                _image.sprite = _status.GetSpriteRenderer().sprite;
                break;
            case "unequipt":
                _damageText.text = data;
                _image.sprite = null;
                break;
            case "damage_text":
                _damageText.text = data;
                break;
            default:
                Debug.Log("wrong type : " + type);
                break;
        }
    }

    public TablewareStatus GetStatus()
    {
        return _status;
    }
}
