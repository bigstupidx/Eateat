using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TripleSpoonAttack : Skill {
    public TripleSpoon tripleSpoon = null;
    public float[] _arrDamage;

    // Use this for initialization
    void Awake() {
        _skillName = "TripleSpoonAttack";
    }
	
    public override void LevelUp()
    {
        base.LevelUp();
    }

    public override void OnButtonClick()
    {
        if (true == IsButtonActive)
        {
            tripleSpoon.Activate();
            ButtonClick();
        }
    }

    public void DealDamage()
    {
        bool bCritical = PlayerStatus.instance.IsCritical;
        DamageEmiter.instance.EmitDamageText(_arrDamage[Level], bCritical);

        MainFoodManager.instance.AddHp(-_arrDamage[Level]);
    }
}
