using System;
using System.Collections.Generic;

[Serializable]
public class PlayerLevelData
{
    public string SubProfilename = "MyLevelDataSet";
    public int AverageScore = 0; //Data still not accurate
    public int NumberOfLevel = 0; //Data still not accurate
    //public int Repetition = 0;

    public Dictionary<string, LevelFirebaseData> levels = new Dictionary<string, LevelFirebaseData>();
    public string customLevelData = "";
    public float? RawScore;      //Still not being used 
    public int? StarRating;     //Still not being used
    public float? HitAccuracy;     //Still not being used
}