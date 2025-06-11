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
 * Now, follow https://firebase.google.com/docs/firestore/manage-data/transactions
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Firestore {


        public class TransactionsAndBatchedWrites : FirebaseExample {
            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                //https://firebase.google.com/docs/firestore/manage-data/transactions#passing_information_out_of_transactions
                try {
                    var sfDocRef = Db.doc(db, "cities", "SF");
                    await Db.setDoc(sfDocRef, new Dictionary<string, object> { ["population"] = 500000 });//create document first
                    var population = await Db.runTransaction(db, async transaction => {
                        var sfDoc = await transaction.get(sfDocRef);
                        if (!sfDoc.exists()) {
                            throw new Exception("Document does not exist!");
                        }

                        var newPop = (int)(long)sfDoc.data<Dictionary<string, object>>()["population"] + 1;
                        if (newPop <= 1000000) {
                            transaction.update(sfDocRef, new Dictionary<string, object> { ["population"] = newPop });
                            return newPop;
                        } else {
                            throw new Exception("Sorry! Population is too big");
                        }
                    }, new TransactionOptions() { maxAttempts = 2 });

                    Debug.Log("Transaction successfully committed! Population: " + population);
                   
                } catch (FirestoreError e) {
                    Debug.LogError("Transaction Error: " + e);
                }

                //https://firebase.google.com/docs/firestore/manage-data/transactions#batched-writes
                try {
                    // Get a new write batch
                    var batch = Db.writeBatch(db);

                    // Set the value of 'NYC'
                    var nycRef = Db.doc(db, "cities", "NYC");

                    batch.set(nycRef, new Dictionary<string, object> { ["name"] = "New York City"});

                    // Update the population of 'SF'
                    var sfRef = Db.doc(db, "cities", "SF");
                    batch.update(sfRef, new Dictionary<string, object> { ["population"] = 1000000});

                    // Delete the city 'LA'
                    var laRef = Db.doc(db, "cities", "LA");
                    batch.delete(laRef);

                    // Commit the batch
                    await batch.commit();

                } catch (FirestoreError e) {
                    Debug.LogError("Batch Error: " + e);
                }


            }

        }
    }
}
