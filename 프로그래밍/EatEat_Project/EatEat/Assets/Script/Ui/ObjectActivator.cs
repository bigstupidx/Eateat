using UnityEngine;
using System.Collections;

public class ObjectActivator : MonoBehaviour
{
    public GameObject   _uiObj;
    public GameObject[] _arrUiObject;

    //버튼 클릭시 
    public void OnButtonClick()
    {
        if (_uiObj.active)
        {
            _uiObj.SetActive(false);
            UiManager.instance.TouchDisable = false;
        }
        else
        {
            //Store Ui 활성화 시에 업데이트
            StoreManager.instance.CheckLevelUpAble();

            foreach (GameObject uiObj in _arrUiObject) {
                uiObj.SetActive(false);
            }

            _uiObj.SetActive(true);
            UiManager.instance.TouchDisable = true;
        }
    }
}
