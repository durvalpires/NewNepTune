#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using System.Text;
using System.Threading.Tasks;

public class RemoveStudentTeacher : EditorWindow
{
    private enum UserType { Student, Teacher }

    // --- UI state ---
    private string baseUrl = "https://neptuneserver.vercel.app";
    private string email = "";
    private UserType type = UserType.Student;
    private string lastResponse = "";
    private bool isBusy = false;
    private Vector2 scroll;

    [MenuItem("Tools/Delete Student-Teacher")]
    public static void Open()
    {
        var win = GetWindow<RemoveStudentTeacher>("Delete Student-Teacher");
        win.minSize = new Vector2(520, 360);
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(6);

        EditorGUILayout.Space(6);

        // Base URL
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Base URL", GUILayout.Width(70));
            baseUrl = EditorGUILayout.TextField(baseUrl);
            if (GUILayout.Button("Test /debug", GUILayout.Width(110)))
                _ = PingDebug();
        }

        // Email + type
        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("Email", GUILayout.Width(70));
            email = EditorGUILayout.TextField(email);
        }

        using (new EditorGUILayout.HorizontalScope())
        {
            EditorGUILayout.LabelField("User Type", GUILayout.Width(70));
            type = (UserType)EditorGUILayout.EnumPopup(type);
        }

        EditorGUILayout.Space(6);

        using (new EditorGUI.DisabledScope(isBusy))
        using (new EditorGUILayout.HorizontalScope())
        {
            if (GUILayout.Button("Delete"))
                _ = DeleteByEmail();

            if (GUILayout.Button("Clear Output", GUILayout.Width(110)))
                lastResponse = string.Empty;
        }

        // Busy spinner
        if (isBusy)
        {
            EditorGUILayout.Space(4);
            EditorGUILayout.LabelField("Working...", EditorStyles.miniLabel);
        }

        // Response area
        EditorGUILayout.Space(6);
        EditorGUILayout.LabelField("Response", EditorStyles.boldLabel);
        using (var sv = new EditorGUILayout.ScrollViewScope(scroll, GUILayout.ExpandHeight(true)))
        {
            scroll = sv.scrollPosition;
            GUIStyle box = new GUIStyle(EditorStyles.helpBox)
            {
                richText = false,
                wordWrap = false
            };
            EditorGUILayout.TextArea(string.IsNullOrEmpty(lastResponse) ? "(no response yet)" : lastResponse, box, GUILayout.ExpandHeight(true));
        }
    }

    // --- Actions ---

    private async Task DeleteByEmail()
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            ShowError("Base URL is empty.");
            return;
        }
        if (string.IsNullOrWhiteSpace(email))
        {
            ShowError("Email is required.");
            return;
        }

        string endpoint = type == UserType.Student
            ? "/admin/deleteStudent"
            : "/admin/deleteTeacher";

        string url = CombineUrl(baseUrl, endpoint);
        var payload = "{\"email\":\"" + EscapeJson(email.Trim()) + "\"}";

        await PostJson(url, payload);
    }

    private async Task PingDebug()
    {
        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            ShowError("Base URL is empty.");
            return;
        }
        string url = CombineUrl(baseUrl, "/debug");
        await Get(url);
    }

    // --- HTTP helpers ---

    private async Task PostJson(string url, string json)
    {
        isBusy = true;
        Repaint();

        using (var req = new UnityWebRequest(url, "POST"))
        {
            byte[] body = Encoding.UTF8.GetBytes(json);
            req.uploadHandler = new UploadHandlerRaw(body);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");

#if UNITY_2020_2_OR_NEWER
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
#else
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
#endif

#if UNITY_2020_2_OR_NEWER
            bool hasError = req.result != UnityWebRequest.Result.Success;
#else
            bool hasError = req.isNetworkError || req.isHttpError;
#endif

            if (hasError)
            {
                lastResponse =
                    $"[POST] {url}\n" +
                    $"Status: {req.responseCode}\n" +
                    $"Error: {req.error}\n" +
                    $"Body: {req.downloadHandler?.text}";
                Debug.LogError(lastResponse);
            }
            else
            {
                lastResponse =
                    $"[POST] {url}\n" +
                    $"Status: {req.responseCode}\n" +
                    $"Body:\n{req.downloadHandler?.text}";
                Debug.Log(lastResponse);
            }
        }

        isBusy = false;
        Repaint();
    }

    private async Task Get(string url)
    {
        isBusy = true;
        Repaint();

        using (var req = UnityWebRequest.Get(url))
        {
#if UNITY_2020_2_OR_NEWER
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
#else
            var op = req.SendWebRequest();
            while (!op.isDone) await Task.Yield();
#endif

#if UNITY_2020_2_OR_NEWER
            bool hasError = req.result != UnityWebRequest.Result.Success;
#else
            bool hasError = req.isNetworkError || req.isHttpError;
#endif

            if (hasError)
            {
                lastResponse =
                    $"[GET] {url}\n" +
                    $"Status: {req.responseCode}\n" +
                    $"Error: {req.error}\n" +
                    $"Body: {req.downloadHandler?.text}";
                Debug.LogError(lastResponse);
            }
            else
            {
                lastResponse =
                    $"[GET] {url}\n" +
                    $"Status: {req.responseCode}\n" +
                    $"Body:\n{req.downloadHandler?.text}";
                Debug.Log(lastResponse);
            }
        }

        isBusy = false;
        Repaint();
    }

    // --- Utils ---

    private static string CombineUrl(string a, string b)
    {
        if (string.IsNullOrEmpty(a)) return b ?? "";
        if (string.IsNullOrEmpty(b)) return a ?? "";
        if (a.EndsWith("/")) a = a.Substring(0, a.Length - 1);
        if (!b.StartsWith("/")) b = "/" + b;
        return a + b;
    }

    private static string EscapeJson(string s)
    {
        // Minimal escaping for JSON string values
        return s.Replace("\\", "\\\\").Replace("\"", "\\\"");
    }

    private void ShowError(string msg)
    {
        lastResponse = $"Error: {msg}";
        Debug.LogError(lastResponse);
        Repaint();
    }
}
#endif
