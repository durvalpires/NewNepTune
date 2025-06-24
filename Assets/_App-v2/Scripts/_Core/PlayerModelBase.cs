using System;
using System.Collections.Generic;
using _App_v2.Scripts._Core.Firebase;
#if UNITY_WEBGL
using _App_v2.Scripts._Core.Firebase.Config;
#endif
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerModelBase
{
    protected static IPlayerDataService _playerDataService;
    protected static ILevelDataService _levelDataService;
    protected static IDataPersistence _dataPersistence;
    protected static string _currentSubProfileName = "Default";

    public static AccountDataDB Data => _playerDataService?.AccountData;
    public static PlayerLevelData LevelData => _levelDataService?.LevelData;
    public static ILevelDataService LevelDataService => _levelDataService;

    static PlayerModelBase()
    {
        InitializeServices();
    }

    private static void InitializeServices()
    {
        InitializePersistenceLayer();
        _playerDataService = new PlayerDataService(_dataPersistence);
        _levelDataService = new LevelDataService(_dataPersistence);
    }

    private static void InitializePersistenceLayer()
    {
        
        
#if UNITY_WEBGL
        // var config = FirebaseConfigService.Instance.CurrentConfig;
        // var emulatorConfig = FirebaseConfigService.Instance.CurrentEmulatorConfig;
        // _dataPersistence = new FirebaseWebGLDataPersistence(config, emulatorConfig);
        _dataPersistence = new LocalDataPersistence();
#elif !UNITY_WEBGL
        var fire = FireService.Instance;
        if (fire.DB.IsConnected)
        {
            _dataPersistence = new FirebaseDataPersistence();
        }
        else
        {
            _dataPersistence = new LocalDataPersistence();
        }
#else
        _dataPersistence = new LocalDataPersistence();
#endif
    }

    public static void UpdateUserId(string userId)
    {
        #if UNITY_WEBGL
        if (_dataPersistence is FirebaseWebGLDataPersistence webGLPersistence)
        {
            webGLPersistence.SetUserId(userId);
        }
        #endif
    }

    public static void SwitchToCurrentProfile()
    {
        SaveData();
        InitializeServices();
    }

    public static void SaveData()
    {
        if (_playerDataService != null)
            _playerDataService.SavePlayerData().Forget();

        if (_levelDataService != null)
            _levelDataService.SaveLevelData().Forget();
    }

    public static void ClearData()
    {
        InitializeServices();

        string profileKey = ProfilesController.CurrentProfileKey;
        string fullKey = $"{profileKey}_{_currentSubProfileName}_PlayerData";
        PlayerPrefs.DeleteKey(fullKey);
    }

    public static async UniTask LoadData()
    {
        if (_playerDataService == null || _levelDataService == null)
            InitializeServices();

        await _playerDataService.LoadPlayerData();
        await _levelDataService.LoadLevelData(_currentSubProfileName);
    }

    public static UniTask Initialize()
    {
        return UniTask.WhenAll(
            _playerDataService.LoadPlayerData(),
            _levelDataService.LoadLevelData(_currentSubProfileName)
        );
    }

    #region Player Data Methods
    public static string GetPlayerName()
    {
        return _playerDataService?.AccountData?.playerName;
    }

    public static void SetPlayerName(string name)
    {
        if (_playerDataService?.AccountData != null)
        {
            _playerDataService.AccountData.playerName = name;
            UpdatePlayerNameInFirebase();
        }
    }

    public static void UpdatePlayerNameInFirebase()
    {
        _playerDataService?.UpdatePlayerNameInFirebase();
    }

    public static bool GetSoundState()
    {
        return _playerDataService?.AccountData?.IsSoundOn ?? true;
    }

    public static void SetSoundState(bool state)
    {
        if (_playerDataService?.AccountData != null)
        {
            _playerDataService.AccountData.IsSoundOn = state;
            _playerDataService.SavePlayerData().Forget();
        }
    }

    public static bool GetMusicState()
    {
        return _playerDataService?.AccountData?.IsMusicOn ?? true;
    }

    public static void SetMusicState(bool state)
    {
        if (_playerDataService?.AccountData != null)
        {
            _playerDataService.AccountData.IsMusicOn = state;
            _playerDataService.SavePlayerData().Forget();
        }
    }

    public static string GetCustomData(string key, string defaultValue = "")
    {
        if (_levelDataService == null || _levelDataService.LevelData == null || 
            String.IsNullOrEmpty(_levelDataService.LevelData.customLevelData))
                LoadData().Forget();

        return _levelDataService?.GetCustomData(key, defaultValue) ?? defaultValue;
    }

    public static void SetCustomData(string key, string value)
    {
        _levelDataService?.SetCustomData(key, value);
        if (_levelDataService != null)
            _levelDataService.SaveLevelData().Forget();
    }

    public static Dictionary<string, object> GetAllCustomData()
    {
        return _levelDataService?.GetAllCustomData() ?? new Dictionary<string, object>();
    }
    #endregion

    #region Level Data Methods
    public static int GetNumberOfLevel()
    {
        return _levelDataService?.GetNumberOfLevel() ?? 0;
    }

    public static int GetNumberOfAttempts()
    {
        return _levelDataService?.GetNumberOfAttempts() ?? 0;
    }

    public static int GetAverageScore()
    {
        return _levelDataService?.GetAverageScore() ?? 0;
    }

    public static Dictionary<int, (int Score, int Repetition)> GetAllPlayerLevelData()
    {
        return _levelDataService?.GetAllPlayerLevelData() ?? new Dictionary<int, (int, int)>();
    }

    public static void PrintAllPlayerLevelData()
    {
        _levelDataService?.PrintAllPlayerLevelData();
    }

    public static void SetLevelUnlocked(int levelIndex)
    {
        _levelDataService?.SetLevelUnlock(levelIndex);
        if (_levelDataService != null)
            _levelDataService.SaveLevelData().Forget();
    }

    public static void SetLevelUnlock(int levelIndex)
    {
        SetLevelUnlocked(levelIndex);
    }

    public static void SetLevelCompleted(int levelIndex)
    {
        _levelDataService?.SetLevelCompleted(levelIndex);
        if (_levelDataService != null)
            _levelDataService.SaveLevelData().Forget();
    }
    

    public static void UpdateCounter(int levelIndex, CounterType counterType)
    {
        _levelDataService?.UpdateCounter(levelIndex, counterType);
        if (_levelDataService != null)
            _levelDataService.SaveLevelData().Forget();
    }

    public static void SetCustomScore(int customScore)
    {
        _levelDataService?.SetCustomScore(customScore);
        if (_levelDataService != null)
            _levelDataService.SaveLevelData().Forget();
    }

    public static async UniTask CreateSubProfile(string subProfileName)
    {
        if (_levelDataService != null)
            await _levelDataService.CreateSubProfile(subProfileName);
    }

    public static async UniTask<List<string>> GetAllSubProfiles()
    {
        return _levelDataService != null ? await _levelDataService.GetAllSubProfiles() : new List<string>();
    }

    public static async UniTask SwitchSubProfile(string subProfileName)
    {
        await SwitchSubProfileByName(subProfileName);
    }

    public static async UniTask SwitchSubProfileByName(string subProfileName)
    {
        _currentSubProfileName = subProfileName;
        if (_levelDataService != null)
            await _levelDataService.SwitchSubProfileByName(subProfileName);
    }

    private static Dictionary<string, object> AllLevelCustomData
    {
        get
        {
            if (string.IsNullOrEmpty(_levelDataService?.LevelData?.customLevelData))
                return new Dictionary<string, object>();
            var dataDictFromJson = Json.Deserialize(_levelDataService.LevelData.customLevelData) as Dictionary<string, object>;
            return dataDictFromJson ?? new Dictionary<string, object>();
        }
    }
    #endregion
}
