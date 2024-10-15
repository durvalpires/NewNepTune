using System.IO;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int currentLevel;
    public int score;
    public string playerName;
}

public class SaveSystem : MonoBehaviour
{
    private string filePath;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
    }

    public void SaveGame(PlayerData playerData)
    {
        string jsonData = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath, jsonData);
    }

    public PlayerData LoadGame()
    {
        if (File.Exists(filePath))
        {
            string jsonData = File.ReadAllText(filePath);
            return JsonUtility.FromJson<PlayerData>(jsonData);
        }
        return new PlayerData(); // Return new data if no save exists
    }
}