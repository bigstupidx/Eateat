using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//하나의 애니메이션 타입에 여러 애니메이션을 재생할 수있도록 한다.
[System.Serializable]
public struct AnimationData
{
    //애니메이션 Type : 공격, 실패, 등등..
    public string       _animationType;
    //애니메이션 이름
    public string[]     _arrAnimationName;
}   

public class PlayerAnimation : MonoBehaviour
{

    public AnimationData[]  _arrAnimationData;
    public float            _feverAccelerate;


    private Animator        _animator;
    private Dictionary<string, string[]> _dicAnimation;

    public float            _hungryTime;
    private float           _timeCounter = 0;

	// Use this for initialization
	public void Initialize () {
        _animator = GetComponentInChildren<Animator>();
        _dicAnimation = new Dictionary<string, string[]>();

        foreach (AnimationData animationData in _arrAnimationData) {
            _dicAnimation[animationData._animationType] = animationData._arrAnimationName;
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        _timeCounter += Time.deltaTime;
        if (_hungryTime < _timeCounter)
        {
            _timeCounter = 0;
            _animator.SetBool("IsHungry", true);
        }

        switch (PlayerStatus.instance.GetState())
        {
            case EPlayerState.normal:
                _animator.SetFloat("Speed", 1);
                _animator.SetBool("IsFever", false);
                break;
            case EPlayerState.fever:
            case EPlayerState.super_fever:
                _animator.SetFloat("Speed", _feverAccelerate);
                _animator.SetBool("IsFever", true);
                break;
        }
    }

    public void PlayAnimation(string animationName)
    {
        if (animationName == "eat")
        {
            _timeCounter = 0;
            _animator.SetBool("IsHungry", false);
        }
        if (_animator.GetBool("IsUnstopAble") == false)
        {
            string[] arrAnimationName = _dicAnimation[animationName];
            int numOfAnimation = arrAnimationName.Length;

            //애니메이션 타입에 있는 애니메이션 이름중 랜덤 애니메이션이름을 뽑아 재생시킨다.
            int idx = Random.Range(0, numOfAnimation);
            _animator.CrossFade(arrAnimationName[idx], 0, -1);
        }
    }

    public void SetController(RuntimeAnimatorController controller)
    {
        _animator.runtimeAnimatorController = controller;
    }

    public void SetTrigger(string name)
    {
        _animator.SetTrigger(name);
    }

    public void SetFloat(string name, float data)
    {
        _animator.SetFloat(name, data);
    }

    public void SetBool(string name, bool data)
    {
        _animator.SetBool(name, data);
    }
}
