using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private const string REMOVE_STUDENT_ENDPOINT = "/teacher/removeStudent";
    private const string GET_TEACHER_STUDENTS_ENDPOINT = "/teacher/getStudents";
    private const string GET_AVAILABLE_STUDENTS_ENDPOINT = "/teacher/getAvailableStudents";
    private const string UPDATE_STUDENT_INFO_ENDPOINT = "/student/updateInfo";
    private const string RESET_STUDENT_PROGRESS_ENDPOINT = "/student/resetProgress";
    private const string UPDATE_STUDENT_TOTAL_TIME_ENDPOINT = "/student/updateTotalTime";
    private const string UPDATE_STUDENT_PROFILE_ENDPOINT = "/teacher/updateStudentProfile";
    private const string GET_USER_BY_EMAIL_ENDPOINT = "/getUserByEmail";


    
    private string _userId;
    private string _authToken;
    private string _userType;
    private string _privateCode;
    private string _teacherPrivateCode;
    private string _username;
    private int _currentLevel;
    private int _currentWorld;
    private WorldsData _worldsData;
    private double _totalTime;

    public string UserId => _userId;
    public string AuthToken => _authToken;
    public string UserType => _userType;
    public string PrivateCode => _privateCode;
    public string teacherPrivateCode => _teacherPrivateCode;
    public string Username => _username;
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
        _username = null;
        _currentLevel = 0;
        _currentWorld = 0;

        Debug.Log("User logged out.");
    }

    public void LogoutWithBackend(Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_userId) && string.IsNullOrEmpty(_authToken))
        {
            Debug.LogWarning("Logout called for already logged out user.");
            // Local cleanup
            ClearLocalData();
            callback?.Invoke(true, "Already logged out");
            return;
        }

        StartCoroutine(LogoutCoroutine(callback));
    }

    public void ResetProgress(Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_userId) || string.IsNullOrEmpty(_authToken) || 
            _userType != "student" )
        {
            Debug.LogWarning("You must be logged in as a student to reset progress.");
            callback?.Invoke(true, "Not logged in (or not a student).");
            return;
        }

        StartCoroutine(ResetProgressCoroutine(callback));
    }

    private void ClearLocalData()
    {
        _userId = null;
        _authToken = null;
        _userType = null;
        _privateCode = null;
        _teacherPrivateCode = null;
        _username = null;
        _currentLevel = 0;
        _currentWorld = 0;

        Debug.Log("User data cleared.");
    }

    public void AddStudentToTeacher(string studentIdentifier, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher" || string.IsNullOrEmpty(_teacherPrivateCode))
        {
            Debug.LogError("You must be logged in as a teacher to add a student.");
            callback?.Invoke(false, "You must be logged in as a teacher to add a student.");
            return;
        }
        if (string.IsNullOrEmpty(studentIdentifier) || !studentIdentifier.StartsWith("STU-"))
        {
            Debug.LogError("Student code must start with STU-.");
            callback?.Invoke(false, "Student code must start with STU-.");
            return;
        }
        StartCoroutine(AddStudentToTeacherCoroutine(studentIdentifier, callback));
    }

    public void RemoveStudentFromTeacher(string studentIdentifier, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher" || string.IsNullOrEmpty(_teacherPrivateCode))
        {
            Debug.LogError("You must be logged in as a teacher to remove a student.");
            callback?.Invoke(false, "You must be logged in as a teacher to remove a student.");
            return;
        }
        if (string.IsNullOrEmpty(studentIdentifier) || !studentIdentifier.StartsWith("STU-"))
        {
            Debug.LogError("Student code must start with STU-.");
            callback?.Invoke(false, "Student code must start with STU-.");
            return;
        }
        StartCoroutine(RemoveStudentFromTeacherCoroutine(studentIdentifier, callback));
    }

    public void GetTeacherStudents(Action<bool, List<StudentInfo>> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher" || string.IsNullOrEmpty(_teacherPrivateCode))
        {
            Debug.LogError("You must be logged in as a teacher to get the student list.");
            callback?.Invoke(false, null);
            return;
        }

        StartCoroutine(GetTeacherStudentsCoroutine(callback));
    }

    public void GetAvailableStudents(Action<bool, List<AvailableStudentInfo>> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher")
        {
            Debug.LogError("You must be logged in as a teacher to get available students.");
            callback?.Invoke(false, null);
            return;
        }

        StartCoroutine(GetAvailableStudentsCoroutine(callback));
    }

    public void UpdateStudentInfo(string studentId, int currentLevel, int currentWorld, string lastTimePlayed, int totalTime, WorldsData worldsData = null, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher")
        {
            Debug.LogError("You must be logged in as a teacher to update student information.");
            callback?.Invoke(false, "You must be logged in as a teacher to update student information.");
            return;
        }

        StartCoroutine(UpdateStudentInfoCoroutine(studentId, currentLevel, currentWorld, lastTimePlayed, totalTime, worldsData, callback));
    }

    public void UpdateStudentTotalTime(int totalTime, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_privateCode) || string.IsNullOrEmpty(_authToken))
        {
            Debug.LogError("Student must be logged in to update student total time.");
            callback?.Invoke(false, "Student must be logged in to update student total time.");
            return;
        }

        StartCoroutine(UpdateStudentTotalTimeCoroutine(_privateCode, totalTime, callback));
    }

    public void UpdateStudentProfile(string studentId, string username, string year, string class_, Action<bool, string> callback = null)
    {
        if (string.IsNullOrEmpty(_authToken) || _userType != "teacher" || string.IsNullOrEmpty(_teacherPrivateCode))
        {
            Debug.LogError("You must be logged in as a teacher to update student profile.");
            callback?.Invoke(false, "You must be logged in as a teacher to update student profile.");
            return;
        }
        
        if (string.IsNullOrEmpty(studentId) || !studentId.StartsWith("STU-"))
        {
            Debug.LogError("Student ID must start with STU-.");
            callback?.Invoke(false, "Student ID must start with STU-.");
            return;
        }

        StartCoroutine(UpdateStudentProfileCoroutine(studentId, username, year, class_, callback));
    }

    public void AdminGetUserByEmail(string email, Action<bool, AdminUserSearchResult> callback = null)
    {
        if (string.IsNullOrEmpty(email))
        {
            Debug.LogError("Email is required for search.");
            callback?.Invoke(false, null);
            return;
        }

        StartCoroutine(AdminGetUserByEmailCoroutine(email, callback));
    }

    private IEnumerator RegisterUserCoroutine(string email, string password, Action<bool, string> callback)
    {
        string authJson = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
        string proxyUrl = PROXY_BASE_URL + REGISTER_ENDPOINT;

        Debug.Log("Connecting to proxy server: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Proxy Authentication error: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "An error occurred during registration: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                _userId = response.userId;
                _userType = response.userType;
                _privateCode = response.privateCode;
                _teacherPrivateCode = response.teacherPrivateCode;
                _username = response.username;

                Debug.Log("User successfully registered! User ID: " + _userId + ", User Type: " + _userType + ", Username: " + _username);
                if (!string.IsNullOrEmpty(_privateCode))
                {
                    Debug.Log("Student Private Code: " + _privateCode);
                }
                if (!string.IsNullOrEmpty(_teacherPrivateCode))
                {
                    Debug.Log("Teacher Private Code: " + _teacherPrivateCode);
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

        Debug.Log("Connecting to proxy server with profile data: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Proxy Authentication error: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "An error occurred during registration: " + www.downloadHandler.text);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                _userId = response.userId;
                _userType = response.userType;
                _privateCode = response.privateCode;
                _teacherPrivateCode = response.teacherPrivateCode;
                _username = response.username;

                Debug.Log("User successfully registered with profile data! User ID: " + _userId + ", User Type: " + _userType + ", Username: " + _username);
                if (!string.IsNullOrEmpty(_privateCode))
                {
                    Debug.Log("Student Private Code: " + _privateCode);
                }
                if (!string.IsNullOrEmpty(_teacherPrivateCode))
                {
                    Debug.Log("Teacher Private Code: " + _teacherPrivateCode);
                }
                callback?.Invoke(true, _userId);
            }
        }
    }

    private IEnumerator LoginUserCoroutine(string email, string password, Action<bool, string> callback)
    {
        string authJson = "{\"email\":\"" + email + "\",\"password\":\"" + password + "\"}";
        string proxyUrl = PROXY_BASE_URL + LOGIN_ENDPOINT;

        Debug.Log("Connecting to proxy server for login: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Proxy Login error: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "An error occurred during login: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy login response: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                _userId = response.userId;
                _authToken = response.token;
                _userType = response.userType;
                _privateCode = response.privateCode;
                _teacherPrivateCode = response.teacherPrivateCode;
                _username = response.username;
                _currentLevel = response.currentLevel;
                _currentWorld = response.currentWorld;
                _worldsData = response.detailedScores;
                _totalTime = response.totalTime;
                
                LevelCompletObserver.currentLevel = response.currentLevel;
                LevelCompletObserver.currentWorld = response.currentWorld;
                SessionTimeTracker.StartTracking(_totalTime);

                Debug.Log("User successfully logged in! User ID: " + _userId + ", User Type: " + _userType + ", Username: " + _username);
                if (!string.IsNullOrEmpty(_privateCode))
                {
                    Debug.Log("Student Private Code: " + _privateCode);
                    Debug.Log("Current Level: " + _currentLevel + ", Current World: " + _currentWorld);

                    //TEST
                    PlayerModel.ClearData();

                    for (int i = 0; i <= _currentWorld; i++)
                    {
                        WorldInfo worldToLoad = null;
                        if(_worldsData != null && _worldsData.worlds != null && 
                           _worldsData.worlds.Count > i && _worldsData.worlds[i] != null)
                        {
                            Debug.Log("World data found for world index: " + i);
                            worldToLoad = _worldsData.worlds[i];
                        }
                        
                        if (i < _currentWorld)
                        {
                            PlayerModel.CompleteWorld(i.ToString());
                        }

                        if (_currentWorld == i)
                        {
                            for (int x = 0; x <= _currentLevel; x++)
                            {
                                if(worldToLoad != null && worldToLoad.levels != null && worldToLoad.levels.Count > 0)
                                {
                                    var levelToLoad = worldToLoad.levels.FirstOrDefault(level => level.levelNumber == x);
                                    if (levelToLoad != null)
                                    {
                                        if(x < _currentLevel)
                                        {
                                            PlayerModel.CompleteLevel(levelToLoad.levelNumber, i, levelToLoad, true);
                                        }
                                        else
                                        {
                                            PlayerModel.CompleteUnfinishedLevel(levelToLoad.levelNumber, i, levelToLoad);
                                        }
                                    }
                                    else if(x < _currentLevel)
                                    {
                                        PlayerModel.CompleteLevel(x, i.ToString());
                                    }
                                }
                                else if(x < _currentLevel)
                                {
                                    PlayerModel.CompleteLevel(x, i.ToString());
                                }
                                //PlayerModel.CompleteLevel(x, i.ToString());
                            }
                        }
                        else
                        {
                            for (int x = 0; x < PlayerModel.AllWorlds.worldsConfigs[i].levels.Length; x++)
                            {
                                if(worldToLoad != null && worldToLoad.levels != null && worldToLoad.levels.Count > 0)
                                {
                                    var levelToLoad = worldToLoad.levels.FirstOrDefault(level => level.levelNumber == x);
                                    if (levelToLoad != null)
                                    {
                                        Debug.LogWarning("Level data found for level index: " + x);
                                        PlayerModel.CompleteLevel(levelToLoad.levelNumber, i, levelToLoad);
                                    }
                                    else
                                    {
                                        Debug.LogWarning("Level data not found for level index: " + x);
                                        PlayerModel.CompleteLevel(x, i.ToString());
                                    }
                                }
                                else
                                {
                                    Debug.LogWarning("World data not found for world index: " + i);
                                    PlayerModel.CompleteLevel(x, i.ToString());
                                }
                            }
                        }
                        PlayerModelBase.LevelDataService.PrintAllPlayerLevelData();
                    }

                }
                if (!string.IsNullOrEmpty(_teacherPrivateCode))
                {
                    Debug.Log("Teacher Private Code: " + _teacherPrivateCode);
                }
                callback?.Invoke(true, _userId);
            }
        }
    }

    // Updated coroutine to support both code and username
    private IEnumerator AddStudentToTeacherCoroutine(string studentIdentifier, Action<bool, string> callback)
    {
        string addStudentJson = "{" +
            "\"teacherId\":\"" + _teacherPrivateCode + "\"," +
            "\"studentId\":\"" + studentIdentifier + "\"" +
            "}";
        Debug.Log("Adding student by code: " + studentIdentifier);
        string proxyUrl = PROXY_BASE_URL + ADD_STUDENT_ENDPOINT;
        Debug.Log("Sending student addition request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + addStudentJson);
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
                Debug.LogError("Student addition error: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                string actualErrorMessage = www.error;
                try
                {
                    ProxyResponse errorResponse = JsonUtility.FromJson<ProxyResponse>(www.downloadHandler.text);
                    if (!string.IsNullOrEmpty(errorResponse.error))
                    {
                        actualErrorMessage = errorResponse.error;
                        Debug.Log("Parsed backend error message: " + actualErrorMessage);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Could not parse backend error response: " + e.Message);
                }
                callback?.Invoke(false, actualErrorMessage);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);
                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Student addition error: " + response.error);
                    callback?.Invoke(false, response.error);
                }
                else
                {
                    Debug.Log("Student successfully added! Message: " + response.message);
                    callback?.Invoke(true, response.message);
                }
            }
        }
    }

    // Updated coroutine to support both code and username
    private IEnumerator RemoveStudentFromTeacherCoroutine(string studentIdentifier, Action<bool, string> callback)
    {
        string removeStudentJson = "{" +
            "\"teacherId\":\"" + _teacherPrivateCode + "\"," +
            "\"studentId\":\"" + studentIdentifier + "\"" +
            "}";
        Debug.Log("Removing student by code: " + studentIdentifier);
        string proxyUrl = PROXY_BASE_URL + REMOVE_STUDENT_ENDPOINT;
        Debug.Log("Sending student removal request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + removeStudentJson);
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(removeStudentJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);
            yield return www.SendWebRequest();
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Student removal error: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                string actualErrorMessage = www.error;
                try
                {
                    ProxyResponse errorResponse = JsonUtility.FromJson<ProxyResponse>(www.downloadHandler.text);
                    if (!string.IsNullOrEmpty(errorResponse.error))
                    {
                        actualErrorMessage = errorResponse.error;
                        Debug.Log("Parsed backend error message: " + actualErrorMessage);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Could not parse backend error response: " + e.Message);
                }
                callback?.Invoke(false, actualErrorMessage);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);
                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Student removal error: " + response.error);
                    callback?.Invoke(false, response.error);
                }
                else
                {
                    Debug.Log("Student successfully removed! Message: " + response.message);
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

        Debug.Log("Getting student list from proxy server: " + proxyUrl);
        Debug.Log("Request content: " + getStudentsJson);

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
                Debug.LogError("Error getting student list: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, null);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);

                TeacherStudentsResponse response = JsonUtility.FromJson<TeacherStudentsResponse>(responseJson);

                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Error getting student list: " + response.error);
                    callback?.Invoke(false, null);
                }
                else
                {
                    Debug.Log("Student list successfully retrieved! Number of students: " + response.students.Count);
                    callback?.Invoke(true, response.students);
                }
            }
        }
    }

    private IEnumerator GetAvailableStudentsCoroutine(Action<bool, List<AvailableStudentInfo>> callback)
    {
        string proxyUrl = PROXY_BASE_URL + GET_AVAILABLE_STUDENTS_ENDPOINT;

        Debug.Log("Sending available students request to proxy server: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.Get(proxyUrl))
        {
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error getting available students: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, null);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);

                AvailableStudentsResponse response = JsonUtility.FromJson<AvailableStudentsResponse>(responseJson);

                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Error getting available students: " + response.error);
                    callback?.Invoke(false, null);
                }
                else
                {
                    Debug.Log("Available students successfully retrieved! Number of students: " + response.students.Count);
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

            // Convert hierarchical structure to flat array format (still using old format in Firebase) THIS SECTION IS NOT USED, BUT WILL NOT BE REMOVED FOR NOW, maybe we can send data from game tracker here in the future or create a new endpoint file from scratch, then we can remove this part.
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
                        //+ "\"fails\":" + level.fails + ","
                        + "\"maxScore\":" + level.maxScore + ","
                        + "\"isCorrect\":" + (level.successes > 0 ? "true" : "false") + "," 
                        + "\"successRate\":" + level.successRate + ","
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

        Debug.Log("Sending student information update request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + updateStudentJson);

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
                Debug.LogError("Error updating student information: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "An error occurred while updating student information: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);

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
        }
    }

    private IEnumerator UpdateStudentTotalTimeCoroutine(string studentId, int totalTime, Action<bool, string> callback)
    {
        string updateStudentTotalTimeJson = "{"
            + "\"studentId\":\"" + studentId + "\","
            + "\"totalTime\":" + totalTime
            + "}";

        string proxyUrl = PROXY_BASE_URL + UPDATE_STUDENT_TOTAL_TIME_ENDPOINT;

        Debug.Log("Sending student total time update request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + updateStudentTotalTimeJson);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(updateStudentTotalTimeJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", "Bearer " + _authToken);

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error updating student total time: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, "An error occurred while updating student total time: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Proxy response: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);

                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Error updating student total time: " + response.error);
                    callback?.Invoke(false, response.error);
                }
                else
                {
                    Debug.Log("Student total time successfully updated! Message: " + response.message);
                    callback?.Invoke(true, response.message);
                }
            }
        }
    }

    private IEnumerator ResetProgressCoroutine(Action<bool, string> callback)
    {
        string resetProgressJson = "{"
           + "\"studentId\":\"" + _privateCode + "\""
           + "}";
        
        string proxyUrl = PROXY_BASE_URL + RESET_STUDENT_PROGRESS_ENDPOINT;
        
        Debug.Log("Sending reset progress request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + resetProgressJson);
        
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(resetProgressJson);
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
                Debug.LogError("Reset progress request failed: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                
                callback?.Invoke(false, "Reset progress request failed: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Reset progress response: " + responseJson);

                try
                {
                    ProgressResetResponse response = JsonUtility.FromJson<ProgressResetResponse>(responseJson);

                    if (!string.IsNullOrEmpty(response.error))
                    {
                        Debug.LogError("Reset progress request failed due to backend error: " + response.error);
                        callback?.Invoke(false, response.error);
                    }
                    else
                    {
                        _currentLevel = response.resetTo.currentLevel;
                        _currentWorld = response.resetTo.currentWorld;

                        LevelCompletObserver.currentLevel = _currentLevel;
                        LevelCompletObserver.currentWorld = _currentWorld;

                        Debug.Log("User progress successfully resetted! User ID: " + _userId + ", User Type: " + _userType + ", Username: " + _username);
                        Debug.Log("Current Level: " + _currentLevel + ", Current World: " + _currentWorld);

                        //TEST
                        PlayerModel.ClearData();

                        //TODO I THINK NEEDS SHOULD BE ERASED
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
                                // for (int x = 0; x <= 31; x++)
                                // {
                                //     PlayerModel.CompleteLevel(x, i.ToString());
                                // }
                            }
                        }
                        callback?.Invoke(true, response.message);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Reset progress response parse error: " + e.Message);
                    callback?.Invoke(false, "Reset progress response parse error");
                }
            }
        }
    }

    private IEnumerator UpdateStudentProfileCoroutine(string studentId, string username, string year, string class_, Action<bool, string> callback)
    {
        string updateProfileJson = "{"
            + "\"teacherId\":\"" + _teacherPrivateCode + "\","
            + "\"studentId\":\"" + studentId + "\""
            + (string.IsNullOrEmpty(username) ? "" : ",\"username\":\"" + username + "\"")
            + (string.IsNullOrEmpty(year) ? "" : ",\"year\":\"" + year + "\"")
            + (string.IsNullOrEmpty(class_) ? "" : ",\"class_\":\"" + class_ + "\"")
            + "}";

        string proxyUrl = PROXY_BASE_URL + UPDATE_STUDENT_PROFILE_ENDPOINT;

        Debug.Log("Sending student profile update request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + updateProfileJson);

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(updateProfileJson);
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
                Debug.LogError("Error updating student profile: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                
                string actualErrorMessage = "An error occurred while updating student profile: " + www.error;
                try
                {
                    ProxyResponse errorResponse = JsonUtility.FromJson<ProxyResponse>(www.downloadHandler.text);
                    if (!string.IsNullOrEmpty(errorResponse.error))
                    {
                        actualErrorMessage = errorResponse.error;
                        Debug.Log("Parsed backend error message: " + actualErrorMessage);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning("Could not parse backend error response: " + e.Message);
                }
                callback?.Invoke(false, actualErrorMessage);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Student profile update response: " + responseJson);

                ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);
                if (!string.IsNullOrEmpty(response.error))
                {
                    Debug.LogError("Student profile update error: " + response.error);
                    callback?.Invoke(false, response.error);
                }
                else
                {
                    Debug.Log("Student profile successfully updated! Message: " + response.message);
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

        Debug.Log("Sending logout request to proxy server: " + proxyUrl);
        Debug.Log("Request content: " + logoutJson);

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
                Debug.LogError("Logout request failed: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                
                // Perform local cleanup even if backend access fails
                ClearLocalData();
                callback?.Invoke(false, "Logout request failed but local cleanup performed: " + www.error);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Logout response: " + responseJson);

                try
                {
                    ProxyResponse response = JsonUtility.FromJson<ProxyResponse>(responseJson);

                    if (!string.IsNullOrEmpty(response.error))
                    {
                        Debug.LogError("Logout backend error: " + response.error);
                        // Perform local cleanup even if there's a backend error
                        ClearLocalData();
                        callback?.Invoke(false, response.error);
                    }
                    else
                    {
                        Debug.Log("Logout successful! Message: " + response.message);
                        // Perform local cleanup after successful logout
                        ClearLocalData();
                        SessionTimeTracker.StopTracking();
                        callback?.Invoke(true, response.message);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Logout response parse error: " + e.Message);
                    // Perform local cleanup even if there's a parse error
                    ClearLocalData();
                    callback?.Invoke(false, "Response parse error but local cleanup performed");
                }
            }
        }
    }

    private IEnumerator AdminGetUserByEmailCoroutine(string email, Action<bool, AdminUserSearchResult> callback)
    {
        string proxyUrl = PROXY_BASE_URL + GET_USER_BY_EMAIL_ENDPOINT + "?email=" + UnityWebRequest.EscapeURL(email);

        Debug.Log("Admin searching for user by email: " + email);
        Debug.Log("Request URL: " + proxyUrl);

        using (UnityWebRequest www = UnityWebRequest.Get(proxyUrl))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Admin search error: " + www.error);
                Debug.LogError("Response: " + www.downloadHandler.text);
                callback?.Invoke(false, null);
            }
            else
            {
                string responseJson = www.downloadHandler.text;
                Debug.Log("Admin search response: " + responseJson);

                try
                {
                    AdminUserSearchResult result = JsonUtility.FromJson<AdminUserSearchResult>(responseJson);

                    if (!string.IsNullOrEmpty(result.error))
                    {
                        Debug.LogError("Admin search error: " + result.error);
                        callback?.Invoke(false, null);
                    }
                    else
                    {
                        Debug.Log("User found: " + result.username + " (" + result.userType + ")");
                        callback?.Invoke(true, result);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.LogError("Failed to parse admin search response: " + e.Message);
                    callback?.Invoke(false, null);
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
        public string username;
        public int currentLevel;
        public int currentWorld;
        public WorldsData detailedScores;
        public double totalTime;
    }

    [System.Serializable]
    private class TeacherStudentsResponse
    {
        public List<StudentInfo> students;
        public string error;
    }

    [System.Serializable]
    private class AvailableStudentsResponse
    {
        public List<AvailableStudentInfo> students;
        public string error;
    }
}

[System.Serializable]
public class ProgressResetResponse
{
    public string message;
    public string error;
    public string studentId;
    public ResetToData resetTo;

    [System.Serializable]
    public class ResetToData
    {
        public int currentLevel;
        public int currentWorld;
        public int totalTime;
        public string lastTimePlayed;
    }
}

[System.Serializable]
public class StudentInfo
{
    public string studentId;
    public string username;
    public int currentLevel;
    public int currentWorld;
    public string lastTimePlayed;
    public int totalTime;
    public WorldsData worldsData;
    // Edited: year, class_, teacher as string
    public string year;
    public string class_;
    public string teacher;
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
    //public int fails;
    public int maxScore;
    //public bool isCorrect; // true = Show Check, false = Show False
    public float successRate;
    public int starRating;
    public List<AccuracyBreakdownItem> accuracyBreakdown;
}

[System.Serializable]
public class AccuracyBreakdownItem
{
    public string noteName;   // Perfect, great etc. will come from here
    public float percentage;  // Percentage value comes from here
}

[System.Serializable]
public class AvailableStudentInfo
{
    public string studentId;
    public string username;
    public string year;
    public string class_;
    public int currentWorld;
}

[System.Serializable]
public class AdminUserSearchResult
{
    public string userId;
    public string email;
    public string username;
    public string userType;
    public string privateCode;
    public string teacherPrivateCode;
    public string schoolCode;
    public string error;
}