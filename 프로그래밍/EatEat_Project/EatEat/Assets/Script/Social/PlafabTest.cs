using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using PlayFab;
using PlayFab.ClientModels;

public class PlafabTest : MonoBehaviour
{
    public static PlafabTest instance = null;

    void Awake()
    {
        if (instance == null)
            instance = this;

        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitPlayfab();
    }

    void InitPlayfab()
    {
        ////초기값은 144였음
        //PlayFabSettings.TitleId = "144"; // Please change this value to your own titleId from PlayFab Game Manager

        ////PC버전
        ////var request = new LoginWithCustomIDRequest { CustomId = "GettingStartedGuide", CreateAccount = true };
        ////PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);

        ////Android 버전
        //var request = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = "GettingStartedGuide", CreateAccount = true };
        //PlayFabClientAPI.LoginWithAndroidDeviceID(request, OnLoginSuccess, OnLoginFailure);
        ////PlayFabClientAPI.LoginWithGoogleAccount();

        ////IOS 버전
        ////PlayFabClientAPI.LoginWithIOSDeviceID(request, OnLoginSuccess, OnLoginFailure);

        ////Facebook 버전
        ////PlayFabClientAPI.LoginWithFacebook(request, OnLoginSuccess, OnLoginFailure);
    }

    public void SignInPlayfab()
    {
        //PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
        //{
        //    TitleId = PlayFabSettings.TitleId,
        //    //ServerAuthCode = serverAu
        //});
        
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Congratulations, you made your first successful API call!");
    }

    private void OnLoginFailure(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with your first API call.  :(");
        Debug.LogError("Here's some debug information:");
        Debug.LogError(error.GenerateErrorReport());
    }

    private void OnPlayFabError(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}

//string _playfabId;
//void PlayfabLogin()
//{
//    string authCode = PlayGamesPlatform.Instance.GetServerAuthCode();

//#if UNITY_ANDROID
//    PlayFabClientAPI.LoginWithGoogleAccount(new LoginWithGoogleAccountRequest()
//    {
//        TitleId = PlayFabSettings.TitleId,
//        ServerAuthCode = authCode,
//        CreateAccount = true,
//    }, (successLoginResult) =>
//    {
//        Debug.Log("---------------------------------------------------------");
//        Debug.Log("Login Playfab With Google Success");
//        Debug.Log("Server Auth Code : " + authCode);
//        Debug.Log("Playfab ID : " + successLoginResult.PlayFabId);
//        _playfabId = successLoginResult.PlayFabId;
//        Debug.Log("Title ID : " + PlayFabSettings.TitleId);
//        Debug.Log("---------------------------------------------------------");
//    }, (playFabError) =>
//    {
//        Debug.LogError(playFabError.GenerateErrorReport());
//    });
//#elif UNITY_IOS
//        //Bla Bla Bla
//#endif
//}
