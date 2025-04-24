using System;
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

    private float speedXPerSec;

    private float distanceBetweenScoreLines;
    private float distanceBetweenOctave;

    private float OneNoteY;
    private float YOfC4;
    private float _durationOneX;
    private bool _onlyFirstStaff;
    private ColorSettings _colorSettings;
    private RhythmGameSettings gameSettings;

    private Score musicScore;
    public float Bpm => musicScore.Tempo ?? 120;
    public int BeatsPerBar = 4;

    //private int _currentMeasureDivision = 4;
    public int MeasureDivision => musicScore.CurrentDivisions.Value;

    private int noteIndex = 1;

    public int currentDivisions;
    
    private bool isRightHand = true;
    public bool IsRightHand => isRightHand;


    void Awake()
    {
        distanceBetweenScoreLines = scoreLines[1].position.y - scoreLines[0].position.y;
        OneNoteY = distanceBetweenScoreLines / 2;
        distanceBetweenOctave = OneNoteY * Pitch.NoteListMain.Count;
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
                    clefImg.transform.localPosition = new Vector3(clefImg.transform.localPosition.x, 
                        clefImg.transform.localPosition.y + OneNoteY*2, clefImg.transform.localPosition.z);
                }

                if (measure.Attribute?.Time == null) continue;
                BeatsPerBar = measure.Attribute.Value.Time.Value.Beats;
                beatsText.text = measure.Attribute.Value.Time.Value.Beats.ToString();
                beatTypeText.text = measure.Attribute.Value.Time.Value.BeatType.ToString();
                
                break; // we only need the first one
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
        float currentBeat = 1;
        bool noNeedToDisplayForTie = false;

        //var secondsPerBeat = 1f / (Bpm / 60);
        // float xCursor = /*-gameSettings.DurationOneX*/
        // initialNoteSpawningPoint.position.x + gameSettings.beatsBeforeStart * secondsPerBeat * speedXPerSec;
        
        // no need fori nitial note spawning cause position is local for notes
        //  MAGIC NUMBER I KNOW, SUPER UGLY I AM SORRY, THIS IS RELATED TO POSITION OF INTERACTION BAR
        //float distanceBetweenBars = this._durationOneX * currentDivisions * (0.25f / 2);
        int barLinesToDraw = 1;
        var lastLineXPosition = 0f;
        var beatsPerSecond = Bpm / 60;
        var secondsPerBeat = 1 / beatsPerSecond;
        float barLength = 4;
        GameObject beatMarker;
        
        float initialNoteSpawningOffsetX = (gameSettings.delayBeforeLevelStart * beatsPerSecond + 
                                           gameSettings.beatsBeforeStart) * this._durationOneX * currentDivisions -
                                           0.35f;
        float xCursor = initialNoteSpawningOffsetX;
        
        foreach (var part in musicScore.ScoreParts)
        {
            // beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
            // beatMarker.transform.position = new Vector3(initialNoteSpawningPoint.position.x + 
            //                                             initialNoteSpawningOffsetX, 1, 0);
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
                barLength = (float)(/*currentDivisions */ speedXPerSec * secondsPerBeat * beatsPerBar);

                // for (int i = 1; i <= 4 ; i++)
                // {
                     beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
                     beatMarker.transform.position = 
                         new Vector3(initialNoteSpawningPoint.position.x + initialNoteSpawningOffsetX - 
                                     (this._durationOneX/2) + barLinesToDraw++ * barLength
                                                                                                   /*-
                                     (_durationOneX /* currentDivisions) / 2*/, 1, 0);
                     
                     lastLineXPosition = beatMarker.transform.position.x; 
                    
                // }

                foreach (IMeasureChild child in measure.Children)
                {
                    if (child is ScoreNote)
                    {
                        var note = (ScoreNote)child;

                        // if (note.Pitch != null)
                        // {
                        //     Debug.LogWarning("note: " + note.Pitch.Value.Step + " " + note.Pitch.Value.Octave + " - " + note.Type);
                        // }

                        float y = scoreLines[3].transform.position.y;
                        // y
                        if (note.Pitch != null)
                        {
                            //Debug.Log("Midinotenumber: " + note.Pitch.Value.GetMidiNoteNumber());
                            //y = note.Pitch.Value.GetMidiNoteNumber() * OneNoteY - YOfC4;
                            y = Pitch.NoteListMain.IndexOf(note.Pitch.Value.Step) * OneNoteY +
                                (note.Pitch.Value.Octave - (4 + (isRightHand ? 0 : -1))) * distanceBetweenOctave;
                            
                            if (!isRightHand) y += OneNoteY * 5f; //5 is the amount of notes that C go 'upwards' between G and F clef

                            if (note.Pitch.Value.Step == "C" && note.Pitch.Value.Octave == 4)
                            {
                                y += OneNoteY / 4 * 3;
                            }
                        }

                        // x
                        
                        float willConsumedTimeUnit = (float)note.Duration/*/currentDivisions*/ * this._durationOneX;
                        //Debug.Log("willConsumedTimeUnit: " + willConsumedTimeUnit);

                        if (note.IsChord)
                        {
                            // 開始位置は、前の音と同じ位置
                            xCursor = notesSortedByScore[Math.Max(notesSortedByScore.Count - 1, 0)].X;
                        }

                        // instantiate
                        if (note.Pitch != null && !noNeedToDisplayForTie || note.IsRest)
                        {
                            var noteObj = InstantiateNote(xCursor, y, willConsumedTimeUnit, note);
                            var noteView = new NoteView() { GameObject = noteObj, X = xCursor, Pitch = note.IsRest ? new Pitch() : note.Pitch.Value,
                                beatNumber = currentBeat, noteTimeInSeconds = currentBeat / Bpm * 60, lastNote = false, isRest = note.IsRest};
                            notesSortedByScore.Add(noteView);

                            //if (note.Staff != 1 && _onlyFirstStaff)
                            //{
                            //    noteObj.SetActive(false);
                            //}
                        }
                        if (note.Pitch != null && noNeedToDisplayForTie)
                        {
                            // 1つ前のNoteに長さを加える
                            var noteController = notesSortedByScore[notesSortedByScore.Count - 1].GameObject.GetComponent<NoteController>();
                            //Debug.Log("WillConsumedTimeUnit: " + willConsumedTimeUnit);
                            noteController.AddWidth(willConsumedTimeUnit);
                        }

                        currentBeat += note.Duration / (float)currentDivisions;
                        xCursor += willConsumedTimeUnit;

                        //Debug.LogWarning("Current beat: " + currentBeat);
                        // if (currentBeat % 5 == 0)
                        // {
                        //     Debug.LogWarning("Bar: " + barLinesToDraw);
                        //     beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
                        //     beatMarker.transform.position = new Vector3(initialNoteSpawningPoint.position.x + 
                        //         initialNoteSpawningOffsetX /*- (currentDivisions * gameSettings.DurationOneX)*/ + 
                        //         barLinesToDraw++ * barLength /*+ (_durationOneX /** currentDivisions)/2*/, 1, 0);
                        // }

                        // If both start and stop are present, noNeedToDisplayForTie should be true.
                        if (note.TieList != null && note.TieList.Exists(x => x.Type == "stop"))
                        {
                            noNeedToDisplayForTie = false;
                        }
                        if (note.TieList != null && note.TieList.Exists(x => x.Type == "start"))
                        {
                            noNeedToDisplayForTie = true;
                        }
                        //Debug.Log("xCursor depois de nota: " + xCursor);
                    }
                    else if (child is Backup)
                    {
                        // backup は、x のCursorを元に戻す
                        var backup = (Backup)child;
                        // どのくらい x を戻るか
                        float backupUnit = backup.Duration * this._durationOneX;

                        xCursor -= backupUnit;
                    }
                }
            }
            //Debug.LogWarning("Bar: " + barLinesToDraw);
//             if (currentBeat % beatsPerBar != 0)
//                  {
//                      beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
//                      beatMarker.transform.position = 
//                          new Vector3(initialNoteSpawningPoint.position.x + initialNoteSpawningOffsetX - 
//                              (gameSettings.DurationOneX/2) + barLinesToDraw++ * barLength 
//                              /*-
// (_durationOneX /* currentDivisions) / 2*/, 1, 0);
//                  }
            // beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
            // beatMarker.transform.position = new Vector3(initialNoteSpawningPoint.position.x + 
            //                                             initialNoteSpawningOffsetX - (currentDivisions * gameSettings.DurationOneX) + 
            //                                             barLinesToDraw++ * barLength + (_durationOneX * currentDivisions)/2, 1, 0);
            
            beatMarker = Instantiate(beatMarkerPrefab, notesContainer.transform);
            beatMarker.transform.position = new Vector3(lastLineXPosition + gameSettings.LastBarLineHorizontalDistance, 
                1, 0);
            beatMarker.transform.localScale = new Vector3(beatMarker.transform.localScale.x * 
                                                          gameSettings.LastBarLineThickness,
                beatMarker.transform.localScale.y, beatMarker.transform.localScale.z);
        }

        return notesSortedByScore;
    }

    GameObject InstantiateNote(float positionX, float positionY, float willConsumedTimeUnit, ScoreNote note)
    {
        //Debug.Log("InstantiateNote: " + note.Pitch?.Octave + " " + note.Pitch?.Step + " " + note.Type);
        GameObject noteObj = Instantiate<GameObject>(
            this.GetPrafabByNote(note),
            new Vector3(0, 0, 0),
            Quaternion.identity,
            notesContainer.transform);
        
        noteObj.transform.localPosition = Vector3.zero; // Keeps it at the parent's exact position
        //noteObj.transform.localRotation = Quaternion.identity; // Aligns rotation with the parent

        noteObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        noteObj.transform.localPosition =  new Vector3(positionX, positionY, 0);

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
        var beats = (float)note.Duration / currentDivisions;
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
