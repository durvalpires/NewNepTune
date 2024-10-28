using System;
using UnityEngine;

public class SectionLevelBtnUI : MonoBehaviour
{
    public Action<SectionLevelBtnUI> onClick;

    private LevelSO levelData;
    public LevelSO.LevelType LevelType => levelData.levelType;
    public void Init(LevelSO data)
    {
        levelData = data;
    }

    public void OnCLick()
    {
        onClick?.Invoke(this);
    }
}
