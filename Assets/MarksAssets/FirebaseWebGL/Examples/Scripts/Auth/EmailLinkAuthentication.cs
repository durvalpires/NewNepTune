using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.UI;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Auth product and email link sign-in method on your firebase console or emulator.
 * Add 'localhost' to the list of Authorized domains.
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/email-link-auth .
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class EmailLinkAuthentication : FirebaseExample {
            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;

                var sendLinkBtn = FindObjectOfType<Button>();
                var emailInputField = FindObjectOfType<InputField>();

                //https://firebase.google.com/docs/auth/web/email-link-auth#completing_sign-in_in_a_web_page
                if (Au.isSignInWithEmailLink(auth, Application.absoluteURL)) {//it's the sign in url that was sent by the user.
                    string email;
                    if (PlayerPrefs.HasKey("emailForSignIn")) {//found the email stored in indexDb
                        email = PlayerPrefs.GetString("emailForSignIn");
                        sendLinkBtn.interactable = emailInputField.interactable = false;
                        emailInputField.text = email;
                        try {
                            await Au.signInWithEmailLink(auth, emailInputField.text, Application.absoluteURL);
                            #pragma warning disable CS0618
                            Application.ExternalEval($"alert('{email} signed in')");
                            await Au.signOut(auth);
                        } catch (AuthError e) {
                            Debug.LogError($"{e.code} - {e.Message}");
                        } finally {//clean up
                            PlayerPrefs.DeleteKey("emailForSignIn");
                        }
                    } else {//didn't find email. Maybe he opened the link on a different device? User will need to type email again...
                        sendLinkBtn.GetComponentInChildren<Text>().text = "sign in";
                        sendLinkBtn.onClick.AddListener(async () => {
                            try {
                                email = emailInputField.text;
                                await Au.signInWithEmailLink(auth, email, Application.absoluteURL);
                                Application.ExternalEval($"alert('{email} signed in')");
                                await Au.signOut(auth);
                            } catch (AuthError e) {
                                Debug.LogError($"{e.code} - {e.Message}");
                            } finally {//clean up
                                PlayerPrefs.DeleteKey("emailForSignIn");
                            }
                        });
                    }

                } else {//it's not the sign in url. The user will send the link.
                    //https://firebase.google.com/docs/auth/web/email-link-auth#send_an_authentication_link_to_the_users_email_address
                    sendLinkBtn.GetComponentInChildren<Text>().text = "send link";

                    var actionCodeSettings = new ActionCodeSettings() {
                        url = Application.absoluteURL,//redirect to the same page
                        handleCodeInApp = true
                    };
                    sendLinkBtn.onClick.AddListener(async () => {
                        try {
                            await Au.sendSignInLinkToEmail(auth, emailInputField.text, actionCodeSettings);
                            PlayerPrefs.SetString("emailForSignIn", emailInputField.text);
                            PlayerPrefs.Save();
                            sendLinkBtn.interactable = false;
                            Application.ExternalEval($"alert('Check email or emulator console for the sign in link.')");
                        } catch (AuthError e) {
                            Debug.LogError($"{e.code} - {e.Message}");
                        }
                    });
                }

            }

        }
    }
}
