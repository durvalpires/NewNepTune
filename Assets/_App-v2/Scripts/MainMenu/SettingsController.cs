using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    [SerializeField] public GameObject settingsPanel;
    [SerializeField] private Toggle pitchToggle;
    [SerializeField] private RhythmGameSettings gameSettings; 

    void Start()
    {
        if (pitchToggle)
        {
            pitchToggle.isOn = gameSettings.pitchControlEnabled;
            pitchToggle.onValueChanged.AddListener(OnPitchToggleChanged);
        }
    }

    void OnDestroy()
    {
        if (pitchToggle) pitchToggle.onValueChanged.RemoveListener(OnPitchToggleChanged);
    }

    void OnPitchToggleChanged(bool on)
    {
        gameSettings.SetPitchControlEnabled(on);
        Debug.Log("PitchControl (SO) = " + on);
    }


    public void OpenSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }
    
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }

    public void ResetProgress()
    {
        PlayerModel.ClearData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
