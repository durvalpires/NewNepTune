using System;
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
 * Now, follow https://firebase.google.com/docs/firestore/manage-data/add-data
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Firestore {

        

        public class AddData : FirebaseExample {

            public class City {
                public string name, state;

                //https://www.newtonsoft.com/json/help/html/jsonpropertyname.htm
                //private fields or properties with private setters require this attribute to make them serializable/deserializable
                //JsonProperty can also be used to rename a property during serialization. In the example below, the property 'privateName' will be
                //saved to Firestore with the name 'MyPrivateName'
                [Newtonsoft.Json.JsonProperty("MyPrivateName")]
                #pragma warning disable CS0414
                private string privateName = "secret name";

                //Last most common usage of JsonProperty. If you combine it with the NullValueHandling attribute.
                //if you assign null to country, it *won't* write to Firestore.
                [Newtonsoft.Json.JsonProperty(NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore)]
                public string country;

                //https://www.newtonsoft.com/json/help/html/PropertyJsonIgnore.htm
                [Newtonsoft.Json.JsonIgnore]
                public string myIgnoredName = "my name doesn't matter T_T";


                //https://www.newtonsoft.com/json/help/html/JsonConstructorAttribute.htm
                [Newtonsoft.Json.JsonConstructor]//JsonConstructor not techincally necessary here, but it's good practice. You need it when *reading* data from firestore.
                public City(string name, string state, string country) {
                    this.name = name;
                    this.state = state;
                    this.country = country;
                }

                //to summarize, if you want to use custom classes instead of Dictionaries with Firestore, familiarize yourself with the JsonProperty, JsonIgnore, and JsonConstructor attributes.

                public override string ToString() { return this.name + ", " + this.state + ", " + this.country; }
            }


            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                try {

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#set_a_document
                    await Db.setDoc(Db.doc(db, "cities", "LA"), new Dictionary<string, object>() { ["name"] = "Los Angeles", ["state"] = "CA", ["country"] = "USA" });

                    var cityRef = Db.doc(db, "cities", "BJ");
                    await Db.setDoc(cityRef, new Dictionary<string, object>() { ["capital"] = true }, new SetOptionsMerge() { merge = true});

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#data_types
                    var docData = new Dictionary<string, object>() {
                        ["stringExample"] = "Hello world!",
                        ["booleanExample"] = true,
                        ["numberExample"] = 3.14159265,
                        ["dateTimeExample"] = DateTime.Parse("December 10, 1815"),
                        ["timestampExample"] = Timestamp.now(),
                        ["arrayExample"] = new object[] { 5, true, "hello" },
                        ["nullExample"] = null,
                        ["objectExample"] = new Dictionary<string, object>() { ["a"] = 5, ["b"] = new Dictionary<string, object>() { ["nested"] = "foo" } },
                        ["geopointExample"] = new GeoPoint(1.3, 1.5),
                        ["referenceExample"] = Db.doc(db, "cities", "LA2"),
                        ["FieldValueServerTime"] = Db.serverTimestamp(),
                        ["bytesExample"] = Array.ConvertAll(new byte[] { 1, 127, 92, 255, 20 }, c => (int)c)//byte arrays are converted to base64 strings when serialized, so convert the byte array to an int array or just use an int array directly! Well, unless you actually want to save the data as a base64 string.
                    };


                    await Db.setDoc(Db.doc(db, "data", "one"), docData);

                    var qs = await Db.getDocs(Db.collection(db, "data"));

                    foreach (QueryDocumentSnapshot doc in qs) {
                        Debug.Log("==FULL DATA==");
                        Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(doc.data()));
                        Debug.Log("==STRING EXAMPLE==");
                        Debug.Log(doc.get<string>("stringExample"));
                        Debug.Log("==BOOLEAN EXAMPLE==");
                        Debug.Log(doc.get<bool>("booleanExample"));
                        Debug.Log("==NUMBER EXAMPLE==");
                        Debug.Log(doc.get<double>("numberExample"));
                        Debug.Log("==DATETIME EXAMPLE==");
                        Debug.Log(doc.get<Timestamp>("dateTimeExample"));
                        Debug.Log("==TIMESTAMP EXAMPLE==");
                        Debug.Log(doc.get<Timestamp>("timestampExample"));
                        Debug.Log("==ARRAY EXAMPLE==");
                        Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(doc.get<object[]>("arrayExample")));
                        Debug.Log("==NULL EXAMPLE==");
                        Debug.Log(doc.get<string>("nullExample"));
                        Debug.Log("==OBJECT EXAMPLE==");
                        Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(doc.get<Dictionary<string, object>>("objectExample")));
                        Debug.Log("==GEOPOINT EXAMPLE==");
                        Debug.Log(doc.get<string>("geopointExample"));
                        Debug.Log("==REFERENCE EXAMPLE==");
                        Debug.Log(doc.get<DocumentReference>("referenceExample"));
                        Debug.Log("==FIELDVALUE(SERVERTIMESTAMP) EXAMPLE==");
                        Debug.Log(doc.get<Timestamp>("FieldValueServerTime"));//don't use doc.get<FieldValue>("FieldValueServerTime"). FieldValue is a value used to perform a custom operation before storing the data to firebase, it can't be used to read from it! In this case, the FieldValueServerTime field is stored using the serverTimestamp method which returns a FieldValue that is used to get the server's time before storing it as a Timestamp. Therefore, to retrieve this field the generic type used is a Timestamp, not FieldValue!
                        Debug.Log("==BYTES EXAMPLE==");//bytes are correctly deserialized to a byte array, but then I convert to an int array to be serialized and displayed on the screen.
                        Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(Array.ConvertAll(doc.get<byte[]>("bytesExample"), c => (int)c)));
                    }

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#custom_objects
                    // Set with custom class
                    var @ref = Db.doc(db, "cities", "LA");
                    await Db.setDoc(@ref, new City("Los Angeles", "CA", "USA"));
                    var @ref2 = Db.doc(db, "cities", "LA2");
                    await Db.setDoc(@ref2, new City("Los Angeles", null, null));//country won't be saved because of NullValueHandling. See the city class. But state will be saved with null.

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#add_a_document
                    // Add a new document with a generated id.
                    var docRef = await Db.addDoc(Db.collection(db, "cities"), new Dictionary<string, object> {
                        ["name"] = "Tokyo",
                        ["country"] = "Japan"
                    });
                    Debug.Log("Document written with ID: " + docRef.id);

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#update-data
                    var washingtonRef = Db.doc(db, "cities", "DC");
                    await Db.setDoc(washingtonRef, new Dictionary<string, object> { ["regions"] = new string[] { "east_coast" } });
                    await Db.updateDoc(washingtonRef, new Dictionary<string, object>() { ["capital"] = true });

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#server_timestamp
                    await Db.updateDoc(washingtonRef, new Dictionary<string, object>() { ["timestamp"] = Db.serverTimestamp() });

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#update_fields_in_nested_objects
                    // Create an initial document to update.
                    var frankDocRef = Db.doc(db, "users", "frank");
                    await Db.setDoc(frankDocRef, new Dictionary<string, object> {
                        ["name"] = "Frank",
                        ["favorites"] = new Dictionary<string, object> { ["food"] = "Pizza", ["color"] = "Blue", ["subject"] = "recess" },
                        ["age"] = 12
                    });

                    // To update age and favorite color:
                    await Db.updateDoc(frankDocRef, new Dictionary<string, object> {
                        ["age"] = 13,
                        ["favorites.color"] = "Red"
                    });

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#update_elements_in_an_array
                    // Atomically add a new region to the "regions" array field.
                    await Db.updateDoc(washingtonRef, new Dictionary<string, object> { ["regions"] = Db.arrayUnion("greater_virginia") });

                    // Atomically remove a region from the "regions" array field.
                    await Db.updateDoc(washingtonRef, new Dictionary<string, object> { ["regions"] = Db.arrayRemove("east_coast") });

                    //https://firebase.google.com/docs/firestore/manage-data/add-data#increment_a_numeric_value
                    await Db.updateDoc(washingtonRef, new Dictionary<string, object> { ["population"] = Db.increment(50) });



                } catch (FirestoreError e) {
                    Debug.LogError("Firestore Error: " + e);
                }

                var querySnapshot = await Db.getDocs(Db.collection(db, "users"));

                foreach (QueryDocumentSnapshot doc in querySnapshot) {
                    Debug.Log($"{doc.id} => {doc.data<object>()}");
                }

            }

        }
    }
}
