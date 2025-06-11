#if !UNITY_WEBGL
using System;
using System.Threading.Tasks;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using EasyUI.Popup;
using UnityEngine;
using Firebase;
using Firebase.Auth;

namespace _App_v2.Scripts._Core.Firebase.Auth.Handlers
{
    internal class PasswordAuthHandler : IAuthHandler
    {
        private readonly FirebaseAuth _firebaseAuth;

        private readonly string _email;
        private readonly string _password;
        private readonly string _username;

        private event Action OnAuthSuccess;

        event Action IAuthHandler.OnAuthSuccess
        {
            add => this.OnAuthSuccess += value;
            remove => this.OnAuthSuccess -= value;
        }

        public event Action<AuthError> OnAuthFailed;
        
        
        public PasswordAuthHandler(FirebaseAuth firebaseAuth, string email, string password, string username)
        {
            _firebaseAuth = firebaseAuth;
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
            _firebaseAuth.SignOut();
        }

        private void PerformSignIn()
        {
            _firebaseAuth.CreateUserWithEmailAndPasswordAsync(_email, _password).ContinueWith(HandleUserSignIn);
        }
        
        private void PerformLogIn()
        {
            var credential = EmailAuthProvider.GetCredential(_email, _password);

            if (!credential.IsValid())
            {
                Debug.LogError("Invalid email or password");
                return;
            }
            
            _firebaseAuth.SignInAndRetrieveDataWithCredentialAsync(credential).ContinueWith(HandleCredentialLogIn);
        }

        private void HandleCredentialLogIn(Task<AuthResult> task)
        {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithCredentialAsync was canceled.");
                OnAuthFailed?.Invoke(AuthError.Cancelled);
                return;
            }

            if (task.IsFaulted)
            {
                if (task.Exception?.InnerException is FirebaseException firebaseException)
                {
                    Debug.LogError($"SignInWithCredentialAsync encountered an error: {firebaseException.Message}");
                    var errorCode = (AuthError)firebaseException.ErrorCode;
                    OnAuthFailed?.Invoke(errorCode);
                    return;
                }

                Debug.LogException(task.Exception);
                OnAuthFailed?.Invoke(AuthError.Failure);
                return;
            }

            var result = task.Result;
            Debug.LogFormat("User signed in successfully: {0} ({1})", result.User.DisplayName, result.User.UserId);
            OnAuthSuccess?.Invoke();
        }
        

        private void HandleUserSignIn(Task<AuthResult> task)
        {
            if (task.IsCanceled) {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                OnAuthFailed?.Invoke(AuthError.Cancelled);
                return;
            }
            
            if (task.IsFaulted) {
                if (task.Exception?.InnerException is FirebaseException firebaseException)
                {
                    Debug.LogError($"CreateUserWithEmailAndPasswordAsync encountered an error: {firebaseException.Message}");
                    var errorCode = (AuthError)firebaseException.ErrorCode;
                    OnAuthFailed?.Invoke(errorCode);
                    return;
                }

                Debug.LogException(task.Exception);
                OnAuthFailed?.Invoke(AuthError.Failure);
                return;
            }

            // Firebase user has been created.
            var result = task.Result;

            Debug.Log($"Firebase user created successfully: {result.User.UserId}");
            result.User.UpdateUserProfileAsync(new UserProfile() {DisplayName = _username}).ContinueWith(HandleUserProfileUpdate);
        }

        private void HandleUserProfileUpdate(Task task)
        {
            if (task.IsCanceled) {
                Debug.LogError("UpdateUserProfileAsync was canceled.");
                OnAuthFailed?.Invoke(AuthError.Cancelled);
                return;
            }
            
            if (task.IsFaulted) {
                if (task.Exception?.InnerException is FirebaseException firebaseException)
                {
                    Debug.LogError($"UpdateUserProfileAsync encountered an error: {firebaseException.Message}");
                    var errorCode = (AuthError)firebaseException.ErrorCode;
                    OnAuthFailed?.Invoke(errorCode);
                    return;
                }

                Debug.LogException(task.Exception);
                OnAuthFailed?.Invoke(AuthError.Failure);
                return;
            }
            
            Debug.Log($"User profile updated successfully. User name {_firebaseAuth.CurrentUser.DisplayName}");
            OnAuthSuccess?.Invoke();
        }
    }
}
#endif