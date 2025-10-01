using System.IO;
using UnityEditor;
using UnityEngine;
using System.Text.RegularExpressions;

public class FixMetaGUIDs
{
    private static readonly Regex guidRegex = new Regex(@"guid: [0-9a-f]{32}", RegexOptions.Compiled);

    [MenuItem("Tools/Fix Missing or Invalid .meta GUIDs")]
    public static void FixInvalidMetaGUIDs()
    {
        int fixedCount = 0;
        int skippedCount = 0;

        string[] metaFiles = Directory.GetFiles(Application.dataPath, "*.meta", SearchOption.AllDirectories);

        foreach (string metaFilePath in metaFiles)
        {
            string metaContent = File.ReadAllText(metaFilePath);
            Match guidMatch = guidRegex.Match(metaContent);

            if (!guidMatch.Success)
            {
                // This meta file doesn't have a valid GUID — regenerate it
                string relativePath = "Assets" + metaFilePath.Replace(Application.dataPath, "").Replace("\\", "/");
                Object asset = AssetDatabase.LoadMainAssetAtPath(relativePath);

                if (asset != null)
                {
                    string newGuid = GUID.Generate().ToString().ToLowerInvariant();
                    string fixedContent = Regex.Replace(metaContent, @"guid: .*", $"guid: {newGuid}");
                    File.WriteAllText(metaFilePath, fixedContent);
                    fixedCount++;
                    Debug.Log($"Fixed GUID for: {relativePath}");
                }
                else
                {
                    skippedCount++;
                    Debug.LogWarning($"Skipped (no asset found): {relativePath}");
                }
            }
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog("Fix Meta GUIDs", 
            $"Done!\nFixed: {fixedCount}\nSkipped: {skippedCount}\nProject refreshed.", "OK");
    }
}