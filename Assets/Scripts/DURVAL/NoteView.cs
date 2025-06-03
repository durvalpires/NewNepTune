using System;
using UnityEngine;

[Serializable]
public struct NoteView
{
    public GameObject GameObject;
    public double X;
    public Pitch Pitch;
    public double beatNumber;
    public double noteTimeInSeconds;
    public bool lastNote;
    public bool isRest;
}

