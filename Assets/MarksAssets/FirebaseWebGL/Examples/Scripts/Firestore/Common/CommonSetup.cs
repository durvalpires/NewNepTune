
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using MarksAssets.FirebaseWebGL.Firestore;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using Db = MarksAssets.FirebaseWebGL.Firestore.Firestore;
using System.Threading.Tasks;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Firestore {
        public static class CommonSetup {

            public async static Task<Db> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var db = Db.getFirestore(app);
                    if (emulatorConfig is not null && emulatorConfig.useEmulator is true) Db.connectFirestoreEmulator(db, emulatorConfig.host, emulatorConfig.port);
                    return db;
                } catch(FirestoreError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set firestore to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
