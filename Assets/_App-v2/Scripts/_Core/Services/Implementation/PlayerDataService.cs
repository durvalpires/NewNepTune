using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
#if !UNITY_WEBGL
using Firebase.Auth;
#endif

public class PlayerDataService : IPlayerDataService
{
    private readonly IDataPersistence _dataPersistence;

    public PlayerDataDB PlayerData { get; private set; }

    public PlayerDataService(IDataPersistence dataPersistence)
    {
        _dataPersistence = dataPersistence;
        PlayerData = new PlayerDataDB();
    }

    public async UniTask LoadPlayerData()
    {
        PlayerData = await _dataPersistence.LoadPlayerData();
    }

    public async UniTask SavePlayerData()
    {
        await _dataPersistence.SavePlayerData(PlayerData);
    }

    public void UpdatePlayerNameInFirebase()
    {
#if !UNITY_WEBGL
        FirebaseUser currentUser = FirebaseAuth.DefaultInstance.CurrentUser;
        if (currentUser != null)
        {
            string nameToSet = string.IsNullOrEmpty(currentUser.DisplayName) ? "Default Name" : currentUser.DisplayName;
            _dataPersistence.UpdatePlayerName(nameToSet).Forget();
        }
#endif
    }

    public string GetCustomData(string key, string defaultValue = "")
    {
        var dataDict = GetAllCustomData();
        if (dataDict.ContainsKey(key))
            return dataDict[key].ToString();
        return defaultValue;
    }

    public void SetCustomData(string key, string value)
    {
        var dataDict = GetAllCustomData();
        if (dataDict.ContainsKey(key))
            dataDict[key] = value;
        else
            dataDict.Add(key, value);

        PlayerData.customUserData = Json.Serialize(dataDict);
        SavePlayerData().Forget();
    }

    public Dictionary<string, object> GetAllCustomData()
    {
        if (string.IsNullOrEmpty(PlayerData.customUserData))
            return new Dictionary<string, object>();

        var dataDictFromJson = Json.Deserialize(PlayerData.customUserData) as Dictionary<string, object>;
        return dataDictFromJson ?? new Dictionary<string, object>();
    }
}
