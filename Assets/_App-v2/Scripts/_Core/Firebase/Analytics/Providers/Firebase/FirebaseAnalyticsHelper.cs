using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
#if !UNITY_WEBGL
using _App_v2.Scripts._Core.Firebase.Analytics.Providers.Firebase.Events;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Providers.Firebase
{
    internal class FirebaseAnalyticsHelper : IAnalyticsHelper
    {
        IAnalyticsEvent IAnalyticsHelper.GetWindowEvent(string windowName) => new FirebaseWindowEvent(windowName);

    }
}
#endif
