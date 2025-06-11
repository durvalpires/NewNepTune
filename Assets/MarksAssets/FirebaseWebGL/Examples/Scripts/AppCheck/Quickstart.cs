using System.Threading.Tasks;
using MarksAssets.FirebaseWebGL.App;
using MarksAssets.FirebaseWebGL.AppCheck;
using UnityEngine;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using Apc = MarksAssets.FirebaseWebGL.AppCheck.AppCheck;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the AppCheck product on your firebase console or emulator.
 * Make sure you have 'appCheck' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Now, follow https://firebase.google.com/docs/app-check/web/recaptcha-enterprise-provider
 * In the inspector, enter the siteKey that you obtained on the previous step.
 * Leave your browser's javascript console (web inspector) open to check the result and if there are any errors in the process.
 * If you did everything correctly, if you open your app from any domain that isn't on the allowed domain list that you defined on the google cloud console, 
 * you should get the following error on the console "AppCheck: ReCAPTCHA error. (appCheck/recaptcha-error)".
 * 
 * This example does NOT support the emulator. AppCheck is not on the supported list. See https://firebase.google.com/docs/emulator-suite#feature-matrix .
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace AppCheck {
        public class Quickstart : FirebaseExample {

            public string siteKey;

            async void Start() {
                try {
                    await Ap.modulesLoaded();
                    FirebaseApp app = Ap.initializeApp(new FirebaseOptions() { apiKey = firebaseConfig.apiKey, appId = firebaseConfig.appId, authDomain = firebaseConfig.authDomain, projectId = firebaseConfig.projectId, storageBucket = firebaseConfig.storageBucket, messagingSenderId = firebaseConfig.messagingSenderId, databaseURL = firebaseConfig.databaseURL });
                    Apc.initializeAppCheck(app, new AppCheckOptions() { provider = new ReCaptchaEnterpriseProvider(siteKey), isTokenAutoRefreshEnabled = true });
                    Debug.Log("AppCheck initialized successfully.");

                    /*CUSTOM PROVIDER LOGIC BELOW

                    //https://firebase.google.com/docs/app-check/web/custom-provider#implement-object
                    TaskCompletionSource<AppCheckToken> tcs = new TaskCompletionSource<AppCheckToken>();
                    var customProvider = new CustomProvider(new CustomProviderOptions() {
                        getToken = () =>
                        {
                            //wrap code if/else below on some async logic that communicates with server
                            //if (successCondition) tcs.SetResult(new AppCheckToken() { token = tokenFromServer, expireTimeMillis = expirationFromServer }) ;
                            //else tcs.SetException(new FirebaseError("could not get token"));

                            //return promise (task in C#)
                            return tcs.Task;
                        }
                    });

                    */


                } catch (Util.FirebaseError e) {
                    Debug.LogError("initialization failure: " + (e.Message.Contains("undefined is not an object") ? $"{e.Message}. Did you set appCheck to 'true' on the modules.jspre file under MarksAssets/FirebaseWebGL/Plugins/Core ?" : e.Message));
                }
            }
        }
    }

    
}
