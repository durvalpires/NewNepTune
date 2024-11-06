using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Levels.SelectionMinigame;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SectionLevelsUI : MonoBehaviour
{
    [SerializeField] private ContentSizeFitter container;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private LevelsPopUpToggleManager popUpToggleManager;
    [SerializeField] private Image popup1Image;
    [SerializeField] private SceneManagerScript sceneManagerScript;
    [SerializeField] private GeneralConfigSO generalConfig;

    private List<SectionLevelBtnUI> allItems = new List<SectionLevelBtnUI>();
    private static float lastScrollPos = 0;
    private int worldIndex = 0;
    private string worldId = "";
    private AllWorldsSO.WordData _data;
    public void Init(AllWorldsSO.WordData data)
    {
        _data = data;
        worldId = data.id;
        if (TempDataStorage.ContainsKey("worldIndex"))
        {
            worldIndex = TempDataStorage.GetData<int>("worldIndex");
        }
        
        .1f.Delay(() =>
        {
            SectionLevelBtnUI openedLevel = null;
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
                    if (openedLevel == null)
                    {
                        openedLevel = item;
                        item.Unlock();
                    }
                    else 
                        item.Lock();
                }
            }
            StartCoroutine(RefreshCanvas());
        });
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))//complete All
        {
            for (int i = 0; i < _data.levels.Length; i++)
            {
                PlayerModel.CompleteLevel(i, worldId);
            }

            SceneManager.LoadScene(generalConfig.levelsScene);
        }
    }
#endif
    public void RefreshLocks()
    {
        SectionLevelBtnUI openedLevel = null;
        //refresh locks;
        foreach (var item in allItems)
        {
            if (PlayerModel.IsLevelCompleted(item.levelIndex, worldId))
            {
                item.Done();
            }
            else
            {
                if (openedLevel == null)
                {
                    openedLevel = item;
                    item.Unlock();
                }
                else 
                    item.Lock();
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
                        popup1Image.sprite = Resources.Load<Sprite>("NoteCard/" + noteDdata.note);
                    }
                    else if (noteDdata.popupStyle == NoteLevelSO.PopupStyle.NotesOnPiano)
                    {
                        popup1Image.sprite = Resources.Load<Sprite>("NotesOnPiano/" + noteDdata.note);
                    }
                    popUpToggleManager.TogglePopup(noteDdata.note);
                }
                break;
            case LevelSO.LevelType.ImageSelection:
                var data = obj.LevelData.level as ImageSelectionLevelSO;
                sceneManagerScript.LoadSelectionMinigame(data.note+","+generalConfig.levelsScene);
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
                var learningInstrumentData = obj.LevelData.level as InstrumentLevelSO;
                popUpToggleManager.ToggleInstrument(learningInstrumentData.instrument);
                break;
            case LevelSO.LevelType.InstrumentGuess:
                //?? data ??
                sceneManagerScript.LoadInstrumentGuessMinigame(generalConfig.levelsScene);
                break;
            case LevelSO.LevelType.MemoryCards:
                TempDataStorage.SetSceneData(obj.LevelData.level);
                
                sceneManagerScript.ChangeScene(generalConfig.memoryCardScene);
                break;
        }
        
    }

    private IEnumerator RefreshCanvas()
    {
        container.enabled = false;
        yield return null;
        container.enabled = true;
        yield return null;
        
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

    private void OnEnable()
    {
        LevelCompletObserver.onLevelComplete += RefreshLocks;
    }
    private void OnDisable()
    {
        LevelCompletObserver.onLevelComplete -= RefreshLocks;
    }
}
