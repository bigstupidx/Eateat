using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Damage Emiter에서 방출되는 객체
public class DamageText : MonoBehaviour, IPoolObject
{
    public float    _maxTime;
    private float   _timeCounter;
    private Text    _damageText;

    public void Initialize()
    {
        _damageText = GetComponent<Text>();
    }

    //Object Pool에서 나올때 선행되는 과정
    public void OutFromQueue()
    {
        gameObject.SetActive(true);
        _timeCounter = _maxTime;   
    }

    //Object Pool에 도로 반환될 때 선행되는 과정
    public void ReturnToQueue()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.instance.Enqueue(this);
    }

    //시간 흐름에 따라, 불투명도를 올리고 소멸
    void Update()
    {
        _timeCounter -= Time.deltaTime;
        Color color = _damageText.color;
        color.a -= Time.deltaTime / _maxTime;
        _damageText.color = color;

        if (_timeCounter < 0)
        {
            ReturnToQueue();
        }

        transform.position += new Vector3(0, 0.5f, 0) * Time.deltaTime;
    }

    //크리티컬 여부에 따라 다르게 출력
    public void SetDamageText(string str, bool bCritical)
    {
        _damageText.text = str;

        if (false == bCritical)
        {
            _damageText.color = new Color(1, 1, 1, 1);
            _damageText.fontSize = 14;
        }
        else{
            _damageText.color = new Color(1, 0, 0, 1);
            _damageText.fontSize = 30;
        }
    }
}
