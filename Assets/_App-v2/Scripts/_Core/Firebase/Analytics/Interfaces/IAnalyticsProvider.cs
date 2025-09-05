namespace _App_v2.Scripts._Core.Firebase.Analytics.Interfaces
{
    public interface IAnalyticsProvider
    {
        string ProviderName { get; }
        bool IsInitialized { get; }
        IAnalyticsHelper AnalyticsHelper { get; }

        void Initialize();
        void SetUserProperty(IUserProperty property);
        void SendEvent(IAnalyticsEvent analyticsEvent);
    }
}