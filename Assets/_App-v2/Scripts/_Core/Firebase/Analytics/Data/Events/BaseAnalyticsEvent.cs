using System.Collections.Generic;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Events
{
    public class BaseAnalyticsEvent : IAnalyticsEvent
    {
        public string Name { get; }
        public List<IAnalyticsData> Parameters { get; }

        protected BaseAnalyticsEvent(string name)
        {
            Name = name;
            Parameters = new List<IAnalyticsData>();
        }

        public void AddParameter(IAnalyticsData data) => Parameters.Add(data);
    }
}