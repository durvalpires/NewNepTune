using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;

namespace _App_v2.Scripts._Core.Firebase.Auth
{
    public class AuthServicesFactory
    {
        public IAuthService Create()
        {
#if UNITY_WEBGL
            return new WebGLAuthService();
#else
            return new AuthService();
#endif
        }
    }
}
