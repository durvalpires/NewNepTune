using UnityEngine;
using System.Runtime.InteropServices;

public class LocalStorageManager : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")]
    private static extern void SaveToLocalStorage(string key, string value);

    [DllImport("__Internal")]
    private static extern string LoadFromLocalStorage(string key, System.IntPtr buffer, int length);

    [DllImport("__Internal")]
    private static extern void RemoveFromLocalStorage(string key);
    
    [DllImport("__Internal")]
    private static extern void ClearMemory();
#endif

    public static void SaveData(string key, string value)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            SaveToLocalStorage(key, value);
#else
        PlayerPrefs.SetString(key, value);
        PlayerPrefs.Save();
#endif
    }

    public static string LoadData(string key)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    // Debug.Log("[LocalStorageManager] Attempting to load data for key: " + key);
    // // Create a StringBuilder to hold the returned data
    // System.Text.StringBuilder sb = new System.Text.StringBuilder(512);  // Adjust this size as needed
    //
    // // Log before calling JS function
    // Debug.Log("[LocalStorageManager] Buffer capacity: " + sb.Capacity);
    //
    // // Call the JavaScript function to load data into the StringBuilder's buffer
    // LoadFromLocalStorage(key, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(sb.ToString()), sb.Capacity);
    //
    // // Check if the length of the loaded string fits the buffer, else resize
    // string loadedData = sb.ToString();
    // Debug.Log("[LocalStorageManager] Loaded data length: " + loadedData.Length);
    //
    // if (loadedData.Length >= sb.Capacity)
    // {
    //     Debug.LogError("[LocalStorageManager] WARNING: Data size exceeds buffer capacity! Consider increasing the buffer size.");
    //
    //     sb = new System.Text.StringBuilder(loadedData.Length + 1);  // Create a new buffer with sufficient size
    //     LoadFromLocalStorage(key, System.Runtime.InteropServices.Marshal.StringToHGlobalAnsi(sb.ToString()), sb.Capacity);
    //     loadedData = sb.ToString();
    // }
    //
    // return loadedData;

    int bufferSize = 8192; // Make sure this matches JS buffer length
    byte[] buffer = new byte[bufferSize];
    GCHandle handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);

    Debug.Log($"[LocalStorageManager] Key: {key}");
    Debug.Log($"[LocalStorageManager] Buffer Address: {handle.AddrOfPinnedObject()}");
    Debug.Log($"[LocalStorageManager] Buffer Size: {bufferSize}");
    
    try
    {
        LoadFromLocalStorage(key, handle.AddrOfPinnedObject(), (int)bufferSize);
        string result = System.Text.Encoding.UTF8.GetString(buffer).TrimEnd('\0'); // Remove trailing null characters
        Debug.Log($"[C#] Loaded Data: {result} (Length: {result.Length})");
        return result;
    }
    finally
    {
        handle.Free();
    }
#else
        return PlayerPrefs.GetString(key, "");
#endif
    }

    public static void RemoveData(string key)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
            RemoveFromLocalStorage(key);
#else
        PlayerPrefs.DeleteKey(key);
#endif
    }
    
    #if UNITY_WEBGL && !UNITY_EDITOR
    void OnApplicationQuit()
    {
        Application.Unload();
        ClearMemory();
    }
    
    void OnDisable()
    {
        ClearMemory();
    }
    #endif
}