using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SectionLevelBtnUI : MonoBehaviour
{
    public Action<SectionLevelBtnUI> onClick;

    [SerializeField] private TMP_Text title;
    [SerializeField] private Image icon;
    [SerializeField] private UnityEvent onUnlock;
    [SerializeField] private UnityEvent onDone;
    [SerializeField] private UnityEvent onForceUnlock;
    [SerializeField] private UnityEvent onLockedClick;
    private AllWorldsSO.LevelsData levelData;
    public LevelSO.LevelType LevelType => levelData.level.levelType;
    public AllWorldsSO.LevelsData LevelData => levelData;
    private bool _isLocked = false;
    public int levelIndex = -1;
    public void Lock()
    {
       _isLocked = true;
    }
    public void Unlock(bool force = false)
    {
        if(force) onForceUnlock.Invoke();
        else onUnlock.Invoke();
        _isLocked = false;
        PlayerModelBase.SetLevelUnlock(levelIndex);
    }

    public void Done()
    {
        onDone.Invoke();
    }
    public bool IsLocked => _isLocked;
    public void Init(AllWorldsSO.LevelsData data, int index)
    {
        levelIndex = index;
        levelData = data;
        icon.sprite = data.image;
        title.text = string.IsNullOrEmpty(data.level.levelTitle) ? ConvertToSpacedWords(data.level.levelType.ToString()) : data.level.levelTitle;
    }

    public void OnCLick()
    {
        if (_isLocked)
        {
            onLockedClick.Invoke();
            return;
        }
        onClick?.Invoke(this);
    }
    public static string ConvertToSpacedWords(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        StringBuilder result = new StringBuilder();
        
        foreach (char c in input)
        {
            if (char.IsUpper(c) && result.Length > 0)
            {
                result.Append(' ');
            }
            result.Append(c);
        }

        return result.ToString();
    }
}
