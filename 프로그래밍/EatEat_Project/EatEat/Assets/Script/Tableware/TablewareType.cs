using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalFunction;

public class TablewareType : MonoBehaviour
{
    //Ui정의 부
    private TablewareStoreItem  _storeItem;
    private TablewareStatus     _status;
    private Button              _button;
    private Image               _starImage;
    private Text                _buttonText;

    //식기 타입
    public float[]          _arrDamage;
    public int              _starPrice;
    public bool             _isCachItem;
    private Sprite          _sprite;
    private bool            _isPurchased = false;
    public bool             _isEquipted = false;

    private TablewareType[] _arrSibling;

    //private bool    _isPurchased = false;

    public void InitializeTablewareType(TablewareStatus status)
    {
        _storeItem = transform.parent.GetComponent<TablewareStoreItem>();
        _status = status;
        _button = Func.GetChildComponent<Button>(gameObject, "FunctionButton");
        _button.onClick.AddListener(OnButtonClick);
        _starImage = Func.GetChildComponent<Image>(gameObject, "Star");
        _buttonText = _button.GetComponentInChildren<Text>();
        _sprite = Func.GetChildComponent<Image>(gameObject, "ItemImage").sprite;

        _arrSibling = transform.parent.GetComponentsInChildren<TablewareType>();
    }

    public void LoadData()
    {
        int nData;
        string key = transform.parent.name + name;
        if (SaveDataManager.instance.Purchased.TryGetValue(key, out nData)) {
            _isPurchased = true;
            if (nData == 0)
            {
                _isEquipted = false;
            }
            else
            {
                _isEquipted = true;
                Equipt();
            }
        }
    }

    public void SaveData()
    {
        string key = transform.parent.name + name;
        if (_isEquipted == true)
        {
            SaveDataManager.instance.Purchased[key] = 1;
        }
        else
        {
            SaveDataManager.instance.Purchased[key] = 0;
        }
    }

    public void Renewal()
    {
        if (_isCachItem == true)    //유료 아이템인 경우
        {
            if (_isPurchased == true)   //구매 된 경우
            {
                if (_isEquipted == true)
                {
                    _buttonText.text = "장착 중";
                    ColorBlock cb = _button.colors;
                    cb.normalColor = new Color(0.5f, 0.7f, 1);
                    _button.colors = cb;
                }
                else
                {
                    if (1 <= _status.Level) //status Level 1 이상
                    {                 
                        _buttonText.text = "장착";
                        ColorBlock cb = _button.colors;
                        cb.normalColor = new Color(1, 1, 1);
                        _button.colors = cb;
                        _button.interactable = true;
                    }
                    else                    //status Level 0인 경우
                    {
                        _buttonText.text = "레벨 1 이상 필요";
                        _button.interactable = false;
                    }
                }
                _starImage.gameObject.SetActive(false);
            }
            else                        //구매가 안된 경우
            {
                if (PlayerStatus.instance.Star < _starPrice)
                {
                    _buttonText.text = _starPrice.ToString();
                    ColorBlock cb = _button.colors;
                    cb.normalColor = new Color(1, 0.8f, 0);
                    _button.colors = cb;
                    _button.interactable = false;
                }
                else
                {
                    _buttonText.text = _starPrice.ToString();
                    ColorBlock cb = _button.colors;
                    cb.normalColor = new Color(1, 0.8f, 0);
                    _button.colors = cb;
                    _button.interactable = true;
                }
                _starImage.gameObject.SetActive(true);
            }
        }
        else                        //무료 아이템인 경우
        {
            _isPurchased = true;
            if (_isEquipted == true)
            {
                _buttonText.text = "장착 중";
                ColorBlock cb = _button.colors;
                cb.normalColor = new Color(0.5f, 0.7f, 1);
                _button.colors = cb;
            }
            else
            {
                if (1 <= _status.Level) //status Level 1 이상
                {
                    _buttonText.text = "장착";
                    ColorBlock cb = _button.colors;
                    cb.normalColor = new Color(1, 1, 1);
                    _button.colors = cb;
                    _button.interactable = true;
                }
                else                    //status Level 0인 경우
                {
                    _buttonText.text = "레벨 1 이상 필요";
                    _button.interactable = false;
                }
            }
            _starImage.gameObject.SetActive(false);
        }
    }

    public void UpdateData()
    {
        if (_isEquipted == true)
        {
            _storeItem.Notify("damage_text", "증가율 : X " + _arrDamage[_status.Level]);
        }
    }

    private void Equipt()
    {
        foreach (TablewareType item in _arrSibling)
        {
            item.UnEquipt();
        }
        _status.SetTablewareType(this);
        _storeItem.Notify("equipt");
        _storeItem.Notify("damage_text", "증가율 : X " + _arrDamage[_status.Level]);
        _status.SetActive(true);
        _isEquipted = true;
        Renewal();
    }

    public void UnEquipt()
    {
        _status.SetActive(false);
        _storeItem.Notify("unequipt");
        _storeItem.Notify("damage_text", "증가율 : X " + _arrDamage[0]);
        _isEquipted = false;
        Renewal();
    }

    private void Purchase()
    {
        _storeItem.Notify("purchase");
        _isPurchased = true;
        _button.interactable = true;
        _starImage.gameObject.SetActive(false);
        PlayerStatus.instance.AddStar(-_starPrice);
        Renewal();
    }

    public void OnButtonClick()
    {
        if (_isPurchased)
        {
            Equipt();
        }
        else
        {
            Purchase();
        }
    }

    //Getter---------------------------------------------------
    public float GetDamage(int level)
    {
        return _arrDamage[level];
    }

    public Sprite SpriteImage
    {
        get
        {
            return _sprite;
        }
    }
}
