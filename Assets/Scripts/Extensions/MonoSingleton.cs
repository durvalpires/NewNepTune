using UnityEngine;

namespace Extensions
{
    public class MonoSingleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject newGO = new GameObject();
                        _instance = newGO.AddComponent<T>();
                    }
                }

                return _instance;
            }
        }

        protected virtual void Awake()
        {
            var foundInstances = FindObjectsOfType<T>();
            
            if (foundInstances.Length > 1)
            {
                Debug.LogWarning($"Multiple Instances of {typeof(T)} were found.");
            }
            
            _instance = this as T;
        }
    }
}