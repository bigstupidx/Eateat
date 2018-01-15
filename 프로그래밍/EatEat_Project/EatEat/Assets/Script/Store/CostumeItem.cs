using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using GlobalFunction;

public class CostumeItem : StoreItem
{
    public RuntimeAnimatorController _controller;
    public int      _starPrice;
    public bool     _isCachItem;
    private bool    _isPurchased = false;
    public bool     _isEquipted = false;

    private Button  _button;
    private Text    _buttonText;
    private Text    _infoText;
    private Image   _image;
    private Image   _starImage;

    private CostumeItem[] _arrSibling;

    // Use this for initialization
    public override void Initialize()
    {
        _button = Func.GetChildComponent<Button>(gameObject, "Button");
        _buttonText = Func.GetChildComponent<Text>(_button.gameObject, "Text");
        _starImage = Func.GetChildComponent<Image>(_button.gameObject, "StarImage");
        _infoText = Func.GetChildComponent<Text>(gameObject, "InfoText");
        _image = Func.GetChildComponent<Image>(gameObject, "ItemImage");
        _button.onClick.AddListener(OnButtonClick);

        _arrSibling = transform.parent.GetComponentsInChildren<CostumeItem>();

        Renewal();
    }

    public override void LoadData()
    {
        int nData;
        if (SaveDataManager.instance.Purchased.TryGetValue(name, out nData))
        {
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

    public override void SaveData()
    {
        if (_isEquipted)
        {
            SaveDataManager.instance.Purchased[name] = 1;
        }
        else
        {
            SaveDataManager.instance.Purchased[name] = 0;
        }
    }

    public override void CheckLevelUpAble()
    {
        Renewal();
    }

    public override void Notify(string type, string data = "")
    {
        
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
                    _buttonText.text = "장착";
                    ColorBlock cb = _button.colors;
                    cb.normalColor = new Color(1, 1, 1);
                    _button.colors = cb;
                    _button.interactable = true;
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
                _buttonText.text = "장착";
                ColorBlock cb = _button.colors;
                cb.normalColor = new Color(1, 1, 1);
                _button.colors = cb;
                _button.interactable = true;
            }
            _starImage.gameObject.SetActive(false);
        }
    }

    private void Equipt()
    {
        foreach (CostumeItem item in _arrSibling)
        {
            item.UnEquipt();
        }
        _isEquipted = true;
        PlayerStatus.instance.SetController(_controller);
        Renewal();
    }

    public void UnEquipt()
    {
        _isEquipted = false;
        Renewal();
    }

    private void Purchase()
    {
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
            if (_isEquipted == true)
            {
                UnEquipt();
            }
            else
            {
                Equipt();
            }
        }
        else
        {
            Purchase();
        }
    }
}
