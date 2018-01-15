using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalFunction;

public class DoubleCalorie : Skill
{
    public float    _durationTime = 5;
    public float[]  _arrCalorieIncreaseRate;

    private float   _timeCounter = 0;
    private bool    _active = false;

    // Use this for initialization
    void Awake()
    {
        _skillName = "DoubleCalorie";
    }

    // Update is called once per frame
    IEnumerator SkillUpdate()
    {
        while (true)
        {
            _timeCounter -= Time.deltaTime;
            _image.fillMethod = Image.FillMethod.Vertical;
            _image.fillAmount = _timeCounter/_durationTime;
            SetText(string.Format("{0:F2}", _timeCounter));
            if (_timeCounter <= 0)
            {
                SetText("");
                _timeCounter = 0;
                _active = false;
                break;
            }
            yield return null;
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }

    public override void OnButtonClick()
    {
        if (true == IsButtonActive)
        {
            _active = true;
            _timeCounter = _durationTime;
            ButtonClick();
            StartCoroutine(SkillUpdate());
        }
    }

    public float CalorieIncreaseRate
    {
        get
        {
            if (_active)
            {
                return _arrCalorieIncreaseRate[Level];
            }
            else
            {
                return 1;
            }
        }
    }
}
