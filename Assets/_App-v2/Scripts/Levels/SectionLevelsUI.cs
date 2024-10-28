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
        // return;
        foreach (var level in data.levels)
        {
            var itemGO = Instantiate(prefab, container.transform);
            var item = itemGO.GetComponent<SectionLevelBtnUI>();
            item.Init(level);
            item.onClick = OnLevelBtnClicked;
        }

        StartCoroutine(RefreshCanvas());
    }

    private void OnLevelBtnClicked(SectionLevelBtnUI obj)
    {
        switch (obj.LevelType)
        {
            case LevelSO.LevelType.LearningNote:
                break;
        }
    }

    private IEnumerator RefreshCanvas()
    {
        container.enabled = false;
        yield return null;
        container.enabled = true;
    }
}
