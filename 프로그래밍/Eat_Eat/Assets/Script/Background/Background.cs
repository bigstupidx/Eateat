using UnityEngine;
using System.Collections;


public class Background : MonoBehaviour
{
    public SpriteRenderer _spriteRenderer = null;
    public float _convertTime = 2;

    private float _timeCounter = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OnOff(bool bFlag)
    {
        if (bFlag)
        {
            StartCoroutine(Co_On());
        }
        else
        {
            StartCoroutine(Co_Off());
        }
    }

    //    private IEnumerator Co_On()
    private IEnumerator Co_On()
    {
        while (true) {
            Color temp = _spriteRenderer.color;

            if (temp.a < 1) {
                temp.a += Time.deltaTime / _convertTime;
                _spriteRenderer.color = temp;
                yield return null;
            }
            else {
                temp.a = 1;
                _spriteRenderer.color = temp;
                break;
            }
        }
    }

    private IEnumerator Co_Off()
    {
        while (true)
        {
            Color temp = _spriteRenderer.color;

            if (temp.a > 0)
            {
                temp.a -= Time.deltaTime / _convertTime;
                _spriteRenderer.color = temp;
                yield return null;
            }
            else
            {
                temp.a = 0;
                _spriteRenderer.color = temp;
                break;
            }
        }
    }
}
