using System.Collections.Generic;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Examples;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Interfaces
{
    public interface IAnalyticsServiceWebGL
    {
        Task InitializeAsync(FirebaseConfigObject firebaseConfig,
                             EmulatorConfigObject emulatorConfig = null);
        void SetUserProperties(Dictionary<string, object> properties);
        void SetUserId(string userId);
        void LogEvent(string eventName, Dictionary<string, object> parameters = null);
    }
}