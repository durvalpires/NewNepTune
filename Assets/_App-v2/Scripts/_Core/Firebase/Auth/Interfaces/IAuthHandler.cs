using System;

#if !UNITY_WEBGL
using AuthError = Firebase.Auth.AuthError;
using Firebase.Auth;
#else
using MarksAssets.FirebaseWebGL.Auth;
#endif

namespace _App_v2.Scripts._Core.Firebase.Auth.Interfaces
{
    public interface IAuthHandler
    {
        event Action OnAuthSuccess;
        #if !UNITY_WEBGL
        event Action<AuthError> OnAuthFailed;
        #else
        event Action<AuthError> OnAuthFailed;
        #endif


        void SignIn();
        void LogIn();
        void LogOut();
    }
}