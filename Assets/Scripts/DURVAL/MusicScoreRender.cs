using System;
using System.Collections.Generic;
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

    private int _currentMeasureDivision = 4;
    public int MeasureDivision => musicScore.CurrentDivisions.Value;

    private int noteIndex = 1;


    void Awake()
    {
        distanceBetweenScoreLines = scoreLines[1].position.y - scoreLines[0].position.y;
        OneNoteY = distanceBetweenScoreLines / 2;
        distanceBetweenOctave = OneNoteY * Pitch.NoteListMain.Count;
        notesContainer.transform.localPosition = new Vector3(notesContainer.transform.localPosition.x, 
            notesContainer.transform.localPosition.y - distanceBetweenScoreLines, notesContainer.transform.localPosition.z);
        
        //YOfC4 = OneNoteY * Pitch.NoteList.Count * 5;
        YOfC4 = OneNoteY * Pitch.NoteListMain.Count * 5;
        Debug.Log("YOC4" + YOfC4);
        Debug.Log("OneNoteY" + OneNoteY);
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
            }
        }


        var beatsPerSecond = Bpm / 60;
        var secondsPerBeat = 1 / beatsPerSecond;
        var speedXPerSec = (_durationOneX * musicScore.CurrentDivisions) * beatsPerSecond;
        int beatsPerBar = 4; // Adjust if you have a different time signature
        float barLength = (float)(musicScore.CurrentDivisions * speedXPerSec * secondsPerBeat);

        for (int i = 1; i <= 4 * gameSettings.numberBeatsToDrawLines ; i++)
        {
            var beatMarker = Instantiate(beatMarkerPrefab, gameObject.transform);
            beatMarker.transform.position = new Vector3(initialNoteSpawningPoint.position.x + i * barLength, 1, 0);
        }
    }

    public IList<NoteView> Render(float speedXPerSec)
    {
        // var score = MusicXMLParser.GetScorePartwise(musicXMLText);
        var notesSortedByScore = new List<NoteView>();

        int currentDivisions = musicScore.CurrentDivisions.Value;
        float currentBeat = 1;
        bool noNeedToDisplayForTie = false;

        var secondsPerBeat = 1f / (Bpm / 60);
        // float xCursor = /*-gameSettings.DurationOneX*/
        // initialNoteSpawningPoint.position.x + gameSettings.beatsBeforeStart * secondsPerBeat * speedXPerSec;
        
        // no need fori nitial note spawning cause position is local for notes
        float xCursor = (gameSettings.beatsBeforeStart+1) * currentDivisions * gameSettings.DurationOneX;
        
        foreach (var part in musicScore.ScoreParts)
        {
            foreach (var measure in part.MeasureList)
            {
                if (measure.Attribute?.Divisions != null)
                {
                    currentDivisions = measure.Attribute.Value.Divisions.Value;
                }

                foreach (IMeasureChild child in measure.Children)
                {
                    if (child is ScoreNote)
                    {
                        var note = (ScoreNote)child;

                        if (note.Pitch != null)
                        {
                            Debug.LogWarning("note: " + note.Pitch.Value.Step + " " + note.Pitch.Value.Octave + " - " + note.Type);
                        }

                        float y = 0;
                        // y
                        if (note.Pitch != null)
                        {
                            //Debug.Log("Midinotenumber: " + note.Pitch.Value.GetMidiNoteNumber());
                            //y = note.Pitch.Value.GetMidiNoteNumber() * OneNoteY - YOfC4;
                            y = Pitch.NoteListMain.IndexOf(note.Pitch.Value.Step) * OneNoteY +
                                (note.Pitch.Value.Octave - 4) * distanceBetweenOctave;
                        }

                        // x
                        
                        float willConsumedTimeUnit = (float)note.Duration/*/currentDivisions*/ * this._durationOneX;
                        Debug.Log("willConsumedTimeUnit: " + willConsumedTimeUnit);

                        if (note.IsChord)
                        {
                            // 開始位置は、前の音と同じ位置
                            xCursor = notesSortedByScore[Math.Max(notesSortedByScore.Count - 1, 0)].X;
                        }

                        // instantiate
                        if (note.Pitch != null && !noNeedToDisplayForTie)
                        {
                            var noteObj = InstantiateNote(xCursor, y, willConsumedTimeUnit, note);
                            var noteView = new NoteView() { GameObject = noteObj, X = xCursor, Pitch = note.Pitch.Value,
                                beatNumber = currentBeat, noteTimeInSeconds = currentBeat / Bpm * 60, lastNote = false };
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
                            noteController.AddWidth(willConsumedTimeUnit);
                        }

                        currentBeat += note.Duration / (float)currentDivisions;
                        xCursor += willConsumedTimeUnit;

                        // If both start and stop are present, noNeedToDisplayForTie should be true.
                        if (note.TieList != null && note.TieList.Exists(x => x.Type == "stop"))
                        {
                            noNeedToDisplayForTie = false;
                        }
                        if (note.TieList != null && note.TieList.Exists(x => x.Type == "start"))
                        {
                            noNeedToDisplayForTie = true;
                        }
                        Debug.Log("xCursor depois de nota: " + xCursor);
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
        }

        return notesSortedByScore;
    }

    GameObject InstantiateNote(float positionX, float positionY, float willConsumedTimeUnit, ScoreNote note)
    {
        Debug.Log("InstantiateNote: " + note.Pitch?.Octave + " " + note.Pitch?.Step + " " + note.Type);
        GameObject noteObj = Instantiate<GameObject>(
            this.GetPrafabByNote(note),
            new Vector3(0, 0, 0),
            Quaternion.identity,
            notesContainer.transform);

        noteObj.transform.localPosition = Vector3.zero; // Keeps it at the parent's exact position
        //noteObj.transform.localRotation = Quaternion.identity; // Aligns rotation with the parent

        noteObj.transform.localRotation = Quaternion.Euler(0, 0, 0);
        noteObj.transform.localPosition =  new Vector3(positionX, positionY, 0);
        var noteController = noteObj.GetComponent<NoteController>();
        
        noteController.X = positionX;
        noteController.Width = willConsumedTimeUnit;
        //noteController.Note = note;
        noteController.ColorSettings = this._colorSettings;
        noteController.Index = noteIndex++;

        noteObj.name = $"{note.Pitch?.Octave}{note.Pitch?.Step}{note.Type}{noteController.Index}";

        noteController.SetNote(note, gameSettings);

        return noteObj;
    }

    GameObject GetPrafabByNote(ScoreNote note)
    {
        // if (note.Type == "16th")
        // {
        //     return this._circleNotePrefab;
        // }

        // return this._notePrefab;

        // if(note.Type == "whole")
        //     return gameSettings.GetPrefabForNoteType(note.Type);
        // else
            return gameSettings.GetPrefabForNoteType("template");

        if(note.IsRest)
            return gameSettings.GetPrefabForNoteType("rest");
        else
            return gameSettings.GetPrefabForNoteType(note.Type);
    }
}
