using System;
using MarksAssets.FirebaseWebGL.Auth;

namespace _App_v2.Scripts._Core.Firebase.WebGL
{
    public struct WebGLUserProfile : UserProfile
    {
        public string DisplayName { get; set; }
        public Uri PhotoUrl { get; set; }
        public string Uid { get; set; }
    }
}