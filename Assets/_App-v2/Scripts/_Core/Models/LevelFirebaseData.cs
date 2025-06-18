using System;
using UnityEngine.Serialization;

[Serializable]
public class LevelFirebaseData
{
    [FormerlySerializedAs("Score")] public int MaxScore;
    public int Repetition;
    public int Attempts;
    public int Success;
    public float? RawScore;
    public int? StarRating;
    [FormerlySerializedAs("Failure")] public int Fails;
    public float TimeSpent;
    public float? HitAccuracy;
}