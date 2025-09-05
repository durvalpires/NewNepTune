using TMPro;
using UnityEngine;

public class PlayerNameDecorator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerNameText;

    private void OnEnable()
    {
        if(playerNameText == null)
            playerNameText = GetComponent<TextMeshProUGUI>();
        
        if(playerNameText == null)
            return;
        //FirebaseProxyService.OnUsernameChanged += UpdateUsernameText;
        UpdateUsernameText(FirebaseProxyService.Instance.Username);
    }

    private void UpdateUsernameText(string username)
    {
        playerNameText.text = username;
    }

    // private void OnDisable()
    // {
    //     FirebaseProxyService.OnUsernameChanged += UpdateUsernameText;
    // }
}
