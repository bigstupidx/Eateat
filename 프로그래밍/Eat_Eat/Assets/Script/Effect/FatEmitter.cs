using UnityEngine;
using System.Collections;

public class FatEmitter : MonoBehaviour
{
    public GameObject _fatPrefab = null;
    public float    _duration = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    public void EmitFat(double calorie)
    {
        StartCoroutine(Co_EmitFat(calorie));
    }

    private IEnumerator Co_EmitFat(double calorie)
    {
        int count = Random.Range(6, 8);
        for (int i=0; i<count ; ++i) {
            Vector3 position = transform.position;
            position.z = 16.5f;
            GameObject fatObj = Instantiate(_fatPrefab, position, Quaternion.identity);
            fatObj.transform.SetParent(transform, true);
            Fat fat = fatObj.GetComponent<Fat>();
            fat.SetCalorie(calorie);
            yield return new WaitForSeconds(_duration);
        }
    }
}

