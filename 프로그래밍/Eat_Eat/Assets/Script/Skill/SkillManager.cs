using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SkillManager : MonoBehaviour
{
    public static SkillManager instance = null;

    public InstanceSuperFever           _instanceSuperFever = null;
    public TripleSpoonAttack            _tripleSpoonAttack = null;
    public CriticalUp                   _criticalUp = null;
    public DoubleCalorie                _doubleCalorie = null;
    public MagicalTableWare             _magicalTableWare = null;
    public DoubleDamage                 _doubleDamage = null;
    //private Dictionary<string, Skill>   _dicSkill;

    private Skill[]                     _arrSkill;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void InitSkillManager()
    {
        _arrSkill = new Skill[6];
        _arrSkill[0] = _instanceSuperFever;
        _arrSkill[1] = _tripleSpoonAttack;
        _arrSkill[2] = _criticalUp;
        _arrSkill[3] = _doubleCalorie;
        _arrSkill[4] = _magicalTableWare;
        _arrSkill[5] = _doubleDamage;

        foreach (Skill skill in _arrSkill)
        {
            skill.Initialize();
            skill.gameObject.SetActive(false);
        }
    }
}
