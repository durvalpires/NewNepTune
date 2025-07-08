using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class StudentUpdateEndpoint : MonoBehaviour
{
    public static StudentUpdateEndpoint Instance { get; private set; }
    
    private const string PROXY_BASE_URL = "https://np-proxy-utkulondons-projects.vercel.app";
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
    
    public void UpdateStudentInfo(string studentId, int currentLevel, int currentWorld, 
                                 string lastTimePlayed, int totalTime, 
                                 List<DetailedLevelInfo> detailedLevelInfo = null, 
                                 Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(studentId))
        {
            Debug.LogError("Student ID cannot be empty!");
            callback?.Invoke(false, "Student ID cannot be empty!");
            return;
        }
        
        StartCoroutine(UpdateStudentInfoCoroutine(studentId, currentLevel, currentWorld, 
                                                 lastTimePlayed, totalTime, detailedLevelInfo, callback));
    }
    
    private IEnumerator UpdateStudentInfoCoroutine(string studentId, int currentLevel, int currentWorld, 
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
                
                detailedInfoJson += "{"
                    + "\"world\":" + levelInfo.world + ","
                    + "\"level\":" + levelInfo.level + ","
                    + "\"attempts\":" + levelInfo.attempts + ","
                    + "\"successes\":" + levelInfo.successes + ","
                    //+ "\"fails\":" + levelInfo.fails + ","
                    + "\"maxScore\":" + levelInfo.maxScore + ","
                    + "\"isCorrect\":" + levelInfo.isCorrect.ToString().ToLower() + ","
                    //+ "\"averageAccuracy\":" + levelInfo.averageAccuracy.ToString(System.Globalization.CultureInfo.InvariantCulture) + ","
                    + "\"accuracyBreakdown\":" + ConvertAccuracyBreakdownToJson(levelInfo.accuracyBreakdown) + ","
                    + "\"starRating\":" + levelInfo.starRating
                    + "}";
                
                if (i < detailedLevelInfo.Count - 1)
                    detailedInfoJson += ",";
            }
            detailedInfoJson += "]";
        }
        
        string updateStudentJson = "{"
            + "\"studentId\":\"" + studentId + "\","
            + "\"currentLevel\":" + currentLevel + ","
            + "\"currentWorld\":" + currentWorld + ","
            + "\"lastTimePlayed\":\"" + lastTimePlayed + "\","
            + "\"totalTime\":" + totalTime + ","
            + "\"detailedLevelInfo\":" + detailedInfoJson
            + "}";
            
        string proxyUrl = PROXY_BASE_URL + UPDATE_STUDENT_INFO_ENDPOINT;
        
        Debug.Log("Updating student information: " + proxyUrl);
        Debug.Log("Request content: " + updateStudentJson);
        
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(updateStudentJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating student information: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "An error occurred while updating student information: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Server response: " + responseJson);
                
                try
                {
                    UpdateResponse response = JsonUtility.FromJson<UpdateResponse>(responseJson);
                    
                    if (!string.IsNullOrEmpty(response.error))
                    {
                        Debug.LogError("Error updating student information: " + response.error);
                        callback?.Invoke(false, response.error);
                    }
                    else
                    {
                        Debug.Log("Student information successfully updated! Message: " + response.message);
                        callback?.Invoke(true, response.message);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError("JSON parse error: " + e.Message);
                    callback?.Invoke(false, "Error processing server response");
                }
            }
        }
    }
    
    private string ConvertAccuracyBreakdownToJson(Dictionary<string, float> accuracyBreakdown)
    {
        if (accuracyBreakdown == null || accuracyBreakdown.Count == 0)
            return "{}";

        StringBuilder jsonBuilder = new StringBuilder("{");
        foreach (var kvp in accuracyBreakdown)
        {
            jsonBuilder.Append("\"");
            jsonBuilder.Append(kvp.Key);
            jsonBuilder.Append("\":");
            jsonBuilder.Append(kvp.Value.ToString("F2", System.Globalization.CultureInfo.InvariantCulture));
            jsonBuilder.Append(",");
        }
        jsonBuilder.Length--; // Remove the last comma
        jsonBuilder.Append("}");
        return jsonBuilder.ToString();
    }
    
    [System.Serializable]
    public class DetailedLevelInfo
    {
        public int world;
        public int level;
        public int attempts;
        public int successes;
        //public int fails;
        public int maxScore;
        public bool isCorrect;
        //public float averageAccuracy;
        public Dictionary<string, float> accuracyBreakdown;
        public int starRating;
        
        public DetailedLevelInfo(int world, int level, int attempts, int successes, int maxScore, 
                               bool isCorrect, int starRating,
                               Dictionary<string, float> accuracyBreakdown = null)
        {
            this.world = world;
            this.level = level;
            this.attempts = attempts;
            this.successes = successes;
            //this.fails = fails;
            this.maxScore = maxScore;
            this.isCorrect = isCorrect;
            //this.averageAccuracy = averageAccuracy;
            this.accuracyBreakdown = accuracyBreakdown;
            this.starRating = starRating;
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