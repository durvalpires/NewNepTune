using System;
using _App_v2.Scripts._Core.Firebase.Auth.Handlers;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
#if !UNITY_WEBGL
using Firebase.Auth;
#else
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;
#endif

namespace _App_v2.Scripts._Core.Firebase.Auth
{
    public class AuthHandlersFactory
    {
        public IAuthHandler Create(AuthMethod method, string email = null, string password = null, string username = null)
        {
            return method switch
            {
#if !UNITY_WEBGL
                AuthMethod.EmailAndPassword => new PasswordAuthHandler(FirebaseAuth.DefaultInstance, email, password, username),
                AuthMethod.Apple => new AppleAuthHandler(FirebaseAuth.DefaultInstance),
#else
                AuthMethod.EmailAndPassword => new WebGLPasswordAuthHandler(
                    Au.getAuth(FireService.Instance.WebGLFirebaseApp), email, password, username),
#endif
                _ => throw new NotImplementedException("Unknown auth method: " + method)
            };
        }
    }
}