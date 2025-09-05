using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Providers
{
    public abstract class BaseProvider : IAnalyticsProvider
    {
        public abstract string ProviderName { get; }
        public abstract bool IsInitialized { get; }
        public abstract IAnalyticsHelper AnalyticsHelper { get; }
        
        public virtual void Initialize()
        {
            if (IsInitialized)
            {
                UnityEngine.Debug.Log($"Analytics {ProviderName} provider was initialized");
            }
            else
            {
                UnityEngine.Debug.LogError($"Analytics {ProviderName} provider was not initialized");
            }
        }

        public abstract void SetUserProperty(IUserProperty property);
        public abstract void SendEvent(IAnalyticsEvent analyticsEvent);
    }
}