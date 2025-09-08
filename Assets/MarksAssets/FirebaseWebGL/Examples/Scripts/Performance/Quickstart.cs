using UnityEngine;
using Pe = MarksAssets.FirebaseWebGL.Performance.Performance;
using System.Threading.Tasks;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Performance product on your firebase console.
 * Make sure you have 'performance' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Follow https://firebase.google.com/docs/perf-mon/get-started-web .
 * Leave your browser's javascript console (web inspector) open to check the results and if there are any errors in the process.
 * This example does NOT support the emulator. Performance is not on the supported list. See https://firebase.google.com/docs/emulator-suite#feature-matrix .
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Performance {
        public class Quickstart : FirebaseExample {

            async void Start() {
                var peformance = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (peformance is null) return;

                var trace = Pe.trace(peformance, "exampleTrace");
                Debug.Log("TRACE EXAMPLE FUNCTION THAT TAKES BETWEEN 0-5 seconds");
                trace.start();
                await DelayAsync(Random.Range(0.0f, 5f));
                trace.putAttribute("myAttribute", "myAttributeValue");
                trace.putMetric("myMetric", 1);
                trace.incrementMetric("myMetric", 2);
                trace.stop();
                Debug.Log("TRACE RECORDED");

            }

            public static async Task DelayAsync(float secondsDelay) {
                float startTime = Time.time;
                while (Time.time < startTime + secondsDelay) await Task.Yield();
            }
        }

    }

    
}
