using UnityEngine;
using In = MarksAssets.FirebaseWebGL.Installations.Installations;

/*
 * == BEFORE YOU BEGIN ==
 * Make sure you have 'installations' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Follow https://firebase.google.com/docs/projects/manage-installations
 * I couldn't find any documentation related to installations for the web.
 * I created a github discussion thread for this https://github.com/firebase/firebase-js-sdk/discussions/7892 .
 * Leave your browser's javascript console (web inspector) open to check the results and if there are any errors in the process.
 * This example does NOT support the emulator. Installations is not on the supported list. See https://firebase.google.com/docs/emulator-suite#feature-matrix .
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Installations {
        public class Quickstart : FirebaseExample {

            async void Start() {
                var installations = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (installations is null) return;

                var id = await In.getId(installations);
                Debug.Log("installations id: " + id);

                var token = await In.getToken(installations);
                Debug.Log("installations token: " + id);

                var unsub = In.onIdChange(installations, id => Debug.Log("new id: " + id));
            }
        }

    }

    
}
