using UnityEngine;

namespace DURVAL.Utils
{
    public static class RhythmUtils
    {
        public struct TimingParams
        {
            public float DurationOneX;
            public float BeatsPerSecond;
            public float SecondsPerBeat;
            public float SpeedXPerSec;
            public float BeatsPerUnit;
        }

        public static TimingParams CalculateTimingParams(float baseDurationOneX, int measureDivision, float bpm)
        {
            float adjustedDurationOneX = baseDurationOneX - (measureDivision / 2f);
            adjustedDurationOneX = Mathf.Max(adjustedDurationOneX, 0.1f); // Prevent negative or zero

            float beatsPerSecond = bpm / 60f;
            float secondsPerBeat = 1f / beatsPerSecond;
            float speedXPerSec = (adjustedDurationOneX * measureDivision) * beatsPerSecond;
            float beatsPerUnit = beatsPerSecond / speedXPerSec;

            return new TimingParams
            {
                DurationOneX = adjustedDurationOneX,
                BeatsPerSecond = beatsPerSecond,
                SecondsPerBeat = secondsPerBeat,
                SpeedXPerSec = speedXPerSec,
                BeatsPerUnit = beatsPerUnit
            };
        }
    }
}