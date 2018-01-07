using UnityEngine;
using System.Collections;

public class TripleSpoon : MonoBehaviour
{
    public Animator _animator = null;

    public float _maxTime = 2;
    public int _maxAttack = 3;

    private float _time = 0;
    private int _attackCount = 0;


    // Use this for initialization
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    IEnumerator UpdateSpoon()
    {
        while (true) {
            if (0 < _attackCount)
            {
                if (_time <= 0)
                {
                    _animator.Play("Attack");
                    _attackCount--;
                    _time = 2;
                }
            }
            else
            {
                if (_time <= 0) {
                    gameObject.SetActive(false);
                    break;
                }
            }
            _time -= Time.deltaTime;
            yield return null;
        }
    }

    public void Activate() {
        gameObject.SetActive(true);
        _attackCount = _maxAttack;
        _time = 0;
        StartCoroutine(UpdateSpoon());
    }
}
