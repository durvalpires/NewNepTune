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
        FirebaseProxyService.Instance.ResetProgress((result, message) =>
            {
                if (result)
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        );
        //PlayerModel.ClearData();
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public void Signout()
    {
        FirebaseProxyService.Instance.LogoutWithBackend((result, message) =>
        {
            if (!result)
            {
                Debug.LogError("Signout failed: " + message);
            }
            
            SceneManager.LoadScene("NeptuneApp");
        });
    }
}
