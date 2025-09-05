
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using St = MarksAssets.FirebaseWebGL.Storage.Storage;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Util;
using MarksAssets.FirebaseWebGL.Storage;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Storage {
        public static class CommonSetup {
            public async static Task<FirebaseStorage> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var storage = St.getStorage(app);
                    if (emulatorConfig is not null && emulatorConfig.useEmulator is true) St.connectStorageEmulator(storage, emulatorConfig.host, emulatorConfig.port);
                    return storage;
                } catch(FirebaseError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set storage to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
