using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "RhythmGameSettings", menuName = "ScriptableObjects/RhythmGameSettings", order = 1)]
public class RhythmGameSettings : ScriptableObject
{
    [Header("Gameplay Settings")]
    public float Bpm = 72; // temp
    public float noteSpeed;
    public float accuracyThreshold;
    public float difficultyMultiplier;
    // duration 1（今は16分音符）あたりにおける x の値
    public float DurationOneX = 1;
    //private const float SpeedXPerSec = (DurationOneX * 4) * Bpm / 60;
    public float delayBeforeFirstNote = 3;
    public bool OnlyUseFirstStaff = true;

    [Header("Note Settings")]
    public List<GameObject> notePrefabs;
    public float holdNoteDurationMultiplier;
    public int comboBonusThreshold;

    [Header("Scoring Settings")]
    public int baseScorePerNote;
    public float comboMultiplier;
    public float missPenalty;
    public float hitAccuracyBonus;

    [Header("Visual and UI Settings")]
    public Color correctNoteColor;
    public Color missNoteColor;
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

    
}
