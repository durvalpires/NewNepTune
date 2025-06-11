#if UNITY_WEBGL
using System;
using _App_v2.Scripts._Core.Firebase;
using _App_v2.Scripts._Core.Firebase.Auth;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using Cysharp.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.Scripting;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

[Preserve]
public class WebGLAuthService : IAuthService
{
    private Au auth;
    private IAuthHandler authHandler;
    private readonly AuthHandlersFactory authHandlersFactory;
    
    public User User => auth.currentUser;

    public event Action OnAuthSuccess;
    public event Action<AuthError> OnAuthFailed;
    public event Action OnAuthStateChanged;

    [Preserve]
    public bool SignedIn { get; private set; }
    public string ProviderId { get; }

    public WebGLAuthService()
    {
        authHandlersFactory = new AuthHandlersFactory();
        //SignedIn = false;
        
        InitAsync();
    }
    
    private async void InitAsync() 
    {
        try
        {
            Debug.LogWarning("INIT ASYING WEBGL AUTH");
            auth = Au.getAuth(FireService.Instance.WebGLFirebaseApp);
            Debug.LogWarning("AFTER GETAUTH WEBGL AUTH");
            
            Au.onAuthStateChanged(auth, user => {
                if (user is not null) {
                    SignedIn = true;
                    Debug.Log($"user {user.email} with uid {user.uid} signed in");
                    PlayerModelBase.UpdateUserId(user.uid);
                } else
                {
                    SignedIn = false;
                    Debug.Log("user signed out");
                }
                
                OnAuthStateChanged?.Invoke();
            });
            
            
        }
        catch (AuthError e)
        {
            Debug.LogError("WebGL Auth initialization failure: " + e.Message);
        }
    }
    
    public async void LogIn(AuthMethod method, string email = null, string password = null)
    {
        if (SignedIn)
        {
            Debug.Log("Already signed in");
            return;
        }
        
        try {
            await Au.signInWithEmailAndPassword(auth, email, password);
        } catch(AuthError e) {
            Debug.LogError($"{e.code} - {e.Message}");
        }
    }

    public void LogOut()
    {
        if (!SignedIn)
        {
            Debug.Log("Already signed out");
            return;
        }

        if (authHandler == null)
        {
            var method = GetAuthMethod();
            if (method == AuthMethod.Unknown)
            {
                Debug.LogError("Unknown auth method");
                return;
            }
            
            CreateAuthHandler(method, User.email, null);
        }
            
        authHandler?.LogOut();
    }
    
    private AuthMethod GetAuthMethod()
    {
        foreach (var info in User.providerData)
        {
            if (info == null) 
                continue;

            switch (info.providerId)
            {
                case "password":
                    return AuthMethod.EmailAndPassword;
            }
        }
            
        return AuthMethod.Unknown;
    }

    public void SignIn(AuthMethod method, string username = null, string email = null, string password = null)
    {
        //currently supports only email and password method
        if (method != AuthMethod.EmailAndPassword)
        {
            Debug.LogError($"Method {method} not supported");
            return;
        }
        
        if (SignedIn)
        {
            Debug.Log("Already signed in. You need to sign out first");
            return;
        }
        
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
        {
            Debug.LogError("Email, password or username is empty");
            return;
        }
        
        ClearAuthHandler();
        CreateAuthHandler(method, email, password, username);
        authHandler.SignIn();
    }

    public string GetUserId()
    {
        if (SignedIn) 
            return User.uid;

            
        Debug.LogError("User is not signed in");
        return null;
    }

    private void ClearAuthHandler()
    {
        if (authHandler == null) 
            return;
            
        authHandler.OnAuthSuccess -= HandleAuthSuccess;
        authHandler.OnAuthFailed -= HandleAuthFailed;

        authHandler = null;
    }
    
    private void CreateAuthHandler(AuthMethod method, string email, string password, string username = null)
    {
        authHandler = authHandlersFactory.Create(method, email, password, username);
        authHandler.OnAuthSuccess += HandleAuthSuccess;
        authHandler.OnAuthFailed += HandleAuthFailed;
    }

    private async void HandleAuthSuccess()
    {
        await UniTask.SwitchToMainThread();
        OnAuthSuccess?.Invoke();
    }

    private async void HandleAuthFailed(AuthError error)
    {
        await UniTask.SwitchToMainThread();
        OnAuthFailed?.Invoke(error);
    }
}
#endif