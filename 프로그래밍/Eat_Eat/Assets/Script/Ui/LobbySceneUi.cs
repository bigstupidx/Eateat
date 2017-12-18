using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySceneUi : MonoBehaviour
{
    public void OnButtonClick(GameObject obj)
    {
        switch (obj.name)
        {
            case "LoginButton":
                GooglePlayManager.instance.Login();
                break;
            case "LogoutButton":
                GooglePlayManager.instance.Logout();
                break;
            case "ShowLeaderboardButton":
                GooglePlayManager.instance.ShowLeaderboard();
                break;
            case "ShowMyLeaderboardButton":
                break;
            case "LoadVideoAdButton":
                GooglePlayManager.instance.LoadVideoAd();
                break;
            case "VideoAdButton":
                GooglePlayManager.instance.ShowVideoAd();
                break;
            case "SaveButton":
                SaveDataManager.instance.SaveGame();
                break;
            case "LoadButton":
                SaveDataManager.instance.LoadGame();
                break;
            case "PlaySceneButton":
                MySceneManager.instance.ChangeScene("Play_Scene");
                break;
        }
    }
}
