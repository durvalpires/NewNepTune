using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class HitAccuracySettings
{
    public List<HitEvaluationValue> NonTimedSettings = new List<HitEvaluationValue>();
    public List<HitEvaluationValue> TimedSettings = new List<HitEvaluationValue>();
}

public enum HitAccuracy
{
    Perfect,
    Great,
    Good,
    OK,
    Miss
}

[System.Serializable]
public struct HitEvaluationValue
{
    public HitAccuracy HitType;
    public float Value;
}
