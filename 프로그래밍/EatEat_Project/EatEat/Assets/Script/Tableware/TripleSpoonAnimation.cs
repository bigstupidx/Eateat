using UnityEngine;
using System.Collections;

public class TripleSpoonAnimation : MonoBehaviour
{
    public TripleSpoonAttack _tripleSpoon = null;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DealDamage()
    {
        _tripleSpoon.DealDamage();
    }
}
