using UnityEngine;
using System.Collections;

public enum EFatState
{
    Emitted,
    Wait,
    Move,
}

public class Fat : MonoBehaviour
{
//    public BoxCollider2D _landArea;
    public float    _gravityPower = 9.8f;
    public Vector3  _targetPosition;
    public float    _power;
    public Sprite[] _arrSprite;
    public SpriteRenderer _spriteRenderer;

    private EFatState   _eFatState = EFatState.Emitted;
    private float       _waitTime;
    private float       _timeCounter;
    private double       _calorie = 0;
    private Vector3     _vecVelocity;

    // Use this for initialization
    void Start()
    {
        _spriteRenderer.sprite = _arrSprite[Random.Range(0, 3)];

        _vecVelocity = new Vector3(Random.Range(-1.5f, 1.5f), Random.Range(4f, 6f), 0);
        _vecVelocity.Normalize();
        _vecVelocity *= Random.Range(5f, 7f);

        _waitTime = Random.Range(0, 0.4f);
        Vector3 localScale = transform.localScale;
        localScale *= Random.Range(1f, 1.7f);
        transform.localScale = localScale;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_eFatState) {
            //초기 생성
            case EFatState.Emitted:
                if (transform.position.y > -0.1)
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

                    //_vecVelocity = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), 0);
                    //_vecVelocity.Normalize();
                    //_vecVelocity *= Random.Range(1f, 3f);
                }
                break;
            //특정 위치로 이동
            case EFatState.Move:
                transform.position += _vecVelocity * Time.deltaTime;
                _vecVelocity += _power * (_targetPosition - transform.position).normalized * Time.deltaTime;
                _power += _power * Time.deltaTime;

                if ((_targetPosition - transform.position).magnitude < 0.3)
                {
                    PlayerStatus.instance.AddCalorie(_calorie);
                    Destroy(gameObject);
                }
                break;
        }
    }

    public void SetCalorie(double calorie)
    {
        _calorie = calorie;
    }
}
