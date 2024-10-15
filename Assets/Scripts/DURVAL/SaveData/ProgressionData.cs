using System.Collections.Generic;

[System.Serializable]
public class ProgressionData
{
    public int currentLevel;
    public List<int> completedLevels;
    public List<string> unlockedInstruments;
    public int totalXP;
}
