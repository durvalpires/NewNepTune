using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "AllWorldsSO", menuName = "Scriptable Objects/v2_AllWorldsSO")]
public class AllWorldsSO : ScriptableObject
{
    public WordData[] worldsData;

    [Serializable]
    public class WordData
    {
        public string title;
        public string id;
        public Sprite worldSprite;
        public WorldSections[] sections;
    }
    [Serializable]
    public class WorldSections
    {
        public string title;
        public string id;
        public Sprite image;
        public string sectionBGImageName;
        public LevelSO[] levels;

        public enum SceneType
        {
            None,
            Day,
            Night,
            Clouds
        }
    }
#if UNITY_EDITOR 
    public bool generateIds;
    private void OnValidate()
    {
        var ids = new HashSet<string>();
        var idCounter = 0;
        if (generateIds)
        {
            generateIds = false;
            foreach (var wordData in worldsData)
            {
                if (wordData.id == "" || ids.Contains(wordData.id))
                {
                    wordData.id = Guid.NewGuid().ToString();
                    idCounter++;
                }

                ids.Add(wordData.id);
                foreach (var section in wordData.sections)
                {
                    if (section.id == "" || ids.Contains(section.id))
                    {
                        section.id = Guid.NewGuid().ToString();
                        idCounter++;
                    }
                    ids.Add(section.id);
                }
            }
            Debug.Log("Generated " + idCounter + " ids");
        }
    }
#endif
}

