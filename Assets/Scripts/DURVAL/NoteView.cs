using System;
using UnityEngine;

[Serializable]
public struct NoteView
{
    public GameObject GameObject;
    public float X;
    public Pitch Pitch;
    public float beatNumber;
    public float noteTimeInSeconds;
    public bool lastNote;
    public bool isRest;
}

