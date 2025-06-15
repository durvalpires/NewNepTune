using System;
using UnityEngine.Serialization;

[Serializable]
public class LevelFirebaseData
{
    [FormerlySerializedAs("Score")] public int MaxScore;
    public int Repetition;
    public int Attempts;
    public int Success;
    [FormerlySerializedAs("Failure")] public int Fails;
    public float TimeSpent;
}