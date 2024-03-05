using UnityEngine;
using LootLocker;
using LootLocker.Requests;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System;

public class LootLockerAuthManager : MonoBehaviour
{
    [Header("Login Inputs")]
    public TMP_InputField loginEmailField;
    public TMP_InputField loginPassField;
    [Header("Register Inputs")]
    public TMP_InputField registerUsernameField;
    public TMP_InputField registerEmailField;
    public TMP_InputField registerPassField;
    //public Toggle rememberMeToggle;
    //private int rememberMe;
    public TextMeshProUGUI debugText;
    
    void Start()
    {
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                // Start a new session
                debugText.text = "I know you!!! Logging in, please wait..";
                SceneManager.LoadScene("MainLevelSelect");
            }
            else
            {
                // Show login form here
                Debug.Log("session is NOT valid, we should show the login form");
            }
        });
    }
    public void Login()
    {
        debugText.text = "Logging in... Please wait...";
        string email = loginEmailField.text;
        string password = loginPassField.text;
        bool rememberMe = true;

        LootLockerSDKManager.WhiteLabelLogin(email, password, Convert.ToBoolean(rememberMe), response =>
        {
            if (!response.success)
            {
                Debug.Log("error while logging in");
                debugText.text = "Login Failed";
                return;
            }
            else
            {
                Debug.Log("Player was logged in succesfully");
                debugText.text = "Logged in! Game loading...";
            }
            LootLockerSDKManager.StartWhiteLabelSession((response) =>
            {
                if (!response.success)
                {
                    Debug.Log("error starting LootLocker session");
                    debugText.text = "Login Failed";
                    return;
                }
                else
                {
                    Debug.Log("session started successfully");
                    debugText.text = "Session started...";
                    SceneManager.LoadScene("MainLevelSelect");
                }
            });
        });
    }

    // Called when pressing "CREATE" on new user screen
    public void NewUser()
    {
        string email = registerEmailField.text;
        string password = registerPassField.text;
        string newNickName = registerUsernameField.text;

        LootLockerSDKManager.WhiteLabelSignUp(email, password, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while signin up");
                debugText.text = "Signup Failed";
                return;
            }
            else
            {
                // Succesful response
                // Log in player to set name
                // Login the player
                LootLockerSDKManager.WhiteLabelLogin(email, password, false, response =>
                {
                    if (!response.success)
                    {
                        Debug.Log("error while whitelabelsignup to whielabellogin");
                        debugText.text = "Signup Failed";
                        return;
                    }
                    // Start session
                    LootLockerSDKManager.StartWhiteLabelSession((response) =>
                    {
                        if (!response.success)
                        {
                            Debug.Log("error while whitelabelsignup to whielabellogin to startwhitelabelsession");
                            return;
                        }
                        // Set nickname to be public UID if nothing was provided
                        if (newNickName == "")
                        {
                            newNickName = response.public_uid;
                        }
                        // Set new nickname for player
                        LootLockerSDKManager.SetPlayerName(newNickName, (response) =>
                        {
                            if (!response.success)
                            {
                                Debug.Log("error while whitelabelsignup to whielabellogin to startwhitelabelsession to setplayername");
                                return;
                            }

                            // End this session
                            LootLockerSessionRequest sessionRequest = new LootLockerSessionRequest();
                            LootLocker.LootLockerAPIManager.EndSession(sessionRequest, (response) =>
                            {
                                if (!response.success)
                                {
                                    Debug.Log("error while whitelabelsignup to whielabellogin to startwhitelabelsession to setplayername to endsession");
                                    return;
                                }
                                Debug.Log("Account Created");

                            debugText.text = "Account created successfully! Please go to Login page...";
                            });
                        });
                    });
                });
            }
        });
    }

}
