namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events
{
    public class LogInEvent : BaseAnalyticsEvent
    {
        public LogInEvent(string providerId) : base("log_in")
        {
            AddParameter(new BaseAnalyticsData("provider_id", providerId));
        }
    }
}