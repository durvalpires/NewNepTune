using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSectionsPopup : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private ContentSizeFitter container;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private GeneralConfigSO configData;
    
    private AllWorldsSO.WordData _data;
    private List<WorldSectionUI> allItems = new List<WorldSectionUI>();
    public void Show(AllWorldsSO.WordData data)
    {
        _data = data;
        Clean();
        CreateItems();
        transform.localScale = Vector3.one * 0.1f;
        gameObject.SetActive(true);
        transform.DOScale(1, 0.3f).SetEase(Ease.OutBack);
        StartCoroutine(RefreshCanvas());
    }

    public void Hide()
    {
        transform.DOScale(0.5f, 0.05f).SetEase(Ease.InCubic).OnStepComplete(() =>
        {
            gameObject.SetActive(false);
        });
    }
    private void CreateItems()
    {
        foreach (var sectionData in _data.sections)
        {
                var itemGO = Instantiate(prefab, container.transform);
                var itemBtn = itemGO.GetComponent<WorldSectionUI>();
                itemBtn.onClick = OnItemClicked;
                allItems.Add(itemBtn);
                itemBtn.Init(sectionData);
        }
    }

    private void OnItemClicked(AllWorldsSO.WorldSections item)
    {
        //load scene with levels
        TempDataStorage.SetData("Section", item);
        SceneManager.LoadScene("v2_Section"); 
    }

    
    private void Clean()
    {
        foreach (var item in allItems)
        {
            Destroy(item.gameObject);
        }
        allItems.Clear();
    }

    private IEnumerator RefreshCanvas()
    {
        container.enabled = false;
        yield return null;
        container.enabled = true;
        yield return null;
        scroll.normalizedPosition = new Vector2(0, 0);
    }
}
