using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector(); // Default inspector setup

        LevelManager levelManager = (LevelManager)target;

        // Add button to sort levels by their order field
        if (GUILayout.Button("Sort Levels by Order"))
        {
            // levelManager.SortLevelsByOrder();
            EditorUtility.SetDirty(levelManager); // Mark as dirty to save changes
        }
    }
}