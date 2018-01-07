using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GlobalFunction;

public class InstanceSuperFever : Skill
{
    public float[] _arrDamageIncreaseRate;

    private float _timeCounter = 0;
    private bool _active = false;

    // Use this for initialization
    void Awake () {
        _skillName = "InstanceSuperFever";
    }

    // Update is called once per frame
    IEnumerator SkillUpdate()
    {
        while (true)
        {
            _image.fillMethod = Image.FillMethod.Vertical;
            _timeCounter -= Time.deltaTime;
            SetText(string.Format("{0:F2}", _timeCounter));
            _image.fillAmount = _timeCounter / FeverSystem.instance._superFeverMaxTime;
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

    public override void OnButtonClick() {
        if (true == IsButtonActive)
        {
            _active = true;
            FeverSystem.instance.SetSuperFever();
            _timeCounter = FeverSystem.instance._superFeverMaxTime;
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
                return 0;
            }
        }
    }
}
