using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetsUI : MonoBehaviour
{
    // [SerializeField] private AllWorldsSO allWorldsSO;
    [SerializeField] private Transform container;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private ContentSizeFitter contentSizeFitter;

    [SerializeField] private CongratsConfigSO congratsConfig;
    [SerializeField] private Transform uiCanvas;
    [SerializeField] private ScrollRect scroll;

    private float holdScrollSpeed = 0.1f;
    private float scrollStep = 0.1f;
    private bool holdingRight = false;
    private bool holdingLeft = false;
    private Tween planetTween;
    private static float lastScrollPos = 0;
    private int lastUnlockedIndex = -1;
    private GeneralConfigSO generalConfigData => PlayerModel.GeneralConfig;
    private List<PlanetBtn> allPlanetBtns = new List<PlanetBtn>();
    public void Init()
    {
        var openedPlanet = -1;
        lastUnlockedIndex = -1;
        for (int i = 0; i < PlayerModel.AllWorlds.worldsConfigs.Length; i++)
        {
            var planetData = PlayerModel.AllWorlds.worldsConfigs[i];
            var planetGO = Instantiate(prefabs[allPlanetBtns.Count % prefabs.Length], container);
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
                    lastUnlockedIndex = i;
                }
                else
                {
                    planetBtn.Lock();
                }
            }
        }
        if (lastUnlockedIndex == congratsConfig.triggerPlanetIndex)
        {
            string worldName = PlayerModel.AllWorlds.worldsConfigs[lastUnlockedIndex].name;
            congratsConfig.ShowCongrats(this, uiCanvas, worldName, lastUnlockedIndex);

        }
        if (openedPlanet == congratsConfig.triggerPlanetIndex)
        {
            string worldName = PlayerModel.AllWorlds.worldsConfigs[openedPlanet].name;
            congratsConfig.ShowCongrats(this, uiCanvas, worldName, openedPlanet);
        }
         HighlightLastUnlockedPlanet();

        scroll.onValueChanged.AddListener(ScrollPosUpdate);
        StartCoroutine(RefreshCanvas());
    }
    private void Update()
    {
#if UNITY_EDITOR || UNITY_WEBGL
        float delta = holdScrollSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow))
        {
            holdingRight = true;
            float targetPos = Mathf.Clamp01(scroll.horizontalNormalizedPosition + delta);
            scroll.horizontalNormalizedPosition = targetPos;
            lastScrollPos = targetPos;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            holdingLeft = true;
            float targetPos = Mathf.Clamp01(scroll.horizontalNormalizedPosition - delta);
            scroll.horizontalNormalizedPosition = targetPos;
            lastScrollPos = targetPos;
        }
        else
        {
            holdingRight = false;
            holdingLeft = false;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            float targetPos = Mathf.Clamp01(scroll.horizontalNormalizedPosition + scrollStep);
            DOTween.Kill(scroll);
            DOTween.To(() => scroll.horizontalNormalizedPosition,
                       x => scroll.horizontalNormalizedPosition = x,
                       targetPos, 0.3f).SetTarget(scroll);
            lastScrollPos = targetPos;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            float targetPos = Mathf.Clamp01(scroll.horizontalNormalizedPosition - scrollStep);
            DOTween.Kill(scroll);
            DOTween.To(() => scroll.horizontalNormalizedPosition,
                       x => scroll.horizontalNormalizedPosition = x,
                       targetPos, 0.3f).SetTarget(scroll);
            lastScrollPos = targetPos;
        }
#endif
    }
        private void OnClick(WorldSO planetData, int index)
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
        yield return null; 

        if (allPlanetBtns.Count > 0)
          {
            int idx = Mathf.Clamp(lastUnlockedIndex, 0, allPlanetBtns.Count - 1);
            CenterOnElement(allPlanetBtns[idx].GetComponent<RectTransform>());
          }

        yield return null; 

        scroll.horizontalNormalizedPosition = lastScrollPos;
    }

    private void CenterOnElement(RectTransform contentElement)
    {
        RectTransform viewport = scroll.viewport;
        RectTransform content = scroll.content;

      
        if (content.rect.width <= viewport.rect.width)
        {
          
            scroll.horizontalNormalizedPosition = 0.5f; 
            lastScrollPos = 0.5f;
            return;
        }

        Vector2 elementWorldPosition = contentElement.transform.TransformPoint(contentElement.rect.center);
        Vector2 viewportWorldPosition = viewport.transform.TransformPoint(viewport.rect.center);

        Vector2 offset = (Vector2)content.InverseTransformPoint(viewportWorldPosition)
                         - (Vector2)content.InverseTransformPoint(elementWorldPosition);

        float newNormalizedPositionX = scroll.horizontalNormalizedPosition - offset.x / (content.rect.width - viewport.rect.width);

        newNormalizedPositionX = Mathf.Clamp01(newNormalizedPositionX);

        scroll.horizontalNormalizedPosition = newNormalizedPositionX;
        lastScrollPos = newNormalizedPositionX;
    }

    public void ScrollPosUpdate(Vector2 pos)
    {
        lastScrollPos = pos.x;
       
    }

    [SerializeField] private Sprite highlightRingSprite;
    private GameObject highlightRing;

    private void HighlightLastUnlockedPlanet()
    {
        if (lastUnlockedIndex < 0 || lastUnlockedIndex >= allPlanetBtns.Count)
            return;

        // find the planet image (skip lock/done overlays)
        Image planetImg = null;
        foreach (var img in allPlanetBtns[lastUnlockedIndex].GetComponentsInChildren<Image>(true))
        {
            var name = img.gameObject.name.ToLower();
            if (name.Contains("lock") || name.Contains("done")) continue;
            planetImg = img;
            break;
        }
        if (planetImg == null) return;

        var rt = planetImg.rectTransform;

        // kill any existing scale tween
        planetTween?.Kill();
        rt.localScale = Vector3.one;

        // pulse scale only
        planetTween = rt
            .DOScale(1.15f, 1f)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }
}