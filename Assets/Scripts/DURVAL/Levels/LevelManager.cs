using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public LevelCollection levelCollection;  // Reference to the SO holding all levels
    private int currentLevelIndex = 0;

    // Load a level by its index in the ScriptableObject array
    public void LoadLevel(int index)
    {
        if (index >= 0 && index < levelCollection.levels.Length)
        {
            LevelData level = levelCollection.levels[index];
            
            if (level.isUnlocked)
            {
                SceneManager.LoadScene(level.sceneName);
                currentLevelIndex = index;
            }
            else
            {
                Debug.LogWarning("Level is locked!");
            }
        }
        else
        {
            Debug.LogError("Invalid level index");
        }
    }

    // Load the next level in the order
    public void LoadNextLevel()
    {
        int nextLevelIndex = currentLevelIndex + 1;
        if (nextLevelIndex < levelCollection.levels.Length)
        {
            LoadLevel(nextLevelIndex);
        }
        else
        {
            Debug.Log("No more levels to load.");
        }
    }
}