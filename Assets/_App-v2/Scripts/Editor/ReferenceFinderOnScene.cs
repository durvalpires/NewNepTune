using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ReferenceFinderOnScene : EditorWindow
{
    UnityEngine.Object Source;
    UnityEngine.Object Place;
    private static UnityEngine.Object _place;
    bool isString;
    string SourceText;
    Vector2 ScrollPosition;
    List<ReferenceData> ReferencedBy = new List<ReferenceData>();

    [MenuItem("Tools/Reference finder on scene")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(ReferenceFinderOnScene));
    }

    void OnGUI()
    {
        EditorGUILayout.BeginHorizontal();

        isString = EditorGUILayout.Toggle("Is string", isString);
        if (!isString)
            Source = EditorGUILayout.ObjectField(Source, typeof(Object), true);
        else
            SourceText = EditorGUILayout.TextField(SourceText);
        EditorGUILayout.EndHorizontal();
        _place = Place = EditorGUILayout.ObjectField("Place",Place, typeof(Object), true);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Find!"))
        {
            if ((!isString && Source == null) || (isString && SourceText == "") )
                ShowNotification(new GUIContent("No object selected for searching"));
            else
            {
                if(isString)
                    ReferencedBy = FindReferencesToString(SourceText);
                else
                    ReferencedBy = FindReferencesTo(Source);
                
                if (ReferencedBy.Count == 0)
                    ShowNotification(new GUIContent("No references have found"));
            }
        }

        ScrollPosition = EditorGUILayout.BeginScrollView(ScrollPosition);
        for (int i = 0; i < ReferencedBy.Count; i++)
        {
            EditorGUILayout.ObjectField(ReferencedBy[i].ReferenceObject, typeof(Object), true);
            EditorGUILayout.LabelField("Script : " + ReferencedBy[i].ReferenceScript);
            EditorGUILayout.LabelField("Field : " + ReferencedBy[i].ReferenceField);
            EditorGUILayout.LabelField("Reference to : " + ReferencedBy[i].ReferenceTo);
        }
        EditorGUILayout.EndScrollView();
    }
    private List<ReferenceData> FindReferencesTo(Object to)
    {
        var referencedBy = new List<ReferenceData>();
        var allObjects = Object.FindObjectsOfType<GameObject>();
        var referenceTo = new List<Object>();
        referenceTo.Add(to);

        //if gameobject, find references to all components of it
        var toGO = to as GameObject;
        if (toGO)
        {
            var components = toGO.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                referenceTo.Add(components[i]);
            }
        }

        foreach (var go in AllSceneObjects())
        {
            if (PrefabUtility.GetPrefabType(go) == PrefabType.PrefabInstance)
            {
                if (PrefabUtility.GetPrefabParent(go) == to)
                {
                    referencedBy.Add(new ReferenceData(go, "Prefab instance", "", ""));
                }
            }

            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                var c = components[i];
                if (!c) continue;

                var so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                    if (sp.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        for (int k = 0; k < referenceTo.Count; k++)
                        {
                            if (sp.objectReferenceValue == referenceTo[k])
                            {
                                referencedBy.Add(new ReferenceData(c.gameObject, c.GetType().ToString(), 
                                    sp.propertyPath, referenceTo[k].GetType().ToString()));
                            }
                        }
                    }
            }
        }

        return referencedBy;
    }

    private List<ReferenceData> FindReferencesToString(string to)
    {
        var referencedBy = new List<ReferenceData>();
        var allObjects = Object.FindObjectsOfType<GameObject>();

        foreach (var go in AllSceneObjects())
        {

            var components = go.GetComponents<Component>();
            for (int i = 0; i < components.Length; i++)
            {
                var c = components[i];
                if (!c) continue;

                var so = new SerializedObject(c);
                var sp = so.GetIterator();

                while (sp.NextVisible(true))
                    if (sp.propertyType == SerializedPropertyType.String)
                    {
                        if (sp.stringValue ==  to)
                        {
                            referencedBy.Add(new ReferenceData(c.gameObject, c.GetType().ToString(), 
                                sp.propertyPath, to));
                        }

                    }
            }
        }

        return referencedBy;
    }

    public static IEnumerable<GameObject> AllSceneObjects()
    {
        GameObject[] allObjects;
        if (_place != null)
        {
            var allTr = (_place as GameObject).GetComponentsInChildren<Transform>(true);
            var allObjectsList = new List<GameObject>();
            for (int i = 0; i < allTr.Length; i++)
            {
                if(allTr[i].parent == (_place as GameObject).transform)
                    allObjectsList.Add(allTr[i].gameObject);
            }

            allObjects = allObjectsList.ToArray();
        }
        else
        {
            allObjects = SceneManager.GetActiveScene().GetRootGameObjects();
        }
        var queue = new Queue<GameObject>();

        foreach (var root in allObjects)
        {
            queue.Enqueue(root);
        }

        while (queue.Count > 0)
        {
            var curGO = queue.Dequeue();
            for (int i = 0; i < curGO.transform.childCount; i++)
            {
                queue.Enqueue(curGO.transform.GetChild(i).gameObject);
            }

            yield return curGO;
        }
    }

    class ReferenceData
    {
        public ReferenceData(GameObject ro, string rs, string rf, string rt)
        {
            ReferenceObject = ro;
            ReferenceScript = rs;
            ReferenceField = rf;
            ReferenceTo = rt;
        }

        public GameObject ReferenceObject;
        public string ReferenceScript;
        public string ReferenceField;
        public string ReferenceTo;
    }
}