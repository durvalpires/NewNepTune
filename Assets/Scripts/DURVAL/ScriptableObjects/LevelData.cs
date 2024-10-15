using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "Scriptable Objects/LevelData")]
public class LevelData : ScriptableObject
{
    public string levelName;  // Friendly name of the level
    //public int levelOrder;    // Order in which the level should appear
    public string sceneName;  // The Unity scene to load for this level
    public bool isUnlocked;   // Track if the level is unlocked by the player
    public Sprite levelThumbnail; // Optional: thumbnail for UI representation
    public LevelGroup levelGroup; // Group of levels this level belongs to
}

public enum LevelGroup
{
    Beginner = 1,
    Intermediate = 2,
    Advanced = 3,
    Expert = 4,
    Custom = 5,
    Rhythm = 6,
    Tutorial = 7
}