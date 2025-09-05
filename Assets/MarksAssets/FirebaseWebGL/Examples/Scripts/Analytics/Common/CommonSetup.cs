
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using An = MarksAssets.FirebaseWebGL.Analytics.Analytics;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Util;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Analytics {
        public static class CommonSetup {
            public async static Task<An> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var analytics = An.getAnalytics(app);
                    return analytics;
                } catch(FirebaseError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set analytics to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
