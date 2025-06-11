
namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events.Logging
{
    public class BaseLogAnalyticsEvent : BaseAnalyticsEvent
    {
        public BaseLogAnalyticsEvent(string type, string tags, string message) : base(Constants.LOG_EVENT)
        {
            AddParameter(new BaseAnalyticsData(Constants.TYPE_KEY, type));
            AddParameter(new BaseAnalyticsData(Constants.TAGS_KEY, tags));
            AddParameter(new BaseAnalyticsData(Constants.MESSAGE_KEY, message));
        }
    }
}