using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using GlobalFunction;

public class FoodConveyor : MonoBehaviour {
    public static FoodConveyor  instance = null;

    private List<SmallDish>     _lstDish;
    private Vector3             _vDishDistance;
    private Vector3             _vBegin;
    private Vector3             _vEnd;
    private Vector3             _vDirection;

    public SmallDish            _smallDishPrefab;
    public int                  _nFood;
    public GameObject           _conveyorBeginPosition;
    public GameObject           _conveyorEndPosition;
    public Vector3              _vMinScale;
    public Vector3              _MaxScale;

    public float                _minDishSpeed;
    public float                _maxDishSpeed;
    private float               _dishSpeed;

    // Use this for initialization
    void Awake () {
        if (instance != null)
        {
            return;
        }
        instance = this;

        InitFoodConveyor();
    }

    void InitFoodConveyor()
    {
        _lstDish = new List<SmallDish>();
        _vBegin = _conveyorBeginPosition.transform.position;
        _vEnd = _conveyorEndPosition.transform.position;

        _vDishDistance = _vEnd - _vBegin;
        _vDishDistance /= _nFood - 1;
        _vDirection = _vDishDistance.normalized;

        _dishSpeed = _minDishSpeed;

        CreateDisheList();
    }

    private SmallDish CreateNewDish(Vector3 vPosition)
    {
        SmallDish smallDish = Instantiate(_smallDishPrefab, vPosition, Quaternion.identity);
        smallDish.transform.SetParent(transform, true);
        smallDish.Initialize();
        smallDish.SetDishColor((EDishColor)Random.Range((int)EDishColor.red, (int)EDishColor.green+1));
        return smallDish;
    }

    void CreateDisheList()
    {
        Vector3 vTemp = _vEnd;

        for (int i = 0; i < _nFood; ++i)
        {
            //Dish의 색깔 개수
            Random.Range(0, 4);
            _lstDish.Add(CreateNewDish(vTemp));
            vTemp -= _vDishDistance;

        }

        ProjectionScaling();

        //Small Dish Animation Begin
        Animator animator = _lstDish.First().GetComponentInChildren<Animator>();
        animator.enabled = true;
    }

	// Update is called once per frame
	void Update () {
        //첫번째 접시의 위치가 마지막 지점이 아닐 경우에만 이동 Update.
        if (_lstDish.First().transform.position.y < _vEnd.y)
        {
            Conveying();
            ProjectionScaling();
        }
        //FirstQueueDishAnimation();
    }

    //접시 이동
    void Conveying()
    {
        Vector3 vTemp = _vEnd - _lstDish.First().transform.position;
        float fFinalDistance = Mathf.Min(_dishSpeed * Time.deltaTime, vTemp.magnitude);

        foreach (SmallDish dish in _lstDish)
        {
            dish.transform.position += (_vDirection * fFinalDistance);
        }
    }

    //접시가 카메라에서 멀어질 수록 작아보이는 효과.
    void ProjectionScaling()
    {
        int idx = 1;
        foreach (SmallDish dish in _lstDish)
        {
            float fPercentage = (float)idx / _nFood;
            dish.transform.localScale = Vector3.Lerp(_vMinScale, _MaxScale, fPercentage);
            idx++;
        }
    }

    //앞의 접시 삭제, 뒤에 접시 추가
    public void DishPushPop()
    {
        //Dish Piler에 접시 삽입
        DishPiler.instance.AddDish(_lstDish.First());

        _lstDish.RemoveAt(0);

        Vector3 vPos = _lstDish.Last().transform.position - _vDishDistance;
        _lstDish.Add(CreateNewDish(vPos));

        //Small Dish Animation Begin
        Animator animator = _lstDish.First().GetComponentInChildren<Animator>();
        animator.enabled = true;
    }

    public void SetDishSpeed(EDishSpeed eSpeed)
    {
        switch (eSpeed) {
            case EDishSpeed.min:
                _dishSpeed = _minDishSpeed;
                break;
            case EDishSpeed.max:
                _dishSpeed = _maxDishSpeed;
                break;
        }
    }

    public EDishColor GetFirstDishColor()
    {
        SmallDish smallDish = _lstDish.First().GetComponentInChildren<SmallDish>();
        return smallDish.GetDishColor();
    }
}