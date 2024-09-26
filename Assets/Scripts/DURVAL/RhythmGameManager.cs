using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Video;

public class RhythmGameManager : MonoBehaviour
{
    // duration 1（今は16分音符）あたりにおける x の値
    //private const float durationOneX = 1;

    //private const float bpm = 57;
    // 1秒で進んで欲しいxの値
    private float speedXPerSec;

    [SerializeField] private GameObject notePrefab;
    [SerializeField] private GameObject circleNotePrefab;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private TextAsset songXmlAsset;
    [SerializeField] private TextAsset songMidiAsset;
    [SerializeField] private ScoreRender scoreRender;
    [SerializeField] private Transform interactionArea;
    
    [SerializeField] private RhythmGameSettings rhythmGameSettings;

    private PlayRecorder playRecorder;

    private SmfLite.MidiTrackSequencer midiTrackSequencer;

    private Dictionary<string, bool> noteInteractableDic = new Dictionary<string, bool>()
    {
        {"C", false},
        {"D", false},
        {"E", false},
        {"F", false},
        {"G", false},
        {"A", false},
        {"B", false}
    };

    void Start()
    {
        InputSystem.onDeviceChange += OnDeviceChange;

        speedXPerSec = (rhythmGameSettings.DurationOneX * 4) * rhythmGameSettings.Bpm / 60;


        scoreRender.Init(rhythmGameSettings, this.notePrefab, this.circleNotePrefab, speedXPerSec);

        var notesSortedByScore = scoreRender.Render(songXmlAsset.text);
        this.playRecorder = new PlayRecorder(notesSortedByScore);

        LoadMidiFile();
    }

    void Update()
    {
        if (this.midiTrackSequencer != null && !this.midiTrackSequencer.Playing)
        {
            // Adjusting the playback position of a MIDI file
            this.DispatchEvents(this.midiTrackSequencer.Start(0.2f));
        }
        MoveBoard();
        this.DispatchEvents(this.midiTrackSequencer.Advance(Time.deltaTime));
    }

    private void LoadMidiFile()
    {
        //var smfAsset = Resources.Load<TextAsset>("BWV846P_MIDI.mid");
        var song = SmfLite.MidiFileLoader.Load(songMidiAsset.bytes);
        // The tempo of this MIDI file has doubled, so you should specify Bpm * 2.
        this.midiTrackSequencer = new SmfLite.MidiTrackSequencer(song.tracks[0], song.division, rhythmGameSettings.Bpm * 2);
    }

    void MoveBoard()
    {
        var addX = speedXPerSec * Time.deltaTime;
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
        this.playRecorder.Played(pitch, rhythmGameSettings.DurationOneX, rhythmGameSettings.Bpm, speedXPerSec);
    }

    public void OnNoteTriggeredInteractionBar(string note, bool entered)
    {
        noteInteractableDic["note"] = entered;
    }
}
