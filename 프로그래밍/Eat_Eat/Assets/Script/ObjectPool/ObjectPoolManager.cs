using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class IPoolObject : MonoBehaviour
{
    public abstract void Initialzie();

    public void SetQueue()
    {

    }

    //public void DestroyOnTime(float time)
    //{
    //    StartCoroutine(Timer(time));
    //}

    //private IEnumerator Timer(float time)
    //{
    //    yield return new WaitForSeconds(time);
    //    ObjectPoolManager.instance.EnqueueSmallDish(this);
    //}

    protected void SetPosition(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.identity;
    }
}

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;

    [SerializeField]
    private int                         _nDishEffect;
    [SerializeField]
    private DishPopEffect               _pfabDishEffect;
    private static Queue<DishPopEffect> _queDishEffect = new Queue<DishPopEffect>();

    void Awake()
    {
        if (null != instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        Initialize();
    }

    void Initialize()
    {
        for (int i=0; i< _nDishEffect; ++i) {
            DishPopEffect smallDish = Instantiate(_pfabDishEffect);
            smallDish.transform.SetParent(transform, true);
            smallDish.Initialize();
            smallDish.gameObject.SetActive(false);
            _queDishEffect.Enqueue(smallDish);
        }
    }

    public void EnqueueSmallDish(DishPopEffect smallDish)
    {
        smallDish.gameObject.SetActive(false);
        smallDish.transform.SetParent(transform, true);
        _queDishEffect.Enqueue(smallDish);
    }
    public DishPopEffect DequeueSmallDish()
    {
        if (0 < _queDishEffect.Count)
        {
            DishPopEffect smallDish = _queDishEffect.Dequeue();
            smallDish.gameObject.SetActive(true);
            Animator animator = smallDish.GetComponentInChildren<Animator>();
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.fullPathHash, 0);
            return smallDish;
        }
        else
        {
            DishPopEffect smallDish = Instantiate(_pfabDishEffect);
            smallDish.transform.SetParent(transform, true);
            smallDish.Initialize();
            return smallDish;
        }
    }

}
