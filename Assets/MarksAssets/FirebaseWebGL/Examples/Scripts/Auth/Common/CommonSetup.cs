
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Auth;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {
        public static class CommonSetup {

            public async static Task<Au> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig, bool isHttps = false) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var auth = Au.getAuth(app);
                    if (emulatorConfig is not null && emulatorConfig.useEmulator is true) Au.connectAuthEmulator(auth, $"{(isHttps ? "https" : "http")}://{emulatorConfig.host}:{emulatorConfig.port}");
                    return auth;
                } catch(AuthError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set auth to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
