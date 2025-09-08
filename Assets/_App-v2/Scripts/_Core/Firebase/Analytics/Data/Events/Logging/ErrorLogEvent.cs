namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events.Logging
{
    public sealed class ErrorLogEvent : BaseLogAnalyticsEvent
    {
        public ErrorLogEvent(string tags, string message) : base( Constants.ERROR_KEY, tags, message)
        {
        }
    }
}