#if !UNITY_WEBGL
using _App_v2.Scripts._Core.Firebase.Analytics.Data;
using _App_v2.Scripts._Core.Firebase.Analytics.Data.Events;

using Firebase.Analytics;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Providers.Firebase.Events
{
    public class FirebaseWindowEvent : WindowBaseEvent
    {
        public FirebaseWindowEvent(string windowName) : base(windowName: windowName,
            windowNameKey: FirebaseAnalytics.ParameterScreenName,
            eventName: FirebaseAnalytics.EventScreenView)
        {
            AddParameter(new BaseAnalyticsData(FirebaseAnalytics.ParameterScreenClass, windowName));
        }
    }
}
#endif
