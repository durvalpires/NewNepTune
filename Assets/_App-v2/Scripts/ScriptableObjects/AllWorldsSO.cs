using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AllWorldsSO", menuName = "Scriptable Objects/v2_AllWorldsSO")]
public class AllWorldsSO : ScriptableObject
{
    public WorldSO[] worldsConfigs;
    
#if UNITY_EDITOR
    [ContextMenu("SaveAll")]
    public void SaveAll()
    {
        foreach (var world in worldsConfigs)
        {
            foreach (var level in world.levels)
            {
                if (level.level != null)
                {
                    EditorUtility.SetDirty(level.level);
                    AssetDatabase.SaveAssets();
                }
                else
                {
                    Debug.LogAssertion($"Level is null in {world.title}");
                }
            }
            EditorUtility.SetDirty(world);
            AssetDatabase.SaveAssets();

        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.LogWarning($"All Data Saved!");
    }
#endif
    
    [HideInInspector]
    [Serializable]
    public class WordData
    {
        public WorldSO worldConfig;
        public string title;
        public string id;
        public Sprite worldSprite;
        public LevelsData[] levels;
    }
    [Serializable]
    public struct LevelsData
    {
        public Sprite image;
        public LevelSO level;

    }
}

