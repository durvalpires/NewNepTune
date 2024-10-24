using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/v2_LevelSO")]
public class LevelSO : ScriptableObject
{
    public string levelID;
    public string levelName;
    public float scorePerStar;
    public LevelType levelType;

    public enum LevelType
    {
        InstrumentGuess,
        MusicPieceGuess,
        ImageSelection,
        MemoryCards,
        VirtualPiano
    }
}
