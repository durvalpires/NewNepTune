using System.Collections.Generic;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Interfaces
{
    public interface IAnalyticsEvent
    {
        string Name { get; }
        List<IAnalyticsData> Parameters { get; }

        void AddParameter(IAnalyticsData data);
    }
}