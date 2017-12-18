using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManager : MonoBehaviour {

    public static UiManager instance = null;
    private bool _touchDisable = false;

    private Dictionary<string, GameObject> _mapChildObject;
   
    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        InitGame();
    }

    void InitGame()
    {
        _mapChildObject = new Dictionary<string, GameObject>();
        UiMapping(transform);
    }

    //Ui Object의 모든 자식들(경로까지)을 Mapping한다. 
    void UiMapping(Transform trans)
    {
        foreach (Transform obj in trans)
        {

            GameObject objTemp;
            _mapChildObject.TryGetValue(obj.name, out objTemp);

            if (objTemp != null)
            {
//                print("동명의 Object가 존재합니다 : " + obj.name);
            }
            _mapChildObject[obj.name] = obj.gameObject;
            UiMapping(obj);
        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    public T GetChildComponent<T>(string childName)
    {
        GameObject obj = _mapChildObject[childName];
        return obj.GetComponentInChildren<T>();
    }

    public bool TouchDisable
    {
        set
        {
            _touchDisable = value;
        }
        get
        {
            return _touchDisable;
        }
    }
}
