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
 * Now, follow https://firebase.google.com/docs/database/web/read-and-write
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Database {
        public class ReadAndWriteData : FirebaseExample {

            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                //https://firebase.google.com/docs/database/web/read-and-write#web_value_events
                var starCountRef = Db.Ref(db, "users/testUser/starCount");
                Db.onValue(starCountRef, snapshot => {
                    var data = snapshot.val();
                    Debug.Log("==ONLY ONCE ONVALUE==");
                    Debug.Log($"updated star count to {data}");
                }, new ListenOptions() { onlyOnce = true });//https://firebase.google.com/docs/database/web/read-and-write#read_data_once_with_an_observer

                var unsub = Db.onValue(starCountRef, snapshot => {
                    var data = snapshot.val();
                    Debug.Log($"updated star count to {data}");
                });//https://firebase.google.com/docs/database/web/read-and-write#read_data_once_with_an_observer

                //call unsub() at some point to detach listener.
                //The off method was not implemented as it's not the recommended way to detach listeners. See readme.

                //https://firebase.google.com/docs/database/web/read-and-write#basic_write
                try {
                    await Db.set(Db.Ref(db, "users/testUser"), new Dictionary<string, object>() {
                        ["username"] = "somename",
                        ["email"] = "someemail@someprovider.com",
                        ["starCount"] = 0,
                    });
                } catch(Util.FirebaseError e) {
                    Debug.LogError($"Could not set: {e}");
                }

                //https://firebase.google.com/docs/database/web/read-and-write#read_data_once_with_get
                try {
                    var dbRef = Db.Ref(Db.getDatabase());
                    var snapshot = await Db.get(Db.child(dbRef, "users/testUser"));
                    if (snapshot.exists()) {
                        Debug.Log("==TEST USER DATA==");
                        Debug.Log(snapshot.val());
                    } else {
                        Debug.Log("No data available");
                    }
                } catch(Util.FirebaseError e) {
                    Debug.LogError($"Could not get: {e}");
                }

                //https://firebase.google.com/docs/database/web/read-and-write#updating_or_deleting_data
                try {
                    await Db.update(Db.Ref(db, "users/testUser"), new Dictionary<string, object>() { ["starCount"] = 10 });
                } catch (Util.FirebaseError e) {
                    Debug.LogError($"Could not update: {e}");
                }

                //https://firebase.google.com/docs/database/web/read-and-write#atomic_server-side_increments
                try {
                    await Db.update(Db.Ref(db, "users/testUser"), new Dictionary<string, object>() { ["starCount"] = Db.increment(1) });
                } catch (Util.FirebaseError e) {
                    Debug.LogError($"Could not update with increment: {e}");
                }

                //https://firebase.google.com/docs/database/web/read-and-write#save_data_as_transactions
                var post = await Db.runTransaction<Dictionary<string, object>>(Db.Ref(db, "users/testUser"), post => {
                    if (post is not null) {
                        int starCount = (int)(long)post["starCount"];
                        starCount++;
                        post["starCount"] = starCount;
                        return post;
                    } else {
                        return null;
                    }
                });

                Debug.Log($"Used transaction to update starCount. Result: {post}");
            }

        }
    }
}
