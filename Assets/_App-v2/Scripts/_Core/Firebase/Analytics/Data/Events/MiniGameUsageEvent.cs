namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events
{
    public class MiniGameUsageEvent : BaseAnalyticsEvent
    {
        public MiniGameUsageEvent(string miniGameId) : base("mini_game_usage")
        {
            AddParameter(new BaseAnalyticsData("mini_game_id", miniGameId));
        }
    }
}