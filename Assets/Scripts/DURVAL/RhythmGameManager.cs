using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class RhythmGameManager : MonoBehaviour
{
    public UnityEvent<float> OnTempoChanged;

    private float speedXPerSec;

    // [SerializeField] private GameObject notePrefab;
    // [SerializeField] private GameObject circleNotePrefab;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private TextAsset songXmlAsset;
    //[SerializeField] private TextAsset songMidiAsset;
    [SerializeField] private MusicScoreRender scoreRender;
    [SerializeField] private Transform interactionArea;

    [SerializeField] private UnityEvent<string> keyPressTxtFeedback;
    [SerializeField] private UnityEvent<string> OnScoreUpdated;
    [SerializeField] private UnityEvent<string> OnComboUpdated;
    
    [SerializeField] private RhythmGameSettings rhythmGameSettings;

    private PlayRecorder playRecorder;

    //private SmfLite.MidiTrackSequencer midiTrackSequencer;

    private float noteMaxDistanceToInteractionBar = 0;

    private RhythmGameScoreController scoreController;

    [SerializeField] private AudioSource trackAudioSource;

    // Assuming you have tempo in BPM
    float beatsPerSecond;
    float secondsPerBeat;

    private bool isPlaying = false;

    private Dictionary<string, NoteController> noteInteractableDic = new Dictionary<string, NoteController>()
    {
        //{"C", false},
        //{"D", false},
        //{"E", false},
        //{"F", false},
        //{"G", false},
        //{"A", false},
        //{"B", false

        {"C", null},
        {"D", null},
        {"E", null},
        {"F", null},
        {"G", null},
        {"A", null},
        {"B", null}
    };

    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        scoreRender.Init(rhythmGameSettings, songXmlAsset.text);
        
        beatsPerSecond = scoreRender.Bpm / 60;
        secondsPerBeat = 1 / beatsPerSecond;
        speedXPerSec = (rhythmGameSettings.DurationOneX * scoreRender.MeasureDivision) * beatsPerSecond;

        scoreController = new RhythmGameScoreController(rhythmGameSettings);

        var notesSortedByScore = scoreRender.Render(speedXPerSec);

        this.playRecorder = new PlayRecorder(notesSortedByScore);

        //LoadMidiFile();
        OnTempoChanged?.Invoke(scoreRender.Bpm);
        //MoveBoard();
    }

    void Update()
    {
        //if (this.midiTrackSequencer != null && !this.midiTrackSequencer.Playing)
        //{
        //    // Adjusting the playback position of a MIDI file
        //    this.DispatchEvents(this.midiTrackSequencer.Start(0.2f));
        //}
        //MoveBoard();
        //this.DispatchEvents(this.midiTrackSequencer.Advance(Time.deltaTime));
    }

    public void StartPlaying()
    {
        Debug.LogWarning("StartPlaying");
        isPlaying = true;
        trackAudioSource.Play();
    }

    private void FixedUpdate()
    {
        if(isPlaying)
        {
            //TODO: Start playing the midi/mp3 file
            //if(!trackAudioSource.isPlaying) trackAudioSource.Play();
            MoveBoard();
        }
    }

    // private void LoadMidiFile()
    // {
    //     //var smfAsset = Resources.Load<TextAsset>("BWV846P_MIDI.mid");
    //     var song = SmfLite.MidiFileLoader.Load(songMidiAsset.bytes);
    //     // The tempo of this MIDI file has doubled, so you should specify Bpm * 2.
    //     this.midiTrackSequencer = new SmfLite.MidiTrackSequencer(song.tracks[0], song.division, scoreRender.Bpm * 2);
    // }

    void MoveBoard()
    {
        var addX = speedXPerSec * Time.fixedDeltaTime;
        var currentBoardPosition = this.scoreBoard.transform.position;
        this.scoreBoard.transform.position -= this.scoreBoard.transform.right * addX;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (change != InputDeviceChange.Added)
        {
            return;
        }

        var midiDevice = device as Minis.MidiDevice;
        if (midiDevice == null) return;

        midiDevice.onWillNoteOn += OnWillNoteOn;
    }

    private void OnWillNoteOn(Minis.MidiNoteControl note, float velocity)
    {
        DispatchNoteOnEvent(note.noteNumber);
    }

    private void DispatchEvents(List<SmfLite.MidiEvent> events)
    {
        if (events == null)
        {
            return;
        }

        foreach (var e in events)
        {
            if ((e.status & 0xf0) == 0x90)
            {
                this.DispatchNoteOnEvent(e.data1);
            }
        }
    }

    private void DispatchNoteOnEvent(int noteNumber)
    {
        var pitch = Pitch.GetPitchByMidiNoteNumber(noteNumber);
        this.PlayNote(pitch);
    }

    void PlayNote(Pitch pitch)
    {
        this.playRecorder.Played(pitch, rhythmGameSettings.DurationOneX, scoreRender.Bpm, speedXPerSec);
    }

    private HitAccuracy EvaluateHit(float noteXPosition)
    {
        float currentDistance = Mathf.Abs(noteXPosition - 
            interactionArea.transform.position.x);

        float percentage = currentDistance * 100 / noteMaxDistanceToInteractionBar;
        var result = HitAccuracy.Miss;

        foreach(var hitType in rhythmGameSettings.HitEvaluationSettings.NonTimedSettings)
        {
            if (percentage <= hitType.Value)
            {
                result = hitType.HitType;
                break;
            }
        }

        keyPressTxtFeedback?.Invoke(result.ToString());

        return result;
    }

    public void OnNoteTriggeredInteractionBar(NoteController note, bool entered)
    {
        Debug.LogWarning("OnNoteTriggeredInteractionBar: " + note.Pitch.Step + " = " + entered);
        
        noteInteractableDic[note.Pitch.Step] = entered ? note : null;
        noteMaxDistanceToInteractionBar = Mathf.Abs(
            note.transform.position.x -
            interactionArea.transform.position.x);
    }

    public void OnPianoKeyStateChanged(string note, bool isPressed)
    {
        if (isPressed)
        {
            var accuracy = HitAccuracy.Miss;
            if (noteInteractableDic[note] != null)
            {
                //keyPressTxtFeedback?.Invoke("ACERTOU CARALHOOOOO");
                //Debug.LogWarning("ACERTOU CARLHOOOOO");
                accuracy = EvaluateHit(noteInteractableDic[note].gameObject.transform.position.x);
                //scoreController.AwardScore(accuracy);
                noteInteractableDic[note] = null;
            }
            else
            {

                accuracy = HitAccuracy.Miss;
                //scoreController.AwardScore(HitAccuracy.Miss);
                
                //keyPressTxtFeedback?.Invoke("BATEU NA ROCHA");
                //Debug.LogWarning("BATEU NA ROCHA");
            }
            ProcessScore(accuracy);
            keyPressTxtFeedback?.Invoke(accuracy.ToString());
        }
    }

    public void ProcessScore(HitAccuracy accuracy)
    {
        var results = scoreController.AwardScore(accuracy);
        if (results != null)
        {
            OnScoreUpdated?.Invoke(results.Item1.ToString());
            OnComboUpdated?.Invoke(results.Item2.ToString());
        }
    }
}
