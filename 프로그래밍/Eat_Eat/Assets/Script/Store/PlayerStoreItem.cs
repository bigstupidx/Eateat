using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalFunction;

//public interface IObserver
//{
//    void Notify(string type, string data);
//}

//public interface IPublisher
//{
//    void AddObserver(IObserver observer);
//    void DeleteObserver(IObserver observer);
//    void notifyObserver();
//}

public class PlayerStoreItem : StoreItem
{
    private Button          _levelUpButton;
    private Text            _levelUpButtonText;
    private Text            _levelText;
    private Text            _damageText;
    private Image           _image;

    public PlayerStatus    _status;

    public override void Initialize()
    {
        _status.SetStoreItem(this);

        _levelUpButton = Func.GetChildComponent<Button>(gameObject, "LvUpButton");
        _levelUpButtonText = Func.GetChildComponent<Text>(_levelUpButton.gameObject, "Text");
        _levelUpButton.onClick.AddListener(OnButtonClick);
        _levelText = Func.GetChildComponent<Text>(gameObject, "LevelText");
        _damageText = Func.GetChildComponent<Text>(gameObject, "DamageText");
        _image = Func.GetChildComponent<Image>(gameObject, "ItemImage");
    }

    public override void LoadData()
    {
        _status.LoadData();
    }

    public override void SaveData()
    {
        _status.SaveData();
    }

    public void OnButtonClick()
    {
        _status.LevelUp();
    }

    public override void CheckLevelUpAble() {
        _status.CheckLevelUpAble();
    }

    //iObserver
    public override void Notify(string type, string data = "")
    {
        switch (type)
        {
        //레벨업 불가
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
                _levelText.text = "Level : " + _status.Level.ToString();
                _damageText.text = "데미지 : " + Func.NumToABC(_status.OriginalDamage);
            break;
            default:
                Debug.Log("wrong type : " + type);
                break;
        }
    }
}
