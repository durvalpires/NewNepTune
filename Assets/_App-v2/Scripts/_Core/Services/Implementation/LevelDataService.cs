using System;
using System.Collections.Generic;
using _App_v2.Scripts.Levels.Score;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using static AllWorldsSO;

public class LevelDataService : ILevelDataService
{
    private readonly IDataPersistence _dataPersistence;
    private int _customScoreOverride = -1;
    private int _customStarRatingOverride = -1;
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
        //return LevelData?.Repetition ?? 0;
        throw new System.NotImplementedException();
    }

    public int GetAverageScore()
    {
        return LevelData?.AverageScore ?? 0;
    }

    public Dictionary<int, (int Score, int Repetition)> GetAllPlayerLevelData()
    {
        if (LevelData == null || LevelData.levels == null)
            return new Dictionary<int, (int, int)>();

        // I Dont understand this logic
        var result = new Dictionary<int, (int, int)>();
        //TODO FIX THIS
        // foreach (var kvp in LevelData.levels)
        //     result[kvp.Key] = (kvp.Value.MaxScore, kvp.Value.Repetition);

        return result;
    }

    public void PrintAllPlayerLevelData()
    {
        var levels = GetAllPlayerLevelData();
        foreach (var kvp in levels)
            Debug.Log("Level " + kvp.Key + " => Score: " + kvp.Value.Score + ", Repetition: " + kvp.Value.Repetition);
    }

    public void SetLevelUnlock(int worldIndex, int levelIndex)
    {
        if (LevelData == null)
            return;

        if (LevelData.levels == null)
            LevelData.levels = new Dictionary<string, LevelFirebaseData>();

        string key = LevelKeyUtil.LevelKey(worldIndex, levelIndex);
        if (!LevelData.levels.ContainsKey(key))
        {
           
            LevelData.levels[key] = new LevelFirebaseData
            {
                MaxScore = 0,
                //Repetition = 0,
                Attempts = 0,
                Success = 0,
                Fails = 0,
                TimeSpent = 0,
                StarRating = 0
            };
            LevelData.NumberOfLevel++;
        }

        SaveLevelData().Forget();
    }

    public void SetLevelCompleted(int worldIndex, int levelIndex)
    {
        int finalScore = _customScoreOverride != -1 ? _customScoreOverride : 100;
        _customScoreOverride = -1;

        int starRating = _customStarRatingOverride != -1 ? _customStarRatingOverride : 3;
        _customStarRatingOverride = -1;

        if (LevelData == null)
            return;

        if (LevelData.levels == null)
            LevelData.levels = new Dictionary<string, LevelFirebaseData>();

        string key = LevelKeyUtil.LevelKey(worldIndex, levelIndex);
        if (!LevelData.levels.ContainsKey(key))
        {

            LevelData.levels[key] = new LevelFirebaseData
            {
                Attempts = 1,
                MaxScore = finalScore,
                //Repetition = 1,
                StarRating = starRating,
                Success = 1,
                HitAccuracy = new Dictionary<HitAccuracy, float>()
            };
            LevelData.NumberOfLevel++;
        }
        else
        {
            var levelData = LevelData.levels[key];
            int completions = levelData.Success; //THIS WAAS REPETITION BEFORE, NOT SURE IF SHOULD BE ATTEMPTS OR SUCCESS
            levelData.MaxScore = finalScore > levelData.MaxScore ? finalScore : levelData.MaxScore;  
            //levelData.Repetition++;
            levelData.StarRating = starRating > levelData.StarRating ? starRating : levelData.StarRating; //THIS WAS ATTEMPTS BEFORE, NOT SURE IF SHOULD BE ATTEMPTS OR SUCCESSa
            levelData.Success++;
        }

        int totalScore = 0;
        //int totalRepetition = 0;
        foreach (var level in LevelData.levels.Values)
        {
            totalScore += level.MaxScore;
            //totalRepetition += level.Repetition;
        }

        LevelData.AverageScore = LevelData.NumberOfLevel > 0
            ? totalScore / LevelData.NumberOfLevel
            : 0;
        //LevelData.Repetition = totalRepetition;

        SaveLevelData().Forget();
    }

    public void UpdateCounter(int worldIndex, int levelIndex, CounterType counterType)
    {
        if (LevelData == null || LevelData.levels == null)
            return;

        string key = LevelKeyUtil.LevelKey(worldIndex, levelIndex);
        if (!LevelData.levels.ContainsKey(key))
        {
            SetLevelUnlock(worldIndex , levelIndex);
        }

        var levelData = LevelData.levels[key];
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
                levelData.Fails++;
                newValue = levelData.Fails;
                break;
        }

        SaveLevelData().Forget();
        _dataPersistence.UpdateLevelCounter(worldIndex, levelIndex, counterType, newValue, _currentSubProfileName).Forget();
    }

    public void SetCustomScore(int customScore)
    {
        _customScoreOverride = customScore;
    }
    public void SetCustomStarRating(int starRating)
    {
       _customStarRatingOverride = starRating;
    }

    public async UniTask CreateSubProfile(string subProfileName)
    {
        PlayerLevelData newProfile = new PlayerLevelData
        {
            SubProfilename = subProfileName,
            AverageScore = 0,
            NumberOfLevel = 0,
            //Repetition = 0,
            StarRating = 3,
            levels = new Dictionary<string, LevelFirebaseData>()
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

    public void SetLevelScoreData(int levelIndex, ILevelScore levelScore, string worldId)
    {
        if (LevelData == null)
            throw new Exception("Level data is null");

        if (LevelData.levels == null)
            throw new Exception("Level data is null");

        if (!LevelData.levels.ContainsKey(levelIndex))
            throw new Exception($"Level {levelIndex} does not exist in level data");
        

        var averagePercentages = LevelData.levels[levelIndex].HitAccuracy;
        var attemptCount = LevelData.levels[levelIndex].Attempts;
        
        foreach (var kvp in levelScore.GetAccuracyPercentage())
        {
            if (!averagePercentages.ContainsKey(kvp.Key))
                averagePercentages[kvp.Key] = kvp.Value;
            else
            {
                float currentAvg = averagePercentages[kvp.Key];
                //THIS ASSUMES UPDATE COUNT ALREADY IS UPDATED WITH THIS PLAY SESSION
                averagePercentages[kvp.Key] = ((currentAvg * attemptCount-1) + kvp.Value) / (attemptCount); 
            }
        }
    }
}