using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
//using UnityEditor;

[System.Serializable]
public struct AnimationData
{
    public string           _animationType;
    public List<string>     _lstAnimationName;
}   

public class PlayerAnimation : MonoBehaviour
{
    public AnimationData[]  _arrAnimationData;
    public float            _feverAccelerate;

    private Animator        _animator;
    private Dictionary<string, List<string>> _dicAnimation;

	// Use this for initialization
	public void Initialize () {
        _animator = GetComponent<Animator>();
        _dicAnimation = new Dictionary<string, List<string>>();

        foreach (AnimationData animationData in _arrAnimationData) {
            _dicAnimation[animationData._animationType] = animationData._lstAnimationName;
        }
    }
	
	// Update is called once per frame
	void Update () {
        switch (PlayerStatus.instance.GetState())
        {
            case EPlayerState.normal:
                _animator.speed = 1;
                break;
            case EPlayerState.fever:
            case EPlayerState.super_fever:
                _animator.speed = _feverAccelerate;
                break;
        }
    }

    public void PlayAnimation(string animationName)
    {
        List<string> lstAnimationName = _dicAnimation[animationName];
        int numOfAnimation = lstAnimationName.Count;
        int idx = Random.Range(0, numOfAnimation);

        switch (PlayerStatus.instance.GetState()) {
            case EPlayerState.normal:
                _animator.Play(lstAnimationName[idx], -1, 0);
                break;
            case EPlayerState.fever:
                _animator.Play(lstAnimationName[idx], -1, 0);
                break;
            case EPlayerState.super_fever:
                _animator.Play(lstAnimationName[0], -1, 0);
                break;
        }
    }

    public void SetController(RuntimeAnimatorController controller)
    {
        _animator.runtimeAnimatorController = controller;
    }
}
