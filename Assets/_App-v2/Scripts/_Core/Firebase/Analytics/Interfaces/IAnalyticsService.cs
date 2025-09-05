
namespace _App_v2.Scripts._Core.Firebase.Analytics.Interfaces
{
    public interface IAnalyticsService
    {
        void Initialize();
        void AddProvider(IAnalyticsProvider provider);
        void SetUserProperty(IUserProperty property);
        void SendEvent(IAnalyticsEvent analyticsEvent);
        void SendWindowEvent(string windowName);
    }
}