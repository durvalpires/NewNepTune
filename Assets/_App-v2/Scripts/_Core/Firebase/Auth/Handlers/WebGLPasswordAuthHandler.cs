#if UNITY_WEBGL
using System;
using System.Threading.Tasks;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using _App_v2.Scripts._Core.Firebase.WebGL;
using EasyUI.Popup;
using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;


namespace _App_v2.Scripts._Core.Firebase.Auth.Handlers
{
    internal class WebGLPasswordAuthHandler : IAuthHandler
    {
        private Au auth;

        private readonly string _email;
        private readonly string _password;
        private readonly string _username;
        
        public event Action OnAuthSuccess;
        public event Action<AuthError> OnAuthFailed;
        
        
        public WebGLPasswordAuthHandler(Au firebaseAuth, string email, string password, string username)
        {
            auth = firebaseAuth;
            _email = email;
            _password = password;
            _username = username;
        }


        public void SignIn()
        {
            PerformSignIn();
        }

        void IAuthHandler.LogIn()
        {
            PerformLogIn();
        }

        void IAuthHandler.LogOut()
        {
            PerformLogOut();

        }

        private async void PerformLogOut()
        {
            try {
                await Au.signOut(auth);
            } catch(AuthError e) {
                Debug.LogError($"{e.code} - {e.Message}");
                OnAuthFailed?.Invoke(e);
            }
        }

        private async void PerformSignIn()
        {
            try {
                await Au.createUserWithEmailAndPassword(auth, _email, _password).ContinueWith(HandleUserSignIn);
            } catch(AuthError e) {
                Debug.LogError($"{e.code} - {e.Message}");
                OnAuthFailed?.Invoke(e);
            }
        }
        
        private async void PerformLogIn()
        {
#if !UNITY_WEBGL
            var credential = EmailAuthProvider.GetCredential(_email, _password);

            if (!credential.IsValid())
            {
                Debug.LogError("Invalid email or password");
                return;
            }
            
            _firebaseAuth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(HandleCredentialLogIn);
#else
            try { 
                await Au.signInWithEmailAndPassword(auth, _email, _password)
                    .ContinueWith(HandleCredentialLogIn);
            } catch(AuthError e) {
                Debug.LogError($"{e.code} - {e.Message}");
                OnAuthFailed?.Invoke(e);
            }
#endif
        }


        private void HandleCredentialLogIn(Task<UserCredential> task)
        {
            var result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.user.displayName, result.user.uid);
            OnAuthSuccess?.Invoke();
        }
        
        
        private async void HandleUserSignIn(Task<UserCredential> task)
        {
            // Firebase user has been created.
            var result = task.Result;
            Debug.Log($"Firebase user created successfully: {result.user.uid}");
            
            try { 
                await Au.updateProfile(result.user, new WebGLUserProfile() { DisplayName = _username })
                    .ContinueWith(HandleUserProfileUpdate);
            } catch(AuthError e) {
                Debug.LogError($"{e.code} - {e.Message}");
                OnAuthFailed?.Invoke(e);
            }
            
        }
        
        private void HandleUserProfileUpdate(Task task)
        {
            Debug.Log($"User profile updated successfully. User name " +
                      $"{auth.currentUser.displayName}");
            OnAuthSuccess?.Invoke();
        }
    }
}
#endif