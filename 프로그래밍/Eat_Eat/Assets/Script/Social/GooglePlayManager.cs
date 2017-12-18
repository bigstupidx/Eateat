using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

using GoogleMobileAds.Api;

using GlobalFunction;

public class GooglePlayManager : MonoBehaviour
{
    public static GooglePlayManager instance = null;

    Sprite          _profileSprite = null;

    public string   _saveFileName = "EatEat";

    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        InitGooglePlay();
//        InitAd();
//      ShowBannerAd();
    }

    void InitGooglePlay()
    {
#if UNITY_ANDROID
        print("UNITY_ANDROID");
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration.Builder()
            .EnableSavedGames()
            //.AddOauthScope("profile")
            //.RequestServerAuthCode(false)
            //.RequestIdToken()
            .RequestEmail()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();
        PlayGamesPlatform.DebugLogEnabled = true;
#elif UNITY_IOS
        GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
#endif
    }

    public void Login()
    {
        if (Social.localUser.authenticated)
        {
            print("You have Logged in already");
            return;
        }

        Social.localUser.Authenticate((bool success) =>
        {
            if (success)
            {
                Debug.Log("Login Successed");
                SetPlayerProfile();
            }
            else
            {
                Debug.LogError("Login Failed");
            }
        });
    }

    public void Logout()
    {
        if (false == Social.localUser.authenticated)
        {
            print("You are not logged in");
            return;
        }

        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("Log out");

        Text profileText = UiManager.instance.GetChildComponent<Text>("ProfileText");
        profileText.text = "Profile";
        Image profileImage = UiManager.instance.GetChildComponent<Image>("ProfileImage");
        profileImage.sprite = null;
        _profileSprite = null;
    }

    public void SetPlayerProfile() {
        if (!Social.localUser.authenticated)
        {
            print("You are Not Logged In");
            return;
        }

        string str = ((PlayGamesLocalUser)Social.localUser).Email + "\n" + Social.localUser.userName + "\n" + Social.localUser.id;
        Text profileText = UiManager.instance.GetChildComponent<Text>("ProfileText");
        profileText.text = str;

        //Get ProfileImage
        if (_profileSprite == null)
        {
            StartCoroutine(
                Func.LoopCallback(0.5f,
                () =>
                {
                    Image profileImage = UiManager.instance.GetChildComponent<Image>("ProfileImage");
                    _profileSprite = Sprite.Create(Social.localUser.image, new Rect(0, 0, Social.localUser.image.width, Social.localUser.image.height), new Vector2(0.5f, 0.5f));
                    profileImage.sprite = _profileSprite;
                }
            ));
        }
        else
        {
            Image profileImage = UiManager.instance.GetChildComponent<Image>("ProfileImage");
            profileImage.sprite = _profileSprite;
        }
    }

    public void ShowLeaderboard()
    {
        string leaderboardId = "CgkIqd3e4PUSEAIQCA";

        Social.ReportScore(100, leaderboardId, (bool send) =>
        {
            if (true == send)
            {
                Debug.Log("Score Uploaded Success");
            }
            else
            {
                Debug.Log("Score Uploaded Fail");
            }
        });
        Social.ShowLeaderboardUI();

        //Social.Active.ReportScore(((long)ScorePageToken, GPGSIds.achievement_1, ));
        //Social.Active.ShowLeaderboardUI();

        //Show My LeaderBoard
        //((PlayGamesPlatform)Social.Active).ShowLeaderboardUI(leaderboardId);
    }

    public void OnButtonClick(GameObject obj)
    {
        switch (obj.name) {
            case "InAppPurchaseButton":
                InAppPurchaser.instance.BuyProductID("item_01");
                break;
            case "LoadVideoAdButton":
                LoadVideoAd();
                break;
            case "VideoAdButton":
                ShowVideoAd();
                break;
        }
    }

#region Advertisement
    private RewardBasedVideoAd _rewardBasedVideoAd;

    private const string    _bannerAdId = "ca-app-pub-3940256099942544/6300978111";
    private const string    _videoAdId = "ca-app-pub-6979901493210969/2610604331";
    private const string    _deviceId = "2077ef9a63d2b398840261c8221a0c9b";

    private BannerView      _bannerAd;
    private InterstitialAd  _videoAd;

    private void InitAd()
    {
        _bannerAd = new BannerView(_bannerAdId, AdSize.SmartBanner, AdPosition.Bottom);
        
        _videoAd = new InterstitialAd(_bannerAdId);

        _rewardBasedVideoAd = RewardBasedVideoAd.Instance;
    }

    public void ShowBannerAd()
    {
        string adID = _bannerAdId;
        AdRequest request = new AdRequest.Builder()
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
            .Build();

        BannerView bannerAd = new BannerView(adID, AdSize.SmartBanner, AdPosition.Bottom);
        bannerAd.LoadAd(request);
        //bannerAd.Hide();

        AdRequest adRequset = new AdRequest.Builder()
            .AddKeyword("game")
            .AddTestDevice(AdRequest.TestDeviceSimulator)
            .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
            .Build();
        _bannerAd.LoadAd(adRequset);
        _bannerAd.Show();
    }

    public void LoadVideoAd()
    {
#if UNITY_EDITOR
        string adUnitId = "unused";
#elif UNITY_ANDROID
        string adUnitId = "ca-app-pub-3940256099942544/7325402514";
#endif

        AdRequest adRequset = new AdRequest.Builder()
            .AddKeyword("game")
        //    .AddTestDevice(AdRequest.TestDeviceSimulator)
        //    .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
            .Build();
        _rewardBasedVideoAd.LoadAd(adRequset, adUnitId);

        //_rewardBasedVideoAd.LoadAd(new AdRequest.Builder().Build(), adUnitId);
    }

    public void ShowVideoAd()
    {
        if (_rewardBasedVideoAd.IsLoaded())
        {
            Debug.Log("Loaded");
            _rewardBasedVideoAd.Show();
        }
        else
        {
            Debug.Log("NotLoaded Yet");
        }

        //AdRequest adRequset = new AdRequest.Builder()
        //    .AddKeyword("game")
        //    .AddTestDevice(AdRequest.TestDeviceSimulator)
        //    .AddTestDevice("2077ef9a63d2b398840261c8221a0c9b")
        //    .Build();
        //_videoAd.LoadAd(adRequset);
        //_videoAd.Show();
    }

    public void HandleOnAdRewarded(object sender, Reward args)
    {
        Debug.Log("Rewarded!!!!!!!! " + args.Amount + " | " + args.Type);
    }
#endregion

    public void StoreItemList()
    {
        //GetStoreItemsRequest storeItemsRequest = new GetStoreItemsRequest{
        //    StoreId = "Store_01",
        //};
        //PlayFabClientAPI.GetStoreItems(storeItemsRequest,
        //    (storeItemResult)=> 
        //    {
        //        Debug.Log("Store Item---------------------(" + storeItemResult.Store.Count + ")");
        //        foreach (StoreItem storeItem in storeItemResult.Store) {
        //            Debug.Log("\t Item ID : " + storeItem.ItemId);
        //            Debug.Log("\t Item Price : " + storeItem.VirtualCurrencyPrices["RM"] + " RM");
        //        }
        //        Debug.Log("-------------------------------");
        //    },
        //    (playFabError)=>
        //    {
        //        Debug.LogError(playFabError.GenerateErrorReport());
        //    }
        //);
    }

    public void PurchaseItem()
    {
        //List<ItemPurchaseRequest> lstItemPurchaseReq = new List<ItemPurchaseRequest>();
        //lstItemPurchaseReq.Add(new ItemPurchaseRequest
        //    {
        //        Annotation = "My-Purchase",
        //        ItemId = "One",
        //        Quantity = 1,
        //        //UpgradeFromItems
        //    }
        //);

        //StartPurchaseRequest startPurchaseReq = new StartPurchaseRequest
        //{
        //    Items = lstItemPurchaseReq,
        //    StoreId = "One"
        //};
        //PlayFabClientAPI.StartPurchase(
        //    startPurchaseReq,
        //    (startPurchaseResult)=>
        //    {
        //        Debug.Log("Start Purchase Success--------------------------------");
        //        Debug.Log("\t Order ID : " + startPurchaseResult.OrderId);
        //        Debug.Log("------------------------------------------------------");
        //        //startPurchaseResult.
        //    },
        //    (playFabError) =>
        //    {
        //        Debug.LogError(playFabError.GenerateErrorReport());
        //    }
        //);



        //PurchaseItemRequest purchaseItemReq = new PurchaseItemRequest
        //{
        //    //CatalogVersion <- Default
        //    //CharacterId = _playfabId,
        //    StoreId = "Store_01",
        //    ItemId = "One",
        //    Price = 1000,
        //    VirtualCurrency = "RM",
        //};

        //PlayFabClientAPI.PurchaseItem(purchaseItemReq, 
        //    (purchaseItemResult)=> 
        //    {
        //        Debug.Log("Prchase Success-------------------");
        //        Debug.Log(purchaseItemResult.CustomData);
        //        Debug.Log(purchaseItemResult.Items);
        //        Debug.Log(purchaseItemResult.Request);
        //        Debug.Log("----------------------------------");
        //    }, 
        //    (playFabError)=> 
        //    {
        //        Debug.LogError(playFabError.GenerateErrorReport());
        //    }
        //);
    }
}
