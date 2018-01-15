using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//사용하지 않음

public class BiteMarkManager : MonoBehaviour
{
    public static BiteMarkManager instance = null;

    public List<GameObject>     _lstBiteMark;
    private int                 _maxCount;
    private int                 _index = 0;

    void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;

        //Sprite Renderer Disable
        foreach (GameObject biteMark in _lstBiteMark)
        {
            SpriteRenderer spriteRenderer = biteMark.GetComponent<SpriteRenderer>();
            spriteRenderer.enabled = false;
        }

        InitBiteMarkManager();
    }

    // Use this for initialization
    void Start () {

    }

    public void InitBiteMarkManager()
    {
        _maxCount = _lstBiteMark.Count;
        foreach (GameObject biteMark in _lstBiteMark) {
            biteMark.SetActive(false);
        }
        _index = 0;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void UpdateBiteMark(float hpPercent)
    {
        int newIndex = (int)(_maxCount * (1-hpPercent));

        if (_index < newIndex)
        {
            for (int i = _index; i < newIndex; ++i)
            {
                _lstBiteMark[i].SetActive(true);
            }
        }
        else
        {
            for (int i = newIndex; i <= _index; ++i)
            {
                _lstBiteMark[i].SetActive(false);
            }
        }
        _index = newIndex;
    }
}
