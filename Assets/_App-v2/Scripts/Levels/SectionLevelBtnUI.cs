using System;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SectionLevelBtnUI : MonoBehaviour
{
    public Action<SectionLevelBtnUI> onClick;

    [SerializeField] private TMP_Text title;
    [SerializeField] private Image icon;
    private AllWorldsSO.LevelsData levelData;
    public LevelSO.LevelType LevelType => levelData.level.levelType;
    public AllWorldsSO.LevelsData LevelData => levelData;
    public void Init(AllWorldsSO.LevelsData data)
    {
        levelData = data;
        icon.sprite = data.image;
        title.text = string.IsNullOrEmpty(data.level.levelTitle) ? ConvertToSpacedWords(data.level.levelType.ToString()) : data.level.levelTitle;
    }

    public void OnCLick()
    {
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
