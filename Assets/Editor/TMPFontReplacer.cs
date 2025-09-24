// TMPFontReplacer.cs
// Place this file in an "Editor" folder (e.g. Assets/Editor/TMPFontReplacer.cs)
// Usage: Window -> TMP Font Replacer

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.Linq;
using TMPro;

public class TMPFontReplacer : EditorWindow
{
    private TMP_FontAsset targetFont;
    private bool includeInactive = false;
    private bool changeInOpenScenes = true;
    private bool changeInPrefabs = true;
    private Vector2 scroll;

    [MenuItem("Tools/TMP Font Replacer", priority = 2050)]
    public static void ShowWindow()
    {
        TMPFontReplacer wnd = GetWindow<TMPFontReplacer>("TMP Font Replacer");
        wnd.minSize = new Vector2(420, 220);
    }

    private void OnGUI()
    {
        GUILayout.Label("TextMeshPro Font Replacer", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("Replace the TMP Font Asset on all TextMeshPro UGUI components in open scenes and/or prefabs in your project.", MessageType.Info);

        EditorGUILayout.Space();

        targetFont = (TMP_FontAsset)EditorGUILayout.ObjectField("Target TMP Font Asset", targetFont, typeof(TMP_FontAsset), false);
        includeInactive = EditorGUILayout.ToggleLeft(new GUIContent("Include Inactive GameObjects", "Also change TMP components on inactive GameObjects in scenes/prefabs"), includeInactive);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Scopes", EditorStyles.boldLabel);
        changeInOpenScenes = EditorGUILayout.ToggleLeft("Open Scenes", changeInOpenScenes);
        changeInPrefabs = EditorGUILayout.ToggleLeft("Project Prefabs", changeInPrefabs);

        EditorGUILayout.Space();

        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUI.enabled = targetFont != null && (changeInOpenScenes || changeInPrefabs);
        if (GUILayout.Button("Replace Font", GUILayout.Width(140), GUILayout.Height(30)))
        {
            if (EditorUtility.DisplayDialog("Confirm Replace",
                "This will modify objects in your scenes and/or prefabs. It's highly recommended you have your project under version control. Continue?", "Yes", "Cancel"))
            {
                ReplaceFont();
            }
        }
        GUI.enabled = true;
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Notes:\n- Changes in scenes will mark the scene dirty (remember to save).\n- Prefab assets will be modified and saved.\n- Have version control (or backup) before running on the whole project.", MessageType.None);
    }

    private void ReplaceFont()
    {
        int changedCount = 0;

        if (changeInOpenScenes)
        {
            changedCount += ReplaceInOpenScenes();
        }

        if (changeInPrefabs)
        {
            changedCount += ReplaceInPrefabs();
        }

        EditorUtility.DisplayDialog("Font Replace Complete", $"Replaced font on {changedCount} TextMeshPro UGUI components.", "OK");
    }

    private int ReplaceInOpenScenes()
    {
        int localCount = 0;

        for (int i = 0; i < EditorSceneManager.sceneCount; i++)
        {
            Scene scene = EditorSceneManager.GetSceneAt(i);
            if (!scene.isLoaded) continue;

            GameObject[] roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var tmpros = root.GetComponentsInChildren<TextMeshProUGUI>(includeInactive);
                foreach (var t in tmpros)
                {
                    if (t.font == targetFont) continue;

                    Undo.RecordObject(t, "Replace TMP Font");
                    t.font = targetFont;
                    EditorUtility.SetDirty(t);
                    localCount++;
                }
            }

            if (localCount > 0)
            {
                EditorSceneManager.MarkSceneDirty(scene);
            }
        }

        return localCount;
    }

    private int ReplaceInPrefabs()
    {
        int localCount = 0;

        // Find all prefabs in project
        string[] guids = AssetDatabase.FindAssets("t:Prefab");
        int total = guids.Length;

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            GameObject prefabRoot = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefabRoot == null) continue;

            // Get components in the prefab (searching the prefab contents)
            var tmpros = prefabRoot.GetComponentsInChildren<TextMeshProUGUI>(true);
            bool prefabModified = false;

            foreach (var t in tmpros)
            {
                if (t.font == targetFont) continue;

                // We need to open the prefab stage to modify the instance safely
#if UNITY_2018_3_OR_NEWER
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
#else
                GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefabRoot);
#endif
                var tmpsInInstance = instance.GetComponentsInChildren<TextMeshProUGUI>(true);
                bool changedInInstance = false;

                foreach (var instT in tmpsInInstance)
                {
                    if (instT.font != targetFont)
                    {
                        Undo.RecordObject(instT, "Replace TMP Font in Prefab");
                        instT.font = targetFont;
                        changedInInstance = true;
                        localCount++;
                    }
                }

                if (changedInInstance)
                {
                    // Apply changes back to the prefab asset
                    PrefabUtility.SaveAsPrefabAsset(instance, path);
                    prefabModified = true;
                }

                // Destroy the temporary instance
                DestroyImmediate(instance);

                // We don't need to continue checking other components for this prefab root since we handled all children via the instantiated copy
                break;
            }

            // Progress bar (optional)
            if (EditorUtility.DisplayCancelableProgressBar("Replacing TMP Font in Prefabs", path, (float)i / total))
            {
                EditorUtility.ClearProgressBar();
                break;
            }
        }

        EditorUtility.ClearProgressBar();
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        return localCount;
    }
}
