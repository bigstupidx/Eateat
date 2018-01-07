using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class StoreItem : MonoBehaviour
{
    public abstract void Initialize();
    public abstract void LoadData();
    public abstract void SaveData();
    public abstract void CheckLevelUpAble();
    public abstract void Notify(string type, string data = "");
}

public class StoreManager : MonoBehaviour
{
    public static StoreManager instance = null;

    public GameObject[] _arrGroupObject;
    private List<StoreItem> _lstStoreItem;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void Initialize()
    {
        List<StoreItem[]> lstStoreItems = new List<StoreItem[]>();
        _lstStoreItem = new List<StoreItem>();

        foreach (GameObject obj in _arrGroupObject) {
            lstStoreItems.Add(obj.GetComponentsInChildren<StoreItem>());
        }

        foreach (StoreItem[] arrItem in lstStoreItems)
        {
            foreach (StoreItem item in arrItem) {
                item.Initialize();
                _lstStoreItem.Add(item);
            }
        }
    }

    public void LoadData()
    {
        foreach (StoreItem item in _lstStoreItem) {
            item.LoadData();
        }
    }

    public void SaveData()
    {
        foreach (StoreItem item in _lstStoreItem)
        {
            item.SaveData();
        }
    }

    public void CheckLevelUpAble()
    {
        foreach (StoreItem item in _lstStoreItem)
        {
            item.CheckLevelUpAble();
        }
    }
}
