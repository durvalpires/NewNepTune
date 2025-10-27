using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.EditorCoroutines.Editor;
using UnityEngine.Networking;

public class TeacherToStudentGenerator : EditorWindow
{
    [Header("Account Settings")]
    [SerializeField] private string inputEmail = "";
    [SerializeField] private string suffix = "ogrenci";
    [SerializeField] private string password = "123456";

    [Header("Proxy Settings")]
    private const string PROXY_BASE_URL = "https://neptuneserver.vercel.app";
    private const string REGISTER_ENDPOINT = "/auth/register";

    private string generatedEmail = "";
    private bool isRegistering = false;

    [MenuItem("Tools/Teacher to Student Registration")]
    public static void ShowWindow()
    {
        GetWindow<TeacherToStudentGenerator>("Teacher to Student Registration");
    }

    void OnGUI()
    {
        GUILayout.Label("Teacher to Student Registration", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("Base Email (e.g. teacher email):");
        inputEmail = EditorGUILayout.TextField(inputEmail);

        EditorGUILayout.LabelField("Suffix to append before '@':");
        suffix = EditorGUILayout.TextField(suffix);

        EditorGUILayout.LabelField("Password for new student account:");
        password = EditorGUILayout.PasswordField(password);

        EditorGUILayout.Space();

        if (GUILayout.Button("Generate Student Email"))
        {
            generatedEmail = GenerateStudentEmail(inputEmail, suffix);
            Debug.Log($"Generated email: {generatedEmail}");
        }

        if (!string.IsNullOrEmpty(generatedEmail))
        {
            EditorGUILayout.LabelField("Generated Email:");
            EditorGUILayout.SelectableLabel(generatedEmail, GUILayout.Height(18));
        }

        EditorGUILayout.Space();

        GUI.enabled = !string.IsNullOrEmpty(generatedEmail) && !isRegistering;
        if (GUILayout.Button("Register Student Account"))
        {
            EditorCoroutineUtility.StartCoroutineOwnerless(
                RegisterUserCoroutine(inputEmail.Split('@')[0], password, (success, msg) =>
                {
                    if (success)
                        Debug.Log($"✅ Successfully registered {generatedEmail}");
                    else
                        Debug.LogError($"❌ Failed to register {generatedEmail}: {msg}");
                })
            );
        }
        GUI.enabled = true;
    }

    private string GenerateStudentEmail(string baseEmail, string suffix)
    {
        if (string.IsNullOrWhiteSpace(baseEmail) || !baseEmail.Contains("@"))
        {
            Debug.LogError("Invalid base email address.");
            return "";
        }

        baseEmail = baseEmail.Trim().ToLower();
        string[] parts = baseEmail.Split('@');
        string local = parts[0];
        string domain = parts[1];

        // normalize spaces and remove double dots
        local = System.Text.RegularExpressions.Regex.Replace(local, @"\s+", ".");
        local = local.Replace("..", ".");

        // add suffix safely (avoid repeating dot)
        string sep = suffix.StartsWith(".") && local.EndsWith(".") ? "" : "";
        return $"{local}{suffix}@{domain}".ToLower();
    }

    private IEnumerator RegisterUserCoroutine(string name, string password, System.Action<bool, string> callback)
    {
        isRegistering = true;

        var userProfile = new Dictionary<string, object>
        {
            { "userType", "student" },
            { "username", name }
        };
        
        string authJson = "{"
                          + "\"email\":\"" + generatedEmail + "\","
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

        isRegistering = false;
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