using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RhythmGameSettings", menuName = "Scriptable Objects/RhythmGameSettings", order = 1)]
public class RhythmGameSettings : ScriptableObject
{
    [Header("Note Placement Settings")]
    public float DurationOneX = 1;
    public float DurationReductionPerDivision = 0.25f;
    public bool OnlyUseFirstStaff = true;
    public Dictionary<HitAccuracy, float> HitWindowsPercentage; //Non timing based, it should add up to 100% all summed up
    public float[] HitWindowsTimings; //Non timing based, it should add up to 100% all summed up
    public HitAccuracySettings HitEvaluationSettings;

    [Header("Gameplay Settings")]
    public int beatsBeforeStart = 4;
    public float noteSpeed;
    public float accuracyThreshold;
    public float difficultyMultiplier;
    public float delayBeforeLevelStart = 3;
    public int numberBeatsToDrawLines = 3;
    public Difficulty difficulty;

    [Header("Note Settings")]
    public List<GameObject> notePrefabs;
    public float holdNoteDurationMultiplier;
    public int comboBonusThreshold;

    [Header("Scoring Settings")]
    public int baseScorePerNote;
    public int comboThreshold = 10;
    public float comboMultiplierStep;
    public float missPenalty;
    public float hitAccuracyBonus;
    public HitEvaluationValue[] BaseScorePerAccuracy;

    [Header("Visual and UI Settings")]
    public List<NotePrefabPair> NotePrefabs;
    // public Color correctNoteColor;
    // public Color missNoteColor;
    // public Color interactableNoteColor;
    // public Color normalNoteColor;

    public List<NoteElementVariationSpritePair> noteCircleVariationsSprites;
    public Sprite beamSprite;

    public List<string> circleLineNotes;
    public GameObject hitEffectPrefab;
    public float hitEffectDuration;
    public Sprite backgroundSprite;
    public ColorSettings ColorSettings;
    public float MusicalScoreLineDistance = 0.5f; //TODO


    [Header("Audio Settings")]
    public AudioClip backgroundMusic;
    public AudioClip hitSoundEffect;
    public AudioClip missSoundEffect;
    public float musicVolume;
    public float sfxVolume;

    [Header("Sync Settings")]
    public float syncOffset;
    public float inputLatency;

    [Header("Difficulty Settings")]
    public List<float> noteDensityByLevel;
    public List<float> speedByLevel;
    public List<int> targetComboByLevel;

    [Header("Game Mode Settings")]
    public bool isPracticeMode;
    public bool isChallengeMode;
    public int maxRetries;

    [Header("Player Feedback Settings")]
    public bool enableVibrationOnHit;
    public float feedbackIntensity;
    
    public int GetScoreForAccuracy(HitAccuracy accuracy)
    {
        foreach(var score in BaseScorePerAccuracy)
        {
            if(score.HitType == accuracy)
            {
                return (int)score.Value;
            }
        }

        return 0;
    }

    public GameObject GetPrefabForNoteType(string noteType)
    {
        foreach (var pair in NotePrefabs)
        {
            if (pair.noteType.Equals(noteType, StringComparison.OrdinalIgnoreCase))
            {
                return pair.prefab;
            }
        }

        Debug.LogWarning($"Prefab for note type '{noteType}' not found.");
        return null;
    }
}

[Serializable]
public struct NotePrefabPair
{
    public string noteType;  // The note type
    public GameObject prefab;         // The prefab associated with the note
}

[Serializable]
public struct NoteElementVariationSpritePair
{
    public string noteType;  // The note type
    public Sprite sprite;         // The prefab associated with the note
}

public enum Difficulty
{
    EASY = 4,
    MEDIUM = 3,
    HARD = 1
}
