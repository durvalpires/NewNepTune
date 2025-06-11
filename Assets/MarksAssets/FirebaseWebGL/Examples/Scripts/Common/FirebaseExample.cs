using UnityEngine;


namespace MarksAssets.FirebaseWebGL.Examples {

    public class FirebaseExample : MonoBehaviour {
        public FirebaseConfigObject firebaseConfig;//always used
        public EmulatorConfigObject emulatorConfig;//not always used

        void Awake() {
            #if !(UNITY_WEBGL && !UNITY_EDITOR)//if not on a webgl build, disable script.
                enabled = false;
            #endif
        }

    }

}
