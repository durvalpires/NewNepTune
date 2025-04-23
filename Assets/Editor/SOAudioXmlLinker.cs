using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.AddressableAssets;

public class SOAudioXmlLinker : EditorWindow
{
    ScriptableObject targetSO;
    DefaultAsset audioFolder;
    DefaultAsset levelSOsFolder;
    string levelSOText = "VirtualPiano";
    private bool backgroundTrackFolder = true;
    DefaultAsset xmlFolder;
    
    
    private string audioFolderPath;
    private string xmlFolderPath;
    private string levelSOsFolderPath;

    [MenuItem("Tools/SO Audio/XML Linker")]
    static void Init()
    {
        GetWindow<SOAudioXmlLinker>("SO Audio/XML Linker");
    }

    void OnGUI()
    {
        GUILayout.Label("Link Audio and MusicXML to ScriptableObject", EditorStyles.boldLabel);

        GUILayout.BeginVertical("box");
        GUILayout.Label("Levels SOs", EditorStyles.boldLabel);
        targetSO = (ScriptableObject)EditorGUILayout.ObjectField("Target ScriptableObject", targetSO, typeof(ScriptableObject), false);
        levelSOsFolder = (DefaultAsset)EditorGUILayout.ObjectField("Target ScriptableObject Folder", levelSOsFolder, typeof(DefaultAsset), false);
        levelSOText = (string)EditorGUILayout.TextField("LevelSOName", levelSOText);
        GUILayout.EndVertical();
        audioFolder = (DefaultAsset)EditorGUILayout.ObjectField("Audio Folder", audioFolder, typeof(DefaultAsset), false);
        backgroundTrackFolder = EditorGUILayout.Toggle("Is Background Track Folder", backgroundTrackFolder);
        xmlFolder = (DefaultAsset)EditorGUILayout.ObjectField("XML Folder", xmlFolder, typeof(DefaultAsset), false);

        GUI.enabled = /*targetSO != null &&*/ audioFolder && levelSOsFolder != null && xmlFolder != null;

        if (GUILayout.Button("Link Files"))
        {
            ApplyContentToFolderLevels();
        }

        GUI.enabled = true;
    }

    void ApplyContentToFolderLevels()
    {
        ConvertFolderAssetsToPaths();
        
        string[] levelSOs = FindFilesWithText(levelSOsFolderPath, levelSOText, ".asset");
        
        foreach (string levelSO in levelSOs)
        {
            ScriptableObject so = AssetDatabase.LoadAssetAtPath<ScriptableObject>(levelSO);
            string number = ExtractNumberFromFile(so);
            if (string.IsNullOrEmpty(number)) continue;
            
            // // Find matching files
            string[] mp3Path = FindFilesWithNumber(audioFolderPath, number, "", ".mp3", ".ogg");
            string[] xmlPath = FindFilesWithNumber(xmlFolderPath, number,"", ".xml", ".musicxml");
            
            
            //SerializedObject soSerialized = new SerializedObject(so);
            
            if (xmlPath.Length > 0)
                AssignAssetReference(so, "songXml", xmlPath[0]);
            
            if (mp3Path.Length > 0)
                AssignAssetReference(so, backgroundTrackFolder ? "backgroundClip" : "songClip", mp3Path[0]);
            
            //soSerialized.ApplyModifiedProperties();
        }
        
        // // Find matching files
        // string[] mp3Path = FindFilesWithNumber(audioFolderPath, number, "VirtualPiano", ".mp3", ".ogg");
        // string[] xmlPath = FindFilesWithNumber(xmlFolderPath, number, "VirtualPiano",".xml", ".musicxml");
        //
        // // Find matching files
        // string[] mp3Paths = Directory.GetFiles(audioFolderPath, "*.mp3");
        // string[] oggPaths = Directory.GetFiles(audioFolderPath, "*.ogg");
        // string[] xmlPaths = Directory.GetFiles(xmlFolderPath, "*.xml");
        // string[] levelSOsPaths = Directory.GetFiles(levelSOsFolderPath, "*.asset");
    }

    void LinkFilesToSO()
    {
        string soName = targetSO.name;

        // Extract number from ScriptableObject name
        Match match = Regex.Match(soName, @"\d+");
        if (!match.Success)
        {
            Debug.LogError("No number found in ScriptableObject name.");
            return;
        }

        string number = match.Value;

        // Convert folder assets to actual paths
        string audioFolderPath = AssetDatabase.GetAssetPath(audioFolder);
        string xmlFolderPath = AssetDatabase.GetAssetPath(xmlFolder);

        // Find matching files
        string mp3Path = FindFileWithNumber(audioFolderPath, number, ".mp3", ".ogg");
        string xmlPath = FindFileWithNumber(xmlFolderPath, number, ".xml", ".musicxml");

        // Load and assign
        SerializedObject soSerialized = new SerializedObject(targetSO);

        if (!string.IsNullOrEmpty(mp3Path))
        {
            AudioClip clip = AssetDatabase.LoadAssetAtPath<AudioClip>(mp3Path);
            soSerialized.FindProperty("backingTrack").objectReferenceValue = clip;
        }

        if (!string.IsNullOrEmpty(xmlPath))
        {
            TextAsset xml = AssetDatabase.LoadAssetAtPath<TextAsset>(xmlPath);
            soSerialized.FindProperty("musicXmlFile").objectReferenceValue = xml;
        }

        soSerialized.ApplyModifiedProperties();
        EditorUtility.SetDirty(targetSO);

        Debug.Log("Assets linked successfully to " + soName);
    }

    string FindFileWithNumber(string folderPath, string number, params string[] extensions)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path).ToLower();

            if (fileName.Contains(number) && System.Array.Exists(extensions, e => e == ext))
            {
                return path;
            }
        }
        return null;
    }

    string[] FindFilesWithNumber(string folderPath, string number, string textToMatch = null, params string[] extensions)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });

        string[] filteredGuids = guids
            .Where(guid =>
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                return Path.GetDirectoryName(path).Replace("\\", "/") == folderPath;
            })
            .ToArray();
        
        List<string> paths = new List<string>();
        
        foreach (string guid in filteredGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path).ToLower();

            if (MatchesExactNumber(fileName, number))
                if(System.Array.Exists(extensions, e => e == ext))
                    if (string.IsNullOrEmpty(textToMatch) || fileName.Contains(textToMatch))
                        {
                            paths.Add(guid);
                        }
        }
        return paths.ToArray();
    }
    
    string[] FindFilesWithText(string folderPath, string textToMatch, params string[] extensions)
    {
        string[] guids = AssetDatabase.FindAssets("", new[] { folderPath });
        List<string> paths = new List<string>();
        
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);
            string ext = Path.GetExtension(path).ToLower();

            if (fileName.Contains(textToMatch) && System.Array.Exists(extensions, e => e == ext))
            {
                paths.Add(path);
            }
        }
        return paths.ToArray();
    }

    string ExtractNumberFromFile(ScriptableObject targetSO)
    {
        string soName = targetSO.name;
        string number = string.Empty;

        // Extract number from ScriptableObject name
        Match match = Regex.Match(soName, @"\d+");
        if (!match.Success)
        {
            Debug.LogError("No number found in ScriptableObject name.");
        }
        else
        {
            number = match.Value;
        }
        return number;
    }
    
    void ConvertFolderAssetsToPaths()
    {
        // Convert folder assets to actual paths
        if (audioFolder != null) audioFolderPath = AssetDatabase.GetAssetPath(audioFolder);
        if (xmlFolder != null) xmlFolderPath = AssetDatabase.GetAssetPath(xmlFolder);
        if (levelSOsFolder != null) levelSOsFolderPath = AssetDatabase.GetAssetPath(levelSOsFolder);
    }
   
    bool MatchesExactNumber(string input, string number)
    {
        return Regex.IsMatch(input, $@"(?<!\d){Regex.Escape(number)}(?!\d)");
    }
    
    void AssignAssetReference(ScriptableObject so, string fieldName, string guid)
    {
        var assetRef = new AssetReference(guid);

        var soType = so.GetType();
        var field = soType.GetField(fieldName);
        if (field != null && field.FieldType == typeof(AssetReference))
        {
            field.SetValue(so, assetRef);
            EditorUtility.SetDirty(so); // mark dirty so Unity saves it
        }
        else
        {
            Debug.LogError($"Field {fieldName} not found or not an AssetReference on {so.name}");
        }
    }
    
}
