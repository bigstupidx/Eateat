using UnityEngine;
using System.Collections;

public class DamageEmiter : MonoBehaviour
{
    public static DamageEmiter instance = null;
    public GameObject[] _arrDamageTextPrefab;

    //public static DamageEmiter Instance
    //{
    //    get
    //    {
    //        if (instance == null)
    //        {
    //            instance = new DamageEmiter();
    //        }
    //        return instance;
    //    }
    //}

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EmitDamageText(double damage, bool critical)
    {
        if (critical)
        {
            GameObject obj = Instantiate(_arrDamageTextPrefab[1], transform);
            DamageText damageText = obj.GetComponent<DamageText>();
            damageText.InitDamageText(GlobalFunction.Func.NumToABC(damage));
        }
        else
        {
            GameObject obj = Instantiate(_arrDamageTextPrefab[0], transform);
            DamageText damageText = obj.GetComponent<DamageText>();
            damageText.InitDamageText(GlobalFunction.Func.NumToABC(damage));
        }
    }

    public void EmitDamageText(string str, bool critical)
    {
        if (critical)
        {
            GameObject obj = Instantiate(_arrDamageTextPrefab[1], transform);
            DamageText damageText = obj.GetComponent<DamageText>();
            damageText.InitDamageText(str);
        }
        else
        {
            GameObject obj = Instantiate(_arrDamageTextPrefab[0], transform);
            DamageText damageText = obj.GetComponent<DamageText>();
            damageText.InitDamageText(str);
        }
    }
}
