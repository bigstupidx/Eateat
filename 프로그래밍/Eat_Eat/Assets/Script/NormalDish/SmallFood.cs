using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SmallFood : MonoBehaviour {
    void Start () {
        Initialize();
    }

    private void Initialize()
    {
        SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = FoodResourceManager.instance.GetFood()._foodImage;
    }

    void Update () {
		
	}
}
