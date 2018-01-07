using UnityEngine;
using System.Collections;

public class FatEmitter : MonoBehaviour
{
    //지방별 분출 delay 시간
    [SerializeField] private float    _delay = 0.1f;

    //분출할 지방 덩어리 (최소, 최대) 개수
    [SerializeField] private int      _minAmount = 6;
    [SerializeField] private int      _maxAmount = 8;

    //지방 분출
    public void EmitFat(double calorie)
    {
        StartCoroutine(Co_EmitFat(calorie));
    }

    //모든 지방이 한번에 분출되지 않고 개별적으로 분출되도록 함
    private IEnumerator Co_EmitFat(double calorie)
    {
        int count = Random.Range(_minAmount, _maxAmount);
        for (int i=0; i<count ; ++i) {
            Fat fat = ObjectPoolManager.instance.DequeueFat();
            fat.transform.SetParent(transform, true);
            fat.transform.position = transform.position;
            fat.SetCalorie(calorie);
            yield return new WaitForSeconds(_delay);
        }
    }
}

