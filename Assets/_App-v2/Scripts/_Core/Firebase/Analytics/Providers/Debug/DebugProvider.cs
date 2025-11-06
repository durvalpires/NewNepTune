using System.Linq;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
using UnityEngine.Scripting;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Providers.Debug
{
    [Preserve]
    public class DebugProvider : BaseProvider
    {
        public override string ProviderName => "Debug";
        public override bool IsInitialized => true;
        public override IAnalyticsHelper AnalyticsHelper => null;

        public override void SetUserProperty(IUserProperty property)
        {
            UnityEngine.Debug.Log($"[DebugProvider] SetUserProperty called");
            UnityEngine.Debug.Log(PropertyToLog(property));
        }

        public override void SendEvent(IAnalyticsEvent analyticsEvent)
        {
            UnityEngine.Debug.Log($"[DebugProvider] SendEvent called for: {analyticsEvent?.Name}");
            UnityEngine.Debug.Log(EventToLog(analyticsEvent));
        }

        private static string PropertyToLog(IUserProperty property) => $"[USER PROPERTY: {property.Name}] {property.Value}";

        private static string EventToLog(IAnalyticsEvent analyticsEvent)
        {
            var log = $"[ANALYTICS EVENT: {analyticsEvent.Name}]";

            log = analyticsEvent.Parameters.Aggregate(log, (current, parameter) => current + $"\n\t{parameter.Key}={parameter.Value}");
            log += "\n[END]";

            return log;
        }
    }
}