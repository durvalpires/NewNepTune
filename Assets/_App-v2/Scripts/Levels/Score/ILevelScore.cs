using System.Collections.Generic;

namespace _App_v2.Scripts.Levels.Score
{
    public interface ILevelScore
    {
        public int PlayerStars { get; }
        public Dictionary<HitAccuracy, float> GetAccuracyPercentage();
    }
}