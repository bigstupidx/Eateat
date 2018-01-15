using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IPoolObject
{
    //Object Poo Manager에서 단 한번씩만 수행되는 초기화 과정
    void Initialize();
    //Object Pool에서 나올때 선행되는 과정
    void OutFromQueue();
    //Object Pool에 도로 반환될 때 선행되는 과정
    void ReturnToQueue();
}

//Generic 한 구조로 구현하지는 못하고, Object Pooling 기반을 잡는 정도로만 구현이 되어있는 상태이다. 1235123
public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager instance = null;

    //Dish Pop Effect
    [SerializeField]
    private int                  _nDishEffect;
    [SerializeField]
    private DishPopEffect        _pfabDishEffect;
    private Queue<DishPopEffect> _queDishEffect = new Queue<DishPopEffect>();

    //Small Menu
    [SerializeField]
    private int              _nSmallMenu;
    [SerializeField]
    private SmallMenu        _pfabSmallMenu;
    private Queue<SmallMenu> _queSmallMenu = new Queue<SmallMenu>();

    //Fat
    [SerializeField]
    private int         _nFat;
    [SerializeField]
    private Fat         _pfabFat;
    private Queue<Fat>  _queFat = new Queue<Fat>();

    //Normal Damage Text
    [SerializeField]
    private int _nDamageText;
    [SerializeField]
    private DamageText _pfabDamageText;
    private Queue<DamageText> _queDamageText = new Queue<DamageText>();

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
            DishPopEffect effect = Instantiate(_pfabDishEffect, transform);
            effect.Initialize();
            effect.ReturnToQueue();
        }
        for (int i = 0; i < _nSmallMenu; ++i)
        {
            SmallMenu menu = Instantiate(_pfabSmallMenu, transform);
            menu.Initialize();
            menu.ReturnToQueue();
        }
        for (int i = 0; i < _nFat; ++i)
        {
            Fat fat = Instantiate(_pfabFat, transform);
            fat.Initialize();
            fat.ReturnToQueue();
        }
        for (int i = 0; i < _nDamageText; ++i)
        {
            DamageText damageText = Instantiate(_pfabDamageText, transform);
            damageText.Initialize();
            damageText.ReturnToQueue();
        }
    }

    public void Enqueue(DishPopEffect effect)
    {
        effect.transform.SetParent(transform, true);
        effect.gameObject.SetActive(false);
        _queDishEffect.Enqueue(effect);
    }

    public void Enqueue(SmallMenu smallMenu)
    {
        smallMenu.transform.SetParent(transform, true);
        smallMenu.gameObject.SetActive(false);
        _queSmallMenu.Enqueue(smallMenu);
    }

    public void Enqueue(Fat fat)
    {
        fat.transform.SetParent(transform, true);
        fat.gameObject.SetActive(false);
        _queFat.Enqueue(fat);
    }

    public void Enqueue(DamageText damageText)
    {
        damageText.transform.SetParent(transform, true);
        damageText.gameObject.SetActive(false);
        _queDamageText.Enqueue(damageText);
    }

    //======================================================================================
    public DishPopEffect DequeueDishEffect()
    {
        DishPopEffect effect;

        //Pool에 여유가 있다면 꺼내서 반환
        if (0 < _queDishEffect.Count)
        {
            effect = _queDishEffect.Dequeue();
        }
        //Pool에 여유가 없다면 새로 생성
        else
        {
            effect = Instantiate(_pfabDishEffect);
            effect.transform.SetParent(transform, true);
            effect.Initialize();
        }
        effect.OutFromQueue();
        return effect;
    }

    public SmallMenu DequeueSmallMenu()
    {
        SmallMenu effect;

        //Pool에 여유가 있다면 꺼내서 반환
        if (0 < _queSmallMenu.Count)
        {
            effect = _queSmallMenu.Dequeue();
        }
        //Pool에 여유가 없다면 새로 생성
        else
        {
            effect = Instantiate(_pfabSmallMenu);
            effect.transform.SetParent(transform, true);
            effect.Initialize();
        }
        effect.OutFromQueue();
        return effect;
    }

    public Fat DequeueFat()
    {
        Fat fat;

        //Pool에 여유가 있다면 꺼내서 반환
        if (0 < _queFat.Count)
        {
            fat = _queFat.Dequeue();
        }
        //Pool에 여유가 없다면 새로 생성
        else
        {
            fat = Instantiate(_pfabFat);
            fat.transform.SetParent(transform, true);
            fat.Initialize();
        }
        fat.OutFromQueue();
        return fat;
    }

    public DamageText DequeueDamageText()
    {
        DamageText damageText;

        //Pool에 여유가 있다면 꺼내서 반환
        if (0 < _queDamageText.Count)
        {
            damageText = _queDamageText.Dequeue();
        }
        //Pool에 여유가 없다면 새로 생성
        else
        {
            damageText = Instantiate(_pfabDamageText);
            damageText.transform.SetParent(transform, true);
            damageText.Initialize();
        }
        damageText.OutFromQueue();
        return damageText;
    }
}
