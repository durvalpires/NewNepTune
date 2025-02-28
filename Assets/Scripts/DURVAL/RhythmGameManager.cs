using Audio;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class RhythmGameManager : MonoBehaviour
{
    public UnityEvent<float> OnTempoChanged;
    public UnityEvent<int> OnNotesAmountCalculated;
    public UnityEvent<RhythmGameScoreController> OnLevelEnded;
    public UnityEvent OnNoteHit;

    private float speedXPerSec;

    // [SerializeField] private GameObject notePrefab;
    // [SerializeField] private GameObject circleNotePrefab;
    [SerializeField] private GameObject scoreBoard;
    [SerializeField] private TextAsset songXmlAsset;
    //[SerializeField] private TextAsset songMidiAsset;
    [SerializeField] private MusicScoreRender scoreRender;
    [SerializeField] private Transform interactionArea;
    [SerializeField] private VirtualPianoController virtualPianoController;

    [SerializeField] private UnityEvent<string> keyPressTxtFeedback;
    [SerializeField] private UnityEvent<string> OnScoreUpdated;
    [SerializeField] private UnityEvent<string> OnComboUpdated;

    [SerializeField] private RhythmGameSettings rhythmGameSettings;

    private PlayRecorder playRecorder;

    //private SmfLite.MidiTrackSequencer midiTrackSequencer;

    private float noteMaxDistanceToInteractionBar = 0;

    private RhythmGameScoreController scoreController;

    [SerializeField] private AudioSource challengeAudioSource;
    [SerializeField] private AudioSource backgroundAudioSource;

    // Assuming you have tempo in BPM
    float beatsPerSecond;
    float secondsPerBeat;
    float beatsPerUnit;

    private bool isPlaying = false;
    private bool isLevelStarted = false;

    [HideInInspector] public bool isAutoPlayTutorial = false;

    private VirtualPianoLevelSO currentLevel;

    private const string autoPlayTutorialKey = "isAutoPlayTutorial";

    private int starsAchieved = 0;
    
    private IList<NoteView> noteViewList;
    public IList<NoteView> NoteViewList
    {
        get => noteViewList;
    }

    private int notesCrossed = 0;
    private int notesCrossedInteractionArea = 0;
    public UnityEvent<NoteView, float, float, float> OnNextNoteUpdated;

    private List<NoteController> noteInteractableList = new List<NoteController>();
    private List<NoteController> noteInteractingList = new List<NoteController>();

    [SerializeField] private VirtualPianoLevelSO testingLevel;

    
    void Awake()
    {
        var data = TempDataStorage.GetSceneData<VirtualPianoLevelSO>();

        if(PlayerPrefs.GetInt(autoPlayTutorialKey) == 0) 
        {
            isAutoPlayTutorial = data.isTutorial;
        }
      
        if (data == null) data = testingLevel;
      
        LoadLevelAssets(data);
    }

    void OnDestroy()
    {
        UnloadLevelAssets();
    }

    void OnStarAchieved()
    {
        starsAchieved++;
        if (starsAchieved == 1) AudioManager.Instance.PlaySFX(Enums.SoundList.Star1Achieved);
        else if (starsAchieved == 2) AudioManager.Instance.PlaySFX(Enums.SoundList.Star2Achieved);
        else if (starsAchieved == 3) AudioManager.Instance.PlaySFX(Enums.SoundList.Star3Achieved);
    }


    private IEnumerator DelayedStart()
    {
        float delay = isAutoPlayTutorial ? rhythmGameSettings.delayBeforeTutorialStart : rhythmGameSettings.delayBeforeLevelStart;
        yield return new WaitForSeconds(delay);
        OnTempoChanged?.Invoke(scoreRender.Bpm);
        isLevelStarted = true;
        yield return null;
    }

    void Update()
    {
        if(isPlaying){
            if(!challengeAudioSource.isPlaying){
                StartCoroutine(CloseLevel());
            }
        }
    }


    private IEnumerator CloseLevel()
    {
        isLevelStarted = false;
        isPlaying = false;
        yield return new WaitForSeconds(.5f);

      
        if (isAutoPlayTutorial)
        {
            isAutoPlayTutorial = false; 
            SaveIsAutoPlayTutorial(isAutoPlayTutorial);
            UnloadLevelAssets();
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            yield break;
        }
       
        Debug.LogWarning("CloseLevelllllllllll");
        OnLevelEnded?.Invoke(scoreController);
        UnloadLevelAssets();

    }



    public void StartPlaying()
    {
        Debug.LogWarning("StartPlaying");
        isPlaying = true;
        //backgroundAudioSource.Play();
        //OnNextNoteUpdated?.Invoke(noteViewList[notesCrossed], 1,secondsPerBeat);
        challengeAudioSource.Play();
    }
    
    public void StartBackTrack()
    {
        Debug.LogWarning("StartPlayingBackTrack");
        //isPlaying = true;
        backgroundAudioSource.Play();
        //OnNextNoteUpdated?.Invoke(noteViewList[notesCrossed], 1,secondsPerBeat);
        //challengeAudioSource.Play();
    }

    private void FixedUpdate()
    {
        if(isLevelStarted)
        {
            //TODO: Start playing the midi/mp3 file
            //if(!trackAudioSource.isPlaying) trackAudioSource.Play();
            MoveBoard();
        }
        
    }
    
    private void LoadLevelAssets(VirtualPianoLevelSO levelConfig)
    {
        // Load text asset
        //levelConfig.songXml.LoadAssetAsync<TextAsset>().Completed += OnTextAssetLoaded;
        Addressables.LoadAssetAsync<TextAsset>(levelConfig.songXml).Completed += OnTextAssetLoaded;
        
        // Load first audio clip
        levelConfig.songClip.LoadAssetAsync<AudioClip>().Completed += handle =>
        {
            challengeAudioSource.clip = handle.Result;
        };

        if (levelConfig.backgroundClip.AssetGUID != "")
        {
            // Load second audio clip
            levelConfig.backgroundClip.LoadAssetAsync<AudioClip>().Completed += handle =>
            {
                backgroundAudioSource.clip = handle.Result;
            };
        }
    }

    public void UnloadLevelAssets()
    {
        scoreController.OnStarAchieved -= OnStarAchieved;
        AudioClip songClip = null;
        AudioClip backgroundClip = null;

        if (challengeAudioSource != null)
        {
            songClip = challengeAudioSource.clip;
            challengeAudioSource.clip = null;
        }
        var textAsset = songXmlAsset;

        if (backgroundAudioSource != null)
        {
            backgroundClip = backgroundAudioSource.clip;
            backgroundAudioSource.clip = null;
        }
        
        // Release loaded assets to free memory
        Addressables.Release(songClip);
        Addressables.Release(textAsset);
        Addressables.Release(backgroundClip);
    }

    private void OnTextAssetLoaded(AsyncOperationHandle<TextAsset> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            songXmlAsset = handle.Result;
            
            scoreRender.Init(rhythmGameSettings, songXmlAsset.text);

            var durationOneX = rhythmGameSettings.DurationOneX - (scoreRender.MeasureDivision / 2) * 
                rhythmGameSettings.DurationReductionPerDivision;
            
            // if(scoreRender.MeasureDivision == 4) 
            //     durationOneX = 0.5f; 
            // else 
            //     durationOneX = 1;
        
            beatsPerSecond = scoreRender.Bpm / 60;
            secondsPerBeat = 1 / beatsPerSecond;
            speedXPerSec = (durationOneX * scoreRender.MeasureDivision) * beatsPerSecond;
            beatsPerUnit = beatsPerSecond / speedXPerSec;
        
            noteViewList = scoreRender.Render();
            var noteCount = noteViewList.Count;
            
            List<string> keysUsed = new List<string>();
            foreach (var note in noteViewList)
            {
                if (!keysUsed.Contains(note.Pitch.Step))
                {
                    keysUsed.Add(note.Pitch.Step);
                }
            }
            virtualPianoController.EnableKeys(keysUsed);

            scoreController = new RhythmGameScoreController(rhythmGameSettings, noteCount);
            scoreController.OnStarAchieved += OnStarAchieved;
            OnNotesAmountCalculated?.Invoke(noteCount);

            // this.playRecorder = new PlayRecorder(notesSortedByScore);

            // //LoadMidiFile();
            // OnTempoChanged?.Invoke(scoreRender.Bpm);
            //MoveBoard();

            StartCoroutine(DelayedStart());
        }
        else
        {
            throw new Exception("Failed to load text asset.");
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

    #region MIDI Controller
    // private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    // {
    //     if (change != InputDeviceChange.Added)
    //     {
    //         return;
    //     }
    //
    //     var midiDevice = device as Minis.MidiDevice;
    //     if (midiDevice == null) return;
    //
    //     midiDevice.onWillNoteOn += OnWillNoteOn;
    // }
    //
    // private void OnWillNoteOn(Minis.MidiNoteControl note, float velocity)
    // {
    //     DispatchNoteOnEvent(note.noteNumber);
    // }
    //
    // private void DispatchEvents(List<SmfLite.MidiEvent> events)
    // {
    //     if (events == null)
    //     {
    //         return;
    //     }
    //
    //     foreach (var e in events)
    //     {
    //         if ((e.status & 0xf0) == 0x90)
    //         {
    //             this.DispatchNoteOnEvent(e.data1);
    //         }
    //     }
    // }
    //
    // private void DispatchNoteOnEvent(int noteNumber)
    // {
    //     var pitch = Pitch.GetPitchByMidiNoteNumber(noteNumber);
    //     this.PlayNote(pitch);
    // }


    // void PlayNote(Pitch pitch)
    // {
    //     this.playRecorder.Played(pitch, rhythmGameSettings.DurationOneX, scoreRender.Bpm, speedXPerSec);
    // }

    // void PlayNote(Pitch pitch)
    // {
    //     this.playRecorder.Played(pitch, rhythmGameSettings.DurationOneX, scoreRender.Bpm, speedXPerSec);
    // }
    
    #endregion

    private HitAccuracy EvaluateHit(GameObject noteObj)
    {
        var noteXPosition = noteObj.transform.position.x;
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

        if(result != HitAccuracy.Miss){
            noteObj.GetComponent<NoteController>().SetState(NoteState.Hit);
        }

        keyPressTxtFeedback?.Invoke(result.ToString());

        return result;
    }

    public void OnNoteTriggeredInteractionBar(NoteController note, bool isEntering)
    {
        //Debug.LogWarning("OnNoteTriggeredInteractionBar: " + note.Pitch.Step + note.Index.ToString() + " = " + isEntering);

        // if(entered && noteInteractableDic[note.Pitch.Step+note.Pitch.Octave] == null){
        //     noteInteractableDic[note.Pitch.Step+note.Pitch.Octave] = new List<NoteController>();
        // }
        
        // if(entered){
        //     noteInteractableDic[note.Pitch.Step+note.Pitch.Octave].Add(note);
        // }
        // else{
        //     noteInteractableDic[note.Pitch.Step+note.Pitch.Octave].Remove(note);
        // }

        if (!isEntering) notesCrossedInteractionArea++;

        if (note.State == NoteState.Miss && isEntering) return;

        if(isEntering){
            noteInteractableList.Add(note);
            
        }
        else{
            if(noteInteractableList.Contains(note))
                noteInteractableList.Remove(note);
        }

        Debug.Log("noteInteractableList: ");
        foreach(var n in noteInteractableList){
            Debug.Log(n.Pitch.Step + n.Index.ToString());
        }
        

        //noteInteractableDic[note.Pitch.Step+note.Pitch.Octave] = entered ? note : null;
        noteMaxDistanceToInteractionBar = Mathf.Abs(
            note.transform.position.x -
            interactionArea.transform.position.x);
    }

    public void OnTriggeredInteractionAreaCenter(NoteController note)
    {
       
        notesCrossed++;
        var nextNoteNotRest = notesCrossed;
        while (nextNoteNotRest < noteViewList.Count && noteViewList[nextNoteNotRest].isRest)
        {
            Debug.LogWarning("Rest note");
            nextNoteNotRest++;
        }
        
        if (notesCrossed < noteViewList.Count)
        {
            Debug.LogWarning("New note to bounce to - " + nextNoteNotRest + " = " + noteViewList[nextNoteNotRest].beatNumber);
            OnNextNoteUpdated?.Invoke(noteViewList[nextNoteNotRest], noteViewList[notesCrossed-1].beatNumber, 
                secondsPerBeat, beatsPerUnit);
        }

        notesCrossed = nextNoteNotRest;
       

        if (isAutoPlayTutorial)
        {
            StartCoroutine(AutoPlayKeyPress("C", 0.2f)); //hardcoded for now 
        }
    }
    private IEnumerator AutoPlayKeyPress(string note, float pressDuration)
    {
     
        OnPianoKeyStateChanged(note, true);
        yield return new WaitForSeconds(pressDuration);    
        OnPianoKeyStateChanged(note, false);
    }

    public void SaveIsAutoPlayTutorial(bool isTutorial)
    {
        PlayerPrefs.SetInt(autoPlayTutorialKey, isTutorial ? 0 : 1);
        PlayerPrefs.Save();
    }



    public void OnPianoKeyStateChanged(string note, bool isPressed)
    {
        if (isAutoPlayTutorial)
        {
            
            foreach (var key in virtualPianoController.GetMainPianoKeys())
            {
                if (key.GetNote() == note)
                {
                    var button = key.GetComponent<Button>();

                    if (button != null)
                    {
                        var colors = button.colors;
                        button.image.color = isPressed ? Color.green : colors.normalColor;
                    }
                }
            }
        }

        if (isPressed)
        {
            var accuracy = HitAccuracy.Miss;

            int indexToRemove = 0;
            foreach(var noteObj in noteInteractableList){
                if(noteObj.Pitch.Step == note){
                    accuracy = EvaluateHit(noteObj.gameObject);
                    OnNoteHit?.Invoke();
                    break;
                }
                indexToRemove++;
            }

            if(accuracy != HitAccuracy.Miss){
                noteInteractableList[indexToRemove].StartAnimation(this.secondsPerBeat, this.speedXPerSec);
                noteInteractingList.Add(noteInteractableList[indexToRemove]);
                noteInteractableList.RemoveAt(indexToRemove);
                
            }
            else
            {
                if (notesCrossedInteractionArea < noteViewList.Count)
                {
                  if(noteViewList[notesCrossedInteractionArea].GameObject.GetComponent<NoteController>().State != NoteState.Hit) 
                      noteViewList[notesCrossedInteractionArea].GameObject.GetComponent<NoteController>().SetState(NoteState.Miss);
                  else
                      return;
                }
            }

            ProcessScore(accuracy);
            keyPressTxtFeedback?.Invoke(accuracy.ToString());
        }
        else
        {
            int indexToRemove = 0;
            foreach(var noteObj in noteInteractingList){
                if(noteObj.Pitch.Step == note && noteObj.IsFilling){
                    noteObj.StopAnimation();
                    break;
                }
                indexToRemove++;
            }
            if(indexToRemove < noteInteractingList.Count) noteInteractingList.RemoveAt(indexToRemove);
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
