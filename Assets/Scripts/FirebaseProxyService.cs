using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class FirebaseProxyService : MonoBehaviour
{
    public static FirebaseProxyService Instance { get; private set; }

    private const string PROXY_BASE_URL = "https://neptuneserver.vercel.app";
    private const string REGISTER_ENDPOINT = "/auth/register";
    private const string LOGIN_ENDPOINT = "/auth/login";
    private const string LOGOUT_ENDPOINT = "/auth/logout";
    private const string ADD_STUDENT_ENDPOINT = "/teacher/addStudent";
    private const string GET_TEACHER_STUDENTS_ENDPOINT = "/teacher/getStudents";
    private const string UPDATE_STUDENT_INFO_ENDPOINT = "/student/updateInfo";

    private string _userId;
    private string _authToken;
    private string _userType;
    private string _privateCode;
    private string _teacherPrivateCode;
    private int _currentLevel;
    private int _currentWorld;

    public string UserId => _userId;
    public string AuthToken => _authToken;
    public string UserType => _userType;
    public string privateCode => _privateCode;
    public string teacherPrivateCode => _teacherPrivateCode;
    public int CurrentLevel => _currentLevel;
    public int CurrentWorld => _currentWorld;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_authToken);

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

    public void RegisterUser(string email, string password, Action<bool, string> callback = null)
    {
        StartCoroutine(RegisterUserCoroutine(email, password, callback));
    }

    public void RegisterUserWithProfile(string email, string password, Dictionary<string, object> userProfile, Action<bool, string> callback = null)
    {
        StartCoroutine(RegisterUserWithProfileCoroutine(email, password, userProfile, callback));
    }

    public void LoginUser(string email, string password, Action<bool, string> callback = null)
    {
        StartCoroutine(LoginUserCoroutine(email, password, callback));
    }

    public void Logout()
    {
        _userId = null;
        _authToken = null;
        _userType = null;
        _privateCode = null;
        _teacherPrivateCode = null;
        _currentLevel = 0;
        _currentWorld = 0;

        Debug.Log("Kullanıcı çıkış yaptı.");
    }

    public void LogoutWithBackend(Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_userId) && string.IsNullOrEmpty(_authToken))
        {
            Debug.LogWarning("Zaten çıkış yapmış kullanıcı için logout çağrıldı.");
            // Local temizlik yap
            ClearLocalData();
            callback?.Invoke(true, "Zaten çıkış yapmış");
            return;
        }

        StartCoroutine(LogoutCoroutine(callback));
    }

    private void ClearLocalData()
    {
        _userId = null;
        _authToken = null;
        _userType = null;
        _privateCode = null;
        _teacherPrivateCode = null;
        _currentLevel = 0;
        _currentWorld = 0;

        Debug.Log("Kullanıcı verileri temizlendi.");
    }

    public void AddStudentToTeacher(string studentId, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher" || string.IsNullOrEmpty(_teacherPrivateCode))
        {
            Debug.LogError("Öğrenci eklemek için öğretmen olarak giriş yapmanız gerekir.");
            callback?.Invoke(false, "Öğrenci eklemek için öğretmen olarak giriş yapmanız gerekir.");
            return;
        }

        StartCoroutine(AddStudentToTeacherCoroutine(studentId, callback));
    }

    public void GetTeacherStudents(Action<bool, List<StudentInfo>> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher" || string.IsNullOrEmpty(_teacherPrivateCode))
        {
            Debug.LogError("Öğrenci listesini almak için öğretmen olarak giriş yapmanız gerekir.");
            callback?.Invoke(false, null);
            return;
        }

        StartCoroutine(GetTeacherStudentsCoroutine(callback));
    }

    public void UpdateStudentInfo(string studentId, int currentLevel, int currentWorld, string lastTimePlayed, int totalTime, WorldsData worldsData = null, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher")
        {
            Debug.LogError("Öğrenci bilgilerini güncellemek için öğretmen olarak giriş yapmanız gerekir.");
            callback?.Invoke(false, "Öğrenci bilgilerini güncellemek için öğretmen olarak giriş yapmanız gerekir.");
            return;
        }

        StartCoroutine(UpdateStudentInfoCoroutine(studentId, currentLevel, currentWorld, lastTimePlayed, totalTime, worldsData, callback));
    }

    private IEnumerator RegisterUserCoroutine(string email, string password, Action<bool, string> callback)
    {
        string authJson = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
        string proxyUrl = PROXY_BASE_URL + REGISTER_ENDPOINT;

        Debug.Log("Proxy sunucusuna bağlanılıyor: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Proxy Authentication hatası: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "Kayıt sırasında bir hata oluştu: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy yanıtı: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                _userId = response.userId;
                _userType = response.userType;
                _privateCode = response.privateCode;
                _teacherPrivateCode = response.teacherPrivateCode;

                Debug.Log("Kullanıcı başarıyla kaydedildi! User ID: " + _userId + ", User Type: " + _userType);
                if (!string.IsNullOrEmpty(_privateCode))
                {
                    Debug.Log("Öğrenci Özel Kodu: " + _privateCode);
                }
                if (!string.IsNullOrEmpty(_teacherPrivateCode))
                {
                    Debug.Log("Öğretmen Özel Kodu: " + _teacherPrivateCode);
                }
                callback?.Invoke(true, _userId);
            }
        }
    }

    private IEnumerator RegisterUserWithProfileCoroutine(string email, string password, Dictionary<string, object> userProfile, Action<bool, string> callback)
    {
        string authJson = "{"
            + "\"email\":\"" + email + "\","
            + "\"password\":\"" + password + "\","
            + "\"userProfile\":" + JsonConvert(userProfile)
            + "}";

        string proxyUrl = PROXY_BASE_URL + REGISTER_ENDPOINT;

        Debug.Log("Proxy sunucusuna profil verileriyle bağlanılıyor: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Proxy Authentication hatası: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "Kayıt sırasında bir hata oluştu: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy yanıtı: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                _userId = response.userId;
                _userType = response.userType;
                _privateCode = response.privateCode;
                _teacherPrivateCode = response.teacherPrivateCode;

                Debug.Log("Kullanıcı profil verileriyle başarıyla kaydedildi! User ID: " + _userId + ", User Type: " + _userType);
                if (!string.IsNullOrEmpty(_privateCode))
                {
                    Debug.Log("Öğrenci Özel Kodu: " + _privateCode);
                }
                if (!string.IsNullOrEmpty(_teacherPrivateCode))
                {
                    Debug.Log("Öğretmen Özel Kodu: " + _teacherPrivateCode);
                }
                callback?.Invoke(true, _userId);
            }
        }
    }

    private IEnumerator LoginUserCoroutine(string email, string password, Action<bool, string> callback)
    {
        string authJson = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
        string proxyUrl = PROXY_BASE_URL + LOGIN_ENDPOINT;

        Debug.Log("Proxy sunucusuna giriş için bağlanılıyor: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Proxy Giriş hatası: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "Giriş sırasında bir hata oluştu: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy giriş yanıtı: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                _userId = response.userId;
                _authToken = response.token;
                _userType = response.userType;
                _privateCode = response.privateCode;
                _teacherPrivateCode = response.teacherPrivateCode;
                _currentLevel = response.currentLevel;
                _currentWorld = response.currentWorld;

                LevelCompletObserver.lastLevel = response.currentLevel;
                LevelCompletObserver.lastWorld = response.currentWorld;

                Debug.Log("Kullanıcı başarıyla giriş yaptı! User ID: " + _userId + ", User Type: " + _userType);
                if (!string.IsNullOrEmpty(_privateCode))
                {
                    Debug.Log("Öğrenci Özel Kodu: " + _privateCode);
                    Debug.Log("Current Level: " + _currentLevel + ", Current World: " + _currentWorld);

                    //DENEME
                    PlayerModel.ClearData();

                    for (int i = 0; i <= _currentWorld; i++)
                    {
                        if (i < _currentWorld)
                        {
                            PlayerModel.CompleteWorld(i.ToString());
                        }

                        if (_currentWorld == i)
                        {
                            for (int x = 0; x < _currentLevel; x++)
                            {
                                PlayerModel.CompleteLevel(x, i.ToString());
                            }
                        }
                        else
                        {
                            for (int x = 0; x <= 31; x++)
                            {
                                PlayerModel.CompleteLevel(x, i.ToString());
                            }
                        }
                    }

                    //DENEME
                }
                if (!string.IsNullOrEmpty(_teacherPrivateCode))
                {
                    Debug.Log("Öğretmen Özel Kodu: " + _teacherPrivateCode);
                }
                callback?.Invoke(true, _userId);
            }
        }
    }

    private IEnumerator AddStudentToTeacherCoroutine(string studentId, Action<bool, string> callback)
    {
        string addStudentJson = "{"
            + "\"teacherId\":\"" + _teacherPrivateCode + "\","
            + "\"studentId\":\"" + studentId + "\""
            + "}";

        string proxyUrl = PROXY_BASE_URL + ADD_STUDENT_ENDPOINT;

        Debug.Log("Proxy sunucusuna öğrenci ekleme isteği gönderiliyor: " + proxyUrl);
        Debug.Log("İstek içeriği: " + addStudentJson);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(addStudentJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Öğrenci ekleme hatası: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "Öğrenci ekleme sırasında bir hata oluştu: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy yanıtı: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);

                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Öğrenci ekleme hatası: " + response.error);
                    callback?.Invoke(false, response.error);
                }
                else
                {
                    Debug.Log("Öğrenci başarıyla eklendi! Mesaj: " + response.message);
                    callback?.Invoke(true, response.message);
                }
            }
        }
    }

    private IEnumerator GetTeacherStudentsCoroutine(Action<bool, List<StudentInfo>> callback)
    {
        string getStudentsJson = "{"
            + "\"teacherId\":\"" + _teacherPrivateCode + "\""
            + "}";

        string proxyUrl = PROXY_BASE_URL + GET_TEACHER_STUDENTS_ENDPOINT;

        Debug.Log("Proxy sunucusundan öğrenci listesi alınıyor: " + proxyUrl);
        Debug.Log("İstek içeriği: " + getStudentsJson);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(getStudentsJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Öğrenci listesi alınırken hata: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, null);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy yanıtı: " + responseJson);

                TeacherStudentsResponse response = JsonUtility.FromJson<TeacherStudentsResponse>(responseJson);

                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Öğrenci listesi alınırken hata: " + response.error);
                    callback?.Invoke(false, null);
                }
                else
                {
                    Debug.Log("Öğrenci listesi başarıyla alındı! Öğrenci sayısı: " + response.students.Count);
                    callback?.Invoke(true, response.students);
                }
            }
        }
    }

    private IEnumerator UpdateStudentInfoCoroutine(string studentId, int currentLevel, int currentWorld, string lastTimePlayed, int totalTime, WorldsData worldsData, Action<bool, string> callback)
    {
        string detailedInfoJson = "null";
        if (worldsData != null && worldsData.worlds != null && worldsData.worlds.Count > 0)
        {
            detailedInfoJson = "[";
            bool first = true;

            // Hiyerarşik yapıdan düz array formatına dönüştür (Firebase'de hala eski format) BU BÖLÜM KULLANILMIYOR, ANCAK KALDIRILMAYACAK ŞİMDİLİK, ilerde game trackerden buraya gönderebiliriz belki datayı veya sıfırdan bir endpoint dosyası da oluşturabilirim o zaman bu kısımı kaldırabilirz.
            foreach (var world in worldsData.worlds)
            {
                foreach (var level in world.levels)
                {
                    if (!first) detailedInfoJson += ",";

                    detailedInfoJson += "{"
                        + "\"world\":" + world.worldNumber + ","
                        + "\"level\":" + level.levelNumber + ","
                        + "\"attempts\":" + level.attempts + ","
                        + "\"successes\":" + level.successes + ","
                        + "\"fails\":" + level.fails + ","
                        + "\"maxScore\":" + level.maxScore + ","
                        + "\"isCorrect\":" + level.isCorrect.ToString().ToLower() + ","
                        + "\"averageAccuracy\":" + level.averageAccuracy + ","
                        + "\"starRating\":" + level.starRating
                        + "}";
                    first = false;
                }
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

        Debug.Log("Proxy sunucusuna öğrenci bilgileri güncelleme isteği gönderiliyor: " + proxyUrl);
        Debug.Log("İstek içeriği: " + updateStudentJson);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(updateStudentJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);

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
                Debug.Log("Proxy yanıtı: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);

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
        }
    }

    private IEnumerator LogoutCoroutine(Action<bool, string> callback)
    {
        string logoutJson = "{"
            + "\"userId\":\"" + _userId + "\","
            + "\"token\":\"" + _authToken + "\""
            + "}";

        string proxyUrl = PROXY_BASE_URL + LOGOUT_ENDPOINT;

        Debug.Log("Proxy sunucusuna logout isteği gönderiliyor: " + proxyUrl);
        Debug.Log("İstek içeriği: " + logoutJson);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(logoutJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            if (!string.IsNullOrEmpty(_authToken))
            {
                www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            }

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Logout isteği başarısız: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                
                // Backend'e erişim başarısız olsa bile local temizlik yap
                ClearLocalData();
                callback?.Invoke(false, "Logout isteği başarısız ama local temizlik yapıldı: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Logout yanıtı: " + responseJson);

                try
                {
                    ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);

                    if (!string.IsNullOrEmpty(response.error))
                    {
                        Debug.LogError("Logout backend hatası: " + response.error);
                        // Backend'de hata olsa bile local temizlik yap
                        ClearLocalData();
                        callback?.Invoke(false, response.error);
                    }
                    else
                    {
                        Debug.Log("Logout başarılı! Mesaj: " + response.message);
                        // Başarılı logout sonrası local temizlik yap
                        ClearLocalData();
                        callback?.Invoke(true, response.message);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Logout response parse hatası: " + e.Message);
                    // Parse hatası olsa bile local temizlik yap
                    ClearLocalData();
                    callback?.Invoke(false, "Response parse hatası ama local temizlik yapıldı");
                }
            }
        }
    }

    private string JsonConvert(Dictionary<string, object> dict)
    {
        StringBuilder json = new StringBuilder();
        json.Append("{");

        int count = 0;
        foreach (KeyValuePair<string, object> kvp in dict)
        {
            json.Append("\"" + kvp.Key + "\":");

            if (kvp.Value is Dictionary<string, object>)
            {
                json.Append(JsonConvert(kvp.Value as Dictionary<string, object>));
            }
            else if (kvp.Value is string)
            {
                json.Append("\"" + kvp.Value + "\"");
            }
            else if (kvp.Value is List<int>)
            {
                json.Append("[");
                var list = kvp.Value as List<int>;
                for (int i = 0; i < list.Count; i++)
                {
                    json.Append(list[i]);
                    if (i < list.Count - 1)
                        json.Append(",");
                }
                json.Append("]");
            }
            else
            {
                json.Append(kvp.Value.ToString().ToLower());
            }

            if (count < dict.Count - 1)
            {
                json.Append(",");
            }

            count++;
        }

        json.Append("}");
        return json.ToString();
    }

    [System.Serializable]
    private class ProxyResponse
    {
        public string userId;
        public string message;
        public string error;
        public string token;
        public string userType;
        public string privateCode;
        public string teacherPrivateCode;
        public int currentLevel;
        public int currentWorld;
    }

    [System.Serializable]
    private class TeacherStudentsResponse
    {
        public List<StudentInfo> students;
        public string error;
    }
}

[System.Serializable]
public class StudentInfo
{
    public string studentId;
    public int currentLevel;
    public int currentWorld;
    public string lastTimePlayed;
    public int totalTime;
    public WorldsData worldsData;
}

[System.Serializable]
public class WorldsData
{
    public List<WorldInfo> worlds;
}

[System.Serializable]
public class WorldInfo
{
    public int worldNumber;
    public List<LevelInfo> levels;
}

[System.Serializable]
public class LevelInfo
{
    public int levelNumber;
    public int attempts;
    public int successes;
    public int fails;
    public int maxScore;
    public bool isCorrect; // true = Check göster, false = False göster
    public float averageAccuracy;
    public int starRating;
    public List<AccuracyBreakdownItem> accuracyBreakdown;
}

[System.Serializable]
public class AccuracyBreakdownItem
{
    public string noteName;   // Perfect,great vs buradan gelecek
    public float percentage;  // Yüzde değeri buradan geliyor
}