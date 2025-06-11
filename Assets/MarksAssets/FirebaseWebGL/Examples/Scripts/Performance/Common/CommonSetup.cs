
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using Pe = MarksAssets.FirebaseWebGL.Performance.Performance;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Util;
using MarksAssets.FirebaseWebGL.Performance;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Performance {
        public static class CommonSetup {
            public async static Task<FirebasePerformance> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var performance = Pe.getPerformance(app);
                    return performance;
                } catch(FirebaseError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set performance to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
