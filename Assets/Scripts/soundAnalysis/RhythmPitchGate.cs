using UnityEngine;

public class RhythmPitchGate : MonoBehaviour
{
   
    [SerializeField] private RhythmGameManager gameManager;
    [SerializeField] private AudioPitchHUD pitchHUD;
    [SerializeField] private VirtualPianoController piano;

   
    [SerializeField] private bool pitchControlEnabled = true;
    [SerializeField] private AudioSourcesMuter extraMute;
    [SerializeField] private bool normalizeEnharmonics = true;

   
    [SerializeField] private float minHoldTime = 0.06f;

    private string expectedStep;  
    private string pressedStep;    
    private float matchSince = -1f;

    private void Awake()
    {
        if (!gameManager) gameManager = FindObjectOfType<RhythmGameManager>();
        if (!pitchHUD) pitchHUD = FindObjectOfType<AudioPitchHUD>();
        if (!piano) piano = FindObjectOfType<VirtualPianoController>();
    }

    private void OnEnable()
    {
        if (gameManager && gameManager.OnNextNoteUpdated != null)
            gameManager.OnNextNoteUpdated.AddListener(HandleNextNoteUpdated);

        if (extraMute)
        {
            extraMute.SetMuted(pitchControlEnabled);
        }
        else
        {
            Debug.LogWarning("[RhythmPitchGate] extraMute not assigned, nothing to mute.");
        }
    }

    private void OnDisable()
    {
        if (gameManager && gameManager.OnNextNoteUpdated != null)
            gameManager.OnNextNoteUpdated.RemoveListener(HandleNextNoteUpdated);
        if (extraMute) extraMute.SetMuted(false);
        ReleaseIfPressed();
    }

   
    private void HandleNextNoteUpdated(NoteView nextNote, double _, double __, double ___)
    {
      
        if (nextNote.isRest)
        {
            SetExpectedStep(null);
            return;
        }

        SetExpectedStep(nextNote.Pitch.Step);
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
        if (extraMute) extraMute.SetMuted(enabled);
       
    }
}