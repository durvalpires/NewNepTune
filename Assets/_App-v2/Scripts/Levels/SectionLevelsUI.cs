using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Levels.SelectionMinigame;
using Minigames;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SectionLevelsUI : MonoBehaviour
{
    [SerializeField] private ContentSizeFitter container;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private LevelsPopUpToggleManager popUpToggleManager;
    [SerializeField] private Image popup1Image;
    [SerializeField] private SceneManagerScript sceneManagerScript;
    [FormerlySerializedAs("PopupCanvas")] [SerializeField] private GameObject PopupGOContainer;
    private GeneralConfigSO generalConfig => PlayerModel.GeneralConfig;

    private List<SectionLevelBtnUI> allItems = new List<SectionLevelBtnUI>();
    private static float lastScrollPos = 0;
    private int worldIndex = 0;
    private string worldId = "";
    private WorldSO _data;
    private SectionLevelBtnUI lastOpenedLevel;
    public void Init(WorldSO data)
    {
        _data = data;
        worldId = data.id;
        if (TempDataStorage.ContainsKey("worldIndex"))
        {
            worldIndex = TempDataStorage.GetData<int>("worldIndex");
        }
        
        .1f.Delay(() =>
        {
            lastOpenedLevel = null;
            for (int i = 0; i <  data.levels.Length; i++)
            {
                var level = data.levels[i];
                var itemGO = Instantiate(prefab, container.transform);
                var item = itemGO.GetComponent<SectionLevelBtnUI>();
                allItems.Add(item);
                item.Init(level, i);
                item.onClick = OnLevelBtnClicked;
                
                if (PlayerModel.IsLevelCompleted(item.levelIndex, worldId))
                {
                    item.Done();
                }
                else
                {
                    if (lastOpenedLevel == null)
                    {
                        lastOpenedLevel = item;
                        item.Unlock();
                    }
                    else 
                        item.Lock();
                    // if(lastOpenedLevel == null) lastOpenedLevel = item;
                    // item.Unlock();
                    // CenterOnElement(item.GetComponent<RectTransform>());
                }
            }
            StartCoroutine(RefreshCanvas());
        });
    }

    private void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.A))//complete All
        {
            for (int i = 0; i < _data.levels.Length; i++)
            {
                PlayerModel.CompleteLevel(i, worldId);
            }

            SceneManager.LoadScene(generalConfig.levelsScene);
        }

        if (!Input.GetKeyDown(KeyCode.N)) return; //complete Next
        {
            for (int i = 0; i < _data.levels.Length; i++)
            {
                if (!PlayerModel.IsLevelCompleted(i, worldId))
                {
                    PlayerModel.CompleteLevel(i, worldId);
                    break;
                }
            }

            SceneManager.LoadScene(generalConfig.levelsScene);
        }
#endif
        
#if UNITY_ANDROID || UNITY_IOS
        var touches = new List<Touch>();
        var started = false;
        var timeCount = 0f;
        if (Input.touchCount > 0)
        {
            touches.Clear();
            foreach (var touch in Input.touches)
            {
                touches.Add(touch);

                if (touches.Count >= 7)
                {
                    if (!started)
                    {
                        started = true;
                        timeCount = 0f;
                    }

                    timeCount += Time.deltaTime;
                    if (timeCount > 5f)
                    {
                        for (int i = 0; i < _data.levels.Length; i++)
                        {
                            PlayerModel.CompleteLevel(i, worldId);
                        }
                    }
                }
                else
                {
                    started = false;
                    timeCount = 0f;
                }
            }
        }
#endif
        
    }

    public void RefreshLocks()
    {
        lastOpenedLevel = null;
        bool setNewCenter = false;
        //refresh locks;
        foreach (var item in allItems)
        {
            if (PlayerModel.IsLevelCompleted(item.levelIndex, worldId))
            {
                item.Done();
            }
            else
            {
                if (lastOpenedLevel == null)
                 {
                     lastOpenedLevel = item;
                     item.Unlock();
                     CenterOnElement(item.GetComponent<RectTransform>());
                 }
                 else 
                     item.Lock();
                
                // if(lastOpenedLevel == null) lastOpenedLevel = item;
                // item.Unlock();
                // CenterOnElement(item.GetComponent<RectTransform>());
            }
        }
    }

    private void OnLevelBtnClicked(SectionLevelBtnUI obj)
    {
        LevelCompletObserver.LevelStart(obj.levelIndex, worldId);
        
        switch (obj.LevelType)
        {
            case LevelSO.LevelType.LearningNote:
                var noteDdata = obj.LevelData.level as NoteLevelSO;
                if (noteDdata != null)
                {
                    if (noteDdata.popupStyle == NoteLevelSO.PopupStyle.NoteOnCard)
                    {
                        Instantiate(Resources.Load<GameObject>("NoteCard/" + noteDdata.note), PopupGOContainer.transform);
                        //popup1Image.sprite = Resources.Load<Sprite>("NoteCard/" + noteDdata.note);
                    }
                    else if (noteDdata.popupStyle == NoteLevelSO.PopupStyle.NotesOnPiano)
                    {
                        Instantiate(Resources.Load<GameObject>("NotesOnPiano/" + noteDdata.note), PopupGOContainer.transform);
                        //popup1Image.sprite = Resources.Load<Sprite>("NotesOnPiano/" + noteDdata.note);
                    }

                    popUpToggleManager.TogglePopup(noteDdata.note);
                }
                break;
            case LevelSO.LevelType.LearningRhythm:
                var rhythmData = obj.LevelData.level as NoteLevelSO;
                if (rhythmData != null)
                {
                    if (rhythmData.popupStyle == NoteLevelSO.PopupStyle.RhythmOnCard)
                    {
                        //popup1Image.sprite = Resources.Load<Sprite>("RhythmCard/" + rhythmData.note);
                        Instantiate(Resources.Load<GameObject>("RhythmCard/" + rhythmData.note), PopupGOContainer.transform);
                    }
                    
                    popUpToggleManager.ToggleRhythmPopup(rhythmData.note);
                }
                break;
            case LevelSO.LevelType.ImageSelection:
                // var data = obj.LevelData.level as ImageSelectionLevelSO;
                // sceneManagerScript.LoadSelectionMinigame(data.note+","+generalConfig.levelsScene);
                TempDataStorage.SetSceneData(obj.LevelData.level);
                sceneManagerScript.ChangeScene(generalConfig.selectionMiniGameScene);
                break;
            case LevelSO.LevelType.VirtualPiano:
                /*****/
                //Use data from keyVirtualPianoSOData after loading virtual piano
                TempDataStorage.SetSceneData(obj.LevelData.level);
                sceneManagerScript.ChangeScene(generalConfig.virtualPianoScene);
                /*****/
                break;
            case LevelSO.LevelType.MusicPieceGuess:
                
                TempDataStorage.SetSceneData(obj.LevelData.level);
                sceneManagerScript.ChangeScene(generalConfig.musicGuessScene);
                
                break;
            case LevelSO.LevelType.LearningInstrument:
                //     var learningInstrumentData = obj.LevelData.level as InstrumentLevelSO;
                //     popUpToggleManager.ToggleInstrument(learningInstrumentData.instrument);
                var instrumentData = obj.LevelData.level as InstrumentLevelSO;
                if (instrumentData != null)
                {
                    if (instrumentData.levelType == InstrumentLevelSO.LevelType.LearningInstrument)
                    {
                        //popup1Image.sprite = Resources.Load<Sprite>("RhythmCard/" + rhythmData.note);
                        Instantiate(Resources.Load<GameObject>("Instruments/Sprites/" + instrumentData.instrument), PopupGOContainer.transform);
                    }
                        
                    popUpToggleManager.ToggleInstrument(instrumentData.instrument);
                }
                break;
            case LevelSO.LevelType.InstrumentGuess:
                TempDataStorage.SetSceneData(obj.LevelData.level);
                sceneManagerScript.ChangeScene(generalConfig.instrumentGuessScene);
                break;
            case LevelSO.LevelType.MemoryCards:
                TempDataStorage.SetSceneData(obj.LevelData.level);
                
                sceneManagerScript.ChangeScene(generalConfig.memoryCardScene);
                break;
        }
        
    }

    private IEnumerator RefreshCanvas()
    {
        //Canvas.ForceUpdateCanvases();
        yield return null;
        
        container.enabled = false;
        yield return null;
        container.enabled = true;
        yield return null;
        
        CenterOnElement(lastOpenedLevel == null ? allItems[0].GetComponent<RectTransform>() : 
            lastOpenedLevel.GetComponent<RectTransform>());
        
        // scroll.normalizedPosition = new Vector2(0, 0);
        float currentX = scroll.normalizedPosition.x;

        DOTween.To(() => currentX, x => {
            currentX = x;
            scroll.normalizedPosition = new Vector2(currentX, scroll.normalizedPosition.y);
        }, lastScrollPos, 1);
    }

    public void ScrollPosUpdate(Vector2 pos)
    {
        lastScrollPos = pos.x;
    }
    
    public void CenterOnElement(RectTransform contentElement)
    {
        // Get the ScrollRect's viewport RectTransform
        RectTransform viewport = scroll.viewport;

        // Calculate the position of the target element relative to the content's anchor
        Vector2 elementWorldPosition = contentElement.transform.TransformPoint(contentElement.rect.center);
        Vector2 viewportWorldPosition = viewport.transform.TransformPoint(viewport.rect.center);

        // Calculate the offset needed to move the content to center the element
        Vector2 offset = (Vector2)scroll.content.InverseTransformPoint(viewportWorldPosition)
                         - (Vector2)scroll.content.InverseTransformPoint(elementWorldPosition);

        // Adjust the scroll position (vertical or horizontal as needed)
        // if (scroll.horizontal)
        // {
            float newNormalizedPositionX  = Mathf.Clamp01(scroll.horizontalNormalizedPosition - offset.x / scroll.content.rect.width);
            if(newNormalizedPositionX > 0.9)
                newNormalizedPositionX = 1f;
            else if(newNormalizedPositionX < 0.1)
                newNormalizedPositionX = 0f;
            lastScrollPos = newNormalizedPositionX; 
            //scroll.horizontalNormalizedPosition = newNormalizedPositionX;
        // }
        //
        // if (scroll.vertical)
        // {
        //     float newNormalizedPositionY = Mathf.Clamp01(scroll.verticalNormalizedPosition - offset.y / scroll.content.rect.height);
        //     scroll.verticalNormalizedPosition = newNormalizedPositionY;
        // }
        
        //lastScrollPos = newNormalizedPositionX;
    }

    private void OnEnable()
    {
        LevelCompletObserver.onLevelComplete += RefreshLocks;
    }
    private void OnDisable()
    {
        LevelCompletObserver.onLevelComplete -= RefreshLocks;
    }
}
