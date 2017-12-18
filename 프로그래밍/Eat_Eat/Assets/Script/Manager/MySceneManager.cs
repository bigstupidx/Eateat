using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager instance = null;

    void Awake()
    {
        if (instance != null) {
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        InitMySceneManager();
    }

 
    void InitMySceneManager()
    {

    }

    public void ChangeScene(string sceneName) {
        switch (sceneName)
        {
            case "Lobby_Scene":
//              SaveDataManager.instance.SaveGame();
                break;
        }
        SceneManager.LoadScene(sceneName);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
