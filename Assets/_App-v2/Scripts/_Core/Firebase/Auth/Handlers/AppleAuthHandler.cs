using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using AppleAuth;
using AppleAuth.Enums;
using AppleAuth.Interfaces;
using AppleAuth.Native;

#if !UNITY_WEBGL
using Firebase.Auth;
using Firebase.Extensions;
using FirebaseAuthSDK = Firebase.Auth;
#else
using MarksAssets.FirebaseWebGL.Auth;
#endif
using UnityEngine;


namespace _App_v2.Scripts._Core.Firebase.Auth.Handlers
{
    internal class AppleAuthHandler : IAuthHandler
    {
        private const string PROVIDER_ID = "apple.com";
        private const int NONCE_LENGTH = 32;
        private IAppleAuthManager appleAuthManager;
        #if !UNITY_WEBGL
        private readonly FirebaseAuth _firebaseAuth;
        #endif


        public event Action OnAuthSuccess;
#if !UNITY_WEBGL
        public event Action<AuthError> OnAuthFailed;
#else
        public event Action<AuthError> OnAuthFailed;
#endif


        #if !UNITY_WEBGL
        public AppleAuthHandler(FirebaseAuth firebaseAuth)
        {
            _firebaseAuth = firebaseAuth;

            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                appleAuthManager = new AppleAuthManager(new PayloadDeserializer());
            }
            else
            {
                Debug.LogError($"Apple Auth is not supported on this platform: {Application.platform}");
            }
        }
        #else
        public AppleAuthHandler()
        {
            if (AppleAuthManager.IsCurrentPlatformSupported)
            {
                appleAuthManager = new AppleAuthManager(new PayloadDeserializer());
            }
            else
            {
                Debug.LogError($"Apple Auth is not supported on this platform: {Application.platform}");
            }
        }
        #endif
        


        public void SignIn()
        {
            throw new NotSupportedException("Apple Auth does not support sign in. Use LogIn instead");
        }

        public void LogIn()
        {
            if (appleAuthManager == null)
            {
                Debug.LogError("Apple Auth is not supported on this platform");
                #if !UNITY_WEBGL
                OnAuthFailed?.Invoke(AuthError.Unimplemented);
                #endif

                return;
            }

            PerformQuickLoginWithFirebase();
        }

        public void LogOut()
        {
            #if !UNITY_WEBGL
            _firebaseAuth.SignOut();
            #endif

        }

        private void PerformQuickLoginWithFirebase()
        {
            if (appleAuthManager == null)
            {
                Debug.LogError("AppleAuthManager is not initialized");
                #if !UNITY_WEBGL
                OnAuthFailed?.Invoke(AuthError.Unimplemented);
                #endif

                return;
            }

            var rawNonce = GenerateRandomString();
            var nonce = GenerateSHA256NonceFromRawNonce(rawNonce);

            var loginArgs = new AppleAuthLoginArgs(LoginOptions.IncludeEmail | LoginOptions.IncludeFullName, nonce);

            appleAuthManager.LoginWithAppleId(
                loginArgs,
                credential =>
                {
                    if (credential is IAppleIDCredential appleIdCredential)
                    {
                        PerformFirebaseAuthentication(appleIdCredential, rawNonce);
                    }
                },
                error =>
                {
                    Debug.LogError("AppleAuthManager is not initialized");
                    #if !UNITY_WEBGL
                    OnAuthFailed?.Invoke(AuthError.Failure);
                    #endif

                });
        }

        private void PerformFirebaseAuthentication(IAppleIDCredential appleIdCredential, string rawNonce)
        {
            if (appleIdCredential.IdentityToken == null)
            {
                Debug.LogError("Identity Token is null");
                #if !UNITY_WEBGL
                OnAuthFailed?.Invoke(AuthError.Failure);
                #endif

                return;
            }

            var identityToken = Encoding.UTF8.GetString(appleIdCredential.IdentityToken);
            #if !UNITY_WEBGL
            var firebaseCredential = FirebaseAuthSDK.OAuthProvider.GetCredential("apple.com", identityToken, rawNonce, null);


            FirebaseAuthSDK.FirebaseAuth auth = FirebaseAuthSDK.FirebaseAuth.DefaultInstance;
            auth.CurrentUser.LinkWithCredentialAsync(firebaseCredential)
                .ContinueWithOnMainThread(task =>
                {
                    if (task.IsCanceled)
                    {
                        Debug.Log("Firebase auth was canceled");
                        OnAuthFailed?.Invoke(AuthError.Cancelled);
                    }
                    else if (task.IsFaulted)
                    {
                        Debug.LogError($"Firebase auth error: {task.Exception}");

                    
                        if (task.Exception != null)
                        {
                            foreach (Exception exception in task.Exception.Flatten().InnerExceptions)
                            {
                                FirebaseAuthSDK.FirebaseAccountLinkException firebaseEx =
                                    exception as FirebaseAuthSDK.FirebaseAccountLinkException;

                                if (firebaseEx != null && firebaseEx.UserInfo.UpdatedCredential.IsValid())
                                {
                                    // Attempt to sign in with the updated credential
                                    auth.SignInAndRetrieveDataWithCredentialAsync(firebaseEx.UserInfo.UpdatedCredential)
                                        .ContinueWithOnMainThread(authResultTask =>
                                        {
                                            if (authResultTask.IsCanceled)
                                            {
                                                Debug.LogError("Sign in was canceled.");
                                                OnAuthFailed?.Invoke(AuthError.Cancelled);
                                            }
                                            else if (authResultTask.IsFaulted)
                                            {
                                                Debug.LogError($"Sign in failed: {authResultTask.Exception}");
                                                OnAuthFailed?.Invoke(AuthError.Failure);
                                            }
                                            else
                                            {
                                                OnAuthSuccess?.Invoke();
                                            }
                                        });
                                }
                                else
                                {
                                    Debug.LogError("Link with Apple failed: " + firebaseEx);
                                    OnAuthFailed?.Invoke(AuthError.Failure);
                                }
                            }
                        }
                    }
                    else
                    {
                        // Successfully linked with the credential
                        Debug.Log("Account linked successfully.");
                        OnAuthSuccess?.Invoke();
                    }
                });
            #endif

        }

        private string GenerateRandomString()
        {
            const string charset = "0123456789ABCDEFGHIJKLMNOPQRSTUVXYZabcdefghijklmnopqrstuvwxyz-._";
            var result = new StringBuilder(NONCE_LENGTH);
            using (var rng = new RNGCryptoServiceProvider())
            {
                var buffer = new byte[NONCE_LENGTH];
                rng.GetBytes(buffer);
                foreach (var b in buffer)
                {
                    result.Append(charset[b % charset.Length]);
                }
            }
            return result.ToString();
        }

        private string GenerateSHA256NonceFromRawNonce(string rawNonce)
        {
            using (var sha = new SHA256Managed())
            {
                var utf8RawNonce = Encoding.UTF8.GetBytes(rawNonce);
                var hash = sha.ComputeHash(utf8RawNonce);
                return BitConverter.ToString(hash).Replace("-", "").ToLower();
            }
        }
    }
}