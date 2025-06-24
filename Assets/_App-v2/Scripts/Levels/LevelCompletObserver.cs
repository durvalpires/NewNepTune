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
                // LocalDataPersistence kullanarak LevelDataService oluştur
                _levelDataService = new LevelDataService(new LocalDataPersistence());
            }
            return _levelDataService;
        }
    }

    public static void LevelStart(int levelIndex, string worldId)
    {
        _openedLevel = levelIndex;
        _openedWorldIndex = int.TryParse(worldId, out var w) ? w : 1;
        PlayerModelBase.LevelDataService.UpdateCounter(_openedWorldIndex, levelIndex, CounterType.Attempts);
    }

    public void SetCurrentLevelComplete()
    {
        //PlayerModelBase.LevelDataService.UpdateCounter(_openedLevel, CounterType.Success);
        LevelComplete();
    }
    
    public void SetCurrentLevelComplete(ILevelScore levelscore = null)
    {
        //PlayerModelBase.LevelDataService.UpdateCounter(_openedLevel, CounterType.Success);
        LevelComplete();
    }
    
    // EndOfLevelScreenController'dan gerçek score controller'ı almak için
    public void SetCurrentLevelCompleteWithScore(RhythmGameScoreController scoreController)
    {
        // RhythmGame için özel score override
        if (scoreController != null && scoreController.PlayerScore > 0)
        {
            // LevelDataService'e custom score gönder
            var levelDataServiceImpl = LevelDataService as LevelDataService;
            if (levelDataServiceImpl != null)
            {
                // Reflection ile _customScoreOverride field'ını set et
                var field = levelDataServiceImpl.GetType().GetField("_customScoreOverride", 
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (field != null)
                {
                    field.SetValue(levelDataServiceImpl, scoreController.PlayerScore);
                }
            }
        }
        LevelComplete();
    }
    
    public static void LevelComplete(ILevelScore levelscore = null)
    {
        // PlayerModel.CompleteLevel(_openedLevel, _openedLevelworldId); 
        // onLevelComplete?.Invoke();

        // Mevcut PlayerModel sistemini koru

        PlayerModel.CompleteLevel(_openedLevel, _openedWorldIndex.ToString());
        
        if(levelscore != null)
            PlayerModel.SetLevelScoreData(_openedLevel, levelscore, _openedLevelworldId);
        
        // LevelDataService'e level complete bilgisini kaydet
        // if (_openedLevel != -1)
        // {
        //     LevelDataService.SetLevelCompleted(_openedLevel);
        //     
        //     // Student endpoint'e veri gönder
        //     SendStudentUpdate(_openedLevel, _openedLevelworldId, levelscore = null);
        // }

        onLevelComplete?.Invoke();
    }

    public static void SendStudentUpdate(int levelIndex, string worldId, ILevelScore levelscore)
    {
        bool star2 = false;
        bool star1 = false;
        bool star3 = false;
        
        // StudentUpdateEndpoint instance'ını kontrol et ve gerekirse oluştur
        if (StudentUpdateEndpoint.Instance == null)
        {
            Debug.Log("StudentUpdateEndpoint instance oluşturuluyor...");
            GameObject endpointGO = new GameObject("StudentUpdateEndpoint");
            endpointGO.AddComponent<StudentUpdateEndpoint>();
            UnityEngine.Object.DontDestroyOnLoad(endpointGO);
        }
        
        if (StudentUpdateEndpoint.Instance == null)
        {
            Debug.LogError("StudentUpdateEndpoint instance oluşturulamadı!");
            return;
        }
        
        try
        {
            // Şu anki level ve world bilgilerini hesapla
            int currentLevel = levelIndex;
            int currentWorld = int.TryParse(worldId, out int world) ? world : 1;
            
            // Şu anki zamanı al
            string lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            
            // Toplam oyun süresini al (PlayerModel'den - şimdilik mock değer)
            int totalTime = 300; // Mock değer - 5 dakika
            
            // Detaylı level bilgilerini hazırla - LevelDataService'den al
            List<StudentUpdateEndpoint.DetailedLevelInfo> detailedLevelInfo = new List<StudentUpdateEndpoint.DetailedLevelInfo>();
            
            // Dictionary<int, LevelFirebaseData> levelsDic = LevelDataService.LevelData.levels;
            // detailedLevelInfo = levelsDic.ToList();
            
            var levelData = LevelDataService.LevelData;
            string key = LevelKeyUtil.LevelKey(_openedWorldIndex, levelIndex);
          if (levelData?.levels != null && levelData.levels.ContainsKey(key))
          {
                var level = levelData.levels[key];

                string currentScene = SceneManager.GetActiveScene().name;

                if (currentScene == "XMLMP3")
                {
                    var rhythmScore = levelscore as RhythmGameScoreController;
                    
                    if (rhythmScore.PlayerStars == 1)
                    {
                        star1 = true;
                    }
                    else if (rhythmScore.PlayerStars == 2)
                    {
                        star1 = true;
                        star2 = true;
                    }
                    else if(rhythmScore.PlayerStars == 3)
                    {
                        star1 = true;
                        star2 = true;
                        star3 = true;
                    }
                }
                else
                {
                    var result = levelscore as LevelScore;
                    
                    if (result.totalPoints >= 30 && result.totalPoints < 60)
                    {
                        star1 = true;
                    }
                    else if (result.totalPoints >= 60 && result.totalPoints < 90)
                    {
                        star1 = true;
                        star2 = true;
                    }
                    else if (result.totalPoints >= 90)
                    {
                        star1 = true;
                        star2 = true;
                        star3 = true;
                    }
                }
                
                // LevelDataService'den gelen gerçek veriler
                var detailedInfo = new StudentUpdateEndpoint.DetailedLevelInfo(
                    world: currentWorld,
                    level: currentLevel,
                    attempts: level.Attempts, // > 0 ? level.Attempts : level.Repetition, // Attempts varsa onu, yoksa Repetition
                    fails: level.Fails,
                    maxScore: level.MaxScore,
                    isCorrect: level.Success > 0 || level.MaxScore > 0, // Success varsa veya Score varsa başarılı
                    averageAccuracy: level.Attempts > 0 ?
                                   (float)level.Success / level.Attempts :
                                   (level.Success + level.Fails > 0 ? (float)level.Success / (level.Success + level.Fails) : 1.0f),
                    star1: star1,
                    star2: star2,
                    star3: star3
                );
                
                detailedLevelInfo.Add(detailedInfo);
                Debug.Log($"LevelDataService verisi kullanıldı - Level: {currentLevel}, Score: {level.MaxScore}, Attempts: {level.Attempts}, Success: {level.Success}, Failure: {level.Fails}");
            }
            else
            {
                // Fallback - minimal veri (level henüz LevelDataService'e kaydedilmemişse)
                var detailedInfo = new StudentUpdateEndpoint.DetailedLevelInfo(
                    world: currentWorld,
                    level: currentLevel,
                    attempts: 1,
                    fails: 0,
                    maxScore: 100,
                    isCorrect: true,
                    averageAccuracy: 1.0f,
                    star1: true,
                    star2: true,
                    star3: true
                );
                
                detailedLevelInfo.Add(detailedInfo);
                Debug.LogWarning("LevelDataService'de veri bulunamadı, fallback kullanıldı - Level: " + currentLevel);
            }
            
            // Endpoint'i çağır
            StudentUpdateEndpoint.Instance.UpdateStudentInfo(
                studentId: "STU-8031", // Mock student ID
                currentLevel: currentLevel,
                currentWorld: currentWorld,
                lastTimePlayed: lastTimePlayed,
                totalTime: totalTime,
                detailedLevelInfo: detailedLevelInfo,
                callback: OnStudentUpdateCallback
            );
            
            Debug.Log($"Student update gönderildi - Level: {currentLevel}, World: {currentWorld}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Student update gönderilirken hata: {e.Message}");
        }
    }
    
    private static void OnStudentUpdateCallback(bool success, string message)
    {
        if (success)
        {
            Debug.Log($"Student update başarılı: {message}");
        }
        else
        {
            Debug.LogError($"Student update başarısız: {message}");
        }
    }
}
