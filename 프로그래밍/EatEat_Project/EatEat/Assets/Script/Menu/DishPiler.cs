using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

//캐릭터 뒤에 쌓이는 접시를 관리
public class DishPiler : MonoBehaviour
{
    public static DishPiler instance = null;
    public int          _maxDish = 30;
    public float        _iTweenTime = 2f;

    [SerializeField] private float _damageIncrease = 5;

    //Itween Node Position
    public GameObject _iTweenBegin;
    public GameObject[] _iTweenMiddle = new GameObject[4];
    public GameObject[] _iTweenEnd = new GameObject[4];
    private Vector3[][] _dArrVector = new Vector3[4][];

    private int[]               _arrDishCount = new int[4];
    private List<SmallMenu>[]   _arrLstDish = new List<SmallMenu>[4];

    private bool    _eventFlag;
    private string  _message;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        for (int i = 0; i < 4; ++i)
        {
            _arrDishCount[i] = 0;
            _arrLstDish[i] = new List<SmallMenu>();
        }

        //Itween Position 삽입
        for (int i = 0; i < 4; ++i)
        {
            _dArrVector[i] = new Vector3[3];
            _dArrVector[i][0] = _iTweenBegin.transform.position + new Vector3(0, 0.5f, 0.5f);
            _dArrVector[i][1] = _iTweenMiddle[i].transform.position;
            _dArrVector[i][2] = _iTweenEnd[i].transform.position;
        }
    }

    private IEnumerator DishMove(SmallMenu smallDish, float time)
    {
        yield return new WaitForSeconds(time);

        int idx = (int)(smallDish.GetDishColor());
        _arrLstDish[idx].Add(smallDish);

        //쌓인 접시가 max 개수 이상인 경우 소멸시킨다.
        if (_arrLstDish[idx].Count == _maxDish)
        {
            foreach (SmallMenu smallMenu in _arrLstDish[idx])
            {
                iTween.Stop(smallMenu.gameObject);
                smallMenu.ReturnToQueue();
            }
            _arrLstDish[idx].Clear();

            //접시 데미지
            double fDamage = PlayerStatus.instance.GetDamage();
            MainFoodManager.instance.AddHp(-fDamage * _damageIncrease);
        }
    }

    //먹은 접시 옆에 쌓기
    public void AddDish(SmallMenu smallDish)
    {
        ItweenDish(smallDish.gameObject, smallDish.GetDishColor());
        smallDish.transform.SetParent(transform, true);

        //Piled Dishes에 삽입되기 전에 Object Size 원래 크기로
        smallDish.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        smallDish.AnimationActivation(false);
        smallDish.FoodActivation(false);

        //접시가 End Position에 시각적으로 도달했을 때 queue에 추가되도록 처리
        StartCoroutine(DishMove(smallDish, _iTweenTime));

        int idx = (int)(smallDish.GetDishColor());
        _arrDishCount[idx]++;

        //End Position 갱신
        if (_arrDishCount[idx] == _maxDish)
        {
            _arrDishCount[idx] = 0;
            _dArrVector[idx][2] = _iTweenEnd[idx].transform.position;
        }
        else
        {
            _dArrVector[idx][2].z -= 0.01f;
            _dArrVector[idx][2].y += 0.1f;
        }
    }

    void ItweenDish(GameObject obj, EDishColor dishColor)
    {
        Vector3[] temp = new Vector3[3] { _dArrVector[(int)dishColor][0], _dArrVector[(int)dishColor][1], _dArrVector[(int)dishColor][2] };
        iTween.MoveTo(obj, iTween.Hash("path", temp, "time", _iTweenTime));
    }
}
