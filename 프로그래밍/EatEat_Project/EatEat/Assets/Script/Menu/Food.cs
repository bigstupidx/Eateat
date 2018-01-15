using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{
    private SpriteRenderer  _spriteRenderer;
    private Sprite[]        _arrFoodImage;
    private Animator        _animator;

    void Start () {
        Initialize();
    }

    public void Initialize()
    {
        _spriteRenderer = transform.GetComponent<SpriteRenderer>();
        _arrFoodImage = ImageManager.instance.GetFood()._arrFoodImage;
        _spriteRenderer.sprite = _arrFoodImage[0];
        _animator = GetComponentInParent<Animator>();
    }

    public void NewFood()
    {
        _arrFoodImage = ImageManager.instance.GetFood()._arrFoodImage;
        _spriteRenderer.sprite = _arrFoodImage[0];
    }

    //음식 체력 퍼센트에 따라서 음식 Image 변경
    public void FoodDamaged(float fPercent)
    {
        //음식의 체력이 닳았을 때 울렁거리는 애니메이션을 재생한다.
        _animator.Play("Damaged");

        //체력 퍼센트에 따른 이미지 변경
        if (0.6f < fPercent)
        {
            _spriteRenderer.sprite = _arrFoodImage[0];
        }
        else if (0.3f < fPercent)
        {
            _spriteRenderer.sprite = _arrFoodImage[1];
        }
        else
        {
            _spriteRenderer.sprite = _arrFoodImage[2];
        }
    }
}
