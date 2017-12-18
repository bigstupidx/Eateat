using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ComboSystem : MonoBehaviour
{
    public static ComboSystem instance = null;

    //전체 콤보 기록
    private int         _nCombo = 0;

    //Combo Ui
    private Animator    _animator;
    private Text        _comboText;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        InitGame();
    }

    private void InitGame()
    {
        _animator = UiManager.instance.GetChildComponent<Animator>("ComboText");
        _comboText = UiManager.instance.GetChildComponent<Text>("ComboText");
        _comboText.text = "";
    }
	
	// Update is called once per frame
	void Update () {

    }

    public void PlayAnimation()
    {
        _animator.Play("Combo", -1, 0);
    }

    public void AddCombo()
    {
        _nCombo++;
        _comboText.text = _nCombo.ToString() + " Combo";
        PlayAnimation();

        //피버 게이지 증가
        FeverSystem.instance.AddFeverGauge();
    }
    public void ResetCombo()
    {
        _nCombo = 0;
        _comboText.text = "";

        //피버 게이지 리셋
        FeverSystem.instance.ResetFeverGauge();
    }
}

