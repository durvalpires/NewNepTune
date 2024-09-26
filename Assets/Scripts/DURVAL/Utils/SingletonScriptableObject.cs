using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<T>(typeof(T).Name);

                // Ensure we only have one instance
                if (_instance == null)
                {
                    Debug.LogError("SingletonScriptableObject: No instance found in Resources folder.");
                }
            }

            return _instance;
        }
    }
}
