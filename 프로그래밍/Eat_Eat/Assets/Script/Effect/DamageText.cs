using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DamageText : MonoBehaviour
{
    public float    _maxTime;
    public Text     _damageText;
    private float   _timeCounter;

    // Use this for initialization
    void Start()
    {
        _timeCounter += _maxTime;   
    }

    public void InitDamageText(string str)
    {
        _damageText.text = str;
    }

    // Update is called once per frame
    void Update()
    {
        _timeCounter -= Time.deltaTime;

        Color color = _damageText.color;
        color.a -= Time.deltaTime / _maxTime;
        _damageText.color = color;

        if (_timeCounter < 0)
        {
            Destroy(gameObject);
        }

        transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime;
    }
}
