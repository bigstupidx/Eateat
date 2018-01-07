using System;
using System.IO;
using System.Xml.Serialization;

using System.Collections;
using System.Collections.Generic;

using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

using UnityEngine;

[Serializable]
public class SaveData
{
    //사용자 정보
    private Dictionary<string, double>  _dicStatus = new Dictionary<string, double>();
    private Dictionary<string, int>     _dicSkillToolLevel = new Dictionary<string, int>();
    private Dictionary<string, int>     _dicPurchasedEquipted = new Dictionary<string, int>();

    public SaveData()
    {
        InitializeData();
    }

    public void Clear()
    {
        _dicStatus.Clear();
        _dicSkillToolLevel.Clear();
        _dicPurchasedEquipted.Clear();
    }

    //기본 초기화 정보, 아래의 정보를 변경하여 Test를 할 수 있다.
    public void InitializeData()
    {
        Clear();

        //캐릭터 Status
        _dicStatus["CAL"] = 0;
        _dicStatus["STAR"] = 100;
        _dicStatus["STG"] = 0;
        _dicStatus["RBT"] = 0;
        _dicStatus["LEV"] = 1;

        //캐릭터 Skill Level 정보
        _dicSkillToolLevel["InstanceSuperFever"] = 0;
        _dicSkillToolLevel["TripleSpoonAttack"] = 0;
        _dicSkillToolLevel["CriticalUp"] = 0;
        _dicSkillToolLevel["DoubleCalorie"] = 0;
        _dicSkillToolLevel["MagicalTableWare"] = 0;
        _dicSkillToolLevel["DoubleDamage"] = 0;

        //도구 Level 정보
        _dicSkillToolLevel["ToolSpoon"] = 0;
        _dicSkillToolLevel["ToolKnife"] = 0;
        _dicSkillToolLevel["ToolChopstick"] = 0;
        _dicSkillToolLevel["ToolFork"] = 0;
        _dicSkillToolLevel["MagicalSpoon"] = 0;
        _dicSkillToolLevel["MagicalKnife"] = 0;
        _dicSkillToolLevel["MagicalChopstick"] = 0;
        _dicSkillToolLevel["MagicalFork"] = 0;

        //도구 구매 & 장착 정보
        //Dictionary에 Key가 존재한다면 구매한 것이다.
        //value가 0이라면 장착하지 않음
        //Value가 1이라면 장착한 상태
        _dicPurchasedEquipted["HelperSpoonItemWood"] = 0;
        _dicPurchasedEquipted["HelperKnifeItemWood"] = 0;
        _dicPurchasedEquipted["HelperChopstickItemWood"] = 0;
        _dicPurchasedEquipted["HelperForkItemWood"] = 0;
        _dicPurchasedEquipted["MagicalSpoonItemWood"] = 0;
        _dicPurchasedEquipted["MagicalKnifeItemWood"] = 0;
        _dicPurchasedEquipted["MagicalChopstickItemWood"] = 0;
        _dicPurchasedEquipted["MagicalForkItemWood"] = 0;

        //코스튬 장착 정보
        //Dictionary에 Key가 존재한다면 구매한 것이다.
        //value가 0이라면 장착하지 않음
        //Value가 1이라면 장착한 상태
        //_dicPurchasedEquipted["SupermanCostume"] = 0;
    }

    public Dictionary<string, double> Status
    {
        get
        {
            return _dicStatus;
        }
    }

    public Dictionary<string, int> Skill
    {
        get
        {
            return _dicSkillToolLevel;
        }
    }

    public Dictionary<string, int> Purchased
    {
        get
        {
            return _dicPurchasedEquipted;
        }
    }
}

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataManager instance = null;

    public string       _name = "EatEat";
    private SaveData    _saveData;

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        _saveData = new SaveData();
    }

    public void Test()
    {
        _saveData.InitializeData();
    }

    public void ResetGame()
    {
        if (!Social.localUser.authenticated)
        {
            Debug.LogError("SaveGame Failed - User Not Logged In");
            return;
        }
        _saveData.InitializeData();
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(_name, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SaveGameData);
    }

    //Save==========================================================================================
    public void SaveGame()
    {
        if (!Social.localUser.authenticated)
        {
            Debug.LogError("SaveGame Failed - User Not Logged In");
            return;
        }
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(_name, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, SaveGameData);
    }

    //OpenWithAutomaticConflictResolution 콜백. 준비된경우 자동으로 호출.
    void SaveGameData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            //저장할데이터바이트배열에 저장하실 데이터의 바이트 배열을 지정.
            byte[] byteData = Serializer.ObjectToByteArraySerialize(_saveData);

            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();

            builder = builder
                .WithUpdatedPlayedTime(DateTime.Now.TimeOfDay)
                .WithUpdatedDescription("Saved game at " + DateTime.Now);

            SavedGameMetadataUpdate updatedMetadata = builder.Build();
            savedGameClient.CommitUpdate(game, updatedMetadata, byteData, CommitResult);

            Debug.Log("SaveGameData - Success");
        }
        else
        {
            Debug.LogError("SaveGameData - Success");
        }
    }

    void CommitResult(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("CommitResult - Success");
        }
        else
        {
            Debug.LogError("CommitResult - Fail");
        }
    }

    //Load==========================================================================================
    public IEnumerator CoRu_LoadGame()
    {
        while (Social.localUser.authenticated == false)
        {
            Debug.LogError("CoRu_LoadGame Failed - User Not Logged In");
            yield return new WaitForSeconds(0.5f);
        }
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(_name, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, LoadGameData);
    }

    public void LoadGame()
    {
        if (Social.localUser.authenticated == false)
        {
            Debug.LogError("LoadGame Failed - User Not Logged In");
            return;
        }
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(_name, DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, LoadGameData);
    }

    void LoadGameData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
            savedGameClient.ReadBinaryData(game, ReadBinaryData);
            Debug.Log("LoadGameData - Success");
        }
        else
        {
            Debug.LogError("LoadGameData - Fail");
        }
    }

    void ReadBinaryData(SavedGameRequestStatus status, byte[] byteData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            Debug.Log("ReadBinaryData - Success");
            _saveData = Serializer.Deserialize<SaveData>(byteData);
        }
        else
        {
            Debug.LogError("ReadBinaryData - Fail");
        }
    }

    public Dictionary<string, double> Status
    {
        get
        {
            return _saveData.Status;
        }
    }

    public Dictionary<string, int> Skill
    {
        get
        {
            return _saveData.Skill;
        }
    }

    public Dictionary<string, int> Purchased
    {
        get
        {
            return _saveData.Purchased;
        }
    }
}
