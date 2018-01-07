using UnityEngine;
using System.Collections;

public class TableManager : MonoBehaviour
{
    public static TableManager instance = null;

    public SpriteRenderer _background;
    public SpriteRenderer[] _arrTouchTable;
    public SpriteRenderer _mainTable;
    public SpriteRenderer _firstTable;
    public SpriteRenderer _conveyorBelt;
    public SpriteRenderer _conveyorBeltTail;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void ValidateTheme() {
        _background.sprite = ImageManager.instance.GetThemeSprite(EResourceType.eBackground);

        foreach (SpriteRenderer spritRenderer in _arrTouchTable)
        {
            spritRenderer.sprite = ImageManager.instance.GetThemeSprite(EResourceType.eTouchTable);
        }

        _mainTable.sprite = ImageManager.instance.GetThemeSprite(EResourceType.eMainTable);
        _firstTable.sprite = ImageManager.instance.GetThemeSprite(EResourceType.eFirstTable);
        _conveyorBelt.sprite = ImageManager.instance.GetThemeSprite(EResourceType.eConveyorBelt);
        _conveyorBeltTail.sprite = ImageManager.instance.GetThemeSprite(EResourceType.eConveyorBeltTail);
    }
}
