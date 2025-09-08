#if !UNITY_WEBGL
using System;
using System.Linq;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using Cysharp.Threading.Tasks;
using Firebase.Auth;
using UnityEngine;

namespace _App_v2.Scripts._Core.Firebase.Auth
{
    public class AuthService : IAuthService
    {
        private readonly FirebaseAuth _auth;
        private FirebaseUser _user;
        
        private readonly AuthHandlersFactory _authHandlersFactory;
        private IAuthHandler _authHandler;
        
        private bool _isSignedIn;

        public event Action OnAuthSuccess;
        
        public event Action<AuthError> OnAuthFailed;

        public event Action OnAuthStateChanged;
        bool IAuthService.SignedIn => _isSignedIn;
        
        string IAuthService.ProviderId => _user?.ProviderData?.FirstOrDefault(x=>x != null)?.ProviderId;

        public AuthService()
        {
            _auth = FirebaseAuth.DefaultInstance;
            _auth.StateChanged += AuthStateChanged;
            _user = _auth.CurrentUser;

            _authHandlersFactory = new AuthHandlersFactory();
        }
        
        void IAuthService.SignIn(AuthMethod method, string username, string email, string password)
        {
            //currently supports only email and password method
            if (method != AuthMethod.EmailAndPassword)
            {
                Debug.LogError($"Method {method} not supported");
                return;
            }

            if (_isSignedIn)
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
            _authHandler.SignIn();
        }

        string IAuthService.GetUserId()
        {
            if (_isSignedIn) 
                #if !UNITY_WEBGL
                return _user.UserId;
                #else
                return null;
                #endif

            
            Debug.LogError("User is not signed in");
            return null;

        }

        void IAuthService.LogIn(AuthMethod method, string email, string password)
        {
            if (_isSignedIn)
            {
                Debug.Log("Already signed in");
                return;
            }
            
            ClearAuthHandler();
            
            CreateAuthHandler(method, email, password);
            _authHandler.LogIn();
        }

        void IAuthService.LogOut()
        {
            if (!_isSignedIn)
            {
                Debug.Log("Already signed out");
                return;
            }

            if (_authHandler == null)
            {
                var method = GetAuthMethod();
                if (method == AuthMethod.Unknown)
                {
                    Debug.LogError("Unknown auth method");
                    return;
                }

                CreateAuthHandler(method, _user.Email, null);
            }
            
            _authHandler?.LogOut();
        }
        
        private void ClearAuthHandler()
        {
            if (_authHandler == null) 
                return;
            
            _authHandler.OnAuthSuccess -= HandleAuthSuccess;
            _authHandler.OnAuthFailed -= HandleAuthFailed;

            _authHandler = null;
        }
        
        private void CreateAuthHandler(AuthMethod method, string email, string password, string username = null)
        {
            _authHandler = _authHandlersFactory.Create(method, email, password, username);
            _authHandler.OnAuthSuccess += HandleAuthSuccess;
            _authHandler.OnAuthFailed += HandleAuthFailed;

        }

        private AuthMethod GetAuthMethod()
        {
            foreach (var info in _user.ProviderData)
            {
                if (info == null) 
                    continue;

                switch (info.ProviderId)
                {
                    case "password":
                        return AuthMethod.EmailAndPassword;
                }
            }

            return AuthMethod.Unknown;
        }

        private void AuthStateChanged(object sender, System.EventArgs e)
        {
            _isSignedIn = _auth.CurrentUser != null;

            if (!_isSignedIn)
            {
                Debug.Log("User is signed out");
            }
            _user = _auth.CurrentUser;
            if (_isSignedIn)
            {
                Debug.Log($"Signed in {_user.UserId}");
            }

            OnAuthStateChanged?.Invoke();
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
}
#endif