using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Analytics.Data.Properties
{
    public class UserProperty : IUserProperty
    {
        public string Name { get; }
        public string Value { get; }

        public UserProperty(string name, string value)
        {
            Name = name;
            Value = value;
        }
    }
}