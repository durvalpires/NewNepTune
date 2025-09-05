using UnityEngine;

namespace MarksAssets.FirebaseWebGL.Examples {
    //https://firebase.google.com/docs/web/learn-more#config-object
    public class FirebaseConfigObject : ScriptableObject {
        public string apiKey;
        public string authDomain;
        public string projectId;
        public string storageBucket;
        public string messagingSenderId;
        public string appId;
        public string databaseURL;
    }
}
