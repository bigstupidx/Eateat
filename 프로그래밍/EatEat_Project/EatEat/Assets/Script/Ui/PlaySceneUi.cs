using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySceneUi : MonoBehaviour
{
    public void OnButtonClick(GameObject obj)
    {
        switch (obj.name) {
            case "SaveButton":
                StageSystem.instance.SaveData();
                StoreManager.instance.SaveData();

                //SaveDataManager.SaveGame은 반드시 맨 아래에 위치해야 한다.
                SaveDataManager.instance.SaveGame();
                break;
            case "LobbySceneButton":
                MySceneManager.instance.ChangeScene("Lobby_Scene");
                break;
        }
    }
}
