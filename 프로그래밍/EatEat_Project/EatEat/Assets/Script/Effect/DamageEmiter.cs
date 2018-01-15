using UnityEngine;
using UnityEngine.UI;
using System.Collections;

//Damage Text을 화면에 뿌리는 클래스
public class DamageEmiter : MonoBehaviour
{
    public static DamageEmiter  instance = null;

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    //double로 정보를 받아서 화면에 출력
    public void EmitDamageText(double damage, bool critical)
    {
        DamageText damageText = ObjectPoolManager.instance.DequeueDamageText();
        damageText.transform.SetParent(transform, false);
        damageText.transform.position = transform.position;
        damageText.transform.localScale = transform.localScale;
        //Func.NumToABC는 수치를 ABC 단계로 만들어주는 함수
        damageText.SetDamageText(GlobalFunction.Func.NumToABC(damage), critical);
    }

    //Text로 정보를 받아서 화면에 출력
    public void EmitDamageText(string str, bool critical)
    {
        DamageText damageText = ObjectPoolManager.instance.DequeueDamageText();
        damageText.transform.SetParent(transform, false);
        damageText.transform.position = transform.position;
        damageText.transform.localScale = transform.localScale;
        damageText.SetDamageText(str, critical);
    }
}
