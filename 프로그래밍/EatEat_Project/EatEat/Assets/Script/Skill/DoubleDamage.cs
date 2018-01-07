using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalFunction;

public class DoubleDamage : Skill
{
    public float[] _arrDamageIncrease;
    public float    _durationTime = 5;

    private float   _timeCounter = 0;
    private bool    _active = false;

    // Use this for initialization
    void Awake() {
        _skillName = "DoubleDamage";
    }

    // Update is called once per frame
    IEnumerator SkillUpdate()
    {
        while (true)
        {
            _image.fillMethod = Image.FillMethod.Vertical;
            _timeCounter -= Time.deltaTime;
            SetText(string.Format("{0:F2}", _timeCounter));
            _image.fillAmount = _timeCounter / _durationTime;
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

    public float IncreaseRate
    {
        get
        {
            if (true == _active)
            {
                return _arrDamageIncrease[Level];
            }
            else
            {
                return 1;
            }
        }
    }
}
