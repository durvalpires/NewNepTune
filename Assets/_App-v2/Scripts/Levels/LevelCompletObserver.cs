using System;
using UnityEngine;

public class LevelCompletObserver : MonoBehaviour
{
    public static event Action onLevelComplete;
    private static int _openedLevel = -1;
    private static string _openedLevelworldId = "";

    public static void LevelStart(int levelIndex, string worldId)
    {
        _openedLevel = levelIndex;
        _openedLevelworldId = worldId;
    }

    public void SetCurrentLevelComplete()
    {
        LevelComplete();
    }
    public static void LevelComplete()
    {
        
        PlayerModel.CompleteLevel(_openedLevel, _openedLevelworldId); 
        onLevelComplete?.Invoke();
    }
   
    // private void OnEnable()
    // {
    //     _levelsPopUpToggleManager.onPopupLevelComeplte += OnPopupLevelComplete;
    // }
    // private void OnDisable()
    // {
    //     _levelsPopUpToggleManager.onPopupLevelComeplte -= OnPopupLevelComplete;
    // }
    //
}
