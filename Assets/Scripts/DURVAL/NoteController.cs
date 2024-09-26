using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteController : MonoBehaviour
{
    //Transform tableObj;
    Transform playNoteObj;

    public float X { get; set; } = 0;

    public float Width { get; set; } = 0;

    public ScoreNote Note { get; set; }
    public Pitch Pitch { get
        {
            return Note.Pitch.Value;
        } 
    }
    public ColorSettings ColorSettings { get; set; }

    public static float ScaleFor16th = 1;

    [SerializeField]
    private ScoreNote note;

    //[SerializeField]
    //private RhythmGameSettings gameSettings;


    void Start()
    {
        this.note = Note;
        //this.tableObj = this.gameObject.transform.Find("TablePrefab");
        //this.playNoteObj = this.gameObject.transform.Find("PlayNotePrefab");
        this.playNoteObj = transform;

        if (this.Note.Type == "16th")
        {
            //this.tableObj.transform.localScale = new Vector3(ScaleFor16th, ScaleFor16th, ScaleFor16th);
            //this.playNoteObj.transform.localScale = new Vector3(ScaleFor16th, ScaleFor16th, ScaleFor16th);
        }
        else
        {
            this.transform.localPosition = new Vector3(this.X /*+ (this.Width / 2)*/,
                this.transform.localPosition.y,
                this.transform.localPosition.z);

            //this.tableObj.transform.localScale = new Vector3(this.Width,
            //    this.tableObj.transform.localScale.y,
            //    this.tableObj.transform.localScale.z);

            this.playNoteObj.transform.localScale = new Vector3(this.Width,
                this.playNoteObj.transform.localScale.y,
                this.playNoteObj.transform.localScale.z);
        }

        this.SetColor();
    }

    void SetColor()
    {
        switch (this.Note.Pitch?.Step)
        {
            case "C":
                this.SetColor(ColorSettings.C.MainColor);
                break;
            case "D":
                this.SetColor(ColorSettings.D.MainColor);
                break;
            case "E":
                this.SetColor(ColorSettings.E.MainColor);
                break;
            case "F":
                this.SetColor(ColorSettings.F.MainColor);
                break;
            case "G":
                this.SetColor(ColorSettings.G.MainColor);
                break;
            case "A":
                this.SetColor(ColorSettings.A.MainColor);
                break;
            case "B":
                this.SetColor(ColorSettings.B.MainColor);
                break;
            default:
                break;
        }
    }

    private void SetColor(Color noteColor)
    {
        //var renderer = this.playNoteObj.GetComponent<Renderer>();
        //renderer.material.SetColor("_Color", noteColor.MainColor);
        this.playNoteObj.GetComponent<SpriteRenderer>().color = noteColor;
        //this.playNoteObj.GetComponent<Image>().color = noteColor;
    }

    public void AddWidth(float width)
    {
        this.Width += width;
    }

    public void StartAnimation(float divisionOneX, float tempo, float speedXPerSec)
    {
        if (Note.Type == "16th")
        {
            StartCoroutine(this.AnimatedBarFor16th(divisionOneX, tempo, speedXPerSec));
        }
        else
        {
            StartCoroutine(this.AnimatedBar(divisionOneX, tempo, speedXPerSec));
        }
        Debug.Log("2");

    }

    float GetThisObjTime(float durationOneX, float bpm)
    {
        //var objDuration = this.tableObj.transform.localScale.x / durationOneX;
        var objDuration = this.playNoteObj.transform.localScale.x / durationOneX;
        // The duration of this object is calculated by multiplying the time with 4 
        // (assuming 16th note duration) and then multiplying it by the duration.
        return 60 / bpm / 4 * objDuration;
        Debug.Log("3");

    }

    IEnumerator AnimatedBar(float durationOneX, float bpm, float speedXPerSec)
    {
        var objTime = GetThisObjTime(durationOneX, bpm);
        var restTime = objTime;
        var originalWidth = this.playNoteObj.transform.localScale.x;
        var originalHeight = this.playNoteObj.transform.localScale.y;

        while (restTime > 0)
        {
            var width = restTime / objTime * originalWidth;

            var currentScale = this.playNoteObj.transform.localScale;
            this.playNoteObj.transform.localScale = new Vector3(width, originalHeight, currentScale.z);

            restTime -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("4");

        Destroy(this.playNoteObj.gameObject);
    }

    IEnumerator AnimatedBarFor16th(float durationOneX, float bpm, float speedXPerSec)
    {
        var objTime = GetThisObjTime(durationOneX, bpm);
        var restTime = objTime;
        while (restTime > 0)
        {
            var size = restTime / objTime * ScaleFor16th;

            var currentScale = this.playNoteObj.transform.localScale;
            this.playNoteObj.transform.localScale = new Vector3(size, size, size);

            restTime -= Time.deltaTime;
            yield return null;
        }
        Destroy(this.playNoteObj.gameObject);
        Debug.Log("5");

    }
}
