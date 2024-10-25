using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GeneralConfigSO", menuName = "Scriptable Objects/v2_GeneralConfigSO")]
public class GeneralConfigSO : ScriptableObject
{
    public ScenesNames sectionSceneNight;
    public ScenesNames sectionSceneDay;
    public ScenesNames sectionSceneClouds;
    [Serializable]
    public struct ScenesNames
    {
        public string sceneName;
    }
    
}
