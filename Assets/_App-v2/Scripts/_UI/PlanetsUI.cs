using UnityEngine;

public class PlanetsUI : MonoBehaviour
{
    [SerializeField] private AllWorldsSO allWorldsSO;
    [SerializeField] private GameObject upperPlanetPrefab;
    [SerializeField] private GameObject bottomPlanetPrefab;
    [SerializeField] private GameObject challengePanel;

    public void Init()
    {
        foreach (var planet in allWorldsSO.worlds)
        {
            var planetGO = Instantiate(upperPlanetPrefab, transform);
            var planetBtn = planetGO.GetComponent<PlanetBtn>();
            planetBtn.onClickAction = OnClick;
            // planetBtn.Init(planet);
        }
    }

    private void OnClick(PlanetBtn planet)
    {
        //open chalages
    }
}
