using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
#if !UNITY_WEBGL
using Firebase.Analytics;
#else
using An = MarksAssets.FirebaseWebGL.Analytics.Analytics;
#endif

namespace _App_v2.Scripts._Core.Firebase.Analytics.Providers.Firebase
{
    internal class FirebaseAnalyticsProvider : BaseProvider
    {
        private bool _isInitialized;
        
        public override string ProviderName => Constants.PROVIDER_NAME;
        public override bool IsInitialized => _isInitialized;
        
        #if !UNITY_WEBGL
        public override IAnalyticsHelper AnalyticsHelper { get; } = new FirebaseAnalyticsHelper();
#else
        public override IAnalyticsHelper AnalyticsHelper { get; } = null;
#endif

        public override void Initialize()
        {
            _isInitialized = true;
            base.Initialize();
        }


        public override void SendEvent(IAnalyticsEvent analyticsEvent)
        {
#if !UNITY_WEBGL
            FirebaseAnalytics.LogEvent(analyticsEvent.Name, ConvertToParameters(analyticsEvent.Parameters));
            #else
            // Use Firebase WebGL Analytics SDK
            try
            {
                var fireService = FireService.Instance;
                if (fireService == null || fireService.WebGLFirebaseApp == null)
                {
                    UnityEngine.Debug.LogWarning("[FirebaseAnalyticsProvider] FireService or WebGLFirebaseApp not initialized yet");
                    return;
                }
                
                var analytics = An.getAnalytics(fireService.WebGLFirebaseApp);

                var parameters = new Dictionary<string, object>();
                foreach (var param in analyticsEvent.Parameters)
                {
                    parameters[param.Key] = param.Value;
                }

                // Development Mode 
                #if DEVELOPMENT_BUILD
                parameters["debug_mode"] = 1;
                #endif

                An.logEvent(analytics, analyticsEvent.Name, parameters);
                UnityEngine.Debug.Log($"[FirebaseAnalyticsProvider] Successfully sent event '{analyticsEvent.Name}' to Firebase Analytics");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[FirebaseAnalyticsProvider] Error sending event: {ex.Message}");
            }
            #endif
        }


        public override void SetUserProperty(IUserProperty property)
        {
            #if !UNITY_WEBGL
            FirebaseAnalytics.SetUserProperty(property.Name, property.Value);
            #else
            // Use Firebase WebGL Analytics SDK
            try
            {
                var fireService = FireService.Instance;
                if (fireService == null || fireService.WebGLFirebaseApp == null)
                {
                    UnityEngine.Debug.LogWarning("[FirebaseAnalyticsProvider] FireService or WebGLFirebaseApp not initialized yet");
                    return;
                }

                var analytics = An.getAnalytics(fireService.WebGLFirebaseApp);
                An.setUserProperties(analytics, new Dictionary<string, object> { [property.Name] = property.Value });
                UnityEngine.Debug.Log($"[FirebaseAnalyticsProvider] Successfully set user property '{property.Name}'");
            }
            catch (System.Exception ex)
            {
                UnityEngine.Debug.LogError($"[FirebaseAnalyticsProvider] Error setting user property: {ex.Message}");
            }
            #endif
        }



        #if !UNITY_WEBGL
        private static Parameter[] ConvertToParameters(IEnumerable<IAnalyticsData> data)
        {
            return data.Select(pair => new Parameter(pair.Key, pair.Value)).ToArray();
        }
#endif
    }
}
