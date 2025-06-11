using System;
using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine.Scripting;
#if !UNITY_WEBGL
using AuthError = Firebase.Auth.AuthError;
using Firebase.Auth;
#endif

namespace _App_v2.Scripts._Core.Firebase.Auth.Interfaces
{
    [Preserve]
    public interface IAuthService
    {
        [Preserve]
        event Action OnAuthSuccess;
        #if !UNITY_WEBGL
        event Action<AuthError> OnAuthFailed;
        #else
        [Preserve]
        event Action<AuthError> OnAuthFailed;
        #endif

        [Preserve]
        event Action OnAuthStateChanged;
        
        [Preserve]
        bool SignedIn { [Preserve] get; }

        [Preserve]
        string ProviderId { get; }

        [Preserve]
        void LogIn(AuthMethod method, string email = null, string password = null);
        [Preserve]
        void LogOut();
        [Preserve]
        void SignIn(AuthMethod method, string username = null, string email = null, string password = null);
        [Preserve]
        string GetUserId();
    }
}