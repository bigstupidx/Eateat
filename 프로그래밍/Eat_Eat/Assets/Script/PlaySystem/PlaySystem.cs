using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySystem : MonoBehaviour
{
    //Inspector Input Object
    public GameObject       _playerCharacter;
    public GameObject       _effectPrefab;

    //Player Info
    private PlayerStatus    _playerStatus;

    //Food Conveyor
    private FoodConveyor    _foodConvoyer;

    //Dish Touch Event
    private DishTouchEvent  _disTouchEvent;

    // Use this for initialization
    void Start () {

#if UNITY_EDITOR
        if (SaveDataManager.instance)
        {
            SaveDataManager.instance.Test();
        }
#endif

        //Play Scene을 위한 SingleTon 객체 초기화
        PlayerStatus.instance.InitPlayerStatus();
        StageSystem.instance.InitStageSystem();
        SkillManager.instance.InitSkillManager();
        TableWareManager.instance.InitTableWareManager();

        StoreManager.instance.Initialize();
        StoreManager.instance.LoadData();

        //화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        _playerStatus = PlayerStatus.instance;
        _foodConvoyer = GetComponentInChildren<FoodConveyor>();
        _disTouchEvent = GetComponentInChildren<DishTouchEvent>();
    }
	
	// Update is called once per frame
	void Update () {
        List<GameObject> lstTouch = new List<GameObject>();
        _disTouchEvent.MouseEvent(ref lstTouch);
        _disTouchEvent.TouchEvent(ref lstTouch);
        if (lstTouch.Count == 0) { return; }

        foreach (GameObject obj in lstTouch)
        {
            Eat(obj);
            break;
            ////노말 State일 때에는 1개의 Touch Input이 사용된다.
            //if (_playerStatus.GetState() == EPlayerState.normal) {
            //    break;
            //}
        }
    }

    void Eat(GameObject dish)
    {
        SmallDish smallDish = dish.transform.parent.GetComponentInChildren<SmallDish>();
        if (smallDish == null) { return; }

        //눌려진 접시와 첫번째 접시가 같은 색이라면.
        if (_foodConvoyer.GetFirstDishColor() == smallDish.GetDishColor() || _playerStatus.GetState() == EPlayerState.fever || _playerStatus.GetState() == EPlayerState.super_fever)
        {
            //GameObject effectDish = Instantiate(_effectPrefab, dish.transform.position, Quaternion.identity);
            //SmallDish effectSmallDish = effectDish.GetComponentInChildren<SmallDish>();
            //effectSmallDish.Initialize();
            //effectSmallDish.SetDishColor(smallDish.GetDishColor());
            //Destroy(effectDish, 1);

            DishPopEffect effectSmallDish = ObjectPoolManager.instance.DequeueSmallDish();
            effectSmallDish.SetDishColor(smallDish.GetDishColor());
            effectSmallDish.transform.position = dish.transform.position;
            effectSmallDish.DestroyOnTime(1);



            //콤보 증가
            ComboSystem.instance.AddCombo();

            //스몰 푸드 칼로리 추가
            double smallDishCalorie = MainFoodManager.instance.GetCalorie(ECalorieType.Small);
            PlayerStatus.instance.AddCalorie(smallDishCalorie);

            //스몰 접시 섭취 사운드
            SoundManager.Instance.PlayEffectSound("eat_sound");

            _foodConvoyer.DishPushPop();

            //음식에 데미지 추가
            _playerStatus.DealDamage(smallDish.GetDishColor());
        }
        //틀렸을 때.
        else
        {
            ComboSystem.instance.ResetCombo();
            MainFoodManager.instance.HpRecovery();
            _playerStatus.DealFail();
            Handheld.Vibrate();
        }
    }
}
