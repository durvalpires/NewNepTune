namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events.Logging
{
    public sealed class ExceptionLogEvent : BaseLogAnalyticsEvent
    {
        public ExceptionLogEvent(string tags, string message) : base(Constants.EXCEPTION_KEY, tags, message)
        {
        }
    }
}