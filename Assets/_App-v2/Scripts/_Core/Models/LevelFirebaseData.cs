using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

[Serializable]
public class LevelFirebaseData
{
    [FormerlySerializedAs("Score")] public int MaxScore;
    //public int Repetition;
    public int Attempts;
    [FormerlySerializedAs("Success")] public int Successes;
    public float? RawScore;
    public int? StarRating;
    //[FormerlySerializedAs("Failure")] public int Fails;
    public float TimeSpent;
    public Dictionary<HitAccuracy, float>? HitAccuracy = null;
}