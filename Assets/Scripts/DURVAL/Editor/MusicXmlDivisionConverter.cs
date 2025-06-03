using System;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml.Linq;
using System.Globalization;
using System.Linq;

public class MusicXMLDivisionConverter : EditorWindow
{
    private DefaultAsset selectedFolder;
    private TextAsset selectedFile;

    [MenuItem("Tools/MusicXML Division Converter")]
    public static void ShowWindow()
    {
        GetWindow<MusicXMLDivisionConverter>("MusicXML Division Converter");
    }

    void OnGUI()
    {
        GUILayout.Label("Convert MusicXML Files to Division = 1", EditorStyles.boldLabel);

        selectedFolder = (DefaultAsset)EditorGUILayout.ObjectField("Folder", selectedFolder, typeof(DefaultAsset), false);
        selectedFile = (TextAsset)EditorGUILayout.ObjectField("Or Single File", selectedFile, typeof(TextAsset), false);

        if (GUILayout.Button("Convert MusicXML Files"))
        {
            if (selectedFile != null)
            {
                string filePath = AssetDatabase.GetAssetPath(selectedFile);
                ConvertFile(filePath);
            }
            else if (selectedFolder != null)
            {
                string folderPath = AssetDatabase.GetAssetPath(selectedFolder);
                var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                    .Where(f => f.EndsWith(".musicxml", StringComparison.OrdinalIgnoreCase) || f.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
                    .ToArray();
                foreach (var file in files)
                {
                    ConvertFile(file);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please select either a folder or a MusicXML file.", "OK");
            }
        }
    }

    void ConvertFile(string filePath)
    {
        var doc = XDocument.Load(filePath);
        var ns = doc.Root.Name.Namespace;
        var divisionsElement = doc.Descendants(ns + "divisions").FirstOrDefault();

        if (divisionsElement == null)
        {
            Debug.LogWarning($"No <divisions> found in: {filePath}");
            return;
        }

        if (!int.TryParse(divisionsElement.Value, out int oldDivision) || oldDivision == 1)
        {
            Debug.LogWarning($"Skipping {filePath} (invalid or already division=1)");
            return;
        }

        float scale = 1f / oldDivision;

        // Update divisions to 1
        divisionsElement.Value = "1";

        // Update all duration values using float conversion
        foreach (var durationElem in doc.Descendants(ns + "duration"))
        {
            if (float.TryParse(durationElem.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out float originalDur))
            {
                float newDur = originalDur * scale;
                durationElem.Value = newDur.ToString("0.###", CultureInfo.InvariantCulture); // Keep decimals up to 3 digits
            }
        }

        // Save to new file with "_DIV1" suffix
        string newPath = Path.Combine(Path.GetDirectoryName(filePath),
            Path.GetFileNameWithoutExtension(filePath) + "_DIV1.xml");

        doc.Save(newPath);
        Debug.Log($"Converted and saved: {newPath}");
    }
}