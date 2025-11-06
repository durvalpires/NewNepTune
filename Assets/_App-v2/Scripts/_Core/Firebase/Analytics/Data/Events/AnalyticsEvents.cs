using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
using _App_v2.Scripts._Core.Firebase.Analytics.Data;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events
{
    public static class AnalyticsEvents
    {
        public static IAnalyticsEvent SignUp(string providerId, string userType)
        {
            return new SignUpEvent(providerId, userType);
        }
    }

    // sign_up
    class SignUpEvent : BaseAnalyticsEvent
    {
        public SignUpEvent(string providerId, string userType)
            : base("sign_up")
        {
            AddParameter(new BaseAnalyticsData("provider_id", providerId));
            AddParameter(new BaseAnalyticsData("user_type", userType));
        }
    }
}
