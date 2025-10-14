using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEngine.Networking;

public class BulkStudentRegisterWindow : EditorWindow
{
    private List<StudentRegistrationInfo> students = new List<StudentRegistrationInfo>();
    private Vector2 scroll;
    private bool isRegistering = false;

    private const string PROXY_BASE_URL = "https://neptuneserver.vercel.app";
    private const string REGISTER_ENDPOINT = "/auth/register";

    [MenuItem("Tools/Bulk Student Register")]
    public static void ShowWindow()
    {
        GetWindow<BulkStudentRegisterWindow>("Bulk Student Register");
    }

    void OnGUI()
    {
        GUILayout.Label("Bulk Student Registration", EditorStyles.boldLabel);

        if (GUILayout.Button("Load CSV File"))
        {
            string path = EditorUtility.OpenFilePanel("Select CSV File", "", "csv");
            if (!string.IsNullOrEmpty(path))
                LoadStudentsFromCSV(path);
        }

        if (students.Count > 0)
        {
            EditorGUILayout.LabelField($"Loaded {students.Count} students:");
            scroll = EditorGUILayout.BeginScrollView(scroll, GUILayout.Height(200));
            foreach (var s in students)
                EditorGUILayout.LabelField($"{s.Name} - {s.Email}");
            EditorGUILayout.EndScrollView();

            EditorGUI.BeginDisabledGroup(isRegistering);
            if (GUILayout.Button("Register All Students"))
                EditorCoroutineUtility.StartCoroutineOwnerless(RegisterAllCoroutine());
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
            string password = values.Length > 2 ? values[2].Trim() : "";

            // 🧠 Auto-generate email if missing
            if (string.IsNullOrEmpty(email))
            {
                // Convert spaces to dots or underscores and lower-case for clean format
                string safeName = name.ToLower().Replace(" ", ".");
                email = $"{safeName}@gmail.com";
            }

            students.Add(new StudentRegistrationInfo
            {
                Name = name,
                Email = email,
                Password = password
            });
        }

        Debug.Log($"Loaded {students.Count} students from {path}");
    }

    private IEnumerator RegisterAllCoroutine()
    {
        isRegistering = true;
        foreach (var student in students)
        {
            yield return RegisterUserCoroutine(student.Name, student.Email, student.Password, (success, msg) =>
            {
                if (success)
                    Debug.Log($"✅ Registered {student.Email}");
                else
                    Debug.LogError($"❌ Failed {student.Email}: {msg}");
            });
            yield return new WaitForSeconds(0.2f); // slight delay to avoid server spam
        }
        isRegistering = false;
        Debug.Log("🎉 Bulk registration completed!");
    }

    private IEnumerator RegisterUserCoroutine(string name, string email, string password, System.Action<bool, string> callback)
    {
        var userProfile = new Dictionary<string, object>
        {
            { "userType", "student" },
            { "username", name }
        };
        
        string authJson = "{"
                          + "\"email\":\"" + email + "\","
                          + "\"password\":\"" + password + "\","
                          + "\"userProfile\":" + JsonConvert(userProfile)
                          + "}";        
        string proxyUrl = PROXY_BASE_URL + REGISTER_ENDPOINT;

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(proxyUrl, ""))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(authJson);
            www.uploadHandler = new UploadHandlerRaw(bodyRaw);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                callback?.Invoke(false, www.error);
            }
            else
            {
                callback?.Invoke(true, www.downloadHandler.text);
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
}