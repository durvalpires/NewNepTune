using MarksAssets.FirebaseWebGL.Util;
using UnityEngine;
using Rc = MarksAssets.FirebaseWebGL.RemoteConfig.RemoteConfig;
using System.Collections.Generic;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the RemoteConfig product on your firebase console.
 * Make sure you have 'remoteConfig' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Follow https://firebase.google.com/docs/remote-config/get-started?platform=web .
 * Instead of the 'welcome_message' parameter, I use the 'greeting' parameter, based on this video https://www.youtube.com/watch?v=0DBRiMWy28Y
 * Leave your browser's javascript console (web inspector) open to check the results and if there are any errors in the process.
 * This example does NOT support the emulator. RemoteConfig is not on the supported list. See https://firebase.google.com/docs/emulator-suite#feature-matrix .
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace RemoteConfig {
        public class Quickstart : FirebaseExample {

            public int minimumFetchIntervalMillis = 0;//default is 43200000 (12h) . But for debugging purposes, set to 0

            async void Start() {
                var remoteConfig = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (remoteConfig is null) return;

                remoteConfig.settings = new FirebaseWebGL.RemoteConfig.RemoteConfigSettings(60000, minimumFetchIntervalMillis);//https://firebase.google.com/docs/remote-config/get-started?platform=web#minimum-fetch
                remoteConfig.defaultConfig = new Dictionary<string, object>() { ["greeting"] = "In-App Default Hello!" };//https://firebase.google.com/docs/remote-config/get-started?platform=web#default-parameter-in-app

                bool isFetched;
                try {
                    isFetched = await Rc.fetchAndActivate(remoteConfig);//https://firebase.google.com/docs/remote-config/get-started?platform=web#fetch-values
                } catch(FirebaseError e) {
                    Debug.LogError($"Failed to call fetch and activate: {e}");
                    return;
                }

                if (isFetched) {
                    Debug.Log("Fetched configs successfully activated.");
                } else {
                    Debug.Log($"fetched configs were already activated!");
                }

                var greeting = Rc.getValue(remoteConfig, "greeting");//https://firebase.google.com/docs/remote-config/get-started?platform=web#get-parameter

                Debug.Log($"GREETING VALUE: {greeting}");

            }
        }
    }

    
}
