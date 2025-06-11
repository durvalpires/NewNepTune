using System.Collections.Generic;
using MarksAssets.FirebaseWebGL.Firestore;
using Newtonsoft.Json;
using UnityEngine;
using Db = MarksAssets.FirebaseWebGL.Firestore.Firestore;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Firestore product on your firebase console or emulator.
 * Make sure the security rules allow all reads and writes for testing purposes.
 * Make sure you cleared all the data for a clean state for testing.
 * Make sure you have 'firestore' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/firestore/query-data/get-data
 * and https://firebase.google.com/docs/firestore/query-data/listen
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Firestore {

        public class GetDataRealTimeUpdates : FirebaseExample {

            public class City {
                public string name, state, country;

                [JsonConstructor]
                public City(string name, string state, string country) {
                    this.name = name;
                    this.state = state;
                    this.country = country;
                }

                public override string ToString() { return name + ", " + state + ", " + country; }
            }

            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                //https://firebase.google.com/docs/firestore/query-data/listen#handle_listen_errors
                var q = Db.query(Db.collection(db, "cities"), Db.where("state", ">=", "CA"), Db.where("population", ">", 100000));//will trigger error as it's an invalid query
                var unsubscribe = Db.onSnapshot(q, snapshot => { }, (error) => Debug.LogError("SnapshotError: " + error));//no need to invoke unsubscribe() here because of the error

                //https://firebase.google.com/docs/firestore/query-data/listen#events-local-changes
                var unsubscribe2 = Db.onSnapshot(Db.doc(db, "cities", "SF"), doc => Debug.Log("onSnapshot2 Current data: " + JsonConvert.SerializeObject(doc.data())));//at some point call unsubscribe2() to detach the listener

                var unsubscribe3 = Db.onSnapshot(Db.doc(db, "cities", "SF"), doc => {
                    var source = doc.metadata.hasPendingWrites ? "Local" : "Server";
                    Debug.Log("onSnapshot3 " + source + " data: " + JsonConvert.SerializeObject(doc.data()));
                });//at some point call unsubscribe3() to detach the listener

                //https://firebase.google.com/docs/firestore/query-data/listen#events-metadata-changes
                var unsubscribe4 = Db.onSnapshot(Db.doc(db, "cities", "SF"), new SnapshotListenOptions() { includeMetadataChanges = true }, doc => {
                    var source = doc.metadata.hasPendingWrites ? "Local" : "Server";
                    Debug.Log("onSnapshot4 " + source + " data: " + JsonConvert.SerializeObject(doc.data()));
                });//at some point call unsubscribe3() to detach the listener

                //https://firebase.google.com/docs/firestore/query-data/listen#listen_to_multiple_documents_in_a_collection
                q = Db.query(Db.collection(db, "cities"), Db.where("state", "==", "CA"));
                var unsubscribe5 = Db.onSnapshot(q, querySnapshot => {
                    var cities = new List<string>();
                    foreach (QueryDocumentSnapshot doc in querySnapshot) {
                        cities.Add((string)doc.data()["name"]);//or more easily doc.get<string>("name")
                    }

                    Debug.Log("unsubscribe5 Current cities in CA: " + string.Join(",", cities));
                });

                //https://firebase.google.com/docs/firestore/query-data/listen#view_changes_between_snapshots
                var unsubscribe6 = Db.onSnapshot(q, snapshot => {
                    var docChanges = snapshot.docChanges();
                    foreach (DocumentChange change in docChanges) {
                        if (change.type == DocumentChangeType.added) {
                            Debug.Log("unsubscribe6 New city: " + change.doc.data<object>());//for Debug.log purposes, you can use change.doc.data<object>() instead of JsonConvert.SerializeObject(change.doc.data())
                        }
                        if (change.type == DocumentChangeType.modified) {
                            Debug.Log("unsubscribe6 Modified city: " + change.doc.data<object>());
                        }
                        if (change.type == DocumentChangeType.removed) {
                            Debug.Log("unsubscribe6 Removed city: " + change.doc.data<object>());
                        }
                    }
                });

                var citiesRef = Db.collection(db, "cities");
                await Db.setDoc(Db.doc(citiesRef, "SF"), new Dictionary<string, object>() { ["name"] = "San Francisco", ["state"] = "CA", ["country"] = "USA", ["capital"] = false, ["population"] = 860000, ["regions"] = new string[] { "west_coast", "norcal" } });
                await Db.setDoc(Db.doc(citiesRef, "LA"), new Dictionary<string, object>() { ["name"] = "Los Angeles", ["state"] = "CA", ["country"] = "USA", ["capital"] = false, ["population"] = 3900000, ["regions"] = new string[] { "west_coast", "socal" } });
                await Db.setDoc(Db.doc(citiesRef, "DC"), new Dictionary<string, object>() { ["name"] = "Washington, D.C.", ["state"] = null, ["country"] = "USA", ["capital"] = true, ["population"] = 680000, ["regions"] = new string[] { "east_coast" } });
                await Db.setDoc(Db.doc(citiesRef, "TOK"), new Dictionary<string, object>() { ["name"] = "Tokyo.", ["state"] = null, ["country"] = "Japan", ["capital"] = true, ["population"] = 9000000, ["regions"] = new string[] { "kanto", "honshu" } });
                await Db.setDoc(Db.doc(citiesRef, "BJ"), new Dictionary<string, object>() { ["name"] = "Beijing", ["state"] = null, ["country"] = "China", ["capital"] = true, ["population"] = 21500000, ["regions"] = new string[] { "jingjinji", "hebei" } });

                await Db.addDoc(Db.collection(citiesRef, "SF", "landmarks"), new Dictionary<string, string>() { ["name"] = "Golden Gate Bridge", ["type"] = "bridge" });
                await Db.addDoc(Db.collection(citiesRef, "LA", "landmarks"), new Dictionary<string, string>() { ["name"] = "Griffith Park", ["type"] = "park" });
                await Db.addDoc(Db.collection(citiesRef, "LA", "landmarks"), new Dictionary<string, string>() { ["name"] = "The Getty", ["type"] = "museum" });
                await Db.addDoc(Db.collection(citiesRef, "DC", "landmarks"), new Dictionary<string, string>() { ["name"] = "Lincoln Memorial", ["type"] = "memorial" });
                await Db.addDoc(Db.collection(citiesRef, "DC", "landmarks"), new Dictionary<string, string>() { ["name"] = "National Air and Space Museum", ["type"] = "museum" });
                await Db.addDoc(Db.collection(citiesRef, "TOK", "landmarks"), new Dictionary<string, string>() { ["name"] = "Ueno Park", ["type"] = "park" });
                await Db.addDoc(Db.collection(citiesRef, "TOK", "landmarks"), new Dictionary<string, string>() { ["name"] = "National Museum of Nature and Science", ["type"] = "museum" });
                await Db.addDoc(Db.collection(citiesRef, "BJ", "landmarks"), new Dictionary<string, string>() { ["name"] = "Jingshan Park", ["type"] = "park" });
                await Db.addDoc(Db.collection(citiesRef, "BJ", "landmarks"), new Dictionary<string, string>() { ["name"] = "Beijing Ancient Observatory", ["type"] = "museum" });

                //https://firebase.google.com/docs/firestore/query-data/get-data#get_a_document
                var docRef = Db.doc(db, "cities", "SF");
                var docSnap = await Db.getDoc(docRef);

                if (docSnap.exists()) {
                    Debug.Log("Document data: " + JsonConvert.SerializeObject(docSnap.data()));
                } else {
                    // docSnap.data() will be undefined in this case
                    Debug.Log("No such document!");
                }

                //https://firebase.google.com/docs/firestore/query-data/get-data#source_options
                try {
                    var doc = await Db.getDocFromCache(docRef);

                    // Document was found in the cache. If no cached document exists,
                    // an error will be returned to the 'catch' block below.
                    Debug.Log("Cached document data: " + JsonConvert.SerializeObject(doc.data()));
                } catch (FirestoreError e) {
                    Debug.LogError("Error getting cached document:" + e);
                }

                //https://firebase.google.com/docs/firestore/query-data/get-data#custom_objects
                var @ref = Db.doc(db, "cities", "LA");
                docSnap = await Db.getDoc(@ref);
                if (docSnap.exists()) {
                    // Convert to City object
                    var city = docSnap.data<City>();
                    // Use a City instance method
                    Debug.Log("city: " + city);
                } else {
                    Debug.Log("No such document!");
                }

                //https://firebase.google.com/docs/firestore/query-data/get-data#get_multiple_documents_from_a_collection
                q = Db.query(Db.collection(db, "cities"), Db.where("capital", "==", true));
                var querySnapshot = await Db.getDocs(q);
                Debug.Log("==Db.collection(db, \"cities\"), Db.where(\"capital\", \"==\", true)==");
                foreach (QueryDocumentSnapshot doc in querySnapshot) {
                    // doc.data() is never undefined for query doc snapshots
                    Debug.Log(doc.id + " => " + JsonConvert.SerializeObject(doc.data()));
                }

                //https://firebase.google.com/docs/firestore/query-data/get-data#get_all_documents_in_a_collection
                q = Db.query(Db.collection(db, "cities"));
                querySnapshot = await Db.getDocs(q);
                Debug.Log("==Db.collection(db, \"cities\")==");
                foreach (QueryDocumentSnapshot doc in querySnapshot) {
                    // doc.data() is never undefined for query doc snapshots
                    Debug.Log(doc.id + " => " + JsonConvert.SerializeObject(doc.data()));
                }

                //https://firebase.google.com/docs/firestore/query-data/get-data#get_all_documents_in_a_subcollection
                q = Db.query(Db.collection(db, "cities", "SF", "landmarks"));
                querySnapshot = await Db.getDocs(q);
                Debug.Log("==Db.collection(db, \"cities\", \"SF\", \"landmarks\")==");
                foreach (QueryDocumentSnapshot doc in querySnapshot) {
                    // doc.data() is never undefined for query doc snapshots
                    Debug.Log(doc.id + " => " + JsonConvert.SerializeObject(doc.data()));
                }

                

            }

        }
    }
}
