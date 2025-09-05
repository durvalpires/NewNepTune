using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Data
{
    public class BaseAnalyticsData : IAnalyticsData
    {
        public string Key { get; }
        public string Value { get; }
        
        public BaseAnalyticsData(string key, string value)
        {
            Key = key;
            Value = value;
        }
    }
}