using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySystem : MonoBehaviour
{
    //Player Info
    private PlayerStatus    _playerStatus;

    //Food Conveyor
    private FoodConveyor    _foodConvoyer;

    //Dish Touch Event
    private DishTouchEvent  _disTouchEvent;

    // Use this for initialization
    void Start ()
    {
        //유니티 에디터에서 실행시 Test를 위하여 SaveData의 기본 데이터를 Load하도록 한다
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
        FoodConveyor.instance.InitFoodConveyor();

        //상점 항목 초기화 및 Data Load
        StoreManager.instance.Initialize();
        StoreManager.instance.LoadData();

        _playerStatus = PlayerStatus.instance;
        _foodConvoyer = GetComponentInChildren<FoodConveyor>();
        _disTouchEvent = GetComponentInChildren<DishTouchEvent>();

        //화면 꺼짐 방지
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
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
        }
    }

    //작은 접시 섭취 활동
    void Eat(GameObject dish)
    {
        SmallMenu smallDish = dish.transform.parent.GetComponentInChildren<SmallMenu>();
        if (smallDish == null) { return; }

        //눌려진 접시와 첫번째 접시가 같은 색이라면.
        if (_foodConvoyer.GetFirstDishColor() == smallDish.GetDishColor() || _playerStatus.GetState() == EPlayerState.fever || _playerStatus.GetState() == EPlayerState.super_fever)
        {
            //Dish Pop Effect
            DishPopEffect effectSmallDish = ObjectPoolManager.instance.DequeueDishEffect();
            effectSmallDish.SetDishColor(smallDish.GetDishColor());
            effectSmallDish.transform.position = dish.transform.position;

            //콤보 증가
            ComboSystem.instance.AddCombo();

            //스몰 푸드 칼로리 추가
            double smallDishCalorie = MainFoodManager.instance.GetCalorie(ECalorieType.Small);
            PlayerStatus.instance.AddCalorie(smallDishCalorie);

            //스몰 접시 섭취 사운드
            SoundManager.Instance.PlayEffectSound("eat_sound");

            //컨베이어에서 음식 제거 후 Dish Piler에 추가
            _foodConvoyer.DishPushPop();

            //음식에 데미지 추가
            _playerStatus.DealDamage(smallDish.GetDishColor());
        }
        //틀렸을 때.
        else
        {
            //콤보 0으로 초기화
            ComboSystem.instance.ResetCombo();
            //Main 음식 체력 회복
            MainFoodManager.instance.HpRecovery();
            //캐릭터 실패 애니메이션 재생
            _playerStatus.DealFail();
            //하드웨어 진동
            Handheld.Vibrate();
        }
    }
}
