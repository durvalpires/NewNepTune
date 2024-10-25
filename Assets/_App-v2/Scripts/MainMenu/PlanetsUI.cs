using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetsUI : MonoBehaviour
{
    [SerializeField] private AllWorldsSO allWorldsSO;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private ContentSizeFitter contentSizeFitter;
    [SerializeField] private WorldSectionsPopup sectionsPanel;

    private List<PlanetBtn> allPlanetBtns = new List<PlanetBtn>();
    public void Init()
    {
        foreach (var planet in allWorldsSO.worldsData)
        {
            var planetGO = Instantiate(prefabs[allPlanetBtns.Count%prefabs.Length], container);
            var planetBtn = planetGO.GetComponent<PlanetBtn>();
            planetBtn.onClickAction = OnClick;
            allPlanetBtns.Add(planetBtn);
            planetBtn.Init(planet);
        }
        StartCoroutine(RefreshCanvas());
    }

    private void OnClick(AllWorldsSO.WordData planetData)
    {
        //open Sections
        sectionsPanel.Show(planetData);
    }

    private IEnumerator RefreshCanvas()
    {
        contentSizeFitter.enabled = false;
        yield return null;
        contentSizeFitter.enabled = true;
    }
}
