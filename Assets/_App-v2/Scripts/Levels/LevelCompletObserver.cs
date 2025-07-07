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
        //PlayerModelBase.LevelDataService.UpdateCounter(_openedWorldIndex, _openedLevel, CounterType.Success);
        LevelComplete();
    }

    public void SetCurrentLevelComplete(ILevelScore levelscore = null)
    {
        LevelComplete(levelscore);
    }

    // EndOfLevelScreenController'dan gerçek score controller'ı almak için
    public void SetCurrentLevelCompleteWithScore(RhythmGameScoreController scoreController)
    {
        if (scoreController != null && scoreController.PlayerScore > 0)
        {
            PlayerModelBase.SetCustomScore(scoreController.PlayerScore);
            
            // ✅ Accuracy verilerini PlayerModelBase.LevelData'ya kaydet
            //LevelDataService.SetLevelScoreData(_openedLevel, scoreController, _openedWorldIndex.ToString());
        }
        LevelComplete();
        //// RhythmGame için özel score override
        //if (scoreController != null && scoreController.PlayerScore > 0)
        //{
        //    // LevelDataService'e custom score gönder
        //    var levelDataServiceImpl = LevelDataService as LevelDataService;
        //    if (levelDataServiceImpl != null)
        //    {
        //        // Reflection ile _customScoreOverride field'ını set et
        //        var field = levelDataServiceImpl.GetType().GetField("_customScoreOverride", 
        //            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        //        if (field != null)
        //        {
        //            field.SetValue(levelDataServiceImpl, scoreController.PlayerScore);
        //        }
        //    }
        //}
        //LevelComplete();
    }

    public static void LevelComplete(ILevelScore levelscore = null)
    {
        PlayerModel.CompleteLevel(_openedLevel, _openedWorldIndex.ToString());

        if (_openedLevel != -1)
        {
            SendStudentUpdate(_openedLevel, _openedWorldIndex.ToString(), levelscore);
        }

        onLevelComplete?.Invoke();

        // PlayerModel.CompleteLevel(_openedLevel, _openedLevelworldId); 
        // onLevelComplete?.Invoke();

        // Mevcut PlayerModel sistemini koru
        //PlayerModel.CompleteLevel(_openedLevel, _openedWorldIndex.ToString());

        // LevelDataService'e level complete bilgisini kaydet
        // if (_openedLevel != -1)
        // {
        //     LevelDataService.SetLevelCompleted(_openedLevel);
        //     
        //     // Student endpoint'e veri gönder
        //     SendStudentUpdate(_openedLevel, _openedLevelworldId, levelscore = null);
        // }

        //onLevelComplete?.Invoke();
    }

    public static int sendWorld;
    public static int sendLevel;

    public static int lastWorld;
    public static int lastLevel;

    public static void SendStudentUpdate(int levelIndex, string worldId, ILevelScore levelscore)
    {



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
            int currentLevel = levelIndex;
            int currentWorld = int.TryParse(worldId, out int world) ? world : 1;

            // Başlangıç logu
            Debug.Log($"DENEME[CHECK] current: W{currentWorld} L{currentLevel} | last: W{lastWorld} L{lastLevel}");

            if (currentWorld > lastWorld)
            {
                // Daha büyük world → kesin ilerleme var
                sendWorld = currentWorld;
                sendLevel = currentLevel;

                lastWorld = currentWorld;
                lastLevel = currentLevel;

                Debug.Log($"DENEME[NEW WORLD] İlerleme var. Yeni kayıt: World {sendWorld}, Level {sendLevel}");
            }
            else if (currentWorld == lastWorld && currentLevel > lastLevel)
            {
                // Aynı world ama daha yüksek level → ilerleme var
                sendWorld = currentWorld;
                sendLevel = currentLevel;

                lastWorld = currentWorld;
                lastLevel = currentLevel;

                Debug.Log($"DENEME[NEW LEVEL] Aynı world. Yeni kayıt: World {sendWorld}, Level {sendLevel}");
            }
            else
            {
                // Daha düşük world veya level → ilerleme yok, kayıt güncellenmez
                Debug.Log($"DENEME[NO PROGRESS] Geriye gidildi ya da aynı seviye. Kayıtlı kalan: World {lastWorld}, Level {lastLevel}");
            }


            string lastTimePlayed = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

            int totalTime = 300;

            List<StudentUpdateEndpoint.DetailedLevelInfo> detailedLevelInfo = new List<StudentUpdateEndpoint.DetailedLevelInfo>();

            var levelData = PlayerModelBase.LevelData;

            string levelKey = LevelKeyUtil.LevelKey(currentWorld, currentLevel);

            foreach (var key in levelData.levels.Keys)
            {
                Debug.Log($"[DEBUG] Mevcut level key: {key}");
            }

            if (levelData?.levels != null && levelData.levels.ContainsKey(levelKey))
            {
                var level = levelData.levels[levelKey];
                //if (levelData?.levels != null && levelData.levels.ContainsKey(levelIndex.ToString()))
                //{
                //var level = levelData.levels[levelIndex.ToString()];
                //string currentScene = SceneManager.GetActiveScene().name;

                //CalculateStarRating(levelscore, currentScene, out star1, out star2, out star3);

                // ✅ Accuracy breakdown verilerini çek ve string key'lere çevir
                Dictionary<string, float> accuracyBreakdownForJson = null;
                if (level.HitAccuracy != null && level.HitAccuracy.Count > 0)
                {
                    accuracyBreakdownForJson = new Dictionary<string, float>();
                    foreach (var kvp in level.HitAccuracy)
                    {
                        accuracyBreakdownForJson[kvp.Key.ToString()] = kvp.Value;
                    }
                }

                var detailedInfo = new StudentUpdateEndpoint.DetailedLevelInfo(
                    world: currentWorld,
                    level: currentLevel,
                    attempts: level.Attempts,
                    successes: level.Successes,
                    //fails: level.Fails,
                    maxScore: level.MaxScore,
                    isCorrect: level.Successes > 0 || level.MaxScore > 0,
                    //averageAccuracy: CalculateAverageAccuracy(level),
                    starRating: level.StarRating ?? 0,
                    accuracyBreakdown: accuracyBreakdownForJson
                );

                detailedLevelInfo.Add(detailedInfo);
                Debug.Log($"PlayerModelBase verisi kullanıldı - Level: {currentLevel}, Score: {level.MaxScore}, Attempts: {level.Attempts}, Success: {level.Successes},Stars: {level.StarRating}");
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
            //    Debug.LogWarning("PlayerModelBase'de veri bulunamadı, fallback kullanıldı - Level: " + currentLevel);
            //}

            string studentId = FirebaseProxyService.Instance.privateCode;
            if (string.IsNullOrEmpty(studentId))
            {
                Debug.LogError("Student ID bulunamadı! Önce öğrenci girişi yapınız");
                return;
            }

            StudentUpdateEndpoint.Instance.UpdateStudentInfo(
                studentId: studentId,
                currentLevel: sendLevel,
                currentWorld: sendWorld,
                lastTimePlayed: lastTimePlayed,
                totalTime: totalTime,
                detailedLevelInfo: detailedLevelInfo,
                 callback: OnStudentUpdateCallback
            //callback: (success, message) => {
            //    OnStudentUpdateCallback(success, message);
            //    if (success && isHigherProgress)
            //    {
            //        // Başarılıysa lastProgress'i güncelleyirouz buradan
            //        FirebaseProxyService.Instance.UpdateLastProgress(currentWorld, currentLevel);
            //    }
            //}
            );

            Debug.Log($"Student update gönderildi - Level: {currentLevel}, World: {currentWorld}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Student update gönderilirken hata: {e.Message}");
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
            Debug.Log($"Student update başarılı: {message}");
        }
        else
        {
            Debug.LogError($"Student update başarısız: {message}");
        }
    }

    //private static bool IsHigherProgress(int newWorld, int newLevel, int lastWorld, int lastLevel)
    //{
    //    // Önce world karşılaştır
    //    if (newWorld > lastWorld) return true;
    //    if (newWorld < lastWorld) return false;

    //    // Eğerki worlder eşitse de levellerı karşılaştırıyorum , öncelik worldde sonra levelda 
    //    return newLevel > lastLevel;
    //}
}