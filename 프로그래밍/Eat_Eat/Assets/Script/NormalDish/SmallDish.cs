using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GlobalFunction;

public class SmallDish : MonoBehaviour
{
 //Private

//Public
    public EDishColor   _dishColor = EDishColor.none;
    public Sprite[]     _arrDishImage;

    private Animator        _animator;
    private SpriteRenderer  _dishRenderer;
    private SmallFood       _food;

    // Use this for initialization
    void Start () {
        Initialize();
    }

    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _dishRenderer = Func.GetChildComponent<SpriteRenderer>(gameObject, "DishSprite");
        _dishRenderer.sprite = _arrDishImage[(int)_dishColor];
        //AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        //_animator.Play(state.fullPathHash, 0);
        _food = GetComponentInChildren<SmallFood>();
    }

    public void SetDishColor(EDishColor dishColor)
    {
        _dishColor = dishColor;
        _dishRenderer.sprite = _arrDishImage[(int)_dishColor];
    }

    //Getter
    public EDishColor GetDishColor()
    {
        return _dishColor;
    }

    public void FoodActivation(bool bFlag) {
        _food.gameObject.SetActive(bFlag);
    }
}
