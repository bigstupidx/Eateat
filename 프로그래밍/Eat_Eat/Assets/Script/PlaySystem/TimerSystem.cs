using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerSystem : MonoBehaviour
{
    public static TimerSystem instance = null;

    public float    _normalMaxTime = 15;
    public float    _bossMaxTime = 30;

    //_timeMaxd에는 normalMaxTime or bossMaxTime 둘중 하나가 삽입된다.
    private float   _timeMax = 0;

    private float   _timeCounter = 0;
    private Text    _timerText;
    private Image   _timerBar;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        InitTimerSystem();
    }

    void InitTimerSystem()
    {
        ResetTimer();
        _timerText = UiManager.instance.GetChildComponent<Text>("Time Group");
        _timerText.text = _timeCounter.ToString();

        _timerBar = UiManager.instance.GetChildComponent<Image>("TimeBar");
    }

    // Update is called once per frame
    void Update () {
        _timeCounter -= Time.deltaTime;

        //시간 제한을 넘긴 경우
        if (_timeCounter < 0) {
            ResetTimer();
            MainFoodManager.instance.CreateMainFood();
        }

        _timerText.text = string.Format("{0:00.00}", _timeCounter);
        _timerBar.fillAmount = _timeCounter / _timeMax;
    }

    public void ResetTimer()
    {
        if (false == StageSystem.instance.IsBossStage())
        {
            _timeMax = _normalMaxTime;
            _timeCounter = _normalMaxTime;
        }
        else
        {
            _timeMax = _bossMaxTime;
            _timeCounter = _bossMaxTime;
        }
    }
}
