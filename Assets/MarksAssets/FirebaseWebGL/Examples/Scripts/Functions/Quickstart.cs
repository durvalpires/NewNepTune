using System.Collections.Generic;
using MarksAssets.FirebaseWebGL.Util;
using UnityEngine;
using Fu = MarksAssets.FirebaseWebGL.Functions.Functions;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Functions product on your firebase console or emulator.
 * Make sure you have 'functions' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check the result and if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/functions/get-started?gen=2nd#create-a-firebase-project to set up your environment.
 * The function on this example just adds 2 numbers, "a", and "b" and returns the result. Place the contents below in the index.js file and deploy the function.
 * 
 * const functions = require("firebase-functions");
 * const admin = require("firebase-admin");
 * admin.initializeApp();
 * exports.simpleCallable = functions.https.onCall((data, ctx) => {
 * const sum = data.a + data.b;
 * return sum;
 * });
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Functions {
        public class Quickstart : FirebaseExample {

            public int a = 1, b = 1;

            async void Start() {
                var functions = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (functions is null) return;

                try {
                    var callable = Fu.httpsCallable(functions, "simpleCallable");
                    var res = await callable(new Dictionary<string, object>() { ["a"] = a, ["b"] = b });
                    Debug.Log("==RESULT==");
                    Debug.Log(res.data);
                } catch(FirebaseError e) {
                    Debug.LogError($"functions error: {e}");
                }

            }
        }
    }

    
}
