using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 


public class FirebaseAuthManager : MonoBehaviour
{
    
    public FirebaseAuth auth;
    public FirebaseUser user;
    //Unity UI LOGIN INPUTS
    
    public TMP_InputField emailField;
    public TMP_InputField passwordField;
    //REGISTER INPUTS
    public TMP_InputField registerEmailField;
    public TMP_InputField registerPasswordField;
    
    public TextMeshProUGUI debugText;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            FirebaseApp app = FirebaseApp.DefaultInstance;
            auth = FirebaseAuth.DefaultInstance;
        });
        InitializeFirebase();
    }


    public void InitializeFirebase()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void RegisterUser()
    {
       
        
        auth.CreateUserWithEmailAndPasswordAsync(registerEmailField.text, registerPasswordField.text).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("RegisterUser was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("RegisterUser encountered an error: " + task.Exception);
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.LogFormat("User signed up successfully: {0} ({1})", newUser.Email, newUser.UserId);
        });
    }
    
    public void LoginUser(string email, string password)
    {
        email = emailField.text;
        password = passwordField.text;
        
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("LoginUser was canceled.");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("LoginUser encountered an error: " + task.Exception);
                return;
            }

            AuthResult authResult = task.Result;
            FirebaseUser newUser = authResult.User;
            Debug.LogFormat("User signed in successfully: {0} ({1})", newUser.Email, newUser.UserId);
            SceneManager.LoadScene("MainLevelSelect");
        });
    }
}
