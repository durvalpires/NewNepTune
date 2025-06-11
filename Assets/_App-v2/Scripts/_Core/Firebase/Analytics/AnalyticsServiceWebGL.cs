using System.Collections.Generic;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL;
using MarksAssets.FirebaseWebGL.Examples;
using MarksAssets.FirebaseWebGL.Analytics;
using An = MarksAssets.FirebaseWebGL.Analytics.Analytics;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

using UnityEngine;
using MarksAssets.FirebaseWebGL.Auth;
using MarksAssets.FirebaseWebGL.Examples.Analytics;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Implementations
{
    public class AnalyticsServiceWebGL : IAnalyticsServiceWebGL
    {
        private object analytics;

        public async Task InitializeAsync(FirebaseConfigObject cfg,
                                          EmulatorConfigObject emu = null)
        {
            analytics = await CommonSetup.setup(cfg, emu);
            if (analytics == null)
                Debug.LogError("FirebaseWebGL Analytics setup failed.");
        }

        public void SetUserProperties(Dictionary<string, object> properties)
        {
            if (analytics == null) return;
            An.setUserProperties((An)analytics, properties);
        }

        public void SetUserId(string userId)
        {
            if (analytics == null) return;
            An.setUserId((An)analytics, userId);
        }

        public void LogEvent(string eventName, Dictionary<string, object> parameters = null)
        {
            if (analytics == null) return;
            if (parameters != null)
                An.logEvent((An)analytics, eventName, parameters);
            else
                An.logEvent((An)analytics, eventName);
        }
    }
}