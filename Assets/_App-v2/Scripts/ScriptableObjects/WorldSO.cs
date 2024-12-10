using System;
using System.Collections.Generic;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CreateAssetMenu(fileName = "WorldSO", menuName = "Scriptable Objects/WorldSO")]
public class WorldSO : ScriptableObject
{
    public string title;
    public string id;
    public Sprite worldSprite;
    public AllWorldsSO.LevelsData[] levels;
#if UNITY_EDITOR
    [ContextMenu("SaveMe")]
    public void SaveMe()
    {
        foreach (var level in levels)
        {
            if (level.level != null)
            {
                EditorUtility.SetDirty(level.level);
                AssetDatabase.SaveAssets();
            }
            else
            {
                Debug.LogAssertion($"Level is null in {title}");
            }
        }
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        Debug.LogWarning("Saved");
    }
#endif
}
