using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using GlobalFunction;

public class SmallMenu : MonoBehaviour, IPoolObject
{
    public EDishColor   _dishColor = EDishColor.none;
    public Sprite[]     _arrDishImage;

    private Animator        _animator;
    private SpriteRenderer  _dishRenderer;
    private Food       _food;

    // Use this for initialization
    void Start () {
        Initialize();
    }

    public void Initialize()
    {
        _animator = GetComponent<Animator>();
        _dishRenderer = Func.GetChildComponent<SpriteRenderer>(gameObject, "DishSprite");
        _dishRenderer.sprite = _arrDishImage[(int)_dishColor];
        _food = GetComponentInChildren<Food>();
        if (_food)
        {
            _food.Initialize();
        }
    }

    public void OutFromQueue()
    {
        gameObject.SetActive(true);
    }

    public void ReturnToQueue()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.instance.Enqueue(this);
    }

    public void DestroyOnTime(float time)
    {
        StartCoroutine(Timer(time));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        ReturnToQueue();
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

    public void FoodActivation(bool bFlag)
    {
        if (bFlag == true)
        {
            _food.NewFood();
        }
        _food.gameObject.SetActive(bFlag);
    }

    public void AnimationActivation(bool bFlag)
    {
        _animator.enabled = bFlag;
    }
}
