using UnityEngine;

public class PanelManagerUserReg : MonoBehaviour
{
    public GameObject RegisterPanel;
    public GameObject Loginpanel; 
    public GameObject MainPanel; 
    
    private void Start()
    {
        MainPanel.SetActive(true);
        Loginpanel.SetActive(false);
        RegisterPanel.SetActive(false);
    }
    
    public void LoginButtonPressed()
    {
        MainPanel.SetActive(false);
        Loginpanel.SetActive(true);
        RegisterPanel.SetActive(false);
    }
    
    public void RegisterButtonPressed()
    {
        MainPanel.SetActive(false);
        Loginpanel.SetActive(false);
        RegisterPanel.SetActive(true);
    }
    
    public void BackButtonPressed()
    {
        MainPanel.SetActive(true);
        Loginpanel.SetActive(false);
        RegisterPanel.SetActive(false);
    }
}
