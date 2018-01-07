using UnityEngine;
using System.Collections;

public enum EFatState
{
    Emitted,
    Wait,
    Move,
}

//Fat Emitter에서 방출되는 Fat 객체
public class Fat : MonoBehaviour, IPoolObject
{
    public float    _gravityPower = 9.8f;
    public Vector3  _targetPosition;
    public float    _power;
    private float    _curPower;
    public Sprite[] _arrSprite;
    public SpriteRenderer _spriteRenderer;
    public float _floorHeight;

    private EFatState   _eFatState = EFatState.Emitted;
    private float       _waitTime;
    private float       _timeCounter;
    private double       _calorie = 0;
    private Vector3     _vecVelocity;

    // Use this for initialization
    public void Initialize()
    {

    }

    public void OutFromQueue()
    {
        gameObject.SetActive(true);
        _eFatState = EFatState.Emitted;

        _spriteRenderer.sprite = _arrSprite[Random.Range(0, 3)];

        _vecVelocity = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(4f, 6f), 0);
        _vecVelocity.Normalize();
        _vecVelocity *= Random.Range(5f, 7f);

        _waitTime = Random.Range(0, 0.4f);
        transform.localScale = new Vector3(0.3f, 0.3f, 0.3f) * Random.Range(1f, 1.7f);
        _curPower = _power;

        _timeCounter = 0;
    }

    public void ReturnToQueue()
    {
        gameObject.SetActive(false);
        ObjectPoolManager.instance.Enqueue(this);
    }

    void Update()
    {
        switch (_eFatState) {
            //초기 생성
            case EFatState.Emitted:
                if (transform.position.y > _floorHeight)
                {
                    transform.position += _vecVelocity * Time.deltaTime;
                    _vecVelocity -= new Vector3(0, 1, 0) * _gravityPower * Time.deltaTime;
                }
                else
                {
                    _eFatState = EFatState.Wait;
                }
                break;
            //바닥에서 대기
            case EFatState.Wait:
                _timeCounter += Time.deltaTime;
                if (_timeCounter >= _waitTime)
                {
                    _eFatState = EFatState.Move;
                    _vecVelocity = (_targetPosition - transform.position).normalized * Time.deltaTime;
                }
                break;
            //특정 위치로 이동
            case EFatState.Move:
                transform.position += _vecVelocity * Time.deltaTime;
                _vecVelocity += _curPower * (_targetPosition - transform.position).normalized * Time.deltaTime;
                _curPower += _curPower * Time.deltaTime;

                //Object Pool로 복귀
                if ((_targetPosition - transform.position).magnitude < 1)
                {
                    PlayerStatus.instance.AddCalorie(_calorie);
                    ReturnToQueue();
                }
                break;
        }
    }

    public void SetCalorie(double calorie)
    {
        _calorie = calorie;
    }
}
