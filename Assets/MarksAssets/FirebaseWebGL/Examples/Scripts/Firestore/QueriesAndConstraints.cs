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
 * Now, follow https://firebase.google.com/docs/firestore/query-data/queries
 * https://firebase.google.com/docs/firestore/query-data/order-limit-data
 * https://firebase.google.com/docs/firestore/query-data/aggregation-queries
 * https://firebase.google.com/docs/firestore/query-data/query-cursors
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Firestore {


        public class QueriesAndConstraints : FirebaseExample {
            async void Start() {
                var db = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (db is null) return;

                var citiesRef = Db.collection(db, "cities");


                await Db.setDoc(Db.doc(citiesRef, "SF"), new Dictionary<string, object>() { ["name"] = "San Francisco", ["state"] = "CA", ["country"] = "USA", ["capital"] = false, ["population"] = 860000, ["regions"] = new string[] { "west_coast", "norcal" } });
                await Db.setDoc(Db.doc(citiesRef, "LA"), new Dictionary<string, object>() { ["name"] = "Los Angeles", ["state"] = "CA", ["country"] = "USA", ["capital"] = false, ["population"] = 3900000, ["regions"] = new string[] { "west_coast", "socal" } });
                await Db.setDoc(Db.doc(citiesRef, "DC"), new Dictionary<string, object>() { ["name"] = "Washington, D.C.", ["state"] = null, ["country"] = "USA", ["capital"] = true, ["population"] = 680000, ["regions"] = new string[] { "east_coast" } });
                await Db.setDoc(Db.doc(citiesRef, "TOK"), new Dictionary<string, object>() { ["name"] = "Tokyo.", ["state"] = null, ["country"] = "Japan", ["capital"] = true, ["population"] = 9000000, ["regions"] = new string[] { "kanto", "honshu" } });
                await Db.setDoc(Db.doc(citiesRef, "BJ"), new Dictionary<string, object>() { ["name"] = "Beijing", ["state"] = null, ["country"] = "China", ["capital"] = true, ["population"] = 21500000, ["regions"] = new string[] { "jingjinji", "hebei" } });
                await Db.setDoc(Db.doc(citiesRef, "SPR1"), new Dictionary<string, object>() { ["name"] = "Springfield", ["state"] = "Missouri", ["country"] = "USA", ["capital"] = false, ["population"] = 169176, ["regions"] = new string[] { "southwest" } });
                await Db.setDoc(Db.doc(citiesRef, "SPR2"), new Dictionary<string, object>() { ["name"] = "Springfield", ["state"] = "Massachusetts", ["country"] = "USA", ["capital"] = false, ["population"] = 155929, ["regions"] = new string[] { "New England" } });
                await Db.setDoc(Db.doc(citiesRef, "SPR3"), new Dictionary<string, object>() { ["name"] = "Springfield", ["state"] = "Wisconsin", ["country"] = "USA", ["capital"] = false, ["population"] = 2929, ["regions"] = new string[] { "Dane" } });


                await Db.addDoc(Db.collection(citiesRef, "SF", "landmarks"), new Dictionary<string, string>() { ["name"] = "Golden Gate Bridge", ["type"] = "bridge" });
                await Db.addDoc(Db.collection(citiesRef, "LA", "landmarks"), new Dictionary<string, string>() { ["name"] = "Griffith Park", ["type"] = "park" });
                await Db.addDoc(Db.collection(citiesRef, "LA", "landmarks"), new Dictionary<string, string>() { ["name"] = "The Getty", ["type"] = "museum" });
                await Db.addDoc(Db.collection(citiesRef, "DC", "landmarks"), new Dictionary<string, string>() { ["name"] = "Lincoln Memorial", ["type"] = "memorial" });
                await Db.addDoc(Db.collection(citiesRef, "DC", "landmarks"), new Dictionary<string, string>() { ["name"] = "National Air and Space Museum", ["type"] = "museum" });
                await Db.addDoc(Db.collection(citiesRef, "TOK", "landmarks"), new Dictionary<string, string>() { ["name"] = "Ueno Park", ["type"] = "park" });
                await Db.addDoc(Db.collection(citiesRef, "TOK", "landmarks"), new Dictionary<string, string>() { ["name"] = "National Museum of Nature and Science", ["type"] = "museum" });
                await Db.addDoc(Db.collection(citiesRef, "BJ", "landmarks"), new Dictionary<string, string>() { ["name"] = "Jingshan Park", ["type"] = "park" });
                await Db.addDoc(Db.collection(citiesRef, "BJ", "landmarks"), new Dictionary<string, string>() { ["name"] = "Beijing Ancient Observatory", ["type"] = "museum" });


                string output = "";

                Debug.Log("all cities");
                var querySnapshot = await Db.getDocs(Db.query(citiesRef));//,BJ,DC,LA,SF,SPR1,SPR2,SPR3,TOK
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("where(\"state\", \"==\", \"CA\")");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("state", "==", "CA")));//,LA,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"capital\", \"==\", true)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("capital", "==", true)));//,BJ,DC,TOK
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"population\", \"<\", 1000000)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("population", "<", 1000000)));//,SPR3,SPR2,SPR1,DC,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"capital\", \"!=\", false)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("capital", "!=", false)));//,BJ,DC,TOK
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"regions\", \"array-contains\", \"west_coast\")");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("regions", "array-contains", "west_coast")));//,LA,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"country\", \"in\", new string[] { \"USA\", \"Japan\" })");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("country", "in", new string[] { "USA", "Japan" })));//,DC,LA,SF,SPR1,SPR2,SPR3,TOK
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"country\", \"not-in\", new string[] { \"USA\", \"Japan\" })");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("country", "not-in", new string[] { "USA", "Japan" })));//,BJ
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"regions\", \"array-contains-any\", new string[] { \"west_coast\", \"east_coast\" })");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("regions", "array-contains-any", new string[] { "west_coast", "east_coast" })));//,DC,LA,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"regions\", \"in\", new object[] { new string[] { \"west_coast\", \"east_coast\" } })");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("regions", "in", new object[] { new string[] { "west_coast", "east_coast" } })));//empty
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"state\", \"==\", \"CO\"), where(\"name\", \"==\", \"Denver\")");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("state", "==", "CO"), Db.where("name", "==", "Denver")));//empty
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"state\", \"==\", \"CA\"), where(\"population\", \"<\", 1000000)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("state", "==", "CA"), Db.where("population", "<", 1000000)));//,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"state\", \">=\", \"CA\"), where(\"state\", \"<=\", \"IN\")");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("state", ">=", "CA"), Db.where("state", "<=", "IN")));//,LA,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"state\", \"==\", \"CA\"), where(\"population\", \">\", 1000000)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("state", "==", "CA"), Db.where("population", ">", 1000000)));//,LA
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"country\", \"==\", \"USA\"), orderBy(\"population\")");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("country", "==", "USA"), Db.orderBy("population")));//,SPR3,SPR2,SPR1,DC,SF,LA
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("orderBy(\"name\"), limit(3)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("name"), Db.limit(3)));//,BJ,LA,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("orderBy(\"name\", OrderByDirection.desc), limit(3)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("name", OrderByDirection.desc), Db.limit(3)));//,DC,TOK,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("orderBy(\"state\"), orderBy(\"population\", OrderByDirection.desc)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("state"), Db.orderBy("population", OrderByDirection.desc)));//,BJ,TOK,DC,LA,SF,SPR2,SPR1,SPR3
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";
                Debug.Log("where(\"population\", \">\", 1000000), orderBy(\"population\"), limit(2)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.where("population", ">", 1000000), Db.orderBy("population"), Db.limit(2)));//,LA,TOK
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";


                Debug.Log("orderBy(\"population\"), startAt(1000000)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("population"), Db.startAt(1000000)));//,LA,TOK,BJ
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("orderBy(\"population\"), endAt(1000000)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("population"), Db.endAt(1000000)));//,SPR3,SPR2,SPR1,DC,SF
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                var docSnap = await Db.getDoc(Db.doc(citiesRef, "SF"));
                Debug.Log("orderBy(\"population\"), startAt(docSnap)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("population"), Db.startAt(docSnap)));//,SF,LA,TOK,BJ
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("orderBy(\"population\"), limit(25)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("population"), Db.limit(25)));//,SPR3,SPR2,SPR1,DC,SF,LA,TOK,BJ
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("penultimate visible");//TOK
                Debug.Log(querySnapshot.docs[querySnapshot.docs.Length - 2].id);

                Debug.Log("orderBy(\"population\"), startAfter(querySnapshot.docs[querySnapshot.docs.Length - 2]), limit(25)");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("population"), Db.startAfter(querySnapshot.docs[querySnapshot.docs.Length - 2]), Db.limit(25)));//,BJ
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("orderBy(\"name\"), orderBy(\"state\"), startAt(\"Springfield\"))");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("name"), Db.orderBy("state"), Db.startAt("Springfield")));//,SPR2,SPR1,SPR3,TOK,DC
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("orderBy(\"name\"), orderBy(\"state\"), startAt(\"Springfield\", \"Missouri\"))");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.orderBy("name"), Db.orderBy("state"), Db.startAt("Springfield", "Missouri")));//,SPR1,SPR3,TOK,DC
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("or(where(\"capital\", \"==\", true), where(\"population\", \">=\", 1000000))");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.or(Db.where("capital", "==", true), Db.where("population", ">=", 1000000))));//,DC,LA,TOK,BJ
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("or(where(\"capital\", \"==\", true), where(\"population\", \">=\", 1000000), limit(2))");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.or(Db.where("capital", "==", true), Db.where("population", ">=", 1000000)), Db.limit(2)));//,DC,LA
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("and(where(\"state\", \"==\", \"CA\"), or(where(\"capital\", \"==\", true), where(\"population\", \">=\", 1000000)))");
                querySnapshot = await Db.getDocs(Db.query(citiesRef, Db.and(Db.where("state", "==", "CA"), Db.or(Db.where("capital", "==", true), Db.where("population", ">=", 1000000)))));//,LA
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.id; }
                Debug.Log(output); output = "";

                Debug.Log("collectionGroup(db, \"landmarks\"), where(\"populationtype\", \"==\", \"museum\")");
                querySnapshot = await Db.getDocs(Db.query(Db.collectionGroup(db, "landmarks"), Db.where("type", "==", "museum")));//,Beijing Ancient Observatory,National Air and Space Museum,The Getty,Legion of Honor,National Museum of Nature and Science
                foreach (QueryDocumentSnapshot doc in querySnapshot) { output = output + "," + doc.data()["name"]; }
                Debug.Log(output); output = "";

                var snapshot = await Db.getCountFromServer(citiesRef);
                Debug.Log("count: " + snapshot.data()["count"]);// 8

                snapshot = await Db.getCountFromServer(Db.query(citiesRef, Db.where("state", "==", "CA")));
                Debug.Log("count from CA: " + snapshot.data()["count"]);// 2

                snapshot = await Db.getAggregateFromServer(citiesRef, new Dictionary<string, object>() { ["totalPopulation"] = Db.sum("population") });
                Debug.Log("totalPopulation: " + snapshot.data()["totalPopulation"]);//36268034

                snapshot = await Db.getAggregateFromServer(Db.query(citiesRef, Db.where("capital", "==", true)), new Dictionary<string, object>() { ["totalPopulation"] = Db.sum("population") });
                Debug.Log("total population from capitals: " + snapshot.data()["totalPopulation"]);//31180000

                snapshot = await Db.getAggregateFromServer(citiesRef, new Dictionary<string, object>() { ["averagePopulation"] = Db.average("population") });
                Debug.Log("averagePopulation: " + snapshot.data()["averagePopulation"]);//4533504.25

                snapshot = await Db.getAggregateFromServer(Db.query(citiesRef, Db.where("capital", "==", true)), new Dictionary<string, object>() { ["averagePopulation"] = Db.average("population") });
                Debug.Log("average population from capitals: " + snapshot.data()["averagePopulation"]);//10393333.333333334

                snapshot = await Db.getAggregateFromServer(citiesRef, new Dictionary<string, object>() { ["countOfDocs"] = Db.count(), ["totalPopulation"] = Db.sum("population"), ["averagePopulation"] = Db.average("population") });
                Debug.Log("Multiple aggregations - countOfDocs: " + snapshot.data()["countOfDocs"] + ", totalPopulation: " + snapshot.data()["totalPopulation"] + ", averagePopulation: " + snapshot.data()["averagePopulation"]);


            }

        }
    }
}
