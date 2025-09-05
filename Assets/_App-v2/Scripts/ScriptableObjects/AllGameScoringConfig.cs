using UnityEngine;

[CreateAssetMenu(fileName = "AllGameScoringConfig", menuName = "Scriptable Objects/AllGameScoringConfig")]
public class AllGameScoringConfig : ScriptableObject
{
    [Header("Memory Game Scoring")]
    public MemoryScoringSettings memoryScoring;
    
    [Header("Selection Game Scoring")]
    public SelectionScoringSettings selectionScoring;
    
    [Header("Music & Instrument Guess Game Scoring")]
    public GuessScoringSettings guessScoring;
    
    [Header("Info Cards Scoring")]
    public InfoCardsScoringSettings infoCardsScoring;

    // [Header("Rhythm Game Scoring")]
    // public RhythmScoringSettings rhythmScoring;
}

[System.Serializable]
public class MemoryScoringSettings
{
    public int maxScore = 100;
    public int mismatchPenalty = 5;
    [Range(0, 1)] public float threeStarThreshold = 0.8f;
    [Range(0, 1)] public float twoStarThreshold = 0.5f;
    [Range(0, 1)] public float oneStarThreshold = 0.2f;
}

[System.Serializable]
public class SelectionScoringSettings
{
    public int maxScore = 100;
    public int wrongAnswerPenalty = 5;
    [Range(0, 1)] public float threeStarThreshold = 0.8f;
    [Range(0, 1)] public float twoStarThreshold = 0.5f;
    [Range(0, 1)] public float oneStarThreshold = 0.2f;
}

[System.Serializable]
public class GuessScoringSettings
{
    public int maxScore = 100;
    public int wrongAnswerPenalty = 35;
    [Range(0, 1)] public float threeStarThreshold = 0.8f;
    [Range(0, 1)] public float twoStarThreshold = 0.5f;
    [Range(0, 1)] public float oneStarThreshold = 0.2f;
}

[System.Serializable]
public class InfoCardsScoringSettings
{
    public int maxScore = 100;
}

// [System.Serializable]
// public class RhythmScoringSettings
// {
//     public int baseScorePerNote = 100;
//     public int comboThreshold = 10;
//     public float comboMultiplierStep = 0.1f;
//     public float missPenalty = 0.5f;
//     public float hitAccuracyBonus = 0.2f;
//     public HitEvaluationValue[] BaseScorePerAccuracy;
// }

