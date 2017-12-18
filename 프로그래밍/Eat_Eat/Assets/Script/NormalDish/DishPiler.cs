using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;


public class DishPiler : MonoBehaviour
{
    public static DishPiler instance = null;
    public int          _maxDish = 30;
    public float        _iTweenTime = 2f;

    //Itween Node Position
    public GameObject _iTweenBegin;
    public GameObject[] _iTweenMiddle = new GameObject[4];
    public GameObject[] _iTweenEnd = new GameObject[4];
    private Vector3[][] _dArrVector = new Vector3[4][];

    private List<SmallDish>[] _arrLstDish;

    private bool    _eventFlag;
    private string  _message;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);

        InitGame();
    }

    //Initializes the game for each level.
    void InitGame()
    {
        _arrLstDish = new List<SmallDish>[4];

        for (int i = 0; i < 4; ++i)
        {
            _arrLstDish[i] = new List<SmallDish>();
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

    private void Update()
    {
        //if (_eventFlag) {
        //    _eventFlag = false;

        //    switch (_message) {
        //        case "fever":
        //            PlayerStatus.instance.SetState(EPlayerState.fever);
        //            Handheld.Vibrate();
        //            SoundManager.Instance.PlayEffectSound("fever");
        //            break;
        //    }
        //}
    }

    public void AddDish(SmallDish smallDish)
    {
        smallDish.transform.SetParent(transform, true);

        //Piled Dishes에 삽입되기 전에 Object Size 원래 크기로
        smallDish.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        //Piled Dishes에 삽입되기 전에 애니메이션 제거
        Animator animator = smallDish.GetComponentInChildren<Animator>();
        animator.transform.localScale = new Vector3(1, 1, 1);
        animator.enabled = false;

        smallDish.FoodActivation(false);

        int idx = (int)(smallDish.GetDishColor());
        _arrLstDish[idx].Add(smallDish);
        ItweenDish(smallDish.gameObject, smallDish.GetDishColor());

        //쌓인 접시가 max 개수 이상인 경우 소멸시킨다.
        if (_arrLstDish[idx].Count == _maxDish)
        {
            foreach (SmallDish obj in _arrLstDish[idx])
            {
                Destroy(obj, _iTweenTime);
            }
            _arrLstDish[idx].Clear();

            //End Position 초기화
            _dArrVector[idx][2] = _iTweenEnd[idx].transform.position;

            //접시 데미지
            double fDamage = PlayerStatus.instance.GetDamage();
            MainFoodManager.instance.AddHp(-fDamage * 5);
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
