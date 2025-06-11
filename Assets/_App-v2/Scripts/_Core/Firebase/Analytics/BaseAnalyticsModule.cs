using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Analytics
{
    public class BaseAnalyticsModule
    {
        private IAnalyticsService _analyticsService;

        protected void SendEvent(IAnalyticsEvent analyticsEvent)
        {
            _analyticsService.SendEvent(analyticsEvent);
        }
        
        protected void SendUserProperty(IUserProperty userProperty)
        {
            _analyticsService.SetUserProperty(userProperty);
        }
    }
}