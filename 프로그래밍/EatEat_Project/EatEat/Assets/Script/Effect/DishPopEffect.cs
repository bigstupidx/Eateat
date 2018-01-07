using UnityEngine;
using System.Collections;
using GlobalFunction;

//반투명 접시 터치 이펙트
//Update는 객체에 붙어있는 Animator Controller에서 처리되고.
//Return To Queue 또한 Unity Editor Animation에서 호출된다.
public class DishPopEffect : MonoBehaviour, IPoolObject
{
    public Sprite[] _arrDishImage;
    private Animator _animator;
    private SpriteRenderer _dishRenderer;

    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _dishRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    //Object Pool에서 나올때 선행되는 과정
    public void OutFromQueue()
    {
        gameObject.SetActive(true);
    }

    //Object Pool에 도로 반환될 때 선행되는 과정
    public void ReturnToQueue()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.instance.Enqueue(this);
    }

    public void SetDishColor(EDishColor dishColor)
    {
        _dishRenderer.sprite = _arrDishImage[(int)dishColor];
    }
}
