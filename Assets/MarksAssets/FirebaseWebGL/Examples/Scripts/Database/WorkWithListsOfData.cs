using System.Collections.Generic;
using MarksAssets.FirebaseWebGL.Database;
using UnityEngine;
using Db = MarksAssets.FirebaseWebGL.Database.Database;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Database product on your firebase console or emulator.
 * Make sure the security rules allow all reads and writes for testing purposes.
 * Make sure you cleared all the data for a clean state for testing.
 * Make sure you have 'database' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/database/web/lists-of-data
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Database {
        public class WorkWithListsOfData : FirebaseExample {

            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                //https://firebase.google.com/docs/database/web/lists-of-data#listen_for_child_events
                Db.onChildAdded(Db.Ref(db), (data, _) => {
                    Debug.Log("==ONCHILDADDED==");
                    Debug.Log(data.val());
                });

                Db.onChildChanged(Db.Ref(db), (data, _) => {
                    Debug.Log("==ONCHILDCHANGED==");
                    Debug.Log(data.val());
                });

                Db.onChildRemoved(Db.Ref(db), (data) => {
                    Debug.Log("==ONCHILDREMOVED==");
                    Debug.Log(data.val());
                });

                //https://firebase.google.com/docs/database/web/lists-of-data#listen_for_value_events
                Db.onValue(Db.Ref(db), (data) => {
                    Debug.Log("==ONVALUE==");
                    Debug.Log(data.val());
                });

                var posts = new Dictionary<string, object>() {
                    ["ts-functions"] = new Dictionary<string, object>() {
                        ["metrics"] = new Dictionary<string, object>() {
                            ["views"] = 1200000,
                            ["likes"] = 251000,
                            ["shares"] = 1200,
                        },
                        ["title"] = "Why you should use TypeScript for writing Cloud Functions",
                        ["author"] = "Doug"
                    },
                    ["android-arch-3"] = new Dictionary<string, object>() {
                        ["metrics"] = new Dictionary<string, object>() {
                            ["views"] = 900000,
                            ["likes"] = 117000,
                            ["shares"] = 144,
                        },
                        ["title"] = "Using Android Architecture Components with Firebase Realtime Database (Part 3)",
                        ["author"] = "Doug"
                    },
                };

                try {
                    await Db.set(Db.Ref(db, "posts"), posts);
                } catch (Util.FirebaseError e) {
                    Debug.LogError($"Could not set: {e}");
                }

                //https://firebase.google.com/docs/database/web/lists-of-data#sorting_and_filtering_data
                var mostViewedPosts = Db.query(Db.Ref(db, "posts"), Db.orderByChild("metrics/views"));

                Debug.Log("==Db.query(Db.Ref(db, \"posts\"), Db.orderByChild(\"metrics/views\"))==");
                DataSnapshot databaseSnapshot = await Db.get(mostViewedPosts);
                foreach (DataSnapshot iteratedDataSnapshot in databaseSnapshot) {
                    Debug.Log("exists: " + iteratedDataSnapshot.exists());
                    Debug.Log("hasChildren: " + iteratedDataSnapshot.hasChildren());
                    Debug.Log("val: " + iteratedDataSnapshot.val());
                    Debug.Log("ref: " + iteratedDataSnapshot.Ref);
                    Debug.Log("size: " + iteratedDataSnapshot.size);
                    Debug.Log("key: " + iteratedDataSnapshot.key);
                }
                mostViewedPosts.clear();//realtime database queries should be cleared after use. See readme.

                Debug.Log("");

                //https://firebase.google.com/docs/database/web/lists-of-data#limit_the_number_of_results
                mostViewedPosts = Db.query(Db.Ref(db, "posts"), Db.orderByChild("metrics/views"), Db.limitToFirst(1));
                Debug.Log("==Db.query(Db.Ref(db, \"posts\"), Db.orderByChild(\"metrics/views\"), Db.limitToFirst(1))==");
                databaseSnapshot = await Db.get(mostViewedPosts);
                foreach (DataSnapshot iteratedDataSnapshot in databaseSnapshot) {
                    Debug.Log("exists: " + iteratedDataSnapshot.exists());
                    Debug.Log("hasChildren: " + iteratedDataSnapshot.hasChildren());
                    Debug.Log("val: " + iteratedDataSnapshot.val());
                    Debug.Log("ref: " + iteratedDataSnapshot.Ref);
                    Debug.Log("size: " + iteratedDataSnapshot.size);
                    Debug.Log("key: " + iteratedDataSnapshot.key);
                }
                mostViewedPosts.clear();//realtime database queries should be cleared after use. See readme.
            }

        }
    }
}
