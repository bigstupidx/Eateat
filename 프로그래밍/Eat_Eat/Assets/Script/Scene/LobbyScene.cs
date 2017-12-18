using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GooglePlayManager.instance.Login();
        //StartCoroutine(SaveDataManager.instance.CoRu_LoadGame());
        GooglePlayManager.instance.SetPlayerProfile();
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
