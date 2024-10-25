using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SectionLevelsUI : MonoBehaviour
{
    [SerializeField] private ContentSizeFitter container;
    [SerializeField] private GameObject prefab;
    [SerializeField] private PopUpToggleManager popUpToggleManager;
    [SerializeField] private SceneManagerScript sceneManagerScript;
    
    public void Init(AllWorldsSO.WorldSections data)
    {
        return;
        foreach (var level in data.levels)
        {
            var item = Instantiate(prefab, container.transform);
            // item.Init(level);
        }

        StartCoroutine(RefreshCanvas());
    }
    private IEnumerator RefreshCanvas()
    {
        container.enabled = false;
        yield return null;
        container.enabled = true;
    }
}
