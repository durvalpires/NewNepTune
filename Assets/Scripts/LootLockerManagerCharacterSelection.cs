using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using LootLocker;
using TMPro;
using LootLocker.Requests;

public class LootLockerManagerCharacterSelection : MonoBehaviour
{
    public TextMeshProUGUI welcomeText;
    // Start is called before the first frame update
    void Start()
    {
        LootLockerSDKManager.StartWhiteLabelSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");
                    return;
                }
                else
                {
                    Debug.Log("session started successfully");
                    LootLockerSDKManager.GetPlayerName((response) =>
                    {
                        if (response.success)
                        {
                            Debug.Log("Successfully retrieved player name: " + response.name);
                            welcomeText.text = "Welcomeee, " + response.name;
                        } else
                        {
                            Debug.Log("Error getting player name");
                        }
                    });
                }
            });
        
    }
}
