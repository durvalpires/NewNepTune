using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum NoteState
{
    Normal,
    Interactable,
    Miss,
    Hit
}

public class NoteController : MonoBehaviour
{
    private bool availableToInteract = true;
    
    //Transform tableObj;
    Transform playNoteObj;

    public float X { get; set; } = 0;

    public float Width { get; set; } = 0;

    // public ScoreNote Note { 
    //     get{
    //         return note;
    //     } 
    //     set{
    //         note = value;
    //         UpdateNoteAppearance();
    //     } 
    // }

    public Pitch Pitch { get
        {
            return note.Pitch.Value;
        } 
    }
    public ColorSettings ColorSettings { get; set; }
    
    public bool IsFilling { get; private set; }

    public static float ScaleFor16th = 1;

    public NoteState State { get; set; } = NoteState.Normal;

    [SerializeField]
    private ScoreNote note;

    public int Index { get; set; }

    public SpriteRenderer CircleSprite
    {
        get => circleSprite;
    }

    [SerializeField] private SpriteRenderer circleSprite;
    [SerializeField] private SpriteRenderer stemSprite;
    [SerializeField] private SpriteRenderer beamTopSprite;
    [SerializeField] private SpriteRenderer beamBottomSprite;
    [SerializeField] private SpriteRenderer circleLineSprite;
    [SerializeField] private SpriteRenderer rightDotSprite;
    [SerializeField] private SpriteRenderer fadePatternSprite;
    [SerializeField] private SpriteRenderer fadeFillingSprite;

    //[SerializeField]
    //private RhythmGameSettings gameSettings;


    void Awake()
    {
        //this.note = Note;
        //this.tableObj = this.gameObject.transform.Find("TablePrefab");
        //this.playNoteObj = this.gameObject.transform.Find("PlayNotePrefab");
        this.playNoteObj = transform;

        // if (this.Note.Type == "16th")
        // {
        //     //this.tableObj.transform.localScale = new Vector3(ScaleFor16th, ScaleFor16th, ScaleFor16th);
        //     //this.playNoteObj.transform.localScale = new Vector3(ScaleFor16th, ScaleFor16th, ScaleFor16th);
        // }
        // else
        // {
        //     //this.transform.localPosition = new Vector3(this.X /*+ (this.Width / 2)*/,
        //     //    this.transform.localPosition.y,
        //     //    this.transform.localPosition.z);

        //     //this.tableObj.transform.localScale = new Vector3(this.Width,
        //     //    this.tableObj.transform.localScale.y,
        //     //    this.tableObj.transform.localScale.z);

        //     Debug.Log("not 16th: " + this.Width + " - " + this.playNoteObj.transform.localScale.x);

        //     this.playNoteObj.transform.localScale = new Vector3(this.Width * 
        //         this.playNoteObj.transform.localScale.x,
        //         this.playNoteObj.transform.localScale.y,
        //         this.playNoteObj.transform.localScale.z);
        // }

        //this.SetColorByPitch();
        //this.SetMusicSheetColor();
    }

    void SetColorByPitch()
    {
        switch (this.note.Pitch?.Step)
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

    void SetColorByState(){
        switch (this.State)
        {
            case NoteState.Normal:
                this.SetColor(ColorSettings.NormalColor);
                break;
            case NoteState.Interactable:
                this.SetColor(ColorSettings.InteractableColor);
                break;
            case NoteState.Miss:
                this.SetColor(ColorSettings.MissColor);
                break;
            case NoteState.Hit:
                this.SetColor(ColorSettings.HitColor);
                break;
        }
    }

    public void SetState(NoteState state){
        this.State = state;
        this.SetColorByState();
    }

    private void SetColor(Color noteColor)
    {
        foreach (var renderer in this.playNoteObj.GetComponentsInChildren<SpriteRenderer>(true)){
            if(renderer.gameObject.name == "FadePattern") continue;
            renderer.color = noteColor;
        }
        //var renderer = this.playNoteObj.GetComponent<Renderer>();
        //renderer.material.SetColor("_Color", noteColor.MainColor);
        this.playNoteObj.GetComponent<SpriteRenderer>().color = noteColor;
        //this.playNoteObj.GetComponent<Image>().color = noteColor;
    }

    private void SetMusicSheetColor(){
        SetColor(ColorSettings.NormalColor);
    }

    public void AddWidth(float width)
    {
        this.Width += width;

        if(this.fadePatternSprite != null)
            this.fadePatternSprite.transform.localScale = new Vector3(this.fadePatternSprite.transform.localScale.x,
                this.Width - this.circleSprite.size.x, this.fadePatternSprite.transform.localScale.z);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("InteractionBar") && State != NoteState.Miss)
        {
            SetState(NoteState.Interactable);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("InteractionBar"))
        {
            if(State == NoteState.Interactable){
                SetState(NoteState.Miss);
            }
        }
    }
    
    public void SetAsUninteractable(){
        availableToInteract = false;
        SetState(NoteState.Miss);
    }

    // private void UpdateNoteAppearance()
    // {
    //     // Stem Direction
    //     if(note.Stem != "up"){
    //         transform.eulerAngles = new Vector3(0, 0, 180);
    //     }


    // }

    public void SetNote(ScoreNote scoreNote, RhythmGameSettings gameSettings, int currentDivisions)
    {
        this.note = scoreNote;

        SetState(NoteState.Normal);

        if(note.IsRest){
            return;
        }

        //  noteTrail
        var showTrail = gameSettings.HaveDurationTrail && note.Duration < 2 * currentDivisions; 
        fadePatternSprite.gameObject.SetActive(showTrail);
        fadeFillingSprite.gameObject.SetActive(showTrail);

        var durationX = gameSettings.DurationOneX - (currentDivisions / 2) * 
            gameSettings.DurationReductionPerDivision;

        // if(currentDivisions == 4) 
        //     durationX = 0.5f; 
        // else 
        //     durationX = 1;
        
        if(note.Type == "half" && note.Duration == 3*currentDivisions)
            rightDotSprite.gameObject.SetActive(true);
        
        if(note.Pitch?.Step == "D" && note.Pitch?.Octave == 4){
            transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.07f, 
                transform.localPosition.z);
        }

        // Stem Direction
        if(note.Type == "whole"){
            stemSprite.gameObject.SetActive(false);
        }
        else if(note.Stem != "up"){
            transform.eulerAngles = new Vector3(0, 0, 180);
        }

        if(!gameSettings.circleLineNotes.Contains(note.Pitch?.Step + note.Pitch?.Octave)){
            circleLineSprite.gameObject.SetActive(false);
        }

        foreach(var variation in gameSettings.noteCircleVariationsSprites){
            if(variation.noteType == note.Type){
                circleSprite.sprite = variation.sprite;
            }
        }

        if(note.BeamList != null && note.BeamList.Count > 0){
            for(int i = 0; i < note.BeamList.Count; i++){
                if(note.BeamList[i].Type != "begin"){
                    if(i == 0){
                        // if(note.Type == "eighth"){
                        //     beamTopSprite.transform.localScale = new Vector3(0.5f, 
                        //     transform.localScale.y, transform.localScale.z);
                        // }
                        beamTopSprite.transform.localScale = new Vector3((float)note.Duration * durationX, 
                        transform.localScale.y, transform.localScale.z);
                        
                        beamTopSprite.gameObject.SetActive(true);
                        beamTopSprite.sprite = gameSettings.beamSprite;
                    }
                    else if(i == 1){
                        // if(note.Type == "eighth"){
                        //     beamBottomSprite.transform.localScale = new Vector3(0.5f, 
                        //     transform.localScale.y, transform.localScale.z);
                        // }
                        beamBottomSprite.transform.localScale = new Vector3((float)note.Duration * durationX, 
                            transform.localScale.y, transform.localScale.z);
                            
                        beamBottomSprite.gameObject.SetActive(true);
                        beamBottomSprite.sprite = gameSettings.beamSprite;
                    }
                }
            }
        }
        
        

    }
    
    private void AdjustColliderToSprite()
    {
        BoxCollider2D boxCollider = GetComponent<BoxCollider2D>();

        // Get the sprite's bounds
        Vector2 spriteSize = circleSprite.sprite.bounds.size;

        // Adjust the collider's size to match the sprite's width and height
        boxCollider.size = new Vector2(spriteSize.x, boxCollider.size.y);
    }

    // public void StartAnimation(float divisionOneX, float tempo, float speedXPerSec)
    // {
    //     if (Note.Type == "16th")
    //     {
    //         StartCoroutine(this.AnimatedBarFor16th(divisionOneX, tempo, speedXPerSec));
    //     }
    //     else
    //     {
    //         StartCoroutine(this.AnimatedBar(divisionOneX, tempo, speedXPerSec));
    //     }
    //     Debug.Log("2");
    
    public void StartAnimation(float secondsPerBeat, float speedXPerSec)
    {
        IsFilling = true;
        StartCoroutine(this.AnimateBar(secondsPerBeat, speedXPerSec));
    }

    IEnumerator AnimateBar(float secondsPerBeat, float speedXPerSec)
    {
        Debug.LogWarning("ANIMATE THAT SHIET");
        var timeElapsed = 0f;
        var noteDurationSeconds = secondsPerBeat * note.Duration;
        
        while (timeElapsed < noteDurationSeconds)
        {
            timeElapsed += Time.deltaTime;
            var width = timeElapsed / noteDurationSeconds;
            var currentScale = this.fadeFillingSprite.transform.localScale;
            this.fadeFillingSprite.transform.localScale = new Vector3(currentScale.x, width, currentScale.z);
            Debug.Log("SERA K DESLIGAAAAAAA");
            yield return null;
        }
        
        yield return null;
    }
    
    public void StopAnimation()
    {
        IsFilling = false;
        StopAllCoroutines();
    }
    

    // float GetThisObjTime(float durationOneX, float bpm)
    // {
    //     //var objDuration = this.tableObj.transform.localScale.x / durationOneX;
    //     var objDuration = this.playNoteObj.transform.localScale.x / durationOneX;
    //     // The duration of this object is calculated by multiplying the time with 4 
    //     // (assuming 16th note duration) and then multiplying it by the duration.
    //     return 60 / bpm / 4 * objDuration;
    //     Debug.Log("3");

    // }

    // IEnumerator AnimatedBar(float durationOneX, float bpm, float speedXPerSec)
    // {
    //     var objTime = GetThisObjTime(durationOneX, bpm);
    //     var restTime = objTime;
    //     var originalWidth = this.playNoteObj.transform.localScale.x;
    //     var originalHeight = this.playNoteObj.transform.localScale.y;

    //     while (restTime > 0)
    //     {
    //         var width = restTime / objTime * originalWidth;

    //         var currentScale = this.playNoteObj.transform.localScale;
    //         this.playNoteObj.transform.localScale = new Vector3(width, originalHeight, currentScale.z);

    //         restTime -= Time.deltaTime;
    //         yield return null;
    //     }
    //     Debug.Log("4");

    //     Destroy(this.playNoteObj.gameObject);
    // }

    // IEnumerator AnimatedBarFor16th(float durationOneX, float bpm, float speedXPerSec)
    // {
    //     var objTime = GetThisObjTime(durationOneX, bpm);
    //     var restTime = objTime;
    //     while (restTime > 0)
    //     {
    //         var size = restTime / objTime * ScaleFor16th;

    //         var currentScale = this.playNoteObj.transform.localScale;
    //         this.playNoteObj.transform.localScale = new Vector3(size, size, size);

    //         restTime -= Time.deltaTime;
    //         yield return null;
    //     }
    //     Destroy(this.playNoteObj.gameObject);
    //     Debug.Log("5");

    // }
}
