
using MarksAssets.FirebaseWebGL.App;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using Db = MarksAssets.FirebaseWebGL.Database.Database;
using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.Util;

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Database {
        public static class CommonSetup {

            public async static Task<Db> setup(FirebaseConfigObject firebaseConfig, EmulatorConfigObject emulatorConfig) {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    var db = Db.getDatabase(app);
                    if (emulatorConfig is not null && emulatorConfig.useEmulator is true) Db.connectDatabaseEmulator(db, emulatorConfig.host, emulatorConfig.port);
                    return db;
                } catch(FirebaseError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set database to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                    return null;
                }
            }

        }
    }
}
