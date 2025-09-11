using System;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "LevelSO", menuName = "Scriptable Objects/v2_LevelSO")]
public class LevelSO : ScriptableObject
{
    // public string id;
    public string levelTitle;
    public LevelType levelType;
    public Galaxies galaxy;
    
    public enum LevelType
    {
        LearningNote,
        InstrumentGuess,
        MusicPieceGuess,
        ImageSelection,
        MemoryCards,
        VirtualPiano,
        LearningInstrument,
        LearningRhythm
    }
    
#if UNITY_EDITOR
    [ContextMenu("SaveMe")]
    public void SaveMe()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.LogWarning(name + " saved!");
    }
#endif
    
    //  SHOULD BE USED TO NAME FOLDER WITH CONTENT
    public enum Galaxies
    {
        LeftHand = 1,
        RightHand = 2,
        MixedNotes = 3
    }
}
