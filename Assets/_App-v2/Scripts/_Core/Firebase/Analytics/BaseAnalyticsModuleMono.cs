using UnityEngine;
using _App_v2.Scripts._Core.Firebase;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Analytics
{
    public class BaseAnalyticsModuleMono : MonoBehaviour
    {
        private IAnalyticsService _analytics;

        protected IAnalyticsService Analytics =>
            _analytics ??= FireService.Instance?.AnalyticsService;

        protected void SendEvent(IAnalyticsEvent analyticsEvent)
        {
            Debug.Log($"[Analytics] SendEvent called for: {analyticsEvent?.Name ?? "NULL"}");

            if (Analytics == null)
            {
                Debug.LogWarning($"[Analytics] Analytics service is NULL, cannot send event {analyticsEvent?.Name}");
                Debug.LogWarning($"[Analytics] FireService.Instance = {FireService.Instance}");
                Debug.LogWarning($"[Analytics] FireService.Instance.AnalyticsService = {FireService.Instance?.AnalyticsService}");
                return;
            }

            Debug.Log($"[Analytics] Sending event {analyticsEvent.Name} to Analytics service");
            Analytics?.SendEvent(analyticsEvent);
        }

        protected void SendUserProperty(IUserProperty userProperty)
        {
            Analytics?.SetUserProperty(userProperty);
        }
        
        protected void SendWindowEvent(string windowName)
        {
            Analytics?.SendWindowEvent(windowName);
        }
    }
}
