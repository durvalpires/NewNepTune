namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events
{
    public class LearningProgressionEvent : BaseAnalyticsEvent
    {
        public LearningProgressionEvent(int levelsCompleted, int challengesAttemped, float averageScore) : base("learning_progression")
        {
            AddParameter(new BaseAnalyticsData("levels_completed", levelsCompleted.ToString()));
            AddParameter(new BaseAnalyticsData("challenges_attempted", challengesAttemped.ToString()));
            AddParameter(new BaseAnalyticsData("average_score", averageScore.ToString()));
        }
    }
}