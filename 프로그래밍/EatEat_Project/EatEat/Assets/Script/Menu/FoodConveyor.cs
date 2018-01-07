using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

using GlobalFunction;

public class FoodConveyor : MonoBehaviour {
    public static FoodConveyor  instance = null;

    private Queue<SmallMenu>     _queDish;
    private Vector3             _vDishDistance;
    private Vector3             _vBegin;
    private Vector3             _vEnd;
    private Vector3             _vDirection;

    public int                  _nFood;
    public GameObject           _conveyorBeginPosition;
    public GameObject           _conveyorEndPosition;
    public Vector3              _minScale;
    public Vector3              _maxScale;

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
    }

    public void InitFoodConveyor()
    {
        _queDish = new Queue<SmallMenu>();
        _vBegin = _conveyorBeginPosition.transform.position;
        _vEnd = _conveyorEndPosition.transform.position;

        _vDishDistance = _vEnd - _vBegin;
        _vDishDistance /= _nFood - 1;
        _vDirection = _vDishDistance.normalized;

        _dishSpeed = _minDishSpeed;

        CreateDisheList();
    }

    private SmallMenu CreateNewDish(Vector3 vPosition)
    {
        SmallMenu smallDish = ObjectPoolManager.instance.DequeueSmallMenu();
        smallDish.transform.SetParent(transform, true);
        smallDish.transform.position = vPosition;
        smallDish.SetDishColor((EDishColor)Random.Range((int)EDishColor.red, (int)EDishColor.green+1));
        smallDish.FoodActivation(true);
        smallDish.AnimationActivation(false);
        return smallDish;
    }

    void CreateDisheList()
    {
        Vector3 vTemp = _vEnd;

        for (int i = 0; i < _nFood; ++i)
        {
            _queDish.Enqueue(CreateNewDish(vTemp));
            vTemp -= _vDishDistance;

        }
        ProjectionScaling();

        //Animator Enable
        _queDish.First().AnimationActivation(true);
    }

	// Update is called once per frame
	void Update () {
        //첫번째 접시의 위치가 마지막 지점이 아닐 경우에만 이동 Update.
        if (_queDish.First().transform.position.y < _vEnd.y)
        {
            Conveying();
            ProjectionScaling();
        }
    }

    //접시 이동
    void Conveying()
    {
        Vector3 vTemp = _vEnd - _queDish.First().transform.position;
        float fFinalDistance = Mathf.Min(_dishSpeed * Time.deltaTime, vTemp.magnitude);

        foreach (SmallMenu dish in _queDish)
        {
            dish.transform.position += (_vDirection * fFinalDistance);
        }
    }

    //접시가 카메라에서 멀어질 수록 작아보이는 효과.
    void ProjectionScaling()
    {
        int idx = 1;
        foreach (SmallMenu dish in _queDish)
        {
            float fPercentage = (float)idx / _nFood;
            dish.transform.localScale = Vector3.Lerp(_minScale, _maxScale, fPercentage);
            idx++;
        }
    }

    //앞의 접시 삭제, 뒤에 접시 추가
    public void DishPushPop()
    {
        //Dequeue 후 Dish Piler에 접시 삽입
        SmallMenu smallMenu = _queDish.Dequeue();
        DishPiler.instance.AddDish(smallMenu);

        Vector3 vPos = _queDish.Last().transform.position - _vDishDistance;
        if (vPos.y > 0)
        {
            //SmallMenu m = _queDish.Last();
            Debug.Log(_queDish.Count);
        }

        _queDish.Enqueue(CreateNewDish(vPos));

        //Animator Enable
        _queDish.First().AnimationActivation(true);
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
        SmallMenu smallDish = _queDish.First().GetComponentInChildren<SmallMenu>();
        return smallDish.GetDishColor();
    }
}