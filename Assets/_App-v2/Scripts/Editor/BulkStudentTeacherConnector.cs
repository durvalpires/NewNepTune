using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEngine.Networking;

public class BulkStudentTeacherConnector : EditorWindow
{
    private List<StudentData> students = new List<StudentData>();
    private string teacherEmail = "";
    private string teacherPrivateCode = "";
    private bool isProcessing = false;
    private Vector2 scroll;

    // Configure your proxy base URL + endpoints
    private const string PROXY_BASE_URL = "https://neptuneserver.vercel.app";
    private const string GET_USER_BY_EMAIL_ENDPOINT = "/getUserByEmail";
    private const string ADD_STUDENT_ENDPOINT = "/teacher/addStudent";

    [MenuItem("Tools/Bulk Student-Teacher Connector")]
    public static void ShowWindow()
    {
        GetWindow<BulkStudentTeacherConnector>("Bulk Student Connector");
    }

    void OnGUI()
    {
        GUILayout.Label("Bulk Student → Teacher Association", EditorStyles.boldLabel);

        teacherEmail = EditorGUILayout.TextField("Teacher Email", teacherEmail);

        if (GUILayout.Button("Load Student CSV File"))
        {
            string path = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (!string.IsNullOrEmpty(path))
                LoadStudentsFromCSV(path);
        }

        if (students.Count > 0)
        {
            EditorGUILayout.Space(5);
            EditorGUILayout.LabelField($"Loaded {students.Count} students:");
            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
            foreach (var s in students)
                EditorGUILayout.LabelField($"{s.Name} - {s.Email}");
            EditorGUILayout.EndScrollView();

            EditorGUI.BeginDisabledGroup(isProcessing);
            if (GUILayout.Button("Start Connecting"))
                EditorCoroutineUtility.StartCoroutineOwnerless(ConnectAllCoroutine());
            EditorGUI.EndDisabledGroup();
        }
    }

    void LoadStudentsFromCSV(string path)
    {
        students.Clear();
        var lines = File.ReadAllLines(path);

        foreach (var line in lines)
        {
            if (string.IsNullOrWhiteSpace(line))
                continue;

            var values = line.Split(',');
            if (values.Length < 1)
                continue;

            string name = values[0].Trim();
            string email = values.Length > 1 ? values[1].Trim() : "";

            // if (string.IsNullOrEmpty(email))
            // {
            //     string safeName = name.ToLower().Replace(" ", ".");
            //     email = $"{safeName}@gmail.com";
            // }
            
            // 🧠 Auto-generate email if missing
            if (string.IsNullOrEmpty(email))
            {
                // 🧹 Normalize spacing: replace multiple spaces with one
                string normalizedName = System.Text.RegularExpressions.Regex.Replace(name.Trim(), @"\s+", " ");

                // 🧠 Then replace single spaces with dots and make lowercase
                string safeName = normalizedName.ToLower().Replace(" ", ".");

                email = $"{safeName}@gmail.com";
            }

            students.Add(new StudentData { Name = name, Email = email });
        }

        Debug.Log($"Loaded {students.Count} students from {path}");
    }

    private IEnumerator ConnectAllCoroutine()
    {
        if (string.IsNullOrEmpty(teacherEmail))
        {
            Debug.LogError("❌ Please enter the teacher's email before starting.");
            yield break;
        }

        isProcessing = true;
        EditorUtility.DisplayProgressBar("Connecting Students", "Fetching teacher code...", 0f);

        // 1️⃣ Get teacher private code
        yield return GetPrivateCodeFromProxy(teacherEmail, "teacher", code => teacherPrivateCode = code);
        if (string.IsNullOrEmpty(teacherPrivateCode))
        {
            Debug.LogError($"❌ Teacher not found or missing private code for {teacherEmail}");
            isProcessing = false;
            EditorUtility.ClearProgressBar();
            yield break;
        }

        Debug.Log($"✅ Teacher {teacherEmail} code: {teacherPrivateCode}");

        // 2️⃣ Connect each student
        int index = 0;
        foreach (var student in students)
        {
            float progress = (float)index / students.Count;
            EditorUtility.DisplayProgressBar("Connecting Students", $"Processing {student.Email}", progress);

            string studentCode = "";
            yield return GetPrivateCodeFromProxy(student.Email, "student", code => studentCode = code);

            if (!string.IsNullOrEmpty(studentCode))
            {
                yield return AddStudentToTeacherCoroutine(studentCode, (success, msg) =>
                {
                    if (success)
                        Debug.Log($"✅ Linked {student.Email} ({studentCode}) to {teacherEmail}");
                    else
                        Debug.LogError($"❌ Failed {student.Email}: {msg}");
                });
            }
            else
            {
                Debug.LogError($"❌ Student not found: {student.Email}");
            }

            index++;
            yield return new WaitForSeconds(0.2f);
        }

        EditorUtility.ClearProgressBar();
        Debug.Log("🎉 Bulk student-teacher linking complete!");
        isProcessing = false;
    }

    private IEnumerator GetPrivateCodeFromProxy(string email, string expectedUserType, System.Action<string> callback)
    {
        string url = $"{PROXY_BASE_URL}{GET_USER_BY_EMAIL_ENDPOINT}?email={email}";
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {
            www.downloadHandler = new DownloadHandlerBuffer();
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError($"Error fetching {expectedUserType} {email}: {www.error}");
                callback?.Invoke(null);
            }
            else
            {
                string json = www.downloadHandler.text;
                ProxyUserResponse response = JsonUtility.FromJson<ProxyUserResponse>(json);

                if (response == null || string.IsNullOrEmpty(response.userType))
                {
                    Debug.LogError($"❌ No valid user data found for {email}");
                    callback?.Invoke(null);
                }
                else if (response.userType != expectedUserType)
                {
                    Debug.LogWarning($"⚠️ {email} is not a {expectedUserType}, it's a {response.userType}");
                    callback?.Invoke(null);
                }
                else
                {
                    string code = expectedUserType == "teacher" ? response.teacherPrivateCode : response.privateCode;
                    callback?.Invoke(code);
                }
            }
        }
    }

    private IEnumerator AddStudentToTeacherCoroutine(string studentIdentifier, System.Action<bool, string> callback)
    {
        string addStudentJson = "{" +
            "\"teacherId\":\"" + teacherPrivateCode + "\"," +
            "\"studentId\":\"" + studentIdentifier + "\"" +
            "}";

        string proxyUrl = PROXY_BASE_URL + ADD_STUDENT_ENDPOINT;
        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(addStudentJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
                callback?.Invoke(false, www.error);
            else
                callback?.Invoke(true, www.downloadHandler.text);
        }
    }
}

[System.Serializable]
public class StudentData
{
    public string Name;
    public string Email;
}

[System.Serializable]
public class ProxyUserResponse
{
    public string userId;
    public string username;
    public string email;
    public string userType;
    public string privateCode;
    public string teacherPrivateCode;
}