using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Auth product and google sign-in method on your firebase console or emulator.
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/google-signin .
 * In theory, this example should work with the emulator, but I couldn't make it work with it. So I recommend that you don't use it. More info below.
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
 * On this example it tries to get the redirect result first, and if it fails it attempts to sign in with redirect.
 * If you are caught in an infinite loop, that's because the redirect result is always returning null. To fix this do the following:
 * Use firebase hosting https://firebase.google.com/docs/hosting/quickstart and the generated https://xxxxxx-xxxxx.firebaseapp.com/ (not web.app) website. 
 * Check https://firebase.google.com/docs/auth/web/redirect-best-practices for more details.
 * The emulator issue is basically that I can't make getRedirectResult work with it, period. 
 * See https://github.com/firebase/firebase-js-sdk/issues/7916 and https://github.com/firebase/firebase-tools/issues/6341 for more details.
 * Maybe you'll have better luck if you try...but don't blame if it doesn't work!
 */

//Second, 

namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class GoogleAuthenticationRedirect : FirebaseExample {
            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;

                auth.languageCode = "it";
                var provider = new GoogleAuthProvider();
                provider.addScope("https://www.googleapis.com/auth/contacts.readonly");

                provider.setCustomParameters(new CustomParameters() { ["login_hint"] = "user@example.com" } );

                try {
                    var result = await Au.getRedirectResult(auth);
                    if (result is null) {
                        Debug.Log("user credential is null. Attempting to sign in with redirect...");
                        await Au.signInWithRedirect(auth, provider);
                    }

                    #pragma warning disable CS0618
                    Application.ExternalEval($"alert('{result.user.email} signed in')");

                    Debug.Log("==USER CREDENTIAL==");
                    Debug.Log(result);
                    // This gives you a Google Access Token. You can use it to access Google APIs.
                    var credential = GoogleAuthProvider.credentialFromResult(result);
                    Debug.Log("==OAUTH CREDENTIAL==");
                    Debug.Log(credential);
                    var token = credential.accessToken;
                    Debug.Log("==ACCESS TOKEN==");
                    Debug.Log(token);

                    // The signed-in user info.
                    var user = result.user;
                    Debug.Log("==USER==");
                    Debug.Log(user);

                } catch(AuthError e) {
                    Debug.LogError($"Auth Error - code: {e.code} - message: {e.Message} - email: {e.customData["email"]}");
                }
            }

        }
    }
}
