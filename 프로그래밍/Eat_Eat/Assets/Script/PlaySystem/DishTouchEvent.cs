using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DishTouchEvent : MonoBehaviour {
    public LayerMask        _layerMask;

    private bool[]          _arrDishFlag;

    void Start () {
        _arrDishFlag = new bool[4];
        _arrDishFlag[(int)EDishColor.red] = true;
        _arrDishFlag[(int)EDishColor.blue] = true;
        _arrDishFlag[(int)EDishColor.yellow] = true;
        _arrDishFlag[(int)EDishColor.green] = true;
    }

    float _timer = 0;
    bool _flag = true;

    private void Update()
    {

    }

    public void MouseEvent(ref List<GameObject> lstHit)
    {
        if (Input.touchCount != 0)
        {
            return;
        }

        if (UiManager.instance.TouchDisable)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;
            Vector3 theTouch = new Vector3(pos.x, pos.y, 0.0f);

            Ray ray = Camera.main.ScreenPointToRay(theTouch);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
            {
                lstHit.Add(hit.transform.gameObject);
            }
        }
    }

    public void TouchEvent(ref List<GameObject> lstHit)
    {
        if (Input.touchCount == 0) { return; }
        int nTouch = Input.touchCount;

        for (int i = 0; i < 1; ++i)
        {
            //UI Object 뒤에 있는 Object를 선택하지 않도록 하기 위함.
            //Store 같은 Ui가 able 되어 있는 경우에만 다음 코드를 수행
            //if (EventSystem.current.IsPointerOverGameObject(i) == true) { break; }

            if (UiManager.instance.TouchDisable) {
                break;
            }

            Vector3 vPos = new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0);
            Ray ray = Camera.main.ScreenPointToRay(vPos);

            if (Input.GetTouch(i).phase == TouchPhase.Began)
            {
                //_flag = false;
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
                {
                    lstHit.Add(hit.transform.gameObject);
                    break;
                }
            }
        }
    }

    //public void TouchEvent(ref List<GameObject> lstHit)
    //{
    //    if (Input.touchCount == 0) { return; }
    //    int nTouch = Input.touchCount;

    //    for (int i = 0; i < 1; ++i)
    //    {
    //        //UI 뒤에 있는 Object를 선택하지 않도록 하기 위함.
    //        if (EventSystem.current.IsPointerOverGameObject(i) == true) { break; }

    //        Vector3 vPos = new Vector3(Input.GetTouch(i).position.x, Input.GetTouch(i).position.y, 0);
    //        Ray ray = Camera.main.ScreenPointToRay(vPos);

    //        if (Input.GetTouch(i).phase == TouchPhase.Began)
    //        {
    //            RaycastHit hit;
    //            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
    //            {
    //                SmallDish smallDish = hit.transform.parent.GetComponentInChildren<SmallDish>();

    //                if (_arrDishFlag[(int)smallDish.GetDishColor()] == false)
    //                {
    //                    continue;
    //                }
    //                _arrDishFlag[(int)smallDish.GetDishColor()] = false;
    //                lstHit.Add(hit.transform.gameObject);
    //                break;
    //            }
    //        }
    //        else if (Input.GetTouch(i).phase == TouchPhase.Ended)
    //        {
    //            RaycastHit hit;
    //            if (Physics.Raycast(ray, out hit, Mathf.Infinity, _layerMask))
    //            {

    //                SmallDish smallDish = hit.transform.parent.GetComponentInChildren<SmallDish>();
    //                _arrDishFlag[(int)smallDish.GetDishColor()] = true;
    //            }
    //        }
    //    }
    //}
}



