using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

[Serializable]
public class LocalDataContainer
{
    public PlayerDataDB playerData;
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

    public async UniTask<PlayerDataDB> LoadPlayerData()
    {
        if (!PlayerPrefs.HasKey("PlayerData")) return new PlayerDataDB();
        string json = PlayerPrefs.GetString("PlayerData");
        return JsonUtility.FromJson<PlayerDataDB>(json);
    }

    public async UniTask SavePlayerData(PlayerDataDB playerData)
    {
        string json = JsonUtility.ToJson(playerData);
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
            LocalDataContainer container = JsonUtility.FromJson<LocalDataContainer>(userData);
            if (container != null && container.playerLevelData != null)
            {
                return container.playerLevelData;
            }
        }
        catch
        {
            try
            {
                LegacyLocalDataContainer legacyContainer = JsonUtility.FromJson<LegacyLocalDataContainer>(userData);
                if (legacyContainer != null && legacyContainer.playerLevelData != null)
                {
                    LocalDataContainer newContainer = new LocalDataContainer
                    {
                        playerData = ConvertLegacyPlayerData(legacyContainer.playerData),
                        playerLevelData = legacyContainer.playerLevelData
                    };

                   
                    string newData = JsonUtility.ToJson(newContainer);
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

    private PlayerDataDB ConvertLegacyPlayerData(LegacyLocalDataContainer.LegacyPlayerData legacyData)
    {
        if (legacyData == null)
            return new PlayerDataDB();

        return new PlayerDataDB
        {
            profileId = legacyData.profileId,
            playerName = legacyData.playerName,
            IsSoundOn = legacyData.IsSoundOn,
            IsMusicOn = legacyData.IsMusicOn,
            customUserData = legacyData.customUserData
           
        };
    }

    public async UniTask SaveLevelData(PlayerLevelData levelData, string subProfileName)
    {
        string key = GetProfileKey(subProfileName);

        PlayerDataDB playerData = new PlayerDataDB();
        if (PlayerPrefs.HasKey(key))
        {
            string existingData = PlayerPrefs.GetString(key, "");
            if (existingData.Contains(":"))
            {
                try
                {
                    LocalDataContainer existingContainer = JsonUtility.FromJson<LocalDataContainer>(existingData);
                    if (existingContainer != null)
                    {
                        playerData = existingContainer.playerData ?? new PlayerDataDB();
                    }
                }
                catch
                {
                  
                    try
                    {
                        LegacyLocalDataContainer legacyContainer = JsonUtility.FromJson<LegacyLocalDataContainer>(existingData);
                        if (legacyContainer != null)
                        {
                            playerData = ConvertLegacyPlayerData(legacyContainer.playerData);
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
            playerData = playerData,
            playerLevelData = levelData
        };

        string data = JsonUtility.ToJson(container);
        if (!string.IsNullOrEmpty(data))
        {
            PlayerPrefs.SetString(key, data);
        }
    }

    public async UniTask UpdatePlayerName(string playerName)
    {
       
    }

    public async UniTask UpdateLevelCounter(int levelIndex, CounterType counterType, int newValue, string subProfileName)
    {
      
        var levelData = await LoadLevelData(subProfileName);

        if (levelData.levels != null && levelData.levels.ContainsKey(levelIndex))
        {
            var level = levelData.levels[levelIndex];

            switch (counterType)
            {
                case CounterType.Attempts:
                    level.Attempts = newValue;
                    break;
                case CounterType.Success:
                    level.Success = newValue;
                    break;
                case CounterType.Failure:
                    level.Failure = newValue;
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