using System;
using System.Collections.Generic;

[Serializable]
public class PlayerLevelData
{
    public string SubProfilename = "MyLevelDataSet";
    public int AverageScore = 0;
    public int NumberOfLevel = 0;
    //public int Repetition = 0;

    public Dictionary<string, LevelFirebaseData> levels = new Dictionary<string, LevelFirebaseData>();
    public string customLevelData = "";
    public float? RawScore;       
    public int? StarRating;
    public float? HitAccuracy;
}