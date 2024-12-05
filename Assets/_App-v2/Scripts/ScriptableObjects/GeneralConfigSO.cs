using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "GeneralConfigSO", menuName = "Scriptable Objects/v2_GeneralConfigSO")]
public class GeneralConfigSO : ScriptableObject
{
    public string planetsScene = "v2_MainMenu";
    public string levelsScene = "v2_Levels";
    public string virtualPianoScene = "VirtualPiano";
    public string musicGuessScene = "v2_LevelMusicGuess";
    public string instrumentGuessScene = "v2_InstrumentGuess";
    public string memoryCardScene = "v2_MemoryCard";
    public string selectionMiniGameScene = "v2_SelectionMinigame";
    public string openedPlanetKey = "OpenedPlanet";
}
