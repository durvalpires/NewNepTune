namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events
{
    public class WindowBaseEvent : BaseAnalyticsEvent
    {
        public WindowBaseEvent(string windowName,
            string windowNameKey = Constants.WINDOW_NAME_KEY,
            string eventName = Constants.WINDOW_EVENT) : base(eventName)
        {
            AddParameter(new BaseAnalyticsData(windowNameKey, windowName));
        }
    }
}
