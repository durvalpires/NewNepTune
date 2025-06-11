using System.Collections.Generic;
using MarksAssets.FirebaseWebGL.Firestore;
using UnityEngine;
using Db = MarksAssets.FirebaseWebGL.Firestore.Firestore;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Firestore product on your firebase console or emulator.
 * Make sure the security rules allow all reads and writes for testing purposes.
 * Make sure you cleared all the data for a clean state for testing.
 * Make sure you have 'firestore' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/firestore/quickstart
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Firestore {

        public class Quickstart : FirebaseExample {
            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                try {
                    var docRef = await Db.addDoc(Db.collection(db, "users"), new Dictionary<string, object>() { ["first"] = "Ada", ["last"] = "Lovelace", ["born"] = 1815 });
                    Debug.Log("Document written with ID: " + docRef.id);
                } catch (FirestoreError e) {
                    Debug.LogError("Error adding document: " + e);
                }

                var querySnapshot = await Db.getDocs(Db.collection(db, "users"));

                foreach (QueryDocumentSnapshot doc in querySnapshot) {
                    Debug.Log($"{doc.id} => {doc.data<object>()}");
                }

            }

        }
    }
}
