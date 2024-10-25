using System;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/v2_LevelSO")]
public class LevelSO : ScriptableObject
{
    public string id;
    public string levelName;
    public float scorePerStar;
    public LevelType levelType;
    public Sprite icon;
    
    public enum LevelType
    {
        LearningNote,
        InstrumentGuess,
        MusicPieceGuess,
        ImageSelection,
        MemoryCards,
        VirtualPiano
    }
#if UNITY_EDITOR 
    public bool generateIds;
    private void OnValidate()
    {
        var idCounter = 0;
        if (generateIds)
        {
            generateIds = false;
            if (id == "")
            {
                id = Guid.NewGuid().ToString();
            }
        }
    }
#endif
}
