using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Audio;
using UnityEngine;
using UnityEngine.Audio;

public class RhythmPitchGate : MonoBehaviour
{

    [SerializeField] private RhythmGameManager gameManager;
    [SerializeField] private AudioPitchHUD pitchHUD;
    [SerializeField] private VirtualPianoController piano;


    [SerializeField] private bool pitchControlEnabled = true;
    [SerializeField] private bool normalizeEnharmonics = true;


    [SerializeField] private float minHoldTime = 0.06f;
    [SerializeField] private bool retriggerOnSameNote = true;
    [SerializeField] private float retriggerGap = 0.05f;

    [SerializeField] private bool controlAudioListener = true;
    [SerializeField] private float mutedListenerVolume = 1f;
    [SerializeField] private float normalListenerVolume = 1f;
    [SerializeField] private List<AudioSource> sourcesToControl = new List<AudioSource>();

    
    private float prevListenerVolume;
    private bool listenerVolumeSaved;
    private float rearmAtTime = 0f;
    private string expectedStep;
    private string pressedStep;
    private float matchSince = -1f;
    private bool toggle;

    [System.Obsolete]
    private void Awake()
    {
        if (!gameManager) gameManager = FindObjectOfType<RhythmGameManager>();
        if (!pitchHUD) pitchHUD = FindObjectOfType<AudioPitchHUD>();
        if (!piano) piano = FindObjectOfType<VirtualPianoController>();
    }

    private void OnEnable()
    {
        OnGameStarted();
        if (gameManager && gameManager.OnNextNoteUpdated != null)
            gameManager.OnNextNoteUpdated.AddListener(HandleNextNoteUpdated);
    }

    private void OnDisable()
    {
        if (gameManager && gameManager.OnNextNoteUpdated != null)
            gameManager.OnNextNoteUpdated.RemoveListener(HandleNextNoteUpdated);
        OnGameEnded();
        ReleaseIfPressed();
    }


    private void HandleNextNoteUpdated(NoteView nextNote, double _, double __, double ___)
    {
        if (nextNote.isRest)
        {
            SetExpectedStep(null);
            return;
        }


        var step = nextNote.Pitch.Step;
        if (normalizeEnharmonics && !string.IsNullOrEmpty(step))
            step = ToSharp(step);


        if (retriggerOnSameNote)
        {
            ReleaseIfPressed();
            matchSince = -1f;
            rearmAtTime = Time.unscaledTime + retriggerGap;
        }


        SetExpectedStep(step);
    }

    private void SetExpectedStep(string step)
    {
        if (normalizeEnharmonics && !string.IsNullOrEmpty(step))
            step = ToSharp(step);

        if (!string.IsNullOrEmpty(pressedStep) && pressedStep != step)
        {

            piano.ReleaseKey(pressedStep);
            pressedStep = null;
            matchSince = -1f;
        }
        expectedStep = step;
    }

    private void Update()
    {
        if (!pitchControlEnabled || pitchHUD == null || piano == null) return;

        if (Time.unscaledTime < rearmAtTime) return;

        if (string.IsNullOrEmpty(expectedStep))
        {
            ReleaseIfPressed();
            return;
        }

        var stable = pitchHUD.LastStable;
        if (!stable.hasNote)
        {
            ReleaseIfPressed();
            return;
        }

        var micStep = normalizeEnharmonics ? ToSharp(stable.noteName) : stable.noteName;

        if (micStep == expectedStep)
        {

            if (pressedStep == expectedStep) return;

            if (matchSince < 0f) matchSince = Time.unscaledTime;
            if (Time.unscaledTime - matchSince >= minHoldTime)
            {
                piano.PressKey(expectedStep);
                pressedStep = expectedStep;
            }
        }
        else
        {
            matchSince = -1f;
            ReleaseIfPressed();
        }
    }

    private void ReleaseIfPressed()
    {
        if (!string.IsNullOrEmpty(pressedStep))
        {
            piano.ReleaseKey(pressedStep);
            pressedStep = null;
        }
    }


    private static string ToSharp(string step)
    {
        switch (step)
        {
            case "Db": return "C#";
            case "Eb": return "D#";
            case "Gb": return "F#";
            case "Ab": return "G#";
            case "Bb": return "A#";
            default: return step;
        }
    }

    public void SetPitchControlEnabled(bool enabled)
    {
        pitchControlEnabled = enabled;
    }

    public void OnGameStarted()
    {
        if (!pitchControlEnabled) return;
        SetMute(true);
        SetPitchControlEnabled(true);
    }

    public void OnGameEnded()
    {
        SetMute(false);
        SetPitchControlEnabled(false);
        ReleaseIfPressed();
      
    }

    public void SetMute(bool mute)
    {
        AudioManager.Instance.sfxSource.mute = mute;
        foreach (var s in sourcesToControl)
        {
            if (!s) continue;
            s.mute = mute;
        }
    }
}