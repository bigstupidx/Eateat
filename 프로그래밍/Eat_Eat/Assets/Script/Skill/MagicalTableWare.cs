using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MagicalTableWare : Skill
{
    public float[] _arrDamageIncreaseRate;
    public float _durationTime = 0;

    private float _timeCounter = 0;
    private bool _active = false;

    // Use this for initialization
    void Awake() {
        _skillName = "MagicalTableWare";
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

    public float DamageIncreaseRate
    {
        get
        {
            if (_active)
            {
                return _arrDamageIncreaseRate[Level];
            }
            else
            {
                return 1;
            }
        }
    }
}
