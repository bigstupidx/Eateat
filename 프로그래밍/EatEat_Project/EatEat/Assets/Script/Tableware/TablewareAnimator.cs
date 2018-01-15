using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TablewareAnimator : MonoBehaviour
{
    private TablewareStatus _tablewareStatus;
    private Animator _animator;

	// Use this for initialization
	void Start () {
        _tablewareStatus = transform.parent.GetComponentInChildren<TablewareStatus>();
       _animator = GetComponent<Animator>();
    }

    public void Attack()
    {
        _animator.Play("Attack", -1, 0);
    }

    void AddHp()
    {
       _tablewareStatus.DealDamage();
    }
}
