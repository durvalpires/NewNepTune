using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSettings
{
    public NoteColor C;
    public NoteColor CSharp;
    public NoteColor E;
    public NoteColor D;
    public NoteColor DSharp;
    public NoteColor F;
    public NoteColor FSharp;
    public NoteColor G;
    public NoteColor GSharp;
    public NoteColor A;
    public NoteColor ASharp;
    public NoteColor B;

    public Color InteractableColor;
    public Color NormalColor;
    public Color MissColor;
    public Color HitColor;

    public Color GetNoteColor(string note)
    {
        return note switch
        {
            "C" => C.MainColor,
            "C#" => CSharp.MainColor,
            "D" => D.MainColor,
            "D#" => DSharp.MainColor,
            "E" => E.MainColor,
            "F" => F.MainColor,
            "F#" => FSharp.MainColor,
            "G" => G.MainColor,
            "G#" => GSharp.MainColor,
            "A" => A.MainColor,
            "A#" => ASharp.MainColor,
            "B" => B.MainColor,
            _ => Color.white
        };
    }
}

[System.Serializable]
public struct NoteColor
{
    [SerializeField]
    public Color MainColor;
}
