using UnityEngine;

[CreateAssetMenu(fileName = "NewRhythmLevelData", menuName = "RhythmGame/LevelData")]
public class RhythmLevelData : ScriptableObject
{
  
    public AudioClip musicClip;   

    [Header("Wave Detection Settings")]
    public float windowSize = 0.02f;       //energy calculation
    public float minWaveSeparation = 0.1f; // Min between waves
    public float thresholdFactor = 1.5f;   // Factor to * the median energy

    [Header("Gameplay Settings")]
    public float minLoudnessThreshold = 0.1f; 
    public int initialCountdown = 3;       
    public int clapCountdown = 3;        
    public float waitAfterTrack = 2f;     
    [Header("Scoring Settings")]
    public int minScoreThreshold = 50;
    public float levelTimingBuffer = 0.3f; 


    [Header("Visualization & Prompts")]
    public bool enableWaveformVisualization = false;
    public bool showClapIndicators = true;
}
