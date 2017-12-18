using UnityEngine;
using System.Collections;
using GlobalFunction;

public class DishPopEffect : MonoBehaviour
{
    public Sprite[] _arrDishImage;
    private Animator _animator;
    private SpriteRenderer _dishRenderer;

    public void Initialize()
    {
        _animator = GetComponentInChildren<Animator>();
        _dishRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetDishColor(EDishColor dishColor)
    {
        _dishRenderer.sprite = _arrDishImage[(int)dishColor];
    }

    public void DestroyOnTime(float time)
    {
        StartCoroutine(Timer(time));
    }

    private IEnumerator Timer(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPoolManager.instance.EnqueueSmallDish(this);
    }
}
