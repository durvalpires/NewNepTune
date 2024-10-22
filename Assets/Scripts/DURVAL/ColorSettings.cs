using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColorSettings
{
    public NoteColor C;
    public NoteColor E;
    public NoteColor D;
    public NoteColor F;
    public NoteColor G;
    public NoteColor A;
    public NoteColor B;

    public Color InteractableColor;
    public Color NormalColor;
    public Color MissColor;
    public Color HitColor;

    //public Dictionary<string, NoteColor> NoteColorDic;

    //public ColorSettings()
    //{
    //    NoteColorDic = new Dictionary<string, NoteColor>()
    //    {
    //        {"C", this.C},
    //        {"D", this.D},
    //        {"E", this.E},
    //        {"F", this.F},
    //        {"G", this.G},
    //        {"A", this.A},
    //        {"B", this.B}
    //    };
    //}

    //public void SetDictionary()
    //{
    //    NoteColorDic = new Dictionary<string, NoteColor>()
    //    {
    //        {"C", this.C},
    //        {"D", this.D},
    //        {"E", this.E},
    //        {"F", this.F},
    //        {"G", this.G},
    //        {"A", this.A},
    //        {"B", this.B}
    //    };
    //}

    public Color GetNoteColor(string note)
    {
        if (note == "C") { return C.MainColor; }
        else if (note == "D") { return D.MainColor; }
        else if (note == "E") { return E.MainColor; }
        else if (note == "F") { return F.MainColor; }
        else if (note == "G") { return G.MainColor; }
        else if (note == "A") { return A.MainColor; }
        else if (note == "B") { return B.MainColor; }

        return Color.white;
        //Debug.LogWarning("GetNoteColor: " + NoteColorDic[note].MainColor);
        //return NoteColorDic[note].MainColor;
    }

    //public static ColorSettings LoadColorSettingsByMaterials()
    //{
    //    var settings = new ColorSettings();
    //    settings.C = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/C"));
    //    settings.D = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/D"));
    //    settings.E = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/E"));
    //    settings.F = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/F"));
    //    settings.G = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/G"));
    //    settings.A = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/A"));
    //    settings.B = LoadNoteColorByMaterial(Resources.Load<Material>("NoteSample/B"));
    //    return settings;
    //}

    //private static NoteColor LoadNoteColorByMaterial(Material material)
    //{
    //    return new NoteColor() { MainColor = material.GetColor("_Color") };
    //}
}

[System.Serializable]
public struct NoteColor
{
    [SerializeField]
    public Color MainColor;
}
