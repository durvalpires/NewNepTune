using System.Collections.Generic;
using _App_v2.Scripts.Levels.Score;
using Cysharp.Threading.Tasks;

public interface ILevelDataService
{
    PlayerLevelData LevelData { get; }

    UniTask LoadLevelData(string subProfileName);
    UniTask SaveLevelData();

    int GetNumberOfLevel();
    int GetNumberOfAttempts();
    int GetAverageScore();
    Dictionary<int, (int Score, int Repetition)> GetAllPlayerLevelData();
    void PrintAllPlayerLevelData();

    void SetLevelUnlock(int worldIndex, int levelIndex);
    void SetLevelCompleted(int worldIndex, int levelIndex);
    void SetLevelCompleted(int worldIndex, LevelInfo worldInfo);
    bool IsLevelCompleted(string worldIndex, int levelIndex);
    void UpdateCounter(int worldIndex, int levelIndex, CounterType counterType);
    void SetCustomScore(int customScore);
    void SetCustomStarRating(int starRating);

    UniTask CreateSubProfile(string subProfileName);
    UniTask<List<string>> GetAllSubProfiles();
    UniTask SwitchSubProfileByName(string subProfileName);
    
    string GetCustomData(string key, string defaultValue = "");
    void SetCustomData(string key, string value);
    Dictionary<string, object> GetAllCustomData();
    void SetLevelScoreData(int levelIndex, ILevelScore levelScore, string worldId);
}