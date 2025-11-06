using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
#if !UNITY_WEBGL
using Firebase.Analytics;
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

        
        public override void SendEvent(IAnalyticsEvent analyticsEvent) =>
            #if !UNITY_WEBGL
            FirebaseAnalytics.LogEvent(analyticsEvent.Name, ConvertToParameters(analyticsEvent.Parameters));
            #else
            // TEMPORARY
            _isInitialized = _isInitialized;
            #endif


        public override void SetUserProperty(IUserProperty property) => 
#if !UNITY_WEBGL
            FirebaseAnalytics.SetUserProperty(property.Name, property.Value);
#else
            // TEMPORARY
            _isInitialized = _isInitialized;
#endif
        


        #if !UNITY_WEBGL
        private static Parameter[] ConvertToParameters(IEnumerable<IAnalyticsData> data)
        {
            return data.Select(pair => new Parameter(pair.Key, pair.Value)).ToArray();
        }
#endif
    }
}
