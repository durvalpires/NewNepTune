
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using Fu = MarksAssets.FirebaseWebGL.Functions.Functions;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Util;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Functions {
        public static class CommonSetup {
            public async static Task<Fu> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var functions = Fu.getFunctions(app);
                    if (emulatorConfig is not null && emulatorConfig.useEmulator is true) Fu.connectFunctionsEmulator(functions, emulatorConfig.host, emulatorConfig.port);
                    return functions;
                } catch(FirebaseError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set functions to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
