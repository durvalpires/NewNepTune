using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class StudentUpdateEndpoint : MonoBehaviour
{
    public static StudentUpdateEndpoint Instance { get; private set; }
    
    private const string PROXY_BASE_URL = "https://neptuneserver.vercel.app";
    private const string UPDATE_STUDENT_INFO_ENDPOINT = "/student/updateInfo";
    
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void UpdateStudentInfo(string studentId, int levelPlayed, int worldPlayed, 
                                 string lastTimePlayed, int totalTime, 
                                 List<DetailedLevelInfo> detailedLevelInfo = null, 
                                 Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(studentId))
        {
            Debug.LogError("Öğrenci ID boş olamaz!");
            callback?.Invoke(false, "Öğrenci ID boş olamaz!");
            return;
        }
        
        StartCoroutine(UpdateStudentInfoCoroutine(studentId, levelPlayed, worldPlayed, 
                                                 lastTimePlayed, totalTime, detailedLevelInfo, callback));
    }
    
    private IEnumerator UpdateStudentInfoCoroutine(string studentId, int levelPlayed, int worldPlayed, 
                                                  string lastTimePlayed, int totalTime, 
                                                  List<DetailedLevelInfo> detailedLevelInfo, 
                                                  Action<bool, string> callback)
    {
        string detailedInfoJson = "null";
        if (detailedLevelInfo != null && detailedLevelInfo.Count > 0)
        {
            detailedInfoJson = "[";
            for (int i = 0; i < detailedLevelInfo.Count; i++)
            {
                var levelInfo = detailedLevelInfo[i];
                var levelData = levelInfo.levelData;

                detailedInfoJson += "{"
                                    + "\"world\":" + levelInfo.world + ","
                                    + "\"level\":" + levelInfo.level + ","
                                    + "\"attempts\":" + levelData.Attempts + ","
                                    + "\"successes\":" + levelData.Successes + ","
                                    + "\"maxScore\":" + levelData.MaxScore + ","
                                    + "\"successRate\":" + (levelData.Attempts != 0 ? (float)levelData.Successes / levelData.Attempts : 0f) + ","
                                    + "\"stars\":" + (levelData.StarRating.HasValue ? levelData.StarRating.Value.ToString() : "null");

                // Append HitAccuracy if not null
                if (levelData.HitAccuracy != null)
                {
                    detailedInfoJson += ",\"hitAccuracy\":{";

                    int hitCount = 0;
                    foreach (var hit in levelData.HitAccuracy)
                    {
                        detailedInfoJson += "\"" + hit.Key.ToString() + "\":" + hit.Value;
                        hitCount++;
                        if (hitCount < levelData.HitAccuracy.Count)
                        {
                            detailedInfoJson += ",";
                        }
                    }

                    detailedInfoJson += "}";
                }

                detailedInfoJson += "}";

                if (i < detailedLevelInfo.Count - 1)
                {
                    detailedInfoJson += ",";
                }
            }
            detailedInfoJson += "]";
        }
        
        string updateStudentJson = "{"
            + "\"studentId\":\"" + studentId + "\","
            + "\"currentLevel\":" + levelPlayed + ","
            + "\"currentWorld\":" + worldPlayed + ","
            + "\"lastTimePlayed\":\"" + lastTimePlayed + "\","
            + "\"totalTime\":" + totalTime + ","
            + "\"detailedLevelInfo\":" + detailedInfoJson
            + "}";
            
        string proxyUrl = PROXY_BASE_URL + UPDATE_STUDENT_INFO_ENDPOINT;
        
        Debug.Log("Öğrenci bilgileri güncelleniyor: " + proxyUrl);
        Debug.Log("İstek içeriği: " + updateStudentJson);
        
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(updateStudentJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Öğrenci bilgileri güncellenirken hata: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "Öğrenci bilgileri güncellenirken bir hata oluştu: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Sunucu yanıtı: " + responseJson);
                
                try
                {
                    UpdateResponse response = JsonUtility.FromJson<UpdateResponse>(responseJson);
                    
                    if (!string.IsNullOrEmpty(response.error))
                    {
                        Debug.LogError("Öğrenci bilgileri güncellenirken hata: " + response.error);
                        callback?.Invoke(false, response.error);
                    }
                    else
                    {
                        Debug.Log("Öğrenci bilgileri başarıyla güncellendi! Mesaj: " + response.message);
                        callback?.Invoke(true, response.message);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON parse hatası: " + e.Message);
                    callback?.Invoke(false, "Sunucu yanıtı işlenirken hata oluştu");
                }
            }
        }
    }
    
    [System.Serializable]
    public class DetailedLevelInfo
    {
        public int world;
        public int level;
        public LevelFirebaseData levelData;
        // public int attempts;
        // public int successes;
        // //public int fails;
        // public int maxScore;
        // //public bool isCorrect;
        // [FormerlySerializedAs("averageAccuracy")] public float successRate;
        // public int stars;
        
        public DetailedLevelInfo(int world, int level, LevelFirebaseData levelData/*, int attempts, int successes, int maxScore, 
            float successRate, int starAmount*/)
        {
            this.world = world;
            this.level = level;
            this.levelData = levelData;
            // this.attempts = attempts;
            // this.successes = successes;
            // //this.fails = fails;
            // this.maxScore = maxScore;
            // //this.isCorrect = isCorrect;
            // this.successRate = successRate;
            // this.stars = starAmount;
        }
    }
    
    [System.Serializable]
    private class UpdateResponse
    {
        public string message;
        public string error;
        public string studentId;
    }
} 