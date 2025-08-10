using System;
using System.Collections.Generic;
using System.Linq;
using _App_v2.Scripts.Levels.Score;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletObserver : MonoBehaviour
{
    public static event Action onLevelComplete;
    private static int _openedLevel = -1;
    private static int _openedWorldIndex = 1;

    // LevelDataService reference
    private static ILevelDataService _levelDataService;
    private static ILevelDataService LevelDataService
    {
        get
        {
            if (_levelDataService == null)
            {
                // Create LevelDataService using LocalDataPersistence
                _levelDataService = new LevelDataService(new LocalDataPersistence());
            }
            return _levelDataService;
        }
    }
    public static string LevelKey(int worldIndex, int levelIndex)
    {
        return $"W{worldIndex}_L{levelIndex}";
    }

    public static void LevelStart(int levelIndex, string worldId)
    {
        _openedLevel = levelIndex;
        _openedWorldIndex = int.TryParse(worldId, out var w) ? w : 1;
        PlayerModelBase.LevelDataService.UpdateCounter(_openedWorldIndex, levelIndex, CounterType.Attempts);
    }

    public void SetCurrentLevelComplete()
    {
        Debug.Log("SetCurrentLevelComplete");
        //PlayerModelBase.LevelDataService.UpdateCounter(_openedWorldIndex, _openedLevel, CounterType.Success);
        LevelComplete();
        // if (_openedLevel != -1)
        // {
        //     SendStudentUpdate(_openedLevel, _openedWorldIndex.ToString(), levelscore);
        // }
    }

    public void SetCurrentLevelComplete(ILevelScore levelscore = null)
    {
        Debug.Log("SetCurrentLevelCompleteWithScore");
        //  Check if this is a RhythmGameScoreController and handle AccuracyBreakdown
        if (levelscore is RhythmGameScoreController rhythmController)
        {
            if (rhythmController.PlayerScore > 0)
            {
                PlayerModelBase.SetCustomScore(rhythmController.PlayerScore);
                PlayerModelBase.LevelDataService.SetCustomStarRating(rhythmController.PlayerStars);
                
                //  Ensure level exists before saving accuracy data
                PlayerModelBase.LevelDataService.SetLevelUnlock(_openedWorldIndex, _openedLevel);
                
                //  Save accuracy data to PlayerModelBase.LevelData
                PlayerModelBase.LevelDataService.SetLevelScoreData(_openedLevel, rhythmController, _openedWorldIndex.ToString());
                
                //  Apply the custom score and star rating to actual data
                //PlayerModelBase.LevelDataService.SetLevelCompleted(_openedWorldIndex, _openedLevel);
                LevelComplete(levelscore);
                
                Debug.Log($"Piano game completed - Score: {rhythmController.PlayerScore}, Stars: {rhythmController.PlayerStars}");
                Debug.Log($"AccuracyBreakdown count: {rhythmController.AccuracyBreakdown?.Count ?? 0}");
            }
        }
        else
        {
            LevelComplete(levelscore);
        }
        
        
        // if (_openedLevel != -1)
        // {
        //     SendStudentUpdate(_openedLevel, _openedWorldIndex.ToString(), levelscore);
        // }
    }

    public void SetCurrentLevelFailed(ILevelScore levelScore = null)
    {
        Debug.Log("Set Current Level Failed With Score");

        if (levelScore is RhythmGameScoreController rhythmController)
        {
            //  I DONT THINK WE NEED THIS VERIFICATION, BUT PUT IT BACK IF EVERYTHING GETS EFFD UP
            // if (rhythmController.PlayerScore > 0)
            // {
            PlayerModelBase.SetCustomScore(rhythmController.PlayerScore);
            PlayerModelBase.LevelDataService.SetCustomStarRating(rhythmController.PlayerStars);
            
            //  Ensure level exists before saving accuracy data
            PlayerModelBase.LevelDataService.SetLevelUnlock(_openedWorldIndex, _openedLevel);
            
            //  Save accuracy data to PlayerModelBase.LevelData
            PlayerModelBase.LevelDataService.SetLevelScoreData(_openedLevel, rhythmController, _openedWorldIndex.ToString());
            
            //  Apply the custom score and star rating to actual data
            //PlayerModelBase.LevelDataService.SetLevelCompleted(_openedWorldIndex, _openedLevel);
            LevelComplete(levelScore);
            
            Debug.Log($"Piano game completed - Score: {rhythmController.PlayerScore}, Stars: {rhythmController.PlayerStars}");
            Debug.Log($"AccuracyBreakdown count: {rhythmController.AccuracyBreakdown?.Count ?? 0}");
            //}
        }
        else
        {
            LevelComplete(levelScore);
        }
    }

    // To get the real score controller from EndOfLevelScreenController
    public void SetCurrentLevelCompleteWithScore(RhythmGameScoreController scoreController)
    {
        if (scoreController != null && scoreController.PlayerScore > 0)
        {
            PlayerModelBase.SetCustomScore(scoreController.PlayerScore);
            
            // ✅ Ensure level exists before saving accuracy data
            PlayerModelBase.LevelDataService.SetLevelUnlock(_openedWorldIndex, _openedLevel);
            
            // ✅ Save accuracy data to PlayerModelBase.LevelData
            PlayerModelBase.LevelDataService.SetLevelScoreData(_openedLevel, scoreController, _openedWorldIndex.ToString());
            
            Debug.Log($"Piano game completed - Score: {scoreController.PlayerScore}, Stars: {scoreController.PlayerStars}");
            Debug.Log($"AccuracyBreakdown count: {scoreController.AccuracyBreakdown?.Count ?? 0}");
        }
        LevelComplete();
    }

    // public static void LevelFailed(ILevelScore levelscore = null)
    // {
    //     Debug.Log("Level Failed");
    //
    //     PlayerModel.FailLevel(_openedLevel, _openedWorldIndex.ToString());
    //
    //     if (_openedLevel != -1)
    //     {
    //         SendStudentUpdate(_openedLevel, _openedWorldIndex.ToString(), levelscore);
    //     }
    //
    //     onLevelComplete?.Invoke();
    // }

    public static void LevelComplete(ILevelScore levelscore = null)
    {
        Debug.Log("LevelComplete");
        
        PlayerModel.CompleteLevel(_openedLevel, _openedWorldIndex.ToString(), levelscore);

        if (_openedLevel != -1)
        {
            SendStudentUpdate(_openedLevel, _openedWorldIndex.ToString(), levelscore);
        }

        onLevelComplete?.Invoke();

        // PlayerModel.CompleteLevel(_openedLevel, _openedLevelworldId); 
        // onLevelComplete?.Invoke();

        // Keep the current PlayerModel system
        //PlayerModel.CompleteLevel(_openedLevel, _openedWorldIndex.ToString());

        // Save level complete information to LevelDataService
        // if (_openedLevel != -1)
        // {
        //     LevelDataService.SetLevelCompleted(_openedLevel);
        //     
        //     // Send data to Student endpoint
        //     SendStudentUpdate(_openedLevel, _openedLevelworldId, levelscore = null);
        // }

        //onLevelComplete?.Invoke();
    }

    public static int sendWorld;
    public static int sendLevel;

    public static int currentWorld;
    public static int currentLevel;

    public static void SendStudentUpdate(int levelIndex, string worldId, ILevelScore levelscore)
    {
        if (StudentUpdateEndpoint.Instance == null)
        {
            Debug.Log("Creating StudentUpdateEndpoint instance...");
            GameObject endpointGO = new GameObject("StudentUpdateEndpoint");
            endpointGO.AddComponent<StudentUpdateEndpoint>();
            UnityEngine.Object.DontDestroyOnLoad(endpointGO);
        }

        if (StudentUpdateEndpoint.Instance == null)
        {
            Debug.LogError("Failed to create StudentUpdateEndpoint instance!");
            return;
        }

        bool passed = levelscore.PlayerStars > 0;

        try
        {
            int playedLevel = levelIndex;
            int playedWorld = int.TryParse(worldId, out int world) ? world : 1;

            // Initial log
            Debug.Log($"TEST[CHECK] current: W{playedWorld} L{playedLevel} | last: W{currentWorld} L{currentLevel}");

            sendWorld = currentWorld;
            sendLevel = currentLevel;

            if (passed)
            {
                if (playedWorld > currentWorld)
                {
                    // Bigger world → definite progress
                    sendWorld = currentWorld = playedWorld;
                    sendLevel = currentLevel = playedLevel;

                    Debug.Log($"TEST[NEW WORLD] Progress detected. New record: World {sendWorld}, Level {sendLevel}");
                }
                else if (playedWorld == currentWorld && playedLevel >= currentLevel)
                {
                    if(IsLastWorldCompleted())
                    {
                        Debug.LogWarning("Last world completed! Completing world...");
                        //TODO THIS IS COMMENTED SO THAT LEVELS INIT SHOWS THE SCREEN OF PLANET COMPLETION
                        //PlayerModel.CompleteWorld(worldId);
                        sendLevel = currentLevel = 0;
                        sendWorld = ++currentWorld;
                    }
                    else
                    {
                        // Same world but higher level → progress
                        sendLevel = currentLevel = ++playedLevel;
                    }
                    Debug.Log($"TEST[NEW LEVEL] Same world. New record: World {sendWorld}, Level {sendLevel}");
                }
                Debug.Log($"TEST[NO PROGRESS] Same level played again. Current position maintained: World {sendWorld}, Level {sendLevel}");
            }

            string lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            List<StudentUpdateEndpoint.DetailedLevelInfo> detailedLevelInfo = new List<StudentUpdateEndpoint.DetailedLevelInfo>();

            var levelData = PlayerModelBase.LevelData;

            string levelKey = LevelKeyUtil.LevelKey(playedWorld, playedLevel);

            // foreach (var key in levelData.levels.Keys)
            // {
            //     Debug.Log($"[DEBUG] Current level key: {key}");
            // }

            if (levelData?.levels != null && levelData.levels.ContainsKey(levelKey))
            {
                var level = levelData.levels[levelKey];
                //if (levelData?.levels != null && levelData.levels.ContainsKey(levelIndex.ToString()))
                //{
                //var level = levelData.levels[levelIndex.ToString()];
                //string currentScene = SceneManager.GetActiveScene().name;

                //CalculateStarRating(levelscore, currentScene, out star1, out star2, out star3);

                //  Get accuracy breakdown data and convert to string keys
                Dictionary<string, float> accuracyBreakdownForJson = null;
                if (level.HitAccuracy != null && level.HitAccuracy.Count > 0)
                {
                    accuracyBreakdownForJson = new Dictionary<string, float>();
                    foreach (var kvp in level.HitAccuracy)
                    {
                        accuracyBreakdownForJson[kvp.Key.ToString()] = kvp.Value;
                    }
                }

                //  Get the actual star rating from the score controller if available
                int actualStarRating = 0;
                if (levelscore is RhythmGameScoreController rhythmScore)
                {
                    actualStarRating = rhythmScore.PlayerStars;
                    Debug.Log($"Using actual star rating from score controller: {actualStarRating}");
                }
                else
                {
                    actualStarRating = level.StarRating ?? 0;
                    Debug.Log($"Using star rating from level data: {actualStarRating}");
                }

                var detailedInfo = new StudentUpdateEndpoint.DetailedLevelInfo(
                    world: playedWorld,
                    level: playedLevel,
                    attempts: level.Attempts,
                    successes: level.Successes,
                    //fails: level.Fails,
                    maxScore: level.MaxScore,
                    isCorrect: level.Successes > 0,
                    successRate: (float)level.Successes / level.Attempts,
                    starRating: actualStarRating,
                    accuracyBreakdown: accuracyBreakdownForJson
                );

                detailedLevelInfo.Add(detailedInfo);
                Debug.Log($"PlayerModelBase data used - Level: {playedLevel}, Score: {level.MaxScore}, Attempts: {level.Attempts}, Success: {level.Successes}, Stars: {level.StarRating}");
            }
            else
            {
                Debug.LogError($"[ERROR] Level key not found in LevelData.levels: {levelKey}");
            }
            //else
            //{
            //    var detailedInfo = new StudentUpdateEndpoint.DetailedLevelInfo(
            //        world: currentWorld,
            //        level: currentLevel,
            //        attempts: 1,
            //        fails: 0,
            //        maxScore: 100,
            //        isCorrect: true,
            //        averageAccuracy: 1.0f,
            //        star1: true,
            //        star2: true,
            //        star3: true
            //    );

            //    detailedLevelInfo.Add(detailedInfo);
            //    Debug.LogWarning("Data not found in PlayerModelBase, using fallback - Level: " + currentLevel);
            //}

            string studentId = FirebaseProxyService.Instance.PrivateCode;
            if (string.IsNullOrEmpty(studentId))
            {
                Debug.LogError("Student ID not found! Please login as a student first");
                return;
            }
            
            //TODO WE SHOULD CHECK IF WE COMPLETED A PLANET
            // if(IsLastWorldCompleted())
            // {
            //     Debug.LogWarning("Last world completed! Completing world...");
            //     //TODO THIS IS COMMENTED SO THAT LEVELS INIT SHOWS THE SCREEN OF PLANET COMPLETION
            //     //PlayerModel.CompleteWorld(worldId);
            //     sendLevel = currentLevel = 0;
            //     currentWorld++;
            //     sendWorld = currentWorld;
            // }
            // else
            // {
            //     if(playedWorld == currentWorld && sendLevel == currentLevel)
            //     {
            //         currentLevel++;
            //     }
            // }
            
            Debug.LogWarning("Send Last World: " + sendWorld + " Last Level: " + sendLevel);

            StudentUpdateEndpoint.Instance.UpdateStudentInfo(
                studentId: studentId,
                currentLevel: sendLevel,
                currentWorld: sendWorld,
                lastTimePlayed: lastTimePlayed,
                totalTime: (int)SessionTimeTracker.TotalMinutesPlayed,
                detailedLevelInfo: detailedLevelInfo,
                 callback: OnStudentUpdateCallback
            //callback: (success, message) => {
            //    OnStudentUpdateCallback(success, message);
            //    if (success && isHigherProgress)
            //    {
            //        // Update lastProgress here if successful
            //        FirebaseProxyService.Instance.UpdateLastProgress(currentWorld, currentLevel);
            //    }
            //}
            );

            Debug.Log($"Student update sent - Level: {playedLevel}, World: {playedWorld}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error sending student update: {e.Message}");
        }
    }

    //private static float CalculateAverageAccuracy(LevelFirebaseData level)
    //{
    //    if (level.Attempts > 0)
    //    {
    //        return (float)level.Successes / level.Attempts;
    //    }
    //    else if (level.Successes + level.Fails > 0)
    //    {
    //        return (float)level.Successes / (level.Successes + level.Fails);
    //    }
    //    return 1.0f;
    //}
    private static void OnStudentUpdateCallback(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Student update successful: {message}");
        }
        else
        {
            Debug.LogError($"Student update failed: {message}");
        }
    }

    public static bool IsLastWorldCompleted()
    {
        //TODO THIS SHOULD BE IMPROVED
        var data = PlayerModel.AllWorlds.worldsConfigs[currentWorld-1];

        for(int i = 0; i < data.levels.Length; i++)
        {
            var level = data.levels[i];
            if (!PlayerModel.IsLevelCompleted(i, data.id))
            {
                return false;
            }
        }
        return true;
    }

    //private static bool IsHigherProgress(int newWorld, int newLevel, int currentWorld, int currentLevel)
    //{
    //    // Compare world first
    //    if (newWorld > currentWorld) return true;
    //    if (newWorld < currentWorld) return false;

    //    // If worlds are equal, compare levels - priority is world first, then level
    //    return newLevel > currentLevel;
    //}
}