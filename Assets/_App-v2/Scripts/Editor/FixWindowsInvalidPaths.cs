using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

public class FixWindowsInvalidPaths : EditorWindow
{
    [MenuItem("Tools/Fix Windows Invalid Paths")]
    public static void ShowWindow()
    {
        if (EditorUtility.DisplayDialog("Fix Windows Invalid Paths",
            "This will scan your Assets/ folder for Windows-incompatible paths and offer to automatically fix them. Continue?",
            "Yes", "Cancel"))
        {
            FixPaths();
        }
    }

    private static void FixPaths()
    {
        string[] paths = Directory.GetFiles("Assets", "*", SearchOption.AllDirectories);
        int fixedCount = 0;
        Regex invalidChars = new Regex("[<>:\"/\\\\|?*]");
        Regex trailingDotsSpaces = new Regex(@"[\. ]+$");
        Regex reservedNames = new Regex(@"^(CON|PRN|AUX|NUL|COM[1-9]|LPT[1-9])(\..*)?$", RegexOptions.IgnoreCase);

        foreach (string path in paths)
        {
            string relativePath = path.Replace("\\", "/");

            string directory = Path.GetDirectoryName(relativePath).Replace("\\", "/");
            string filename = Path.GetFileName(relativePath);

            string newFilename = filename;

            // Remove trailing spaces and dots
            newFilename = trailingDotsSpaces.Replace(newFilename, "");

            // Replace invalid characters with '_'
            newFilename = invalidChars.Replace(newFilename, "_");

            // Check for reserved names
            if (reservedNames.IsMatch(Path.GetFileNameWithoutExtension(newFilename)))
            {
                Debug.LogWarning($"[FixWindowsInvalidPaths] Reserved name detected, please rename manually: {relativePath}");
                continue; // skip automatic rename
            }

            if (newFilename != filename)
            {
                string newPath = Path.Combine(directory, newFilename).Replace("\\", "/");

                if (File.Exists(newPath))
                {
                    Debug.LogWarning($"[FixWindowsInvalidPaths] Cannot rename {relativePath} -> {newPath} as target already exists.");
                    continue;
                }

                AssetDatabase.MoveAsset(relativePath, newPath);
                Debug.Log($"[FixWindowsInvalidPaths] Renamed:\n{relativePath}\n->\n{newPath}");
                fixedCount++;
            }
        }

        AssetDatabase.Refresh();

        EditorUtility.DisplayDialog("Fix Windows Invalid Paths",
            $"Finished.\n{fixedCount} paths were automatically fixed.\nCheck the Console for details.",
            "OK");
    }
}