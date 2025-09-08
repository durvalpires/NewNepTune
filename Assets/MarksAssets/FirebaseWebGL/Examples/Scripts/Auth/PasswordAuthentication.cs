using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Auth product and email sign-in method on your firebase console or emulator.
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/password-auth .
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class PasswordAuthentication : FirebaseExample {
            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;

                //https://firebase.google.com/docs/auth/web/start#set_an_authentication_state_observer_and_get_user_data
                Au.onAuthStateChanged(auth, user => {
                    if (user is not null) {
                        Debug.Log($"user {user.email} with uid {user.uid} signed in");
                        // ...
                    } else {
                        Debug.Log("user signed out");
                    }
                });


                try {
                    await Au.createUserWithEmailAndPassword(auth, "mytestemail@testprovider.com", "123456");
                    await Au.signInWithEmailAndPassword(auth, "mytestemail@testprovider.com", "123456");
                    await Au.signOut(auth);
                } catch(AuthError e) {
                    Debug.LogError($"{e.code} - {e.Message}");
                }
            }

        }
    }
}
