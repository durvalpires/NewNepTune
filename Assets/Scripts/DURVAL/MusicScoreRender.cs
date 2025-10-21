using System;
using System.Collections;
using System.Collections.Generic;
using DURVAL;
using TMPro;
using UnityEngine;

public class MusicScoreRender : MonoBehaviour {

    [SerializeField]
    private Transform[] scoreLines;

    [SerializeField]
    private Transform initialNoteSpawningPoint;

    [SerializeField]
    private GameObject notesContainer;

    [SerializeField]
    private GameObject beatMarkerPrefab;

    [SerializeField] private TextMeshProUGUI beatsText;
    [SerializeField] private TextMeshProUGUI beatTypeText;
    [SerializeField] private SpriteRenderer clefImg;

    private double speedXPerSec;

    private float distanceBetweenScoreLines;
    private float distanceBetweenOctave;

    private float OneNoteY;
    private float YOfC4;
    private double _durationOneX;
    public float minDurationValue = 1;
    private bool _onlyFirstStaff;
    private ColorSettings _colorSettings;
    private RhythmGameSettings gameSettings;

    private Score musicScore;
    public float Bpm => musicScore.Tempo ?? 80;
    public int BeatsPerBar = 4;

    //private int _currentMeasureDivision = 4;
    public int MeasureDivision => musicScore.CurrentDivisions.Value;

    private int noteIndex = 1;

    public int currentDivisions;
    
    private bool isRightHand = true;
    public bool IsRightHand => isRightHand;

    public event Action<HandType> OnClefChanged;

    void Awake()
    {
        distanceBetweenScoreLines = scoreLines[1].position.y - scoreLines[0].position.y;
        OneNoteY = distanceBetweenScoreLines / 2;

        // Debug: Check the note list counts
        Debug.Log($"[Octave Debug] Pitch.NoteListMain.Count: {Pitch.NoteListMain.Count}");
        Debug.Log($"[Octave Debug] Pitch.NoteList.Count: {Pitch.NoteList.Count}");
        Debug.Log($"[Octave Debug] OneNoteY: {OneNoteY}");

        distanceBetweenOctave = OneNoteY * Pitch.NoteListMain.Count;
        Debug.Log($"[Octave Debug] distanceBetweenOctave (using NoteListMain): {distanceBetweenOctave}");

        // POTENTIAL FIX: Use 12 semitones for a proper octave instead of 7 natural notes
        // distanceBetweenOctave = OneNoteY * 12; // 12 semitones in an octave

        notesContainer.transform.localPosition = new Vector3(notesContainer.transform.localPosition.x,
            notesContainer.transform.localPosition.y - distanceBetweenScoreLines /*+ (4-3)*distanceBetweenOctave*/, notesContainer.transform.localPosition.z);

        //YOfC4 = OneNoteY * Pitch.NoteList.Count * 5;
        YOfC4 = OneNoteY * Pitch.NoteListMain.Count * 5;
    }

    public void Init(RhythmGameSettings gameSettings, string musicXMLText)
    {
        this._colorSettings = gameSettings.ColorSettings;
        this._durationOneX = gameSettings.DurationOneX;
        this._onlyFirstStaff = gameSettings.OnlyUseFirstStaff;

        //this.speedXPerSec = speedXPerSec;
        this.gameSettings = gameSettings;

        this.musicScore = MusicXMLParser.GetScorePartwise(musicXMLText);
        foreach (var part in musicScore.ScoreParts)
        {
            foreach (var measure in part.MeasureList)
            {
                if (measure.Attribute?.Divisions != null)
                {
                    musicScore.CurrentDivisions = measure.Attribute.Value.Divisions.Value;
                }
                
                if(measure.Attribute.Value.Clef != null)
                {
                    isRightHand = measure.Attribute.Value.Clef.Value.Sign == "G";
                    
                    clefImg.sprite = gameSettings.GetClefSprite(
                        measure.Attribute.Value.Clef.Value.Sign);
                    clefImg.color = Color.black;
                    
                    if(!isRightHand)
                        clefImg.transform.localPosition = new Vector3(clefImg.transform.localPosition.x, 
                            clefImg.transform.localPosition.y + OneNoteY*2, clefImg.transform.localPosition.z);

                    if (!isRightHand)
                    {
                        clefImg.transform.localPosition = new Vector3(
                            clefImg.transform.localPosition.x,
                            clefImg.transform.localPosition.y + OneNoteY * 2,
                            clefImg.transform.localPosition.z);
                    }
                    string clefSign = measure.Attribute.Value.Clef.Value.Sign;
                    HandType handType;
                    if (clefSign == "G") handType = HandType.Right;
                    else if (clefSign == "F") handType = HandType.Left;
                    else handType = HandType.Both;

                    OnClefChanged?.Invoke(handType);
                }

                if (measure.Attribute?.Time == null) continue;
                BeatsPerBar = measure.Attribute.Value.Time.Value.Beats;
                beatsText.text = measure.Attribute.Value.Time.Value.Beats.ToString();
                beatTypeText.text = measure.Attribute.Value.Time.Value.BeatType.ToString();
                
                break; // we only need the first one
            }
        }
        
        //TODO remove this when notes with division > 1 are on sync
        foreach (var part in musicScore.ScoreParts)
        {
            foreach (var measure in part.MeasureList)
            {
                foreach (IMeasureChild child in measure.Children)
                {
                    if (child is ScoreNote)
                    {
                        var note = (ScoreNote)child;
                        if (note.Duration < minDurationValue)
                            minDurationValue = note.Duration;
                    }
                }
            }
        }
        
        this._durationOneX = gameSettings.DurationOneX - (musicScore.CurrentDivisions.Value / 2) * 
                gameSettings.DurationReductionPerDivision;
    }

    public IList<NoteView> Render()
    {
        // var score = MusicXMLParser.GetScorePartwise(musicXMLText);
        var notesSortedByScore = new List<NoteView>();

        currentDivisions = musicScore.CurrentDivisions.Value;
        double currentBeat = 1;
        bool noNeedToDisplayForTie = false;

        //var secondsPerBeat = 1f / (Bpm / 60);
        // double xCursor = /*-gameSettings.DurationOneX*/
        // initialNoteSpawningPoint.position.x + gameSettings.beatsBeforeStart * secondsPerBeat * speedXPerSec;
        
        // no need fori nitial note spawning cause position is local for notes
        //  MAGIC NUMBER I KNOW, SUPER UGLY I AM SORRY, THIS IS RELATED TO POSITION OF INTERACTION BAR
        //double distanceBetweenBars = this._durationOneX * currentDivisions * (0.25f / 2);
        int barLinesToDraw = 1;
        var lastLineXPosition = 0f;
        double beatsPerSecond = Bpm / 60;
        double secondsPerBeat = 1 / beatsPerSecond;
        double barLength = 4;
        GameObject beatMarker;
        
        double initialNoteSpawningOffsetX = (gameSettings.delayBeforeLevelStart * beatsPerSecond + 
                                             gameSettings.beatsBeforeStart) * this._durationOneX * currentDivisions -
                                            gameSettings.GetMainCircleWidth();
        
        double xCursor = initialNoteSpawningOffsetX;
        
        foreach (var part in musicScore.ScoreParts)
        {
            int beatsPerBar = 4;
            foreach (var measure in part.MeasureList)
            {
                
                if (measure.Attribute?.Divisions != null)
                {
                    currentDivisions = measure.Attribute.Value.Divisions.Value;
                    this._durationOneX = gameSettings.DurationOneX - (currentDivisions / 2) * 
                        gameSettings.DurationReductionPerDivision;
                    beatsPerBar = measure.Attribute.Value.Time.Value.Beats;
                }

                var speedXPerSec = (_durationOneX * currentDivisions) * beatsPerSecond;
                 
                // Adjust if you have a different time signature
                barLength = (speedXPerSec * secondsPerBeat * beatsPerBar);
                
                beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
                var durationMinus1Offset = (minDurationValue < 1) ?((minDurationValue/2)*(float)this._durationOneX) : 0;
                beatMarker.transform.position = 
                     new Vector3(initialNoteSpawningPoint.position.x + (float)(initialNoteSpawningOffsetX - 
                                 (this._durationOneX/2)) +  durationMinus1Offset + barLinesToDraw++ * (float)barLength,
                                    1, 0);
                 
                lastLineXPosition = beatMarker.transform.position.x; 
                    
                // }
                if (gameSettings.showLinePerBeat)
                {
                    var spawnXCoordinate = xCursor; 
                    for (int i = 0; i < 40; i++)
                    {
                        Instantiate(beatMarkerPrefab, notesContainer.transform).transform.position = 
                            new Vector3((float)spawnXCoordinate, 1, 0);
                        spawnXCoordinate += this._durationOneX;
                    }
                    
                }

                IMeasureChild child;
                IMeasureChild followingChild;
                
                for (int i = 0; i < ((ICollection)measure.Children).Count; i++)
                {
                    child = measure.Children[i];
                    // followingChild = null;
                    // if (i + 1 >= ((ICollection)measure.Children).Count)
                    //     followingChild = measure.Children[i+1];

                    if (child is ScoreNote)
                    {
                        var note = (ScoreNote)child;
                        // var nextNote = (ScoreNote)followingChild;

                        double y = scoreLines[3].transform.position.y;

                        // y
                        if (note.Pitch != null)
                        {
                            int stepIndex = Pitch.NoteListMain.IndexOf(note.Pitch.Value.Step);
                            int octave = note.Pitch.Value.Octave;
                            int octaveOffset = 4 + (isRightHand ? 0 : -1);
                            int octaveDifference = octave - octaveOffset;

                            // Debug.Log($"[Y Calculation] Note: {note.Pitch.Value.Step}{octave}");
                            // Debug.Log($"[Y Calculation] Step Index: {stepIndex}, OneNoteY: {OneNoteY}");
                            // Debug.Log($"[Y Calculation] Octave: {octave}, Octave Offset: {octaveOffset}, Difference: {octaveDifference}");
                            // Debug.Log($"[Y Calculation] Distance Between Octave: {distanceBetweenOctave}");
                            // Debug.Log($"[Y Calculation] Is Right Hand: {isRightHand}");

                            y = stepIndex * OneNoteY + octaveDifference * distanceBetweenOctave;
                            //Debug.Log($"[Y Calculation] Base Y calculation: {stepIndex} * {OneNoteY} + {octaveDifference} * {distanceBetweenOctave} = {y}");

                            if (!isRightHand)
                            {
                                float leftHandAdjustment = OneNoteY * 5f;
                                y += leftHandAdjustment;
                                //Debug.Log($"[Y Calculation] Left hand adjustment: +{leftHandAdjustment}, New Y: {y}");
                            }

                            if (note.Pitch.Value.Step == "C" && note.Pitch.Value.Octave == 4)
                            {
                                float c4Adjustment = OneNoteY / 4 * 3;
                                y += c4Adjustment;
                                //Debug.Log($"[Y Calculation] C4 special adjustment: +{c4Adjustment}, Final Y: {y}");
                            }

                            // Debug.Log($"[Y Calculation] Final Y position for {note.Pitch.Value.Step}{octave}: {y}");
                            // Debug.Log("----------------------------------------");
                        }

                        // x
                        double willConsumedTimeUnit = note.Duration * this._durationOneX;

                        if (note.IsChord)
                        {
                            xCursor = notesSortedByScore[Math.Max(notesSortedByScore.Count - 1, 0)].X;
                        }

                        // instantiate
                        if (note.Pitch != null && !noNeedToDisplayForTie || note.IsRest)
                        {
                            var noteObj = InstantiateNote(xCursor, y, willConsumedTimeUnit, note);
                            var noteView = new NoteView()
                            {
                                GameObject = noteObj,
                                X = xCursor,
                                Pitch = note.IsRest ? new Pitch() : note.Pitch.Value,
                                beatNumber = currentBeat,
                                noteTimeInSeconds = currentBeat / Bpm * 60,
                                lastNote = false,
                                isRest = note.IsRest
                            };
                            notesSortedByScore.Add(noteView);
                        }
                        
                        var noteController = notesSortedByScore[notesSortedByScore.Count - 1].GameObject.GetComponent<NoteController>();
                        
                        //BEAM LOGIC
                        if (note.BeamList != null && note.BeamList.Count > 0)
                        {
                            for (int j = 0; j < note.BeamList.Count; j++)
                            {
                                if (note.BeamList[j].Type != "begin")
                                {
                                    var previousNoteController = notesSortedByScore[notesSortedByScore.Count - 2].GameObject.GetComponent<NoteController>();
                                    
                                    ConnectBeam(noteController, previousNoteController, j);
                                }
                            }
                        }

                        if (note.Pitch != null && noNeedToDisplayForTie)
                        {
                            
                            noteController.AddWidth(willConsumedTimeUnit);
                        }

                        currentBeat += note.Duration / (double)currentDivisions;
                        xCursor += willConsumedTimeUnit;

                        if (note.TieList != null && note.TieList.Exists(x => x.Type == "stop"))
                        {
                            noNeedToDisplayForTie = false;
                        }
                        if (note.TieList != null && note.TieList.Exists(x => x.Type == "start"))
                        {
                            noNeedToDisplayForTie = true;
                        }

                        // Optional: Access the next element if you need it
                        // if (i + 1 < measure.Children.Count)
                        // {
                        //     IMeasureChild next = measure.Children[i + 1];
                        //     // Do something with next...
                        // }
                    }
                    else if (child is Backup)
                    {
                        var backup = (Backup)child;
                        double backupUnit = backup.Duration * this._durationOneX;
                        xCursor -= backupUnit;
                    }
                }
            }
            
            beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
            beatMarker.transform.position = new Vector3(lastLineXPosition + gameSettings.LastBarLineHorizontalDistance, 
                1, 0);
            beatMarker.transform.localScale = new Vector3(beatMarker.transform.localScale.x * 
                                                          gameSettings.LastBarLineThickness,
                beatMarker.transform.localScale.y, beatMarker.transform.localScale.z);
        }

        return notesSortedByScore;
    }
    
    private void ConnectBeam(NoteController noteController, NoteController previousNoteController, int beamIndex)
    {
        Debug.Log($"Connecting beam {beamIndex}");
        Vector3 start = beamIndex == 0 ? noteController.BeamTopSprite.transform.position : noteController.BeamBottomSprite.transform.position;
        Vector3 end = beamIndex == 0 ? previousNoteController.BeamTopSprite.transform.position : previousNoteController.BeamBottomSprite.transform.position;
        var beamSprite = beamIndex == 0 ? noteController.BeamTopSprite : noteController.BeamBottomSprite;

        // Calculate distance
        // float distance = Vector3.Distance(start, end);
        // beamSprite.transform.localScale = new Vector3(-distance, beamSprite.transform.localScale.y, 1);
        //
        // // Calculate angle
        // Vector3 direction = end - start;
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // beamSprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        Vector3 direction = end - start;
        float distance = direction.magnitude;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        
        var beamChild = beamSprite.transform.GetChild(0);

        //beamSprite.transform.position = (start + end) * 0.5f;
        beamChild.transform.rotation = Quaternion.Euler(0, 0, -angle);

        // Scale uniformly (avoid skew)
        beamSprite.localScale = new Vector3(-distance, 1/*thickness*/, 1);
        
        beamSprite.gameObject.SetActive(true);
        beamChild.GetComponentInChildren<SpriteRenderer>().sprite = gameSettings.beamSprite;
    }

    GameObject InstantiateNote(double positionX, double positionY, double willConsumedTimeUnit, ScoreNote note)
    {
        GameObject noteObj = Instantiate<GameObject>(
            this.GetPrafabByNote(note),
            new Vector3(0, 0, 0),
            Quaternion.identity,
            notesContainer.transform);
        
        noteObj.transform.localPosition = Vector3.zero; // Keeps it at the parent's exact position
        //noteObj.transform.localRotation = Quaternion.identity; // Aligns rotation with the parent

        noteObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        noteObj.transform.localPosition =  new Vector3((float)positionX, (float)positionY, 0);

        if (note.IsRest)
        {
            noteObj.GetComponent<RestController>().SetupRestSprite(GetRestType(note));
            noteObj.name = $"{"Rest"}{noteIndex}";
        }
        else
        {
            var noteController = noteObj.GetComponent<NoteController>();
        
            noteController.X = positionX;
            noteController.AddWidth(willConsumedTimeUnit);
            //noteController.Note = note;
            noteController.ColorSettings = this._colorSettings;
            noteController.Index = noteIndex++;

            noteObj.name = $"{note.Pitch?.Octave}{note.Pitch?.Step}{note.Type}{noteController.Index}";

            noteController.SetNote(note, gameSettings, currentDivisions);
        }

        return noteObj;
    }

    GameObject GetPrafabByNote(ScoreNote note)
    {
        // if (note.Type == "16th")
        // {
        //     return this._circleNotePrefab;
        // }

        // return this._notePrefab;
        
        if(note.IsRest)
            return gameSettings.GetPrefabForNoteType("rest");
        else
            return gameSettings.GetPrefabForNoteType("template");

        // if(note.Type == "whole")
        //     return gameSettings.GetPrefabForNoteType(note.Type);
        // else
    }
   

    private string GetRestType(ScoreNote note)
    {
        var beats = (double)note.Duration / currentDivisions;
        switch (beats)
        {
            case 4: return "whole";
            case 2: return "half";
            case 1: return "quarter";
            case 0.5f: return "eighth";
            case 0.25f: return "sixteenth";
        }

        return "quarter";
    }
}
