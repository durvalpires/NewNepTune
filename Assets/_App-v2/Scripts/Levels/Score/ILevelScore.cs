using System.Collections.Generic;

namespace _App_v2.Scripts.Levels.Score
{
    public interface ILevelScore
    {
        public Dictionary<HitAccuracy, float> GetAccuracyPercentage();
    }
}