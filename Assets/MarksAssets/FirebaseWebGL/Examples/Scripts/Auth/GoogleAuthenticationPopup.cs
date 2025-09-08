using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Auth product and google sign-in method on your firebase console or emulator.
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/google-signin .
 * 
 * == OTHER OBSERVATIONS ==
 * Please note, the logic is basically the same for the following
 * https://firebase.google.com/docs/auth/web/facebook-login
 * https://firebase.google.com/docs/auth/web/apple
 * https://firebase.google.com/docs/auth/web/twitter-login
 * https://firebase.google.com/docs/auth/web/github-auth
 * https://firebase.google.com/docs/auth/web/microsoft-oauth
 * https://firebase.google.com/docs/auth/web/yahoo-oauth
 * https://firebase.google.com/docs/auth/web/saml
 * https://firebase.google.com/docs/auth/web/openid-connect
 * */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class GoogleAuthenticationPopup : FirebaseExample {
            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;

                auth.languageCode = "it";
                var provider = new GoogleAuthProvider();
                provider.addScope("https://www.googleapis.com/auth/contacts.readonly");

                provider.setCustomParameters(new CustomParameters() { ["login_hint"] = "user@example.com" } );

                /* without running signInWithPopup on a pointerdown, the popup will likely get blocked by the browser.
                 * You'd get 'Firebase: Error (auth/popup-blocked)' on your console.
                 * You could instruct the user to configure the browser to allow popups for your app, but that's not
                 * a good user experience (in my opinion). Instead, the browser doesn't block the popup if it detects that it's initiated
                 * by a user action (button click). Hence, the pointerdown. The reason it has to be on a pointerdown
                 * and not a normal click is best explained here https://forum.unity.com/threads/popup-blocker-and-pointerdown-pointerclick.383233/#post-2491032
                 */

                //await Au.signInWithPopup(auth, provider); //uncomment this line to call signInWithPopup without user action. It can work too by configuring the browser settings, but I don't recommend it. Then you could remove everything below.
                var signInBtn = FindObjectOfType<Button>();
                signInBtn.GetComponent<EventTrigger>().triggers[0].callback.AddListener(e => {//get pointerdown trigger and add listener
                    Util.Util.runOnPointerDown(async () => {
                        try {
                            var userCred = await Au.signInWithPopup(auth, provider);
                            Debug.Log("==USER CREDENTIAL==");
                            Debug.Log(userCred);
                            #pragma warning disable CS0618
                            Application.ExternalEval($"alert('{userCred.user.email} signed in')");
                        } catch (AuthError e) {
                            Debug.LogError($"Auth Error - code: {e.code} - message: {e.Message} - email: {e.customData["email"]}");
                            try {
                                Debug.Log($"credential from error: {GoogleAuthProvider.credentialFromError(e)}");
                            } catch (AuthError e2) {
                                Debug.LogError($"Could not get credential from previous error: {e2}");
                            }
                        }
                    });
                });
            }

        }
    }
}
