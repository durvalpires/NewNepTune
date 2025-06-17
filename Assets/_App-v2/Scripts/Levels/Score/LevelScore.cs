namespace _App_v2.Scripts.Levels.Score
{
    public class LevelScore : ILevelScore
    {
        public string levelId;
        public LevelSO.LevelType levelType;

        public int totalPoints;
        public int starsEarned; // 0–3 if applicable

        public RhythmStats rhythmStats;
        public QuizStats quizStats;
        public CardMatchStats cardStats;
    }

    [System.Serializable]
    public class RhythmStats
    {
        public int notesHit;
        public int totalNotes;
        public float accuracy;
    }

    [System.Serializable]
    public class QuizStats
    {
        public int correctAnswers;
        public int totalQuestions;
        public float avgResponseTime;
    }

    [System.Serializable]
    public class CardMatchStats
    {
        public int pairsFound;
        public int totalPairs;
        public int movesUsed;
    }
}