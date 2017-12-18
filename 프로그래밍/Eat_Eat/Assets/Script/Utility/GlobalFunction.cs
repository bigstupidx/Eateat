using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//전역 함수 정의
namespace GlobalFunction{
    public class Func
    {
        //IEnumerator waitThenCallback(float time, Action callback)
        public static IEnumerator LoopCallback(float backOff, Action callback)
        {
            while (Social.localUser.image == null)
            {
                yield return new WaitForSeconds(backOff);
            }
            //yield return new WaitForSeconds(time);
            callback();
        }


        public static string NumToABC(double fData)
        {
            //1,000         = A 4
            //1,000,000     = B 7
            //1,000,000,000 = C 10
            //...

            int nFigures = (int)Math.Log10(fData) + 1;
            nFigures = nFigures - ((nFigures - 1) % 3);

            switch (nFigures) {
                default:
                    if (31 < nFigures)
                    {
                        string data = string.Format("{0:F0}J", fData / 1000000000000000000000000000000.0f);
                        data += "+E" + (nFigures-31);
                        return data;
                    }
                    else
                    {
                        return fData.ToString("N2");
                    }
                case 4:
                    return string.Format("{0:F2}A", fData / 1000);
                case 7:
                    return string.Format("{0:F2}B", fData / 1000000);
                case 10:
                    return string.Format("{0:F2}C", fData / 1000000000);
                case 13:
                    return string.Format("{0:F2}D", fData / 1000000000000);
                case 16:
                    return string.Format("{0:F2}E", fData / 1000000000000000);
                case 19:
                    return string.Format("{0:F2}F", fData / 1000000000000000000);
                case 22:
                    return string.Format("{0:F2}G", fData / 1000000000000000000000.0f);
                case 25:
                    return string.Format("{0:F2}H", fData / 1000000000000000000000000.0f);
                case 28:
                    return string.Format("{0:F2}I", fData / 1000000000000000000000000000.0f);
                case 31:
                    return string.Format("{0:F2}J", fData / 1000000000000000000000000000000.0f);
            }
        }

        public static T GetChildComponent<T>(GameObject obj, string objectName) where T:Component
        {

            T[] arrComp = obj.GetComponentsInChildren<T>();
            foreach (T comp in arrComp)
            {
                if (comp.transform.name == objectName)
                {
                    return comp;
                }
            }

            return null;
        }

        public static GameObject GetChildObject(GameObject obj, string objectName)
        {
            foreach (Transform trans in obj.transform) {
                if (trans.name == objectName)
                {
                    return trans.gameObject;
                }
            }
            return null;
        }

    }
}
