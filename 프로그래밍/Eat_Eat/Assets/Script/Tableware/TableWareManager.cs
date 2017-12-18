using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalFunction;

public enum ETablewareType
{
    HelperSpoon         = 0,
    HelperKnife,
    HelperChopstick,
    HelperFork,
    MagicalSpoon,
    MagicalKnife,
    MagicalChopstick,
    MagicalFork
}

public enum ETablewareGrade
{
    None,
    Wood,
    Silver,
    Gold,
}

public class TableWareManager : MonoBehaviour
{
    public static TableWareManager instance = null;

    public TablewareStatus  _helperSpoon;
    public TablewareStatus  _helperKnife;
    public TablewareStatus  _helperChopStick;
    public TablewareStatus  _helperFork;
    private TablewareStatus[] _arrHelper;

    public TablewareStatus  _magicalSpoon;
    public TablewareStatus  _magicalKnife;
    public TablewareStatus  _magicalChopStick;
    public TablewareStatus  _magicalFork;
    private TablewareStatus[] _arrMagical;

    private int[]               _arrTablewareGrade;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    public void InitTableWareManager()
    {
        _arrHelper = new TablewareStatus[4];
        _arrHelper[0] = _helperSpoon;
        _arrHelper[1] = _helperKnife;
        _arrHelper[2] = _helperChopStick;
        _arrHelper[3] = _helperFork;
        foreach (TablewareStatus status in _arrHelper) {
            status.Initialize();
            status.SetActive(false);
        }

        _arrMagical = new TablewareStatus[4];
        _arrMagical[0] = _magicalSpoon;
        _arrMagical[1] = _magicalKnife;
        _arrMagical[2] = _magicalChopStick;
        _arrMagical[3] = _magicalFork;

        //Magical Tableware 랜덤 시작 및 disable
        foreach (TablewareStatus status in _arrMagical)
        {
            //Animator animator = Func.GetChildComponent<Animator>(status.gameObject, "Animation");
            //AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            //animator.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
            status.Initialize();
            status.SetActive(false);
        }

        int index = 0;
        _arrTablewareGrade = new int[1];
        _arrTablewareGrade[0] = 1;
    }

    public void CheckLevelUpAble()
    {
        foreach (TablewareStatus status in _arrHelper)
        {
            status.CheckLevelUpAble();
        }

        foreach (TablewareStatus status in _arrMagical)
        {
            status.CheckLevelUpAble();
        }
    }

    public float GetDamageBuff(EDishColor dishColor)
    {
        if (_arrHelper[(int)dishColor].isActiveAndEnabled)
        {
            TablewareStatus status = _arrHelper[(int)dishColor];
            return status.Damage;
        }
        else
        {
            return 1;
        }
    }
}
