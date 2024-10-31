using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelsInitController : MonoBehaviour
{
    [SerializeField] private GeneralConfigSO generalConfig;
    [SerializeField] private TMP_Text worldName;
    #if UNITY_EDITOR
    [SerializeField] private AllWorldsSO allLevelsConfig_Editor;
    #endif
    
    [SerializeField] private WorldDataUnityEvent onInit;
    private void Start()
    {
        AllWorldsSO.WordData data = null;
        if (TempDataStorage.ContainsKey(generalConfig.openedPlanetKey))
        {
            data = TempDataStorage.GetData<AllWorldsSO.WordData>(generalConfig.openedPlanetKey);
        }
        else
        {
            Debug.Log("No data found for key " + generalConfig.openedPlanetKey);
#if UNITY_EDITOR
            data = allLevelsConfig_Editor.worldsData[0];
#endif
        }

        worldName.text = data.title;
        if(data != null)
            onInit.Invoke(data);
    }
}
