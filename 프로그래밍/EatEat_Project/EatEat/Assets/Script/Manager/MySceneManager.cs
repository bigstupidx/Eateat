using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MySceneManager : MonoBehaviour
{
    public static MySceneManager instance = null;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        InitMySceneManager();
    }

    void InitMySceneManager()
    {

    }

    //Scene 변경
    public void ChangeScene(string sceneName) {
        switch (sceneName)
        {
            case "Lobby_Scene":
                break;
        }
        SceneManager.LoadScene(sceneName);
    }
}
