using UnityEngine;

[CreateAssetMenu(fileName = "LevelCollection", menuName = "Scriptable Objects/Level Collection", order = 1)]
public class LevelCollection : ScriptableObject
{
    public LevelData[] levels;  // Array of all level data

    // Optionally, you could include helper functions here
    public LevelData GetLevel(int index)
    {
        if (index >= 0 && index < levels.Length)
        {
            return levels[index];
        }
        Debug.LogError("Invalid level index");
        return null;
    }
}