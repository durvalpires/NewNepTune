using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetsUI : MonoBehaviour
{
    // [SerializeField] private AllWorldsSO allWorldsSO;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private ContentSizeFitter contentSizeFitter;
    private GeneralConfigSO generalConfigData => PlayerModel.GeneralConfig;

    private List<PlanetBtn> allPlanetBtns = new List<PlanetBtn>();
    public void Init()
    {
        var openedPlanet = -1;
        for (int i = 0; i < PlayerModel.AllWorlds.worldsData.Length; i++)
        {
            var planetData = PlayerModel.AllWorlds.worldsData[i];
            var planetGO = Instantiate(prefabs[allPlanetBtns.Count%prefabs.Length], container);
            var planetBtn = planetGO.GetComponent<PlanetBtn>();
            planetBtn.onClickAction = OnClick;
            allPlanetBtns.Add(planetBtn);
            planetBtn.Init(planetData, i);
            // 0  > -1
            if (!PlayerModel.IsWorldCompleted(planetData.id))
            {
                if (openedPlanet == -1)
                {
                    openedPlanet = i;
                }
                else
                {
                    planetBtn.Lock();
                }
            }
        }
        StartCoroutine(RefreshCanvas());
    }

    private void OnClick(AllWorldsSO.WordData planetData, int index)
    {
        //open Levels
        TempDataStorage.SetData(generalConfigData.openedPlanetKey, planetData);
        TempDataStorage.SetData("worldIndex", index);
        SceneManager.LoadScene(generalConfigData.levelsScene); 
    }

    private IEnumerator RefreshCanvas()
    {
        contentSizeFitter.enabled = false;
        yield return null;
        contentSizeFitter.enabled = true;
    }
}
