using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelsInitController : MonoBehaviour
{
    [SerializeField] private GeneralConfigSO generalConfig;
    [SerializeField] private TMP_Text worldName;
    #if UNITY_EDITOR
    [SerializeField] private AllWorldsSO allLevelsConfig_Editor;
    #endif
    
    [SerializeField] private WorldDataUnityEvent onInit;
    private AllWorldsSO.WordData data = null;

    private void Awake()
    {
        
    }

    private void Start()
    {
        int worldIndex = 0;

        if (TempDataStorage.ContainsKey(generalConfig.openedPlanetKey))
        {
            data = TempDataStorage.GetData<AllWorldsSO.WordData>(generalConfig.openedPlanetKey);
            worldIndex = TempDataStorage.GetData<int>("worldIndex");
        }
        else
        {
            Debug.Log("No data found for key " + generalConfig.openedPlanetKey);
#if UNITY_EDITOR
            data = allLevelsConfig_Editor.worldsData[0];
#endif
        }

        worldName.text = data.title;
        if (data != null)
        {
            // retern to worlds if all levels done in that world first time 
            if (IsAllLevelsDone && !PlayerModel.IsWorldCompleted(data.id))
            {
                PlayerModel.CompleteWorld(data.id);
                SceneManager.LoadScene(generalConfig.planetsScene);
                return;
            }
            onInit.Invoke(data);
        }
    }

    private bool IsAllLevelsDone
    {
        get
        {
            for (int i = 0; i < data.levels.Length; i++)
            {
                var level = data.levels[i];
                if (!PlayerModel.IsLevelCompleted(i, data.id))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
