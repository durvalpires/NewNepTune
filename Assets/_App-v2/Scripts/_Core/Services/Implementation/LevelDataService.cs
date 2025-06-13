using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class LevelDataService : ILevelDataService
{
    private readonly IDataPersistence _dataPersistence;
    private int _customScoreOverride = -1;
    private string _currentSubProfileName = "Default";

    public PlayerLevelData LevelData { get; private set; }

    public LevelDataService(IDataPersistence dataPersistence)
    {
        _dataPersistence = dataPersistence;
        LevelData = new PlayerLevelData { SubProfilename = _currentSubProfileName };
    }

    public async UniTask LoadLevelData(string subProfileName)
    {
        _currentSubProfileName = subProfileName;
        LevelData = await _dataPersistence.LoadLevelData(subProfileName);
    }

    public async UniTask SaveLevelData()
    {
        await _dataPersistence.SaveLevelData(LevelData, _currentSubProfileName);
    }

    public int GetNumberOfLevel()
    {
        return LevelData?.NumberOfLevel ?? 0;
    }

    public int GetNumberOfAttempts()
    {
        return LevelData?.Repetition ?? 0;
    }

    public int GetAverageScore()
    {
        return LevelData?.AverageScore ?? 0;
    }

    public Dictionary<int, (int Score, int Repetition)> GetAllPlayerLevelData()
    {
        if (LevelData == null || LevelData.levels == null)
            return new Dictionary<int, (int, int)>();

        var result = new Dictionary<int, (int, int)>();
        foreach (var kvp in LevelData.levels)
            result[kvp.Key] = (kvp.Value.Score, kvp.Value.Repetition);

        return result;
    }

    public void PrintAllPlayerLevelData()
    {
        var levels = GetAllPlayerLevelData();
        foreach (var kvp in levels)
            Debug.Log("Level " + kvp.Key + " => Score: " + kvp.Value.Score + ", Repetition: " + kvp.Value.Repetition);
    }

    public void SetLevelUnlock(int levelIndex)
    {
        if (LevelData == null)
            return;

        if (LevelData.levels == null)
            LevelData.levels = new Dictionary<int, LevelFirebaseData>();

        if (!LevelData.levels.ContainsKey(levelIndex))
        {
            LevelData.levels[levelIndex] = new LevelFirebaseData
            {
                Score = 0,
                Repetition = 0,
                Attempts = 0,
                Success = 0,
                Failure = 0,
                TimeSpent = 0
            };
            LevelData.NumberOfLevel++;
        }

        SaveLevelData().Forget();
    }

    public void SetLevelCompleted(int levelIndex)
    {
        int finalScore = (_customScoreOverride != -1) ? _customScoreOverride : 100;
        _customScoreOverride = -1;

        if (LevelData == null)
            return;

        if (LevelData.levels == null)
            LevelData.levels = new Dictionary<int, LevelFirebaseData>();

        if (!LevelData.levels.ContainsKey(levelIndex))
        {
            LevelData.levels[levelIndex] = new LevelFirebaseData { Score = finalScore, Repetition = 1 };
            LevelData.NumberOfLevel++;
        }
        else
        {
            var levelData = LevelData.levels[levelIndex];
            int completions = levelData.Repetition;
            levelData.Score = (levelData.Score * completions + finalScore) / (completions + 1);
            levelData.Repetition++;
        }

        int totalScore = 0;
        int totalRepetition = 0;
        foreach (var level in LevelData.levels.Values)
        {
            totalScore += level.Score;
            totalRepetition += level.Repetition;
        }

        LevelData.AverageScore = LevelData.NumberOfLevel > 0 ? totalScore / LevelData.NumberOfLevel : 0;
        LevelData.Repetition = totalRepetition;

        SaveLevelData().Forget();
    }

    public void UpdateCounter(int levelIndex, CounterType counterType)
    {
        if (LevelData == null || LevelData.levels == null || !LevelData.levels.ContainsKey(levelIndex))
            return;

        var levelData = LevelData.levels[levelIndex];
        int newValue = 0;

        switch (counterType)
        {
            case CounterType.Attempts:
                levelData.Attempts++;
                newValue = levelData.Attempts;
                break;
            case CounterType.Success:
                levelData.Success++;
                newValue = levelData.Success;
                break;
            case CounterType.Failure:
                levelData.Failure++;
                newValue = levelData.Failure;
                break;
        }

        SaveLevelData().Forget();
        _dataPersistence.UpdateLevelCounter(levelIndex, counterType, newValue, _currentSubProfileName).Forget();
    }

    public void SetCustomScore(int customScore)
    {
        _customScoreOverride = customScore;
    }

    public async UniTask CreateSubProfile(string subProfileName)
    {
        PlayerLevelData newProfile = new PlayerLevelData
        {
            SubProfilename = subProfileName,
            AverageScore = 0,
            NumberOfLevel = 0,
            Repetition = 0,
            levels = new Dictionary<int, LevelFirebaseData>()
        };

        await _dataPersistence.CreateSubProfile(subProfileName, newProfile);
    }

    public async UniTask<List<string>> GetAllSubProfiles()
    {
        return await _dataPersistence.GetAllSubProfiles();
    }

    public async UniTask SwitchSubProfileByName(string subProfileName)
    {
        await LoadLevelData(subProfileName);
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

        LevelData.customLevelData = Json.Serialize(dataDict);
        SaveLevelData().Forget();
    }

    public Dictionary<string, object> GetAllCustomData()
    {
        if (string.IsNullOrEmpty(LevelData.customLevelData))
            return new Dictionary<string, object>();

        var dataDictFromJson = Json.Deserialize(LevelData.customLevelData) as Dictionary<string, object>;
        return dataDictFromJson ?? new Dictionary<string, object>();
    }
}