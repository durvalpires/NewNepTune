using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GeneralConfigSO", menuName = "Scriptable Objects/v2_GeneralConfigSO")]
public class GeneralConfigSO : ScriptableObject
{
    public string levelsScene = "v2_Levels";
    public string virtualPianoScene = "VirtualPiano";
    public string musicGuessScene = "v2_LevelMusicGuess";
    public string memoryCardScene = "v2_MemoryCard";
    public string openedPlanetKey = "OpenedPlanet";
}
