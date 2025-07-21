using System;
using System.Collections.Generic;
using System.Linq;
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
                Successes = 0,
                //Fails = 0,
                TimeSpent = 0,
                StarRating = 0,
                HitAccuracy = null
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
                Successes = 1,
                HitAccuracy = new Dictionary<HitAccuracy, float>()
            };
            LevelData.NumberOfLevel++;
        }
        else
        {
            var levelData = LevelData.levels[key];
            int completions = levelData.Successes; //THIS WAAS REPETITION BEFORE, NOT SURE IF SHOULD BE ATTEMPTS OR SUCCESS
            levelData.MaxScore = finalScore > levelData.MaxScore ? finalScore : levelData.MaxScore;  
            //levelData.Repetition++;
            levelData.StarRating = starRating > levelData.StarRating ? starRating : levelData.StarRating; //THIS WAS ATTEMPTS BEFORE, NOT SURE IF SHOULD BE ATTEMPTS OR SUCCESSa
            levelData.Successes++;
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

    public bool IsLevelCompleted(string worldIndex, int levelIndex)
    {
        if (LevelData == null || LevelData.levels == null)
            return false;

        string key = LevelKeyUtil.LevelKey(worldIndex, levelIndex);
        if (!LevelData.levels.ContainsKey(key))
            return false;

        return LevelData.levels[key].Successes > 0;
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
                levelData.Successes++;
                newValue = levelData.Successes;
                break;
            // case CounterType.Failure:
            //     levelData.Fails++;
            //     newValue = levelData.Fails;
            //     break;
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
        
        string levelKey = LevelKeyUtil.LevelKey(worldId, levelIndex);

        if (!LevelData.levels.ContainsKey(levelKey))
            throw new Exception($"Level {levelIndex} does not exist in level data");
        
        if(LevelData.levels[levelKey].HitAccuracy == null)
            LevelData.levels[levelKey].HitAccuracy = new Dictionary<HitAccuracy, float>();
        

        var averagePercentages = LevelData.levels[levelKey].HitAccuracy;
        var lastSessionPercentages = levelScore.GetAccuracyPercentage();
        var attemptCount = LevelData.levels[levelKey].Attempts;
        
        var allAccuracyValues = System.Enum.GetValues(typeof(HitAccuracy)).Cast<HitAccuracy>().ToList();
        foreach (var accuracy in allAccuracyValues)
        {
            if (lastSessionPercentages.ContainsKey(accuracy))
            {
                var value = lastSessionPercentages[accuracy];
                
                if (!averagePercentages.ContainsKey(accuracy))
                    averagePercentages[accuracy] = value / attemptCount;
                else
                {
                    Debug.LogWarning($"Level {levelIndex} has existing accuracy data");
                    Debug.LogWarning($"Current attempt count is {attemptCount}");
                    Debug.LogWarning($"Current accuracy for {accuracy} is {averagePercentages[accuracy]}");
                    Debug.LogWarning($"Attempting to update accuracy with {value}");
                    float currentAvg = averagePercentages[accuracy];
                    Debug.LogWarning(currentAvg * (attemptCount-1));
                    Debug.LogWarning(currentAvg * (attemptCount-1) + value);
                    //THIS ASSUMES UPDATE COUNT ALREADY IS UPDATED WITH THIS PLAY SESSION
                    averagePercentages[accuracy] = (currentAvg * (attemptCount-1) + value) / (attemptCount);
                    Debug.LogWarning($"New accuracy for {accuracy} is {averagePercentages[accuracy]}");
                }
            }
            else
            {
                if (!averagePercentages.ContainsKey(accuracy))
                    averagePercentages[accuracy] = 0;
                else
                {
                    averagePercentages[accuracy] = averagePercentages[accuracy] * (attemptCount-1) / attemptCount;
                }
            }
        }

        // foreach (var kvp in levelScore.GetAccuracyPercentage())
        // {
        //     if (!averagePercentages.ContainsKey(kvp.Key))
        //         averagePercentages[kvp.Key] = kvp.Value / attemptCount;
        //     else
        //     {
        //         Debug.LogWarning($"Level {levelIndex} has existing accuracy data");
        //         Debug.LogWarning($"Current attempt count is {attemptCount}");
        //         Debug.LogWarning($"Current accuracy for {kvp.Key} is {averagePercentages[kvp.Key]}");
        //         Debug.LogWarning($"Attempting to update accuracy with {kvp.Value}");
        //         float currentAvg = averagePercentages[kvp.Key];
        //         Debug.LogWarning(currentAvg * (attemptCount-1));
        //         Debug.LogWarning(currentAvg * (attemptCount-1) + kvp.Value);
        //         //THIS ASSUMES UPDATE COUNT ALREADY IS UPDATED WITH THIS PLAY SESSION
        //         averagePercentages[kvp.Key] = (currentAvg * (attemptCount-1) + kvp.Value) / (attemptCount);
        //         Debug.LogWarning($"New accuracy for {kvp.Key} is {averagePercentages[kvp.Key]}");
        //     }
        // }
        
        SaveLevelData().Forget();
    }
}