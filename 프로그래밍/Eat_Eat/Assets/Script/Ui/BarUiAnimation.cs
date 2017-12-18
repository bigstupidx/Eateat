using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarUiAnimation : MonoBehaviour {
    public float _animationSpeed = 1;

    private Image _image;
    private float _newPercentage;

    // Use this for initialization
    void Awake () {
        _image = GetComponent<Image>();
        _newPercentage = _image.fillAmount;
    }

    // Update is called once per frame
    void Update () {
        if (_newPercentage != _image.fillAmount) {
            if (_newPercentage < _image.fillAmount)
            {
                _image.fillAmount -= _animationSpeed * Time.deltaTime;
                if (_image.fillAmount < _newPercentage)
                {
                    _image.fillAmount = _newPercentage;
                }
            }
            else
            {
                _image.fillAmount += _animationSpeed * Time.deltaTime;
                if (_image.fillAmount > _newPercentage)
                {
                    _image.fillAmount = _newPercentage;
                }
            }
        }
	}

    public void SetPercentage(float percentage)
    {
        _newPercentage = percentage;
    }

    public void SetPercentage(double percentage)
    {
        _newPercentage = (float)percentage;
    }

    public void SetAnimationSpeed(float speed)
    {
        _animationSpeed = speed;
    }

    public void SetBeginPercentage(float percentage)
    {
        _image.fillAmount = percentage;
    }
}
