using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{

    [SerializeField] public GameObject settingsPanel;
    [SerializeField] private Toggle pitchToggle;

    void Start()
    {
        if (pitchToggle)
        {
            pitchToggle.SetIsOnWithoutNotify(LivePitchSettings.PitchControlEnabled);
            pitchToggle.onValueChanged.AddListener(OnPitchToggleChanged);
            OnPitchToggleChanged(pitchToggle.isOn);
        }
    }

    void OnDestroy()
    {
        if (pitchToggle) pitchToggle.onValueChanged.RemoveListener(OnPitchToggleChanged);
    }

    void OnPitchToggleChanged(bool on)
    {
        LivePitchSettings.PitchControlEnabled = on;
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
