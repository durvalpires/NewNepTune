#if UNITY_WEBGL
using _App_v2.Scripts._Core.Firebase;
using Cysharp.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Examples;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System;

using _App_v2.Scripts._Core.Firebase.Config;
using MarksAssets.FirebaseWebGL.Examples;
using MarksAssets.FirebaseWebGL.App;
using MarksAssets.FirebaseWebGL.Auth;
using MarksAssets.FirebaseWebGL.Database;
using Db = MarksAssets.FirebaseWebGL.Database.Database;
using _App_v2.Scripts._Core.Firebase.Databases;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;


public class FirebaseWebGLDataPersistence : IDataPersistence , IDBService
{
    private readonly FirebaseConfigObject _config;
    private readonly EmulatorConfigObject _emulatorConfig;
    private bool _isConnected;
    private string _userId;

#if UNITY_WEBGL 
    private MarksAssets.FirebaseWebGL.Database.Database _dbInstance;
    private static FirebaseApp _sharedApp;

    private static MarksAssets.FirebaseWebGL.Database.Database _sharedDb;
#endif

    public bool IsConnected => _isConnected;

    public FirebaseWebGLDataPersistence(FirebaseConfigObject config, EmulatorConfigObject emulatorConfig)
    {
        _config = config;
        _emulatorConfig = emulatorConfig;
        _isConnected = false;
    }

    public void SetUserId(string userId)
    {
        _userId = userId;
        _isConnected = !string.IsNullOrEmpty(_userId);
    }

    public async UniTask<AccountDataDB> LoadPlayerData()
    {
#if UNITY_WEBGL
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return new AccountDataDB();

        try
        {
            await EnsureDatabaseInitialized();

            var userRef = Db.Ref(_dbInstance, $"users/{_userId}/PlayerData");
            var snapshot = await Db.get(userRef);

            if (snapshot.exists())
            {
                string jsonData = snapshot.val().ToString();
                return JsonUtility.FromJson<AccountDataDB>(jsonData) ?? new AccountDataDB();
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"[WebGL] LoadPlayerData failed: {e.Message}\n{e.StackTrace}");
        }
#endif
        return new AccountDataDB();
    }

    public async UniTask SavePlayerData(AccountDataDB accountData)
    {
#if UNITY_WEBGL 
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return;

        try
        {
            await EnsureDatabaseInitialized();
            
            var dbRef = Db.Ref(_dbInstance, $"users/{_userId}/PlayerData");
            
            var playerDataDict = new Dictionary<string, object>
            {
                ["profileId"] = accountData.profileId,
                ["playerName"] = accountData.playerName,
                ["IsSoundOn"] = accountData.IsSoundOn,
                ["IsMusicOn"] = accountData.IsMusicOn,
                //["customUserData"] = playerData.customUserData,
                ["totalPlayTime"] = accountData.totalPlayTime
            };
            
            await Db.set(dbRef, playerDataDict);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase SavePlayerData error: {e.Message}");
        }
#endif
    }

    public async UniTask<PlayerLevelData> LoadLevelData(string subProfileName)
    {
#if UNITY_WEBGL
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return new PlayerLevelData { SubProfilename = subProfileName };

        try
        {
            await EnsureDatabaseInitialized();

            var levelDataPath = $"users/{_userId}/SubProfile/{subProfileName}/playerLeveldata";
            var levelDataRef = Db.Ref(_dbInstance, levelDataPath);
            var snapshot = await Db.get(levelDataRef);

            if (snapshot.exists())
            {
                var result = new PlayerLevelData { SubProfilename = subProfileName };

                if (snapshot.hasChild("AverageScore"))
                    result.AverageScore = System.Convert.ToInt32(snapshot.child("AverageScore").val());

                if (snapshot.hasChild("NumberOfLevel"))
                    result.NumberOfLevel = System.Convert.ToInt32(snapshot.child("NumberOfLevel").val());

                if (snapshot.hasChild("Repetition"))
                    result.Repetition = System.Convert.ToInt32(snapshot.child("Repetition").val());

                if (snapshot.hasChild("customLevelData"))
                    result.customLevelData = snapshot.child("customLevelData").val().ToString();

                if (snapshot.hasChild("levels"))
                {
                    var levelsSnapshot = snapshot.child("levels");
                    foreach (DataSnapshot levelSnapshot in levelsSnapshot)
                    {
                        string levelKey = levelSnapshot.key;
                        if (int.TryParse(levelKey, out int levelIndex))
                        {
                            var entry = new LevelFirebaseData();

                            if (levelSnapshot.hasChild("Score"))
                                entry.Score = System.Convert.ToInt32(levelSnapshot.child("Score").val());

                            if (levelSnapshot.hasChild("Repetition"))
                                entry.Repetition = System.Convert.ToInt32(levelSnapshot.child("Repetition").val());

                            if (levelSnapshot.hasChild("Attempts"))
                                entry.Attempts = System.Convert.ToInt32(levelSnapshot.child("Attempts").val());

                            if (levelSnapshot.hasChild("Success"))
                                entry.Success = System.Convert.ToInt32(levelSnapshot.child("Success").val());

                            if (levelSnapshot.hasChild("Failure"))
                                entry.Failure = System.Convert.ToInt32(levelSnapshot.child("Failure").val());

                            if (levelSnapshot.hasChild("TimeSpent"))
                                entry.TimeSpent = System.Convert.ToSingle(levelSnapshot.child("TimeSpent").val());

                            result.levels[levelIndex] = entry;
                        }
                    }
                }

                return result;
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase LoadLevelData error: {e.Message}");
        }

#endif
        return new PlayerLevelData { SubProfilename = subProfileName };
    }

    public async UniTask SaveLevelData(PlayerLevelData levelData, string subProfileName)
    {
#if UNITY_WEBGL 
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return;

        try
        {
            await EnsureDatabaseInitialized();
            
            var dbRef = Db.Ref(_dbInstance, $"users/{_userId}/SubProfile/{subProfileName}/playerLeveldata");
            
            
            await Db.set(Db.child(dbRef, "SubProfilename"), levelData.SubProfilename);
            await Db.set(Db.child(dbRef, "AverageScore"), levelData.AverageScore);
            await Db.set(Db.child(dbRef, "NumberOfLevel"), levelData.NumberOfLevel);
            await Db.set(Db.child(dbRef, "Repetition"), levelData.Repetition);
            await Db.set(Db.child(dbRef, "customLevelData"), levelData.customLevelData);
            
          
            var levelsRef = Db.child(dbRef, "levels");
            foreach (var entry in levelData.levels)
            {
                var levelRef = Db.child(levelsRef, entry.Key.ToString());
                await Db.set(Db.child(levelRef, "Score"), entry.Value.Score);
                await Db.set(Db.child(levelRef, "Repetition"), entry.Value.Repetition);
                await Db.set(Db.child(levelRef, "Attempts"), entry.Value.Attempts);
                await Db.set(Db.child(levelRef, "Success"), entry.Value.Success);
                await Db.set(Db.child(levelRef, "Failure"), entry.Value.Failure);
                await Db.set(Db.child(levelRef, "TimeSpent"), entry.Value.TimeSpent);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase SaveLevelData error: {e.Message}");
        }
#endif
    }

    public async UniTask UpdatePlayerName(string playerName)
    {
#if UNITY_WEBGL
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return;

        try
        {
            await EnsureDatabaseInitialized();
            var dbRef = Db.Ref(_dbInstance, $"users/{_userId}/PlayerData/playerName");
            await Db.set(dbRef, playerName);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase UpdatePlayerName error: {e.Message}");
        }
#endif
    }

    public async UniTask UpdateLevelCounter(int levelIndex, CounterType counterType, int newValue, string subProfileName)
    {
#if UNITY_WEBGL 
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return;

        try
        {
            await EnsureDatabaseInitialized();
            var dbRef = Db.Ref(_dbInstance, $"users/{_userId}/SubProfile/{subProfileName}/playerLeveldata/levels/{levelIndex}/{counterType}");
            await Db.set(dbRef, newValue);
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase UpdateLevelCounter error: {e.Message}");
        }
#endif
    }

    public async UniTask CreateSubProfile(string subProfileName, PlayerLevelData initialData)
    {
#if UNITY_WEBGL 
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return;

        try
        {
            await EnsureDatabaseInitialized();
            
           
            var subProfileRef = Db.Ref(_dbInstance, $"users/{_userId}/SubProfile/{subProfileName}");
            var snapshot = await Db.get(subProfileRef);
            
            if (!snapshot.exists())
            {
              
                var levelDataDict = new Dictionary<string, object>
                {
                    ["SubProfilename"] = initialData.SubProfilename,
                    ["AverageScore"] = initialData.AverageScore,
                    ["NumberOfLevel"] = initialData.NumberOfLevel,
                    ["Repetition"] = initialData.Repetition,
                    ["customLevelData"] = initialData.customLevelData
                };
                
             
                if (initialData.levels.Count > 0)
                {
                    var levelsDict = new Dictionary<string, object>();
                    foreach (var entry in initialData.levels)
                    {
                        levelsDict[entry.Key.ToString()] = new Dictionary<string, object>
                        {
                            ["Score"] = entry.Value.Score,
                            ["Repetition"] = entry.Value.Repetition,
                            ["Attempts"] = entry.Value.Attempts,
                            ["Success"] = entry.Value.Success,
                            ["Failure"] = entry.Value.Failure,
                            ["TimeSpent"] = entry.Value.TimeSpent
                        };
                    }
                    levelDataDict["levels"] = levelsDict;
                }
                
                await Db.set(Db.child(subProfileRef, "playerLeveldata"), levelDataDict);
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase CreateSubProfile error: {e.Message}");
        }
#endif
    }

    public async UniTask<List<string>> GetAllSubProfiles()
    {
        List<string> subProfileNames = new List<string>();

#if UNITY_WEBGL
        if (!_isConnected || string.IsNullOrEmpty(_userId))
            return subProfileNames;

        try
        {
            await EnsureDatabaseInitialized();
            
            var subProfilesRef = Db.Ref(_dbInstance, $"users/{_userId}/SubProfile");
            var snapshot = await Db.get(subProfilesRef);
            
            if (snapshot.exists())
            {
                foreach (DataSnapshot child in snapshot)
                {
                    subProfileNames.Add(child.key);
                }
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"WebGL Firebase GetAllSubProfiles error: {e.Message}");
        }
#endif

        return subProfileNames;
    }

#if UNITY_WEBGL
    private async UniTask EnsureDatabaseInitialized()
    {
        if (_sharedDb != null)
        {
            _dbInstance = _sharedDb;
            Debug.Log($"Re-using cached DB handle {_dbInstance}");
            return;
        }

        Debug.Log("Waiting for FireService.WebGLFirebaseApp …");
        while (FireService.Instance == null)
            await UniTask.Yield();

        while (FireService.Instance.WebGLFirebaseApp == null)
            await UniTask.Yield();

        _sharedApp = FireService.Instance.WebGLFirebaseApp;
        Debug.Log($"Got FirebaseApp   name='{_sharedApp.name}'   apiKey='{_sharedApp.options.apiKey}'");

        _sharedDb = Db.getDatabase(_sharedApp);
        _dbInstance = _sharedDb;
        Debug.Log($"Db.getDatabase() → {_dbInstance}");
    }

    public void Connect(string userId, string unused = null)
    {
        _userId = userId;
        _isConnected = !string.IsNullOrEmpty(_userId);
    }

    public void Disconnect()
    {
        _userId = null;
        _isConnected = false;
    }

    public void SaveData<T>(T data) where T : class, IDBData, new()
    {
        
    }

    public UniTask<T> GetData<T>() where T : class, IDBData, new()
    {
        return new UniTask<T>(new T());
    }

#endif
}
#endif