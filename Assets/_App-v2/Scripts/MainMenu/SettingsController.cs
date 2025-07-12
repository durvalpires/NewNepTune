using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsController : MonoBehaviour
{
    [SerializeField]
    public GameObject settingsPanel;


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
        FirebaseProxyService.Instance.ResetProgress();
        //PlayerModel.ClearData();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Signout()
    {
        SceneManager.LoadScene("NeptuneApp");
    }
}
