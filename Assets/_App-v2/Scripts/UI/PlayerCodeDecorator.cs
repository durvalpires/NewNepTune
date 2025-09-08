using TMPro;
using UnityEngine;

public class PlayerCodeDecorator : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI playerCodeText;

    private void OnEnable()
    {
        if(playerCodeText == null)
            playerCodeText = GetComponent<TextMeshProUGUI>();
        
        if(playerCodeText == null)
            return;
        //FirebaseProxyService.OnUsernameChanged += UpdateUsernameText;
        UpdatePrivateCodeText(FirebaseProxyService.Instance.PrivateCode);
    }

    private void UpdatePrivateCodeText(string username)
    {
        playerCodeText.text = username;
    }

    // private void OnDisable()
    // {
    //     FirebaseProxyService.OnUsernameChanged += UpdateUsernameText;
    // }
}
