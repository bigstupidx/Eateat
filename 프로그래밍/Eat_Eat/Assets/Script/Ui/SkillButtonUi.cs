using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using GlobalFunction;

public class SkillButtonUi : MonoBehaviour
{
    public Skill    _skill;
    private Image   _image;

	// Use this for initialization
	void Start () {
        _skill = GetComponentInChildren<Skill>();
        _image = Func.GetChildComponent<Image>(gameObject, "SkillButton");
    }

    // Update is called once per frame
    void Update () {

    }

    public void OnButtonClick()
    {
        _skill.OnButtonClick();
    }
}
