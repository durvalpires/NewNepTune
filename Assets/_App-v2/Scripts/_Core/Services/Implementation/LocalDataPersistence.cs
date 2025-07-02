using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Serialization;
using Newtonsoft.Json;

[Serializable]
public class LocalDataContainer
{
    [FormerlySerializedAs("playerData")] public AccountDataDB accountData;
    public PlayerLevelData playerLevelData;
}


[Serializable]
public class LegacyLocalDataContainer
{
    
    public LegacyPlayerData playerData;
    public PlayerLevelData playerLevelData;

    [Serializable]
    public class LegacyPlayerData
    {
        public string profileId = "";
        public string playerName = "";
        public bool IsSoundOn = true;
        public bool IsMusicOn = true;
        public string customUserData = "";
        public float totalPlayTime = 0f;
    }
}

public class LocalDataPersistence : IDataPersistence
{
    public bool IsConnected => true;

    private string GetProfileKey(string subProfileName)
    {
        string profileKey = ProfilesController.CurrentProfileKey;
        return $"{profileKey}_{subProfileName}_PlayerData";
    }

    public async UniTask<AccountDataDB> LoadPlayerData()
    {
        if (!PlayerPrefs.HasKey("PlayerData")) return new AccountDataDB();
        string json = PlayerPrefs.GetString("PlayerData");
        return JsonConvert.DeserializeObject<AccountDataDB>(json);
    }

    public async UniTask SavePlayerData(AccountDataDB accountData)
    {
        string json = JsonConvert.SerializeObject(accountData);
        PlayerPrefs.SetString("PlayerData", json);
        PlayerPrefs.Save();
    }

    public async UniTask<PlayerLevelData> LoadLevelData(string subProfileName)
    {
        string key = GetProfileKey(subProfileName);

        if (!PlayerPrefs.HasKey(key))
        {
            return new PlayerLevelData { SubProfilename = subProfileName };
        }

        string userData = PlayerPrefs.GetString(key, "");
        if (string.IsNullOrEmpty(userData) || !userData.Contains(":"))
        {
            return new PlayerLevelData { SubProfilename = subProfileName };
        }

        try
        {
            LocalDataContainer container = JsonConvert.DeserializeObject<LocalDataContainer>(userData);
            if (container != null && container.playerLevelData != null)
            {
                return container.playerLevelData;
            }
        }
        catch
        {
            try
            {
                LegacyLocalDataContainer legacyContainer = JsonConvert.DeserializeObject<LegacyLocalDataContainer>(userData);
                if (legacyContainer != null && legacyContainer.playerLevelData != null)
                {
                    LocalDataContainer newContainer = new LocalDataContainer
                    {
                        accountData = ConvertLegacyPlayerData(legacyContainer.playerData),
                        playerLevelData = legacyContainer.playerLevelData
                    };

                   
                    string newData = JsonConvert.SerializeObject(newContainer);
                    if (!string.IsNullOrEmpty(newData))
                    {
                        PlayerPrefs.SetString(key, newData);
                        Debug.Log($"Migrated player data format for profile {subProfileName}");
                    }

                    return legacyContainer.playerLevelData;
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error migrating legacy player data: {ex.Message}");
            }
        }

        return new PlayerLevelData { SubProfilename = subProfileName };
    }

    private AccountDataDB ConvertLegacyPlayerData(LegacyLocalDataContainer.LegacyPlayerData legacyData)
    {
        if (legacyData == null)
            return new AccountDataDB();

        return new AccountDataDB
        {
            profileId = legacyData.profileId,
            playerName = legacyData.playerName,
            IsSoundOn = legacyData.IsSoundOn,
            IsMusicOn = legacyData.IsMusicOn,
            totalPlayTime = legacyData.totalPlayTime,
            //customUserData = legacyData.customUserData
        };
    }

    public async UniTask SaveLevelData(PlayerLevelData levelData, string subProfileName)
    {
        string key = GetProfileKey(subProfileName);

        AccountDataDB accountData = new AccountDataDB();
        if (PlayerPrefs.HasKey(key))
        {
            string existingData = PlayerPrefs.GetString(key, "");
            if (existingData.Contains(":"))
            {
                try
                {
                    LocalDataContainer existingContainer = JsonConvert.DeserializeObject<LocalDataContainer>(existingData);
                    if (existingContainer != null)
                    {
                        accountData = existingContainer.accountData ?? new AccountDataDB();
                    }
                }
                catch
                {
                  
                    try
                    {
                        LegacyLocalDataContainer legacyContainer = JsonConvert.DeserializeObject<LegacyLocalDataContainer>(existingData);
                        if (legacyContainer != null)
                        {
                            accountData = ConvertLegacyPlayerData(legacyContainer.playerData);
                        }
                    }
                    catch
                    {
                        Debug.LogWarning("Could not read existing player data, using defaults");
                    }
                }
            }
        }

        LocalDataContainer container = new LocalDataContainer
        {
            accountData = accountData,
            playerLevelData = levelData
        };
        
        string data = JsonConvert.SerializeObject(container);
        if (!string.IsNullOrEmpty(data))
        {
            PlayerPrefs.SetString(key, data);
        }
    }

    public async UniTask UpdatePlayerName(string playerName)
    {
       
    }

    public async UniTask UpdateLevelCounter(int worldIndex, int levelIndex,CounterType counterType, int newValue,string subProfileName)
    {
      
        var levelData = await LoadLevelData(subProfileName);
        string key = LevelKeyUtil.LevelKey(worldIndex, levelIndex);
        if (levelData.levels != null && levelData.levels.ContainsKey(key))
        {
            var level = levelData.levels[key];

            switch (counterType)
            {
                case CounterType.Attempts:
                    level.Attempts = newValue;
                    break;
                case CounterType.Success:
                    level.Successes = newValue;
                    break;
                case CounterType.Failure:
                    level.Fails = newValue;
                    break;
            }

            await SaveLevelData(levelData, subProfileName);
        }
    }

    public async UniTask CreateSubProfile(string subProfileName, PlayerLevelData initialData)
    {
        await SaveLevelData(initialData, subProfileName);
    }

    public async UniTask<List<string>> GetAllSubProfiles()
    {
        return new List<string>();
    }
}