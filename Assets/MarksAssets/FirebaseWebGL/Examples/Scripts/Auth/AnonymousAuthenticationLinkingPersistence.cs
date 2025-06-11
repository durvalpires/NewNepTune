using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Auth product, anonymous sign-in method, and google sign-in method on your firebase console or emulator.
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/anonymous-auth.
 * 
 * == OTHER OBSERVATIONS ==
 * I merged 3 examples with one here. You sign in anonymously, but I convert
 * the anonymous account to a permanent one with https://firebase.google.com/docs/auth/web/account-linking
 * and also set the persistence beforehand with //https://firebase.google.com/docs/auth/web/auth-state-persistence
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class AnonymousAuthenticationLinkingPersistence : FirebaseExample {
            public string persistence = "browserLocalPersistence";

            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;

                //https://firebase.google.com/docs/auth/web/auth-state-persistence
                try {
                    switch (persistence) {
                        case "browserLocalPersistence": await Au.setPersistence(auth, Au.browserLocalPersistence); break;
                        case "browserSessionPersistence": await Au.setPersistence(auth, Au.browserSessionPersistence); break;
                        case "indexedDBLocalPersistence": await Au.setPersistence(auth, Au.indexedDBLocalPersistence); break;
                        case "inMemoryPersistence": await Au.setPersistence(auth, Au.inMemoryPersistence); break;
                    }
                } catch(AuthError e) {
                    Debug.LogError($"Could not set persistence - {e}");
                }

                Au.onAuthStateChanged(auth, user => {
                    if (user is not null) {
                        Debug.Log($"user {user.email} with uid {user.uid} signed in");
                        // ...
                    } else {
                        Debug.Log("user signed out");
                    }
                });

                try {
                    var uc = await Au.signInAnonymously(auth);
                    #pragma warning disable CS0618
                    Application.ExternalEval($"alert('Anonymous user with uid {uc.user.uid} signed in.')");

                    //https://firebase.google.com/docs/auth/web/account-linking#link-federated-auth-provider-credentials-to-a-user-account
                    //See GoogleAuthenticationPopup.cs for the explanation of the setup below.
                    var signInBtn = FindObjectOfType<Button>();
                    signInBtn.GetComponent<Image>().raycastTarget = signInBtn.interactable = true;
                    signInBtn.GetComponent<EventTrigger>().triggers[0].callback.AddListener(e => {//get pointerdown trigger and add listener
                        Util.Util.runOnPointerDown(async () => {
                            try {
                                var provider = new GoogleAuthProvider();
                                var userCred = await Au.linkWithPopup(uc.user, provider);
                                signInBtn.GetComponent<Image>().raycastTarget = signInBtn.interactable = false;
                                #pragma warning disable CS0618
                                Application.ExternalEval($"alert('User with uid {uc.user.uid} linked with google.')");
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
                } catch(AuthError e) {
                    Debug.LogError($"{e.code} - {e.Message}");
                }
            }

        }
    }
}
