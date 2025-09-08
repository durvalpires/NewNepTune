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
    public float delayBeforeTutorialStart = 8;
    public int numberBeatsToDrawLines = 3;
    public Difficulty difficulty;
    public bool HaveDurationTrail = false;

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
    public List<HandSpritePair> handSprites;
    public List<NoteElementVariationSpritePair> clefSprites;
  

    public List<string> circleLineNotes;
    public GameObject hitEffectPrefab;
    public float hitEffectDuration;
    public Sprite backgroundSprite;
    public ColorSettings ColorSettings;
    public float MusicalScoreLineDistance = 0.5f; //TODO
    public float LastBarLineHorizontalDistance = 0.25f;
    public float LastBarLineThickness = 5f; //multiplied to scale
    public bool showLinePerBeat = false;


    [Header("Audio Settings")]
    public AudioClip backgroundMusic;
    public AudioClip hitSoundEffect;
    public AudioClip missSoundEffect;
    public float musicVolume;
    public float sfxVolume;
    public List<NoteAudioPair> noteSounds;

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
    
    [Header("Tutorial Settings")]
    public List<TutorialPopup> tutorialPopups;
    
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

    public Sprite GetHandSprite(LevelHandType handType)
    {
        string key = handType.ToString();
        foreach (var pair in handSprites)
        {
            if (pair.handType.Equals(key, StringComparison.OrdinalIgnoreCase))
                return pair.sprite;
        }
        Debug.LogWarning($"Hand Sprite for '{key}' not found.");
        return null;
    }

    public Sprite GetClefSprite(string clefType)
    {
        foreach (var pair in clefSprites)
        {
            if (pair.noteType.Equals(clefType, StringComparison.OrdinalIgnoreCase))
            {
                return pair.sprite;
            }
        }

        Debug.LogWarning($"Clef Sprite for note type '{clefType}' not found.");
        return null;
    }

    
    public AudioClip GetNoteAudio(string note)
    {
        foreach (var pair in noteSounds)
        {
            if (pair.note.Equals(note, StringComparison.OrdinalIgnoreCase))
            {
                return pair.audio;
            }
        }

        Debug.LogWarning($"Audio for note type '{note}' not found.");
        return null;
    }
    
    public float GetMainCircleWidth()
    {
        foreach (var pair in NotePrefabs)
        {
            if (pair.noteType.Equals("template", StringComparison.OrdinalIgnoreCase))
            {
                var noteTemplate = pair.prefab;
                var mainCircle = noteTemplate.transform.Find("mainCircle");
                if (mainCircle != null)
                {
                    var spriteRenderer = mainCircle.GetComponent<SpriteRenderer>();
                    if (spriteRenderer != null && spriteRenderer.sprite != null)
                    {
                        return spriteRenderer.sprite.bounds.size.x;
                    }
                }
            }
        }

        Debug.LogWarning("mainCircle or noteTemplate not found in NotePrefabs.");
        return 0f;
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
[Serializable]
public struct HandSpritePair
{
    public string handType;
    public Sprite sprite;
}

[Serializable]
public struct NoteAudioPair
{
    public string note;  // The note type
    public AudioClip audio;         // The audioclip associated with the note
}

public enum Difficulty
{
    EASY = 4,
    MEDIUM = 3,
    HARD = 1
}

[Serializable]
public struct TutorialPopup
{
    [TextArea(3, 10)]
    public string message;
}
