using System.Collections;
using DG.Tweening;
using Levels.SelectionMinigame;
using UnityEngine;
using UnityEngine.UI;

public class SectionLevelsUI : MonoBehaviour
{
    [SerializeField] private ContentSizeFitter container;
    [SerializeField] private GameObject prefab;
    [SerializeField] private ScrollRect scroll;
    [SerializeField] private PopUpToggleManager popUpToggleManager;
    [SerializeField] private Image popup1Image;
    [SerializeField] private SceneManagerScript sceneManagerScript;
    [SerializeField] private GeneralConfigSO generalConfig;

    private static float lastScrollPos = 0;
    
    public void Init(AllWorldsSO.WordData data)
    {
        .1f.Delay(() =>
        {
            foreach (var level in data.levels)
            {
                var itemGO = Instantiate(prefab, container.transform);
                var item = itemGO.GetComponent<SectionLevelBtnUI>();
                item.Init(level);
                item.onClick = OnLevelBtnClicked;
            }
            StartCoroutine(RefreshCanvas());
        });
        
    }

    private void OnLevelBtnClicked(SectionLevelBtnUI obj)
    {
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
                    popUpToggleManager.TogglePopup1(noteDdata.note);
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
                popUpToggleManager.ToggleInstrument1(learningInstrumentData.instrument);
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
}
