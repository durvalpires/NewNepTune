using System;
using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.UI;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Auth product and email sign-in method on your firebase console. Then, enable totp on your project using firebase admin.
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/totp-mfa .
 * This example does NOT support the emulator! See https://github.com/firebase/firebase-tools/issues/6224
 * 
 * == OTHER OBSERVATIONS ==
/* I implemented multifactor auth with email and totp on this example. First, you have to create a user with an email, and you will have to verify it.
 * I recommend using https://temp-mail.org/en/ for temporary emails. Then, your secret key will show up. I recommend using
 * https://dan.hersam.com/tools/gen-qr-code.php to generate a qr code for quick testing with the Google Authenticator app https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2 .
 * At this point, if everything went well, your user was created and enrolled with multifactor. Right after, the user is signed out, and you can sign in.
 * The create user button will have a new text, 'sign in'.
 * The steps are the same, but instead of enrolling, you will sign in the user.
 * 
*/
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class TOTPMultiFactorAuthentication : FirebaseExample {
            public InputField verificationCode, email;
            public Button confirmVerificationCode, createUser;
            public GameObject verifyEmail;

            private MultiFactorSession multiFactorSession = null;
            private MultiFactorResolver resolver = null;
            private User user = null;
            private TotpSecret totpSecret = null;

            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;
                
              
                createUser.onClick.AddListener(async () => {
                    createUser.interactable = email.interactable = false;

                    if (user is null) {
                        try {
                            var userCredential = await Au.createUserWithEmailAndPassword(auth, $"{email.text}", "123456");
                            user = userCredential.user;
                            try {
                                verifyEmail.SetActive(true);
                                await Au.sendEmailVerification(user);
                            } catch (AuthError e) {
                                Debug.LogError($"could not send email verification: {e}");
                                return;
                            }
                        } catch (AuthError e) {
                            createUser.interactable = email.interactable = true;
                            Debug.LogError($"could not sign in: {e}");
                        }
                    } else {//already enrolled
                        try {
                            var userCredential = await Au.signInWithEmailAndPassword(auth, $"{email.text}", "123456");
                        } catch(AuthError e) {
                            if (e.code == AuthErrorCodes.MFA_REQUIRED) {
                                resolver = Au.getMultiFactorResolver(auth, e);
                                verificationCode.interactable = confirmVerificationCode.interactable = true;
                                Debug.Log("got resolver");
                            } else {
                                createUser.interactable = true;
                                Debug.LogError($"could not sign in: {e}");
                            }
                        }
                    }
                    
                });


                confirmVerificationCode.onClick.AddListener(async () => {
                    try {
                        verificationCode.interactable = confirmVerificationCode.interactable = false;

                        if (resolver is null) {//enroll
                            var multiFactorAssertion = TotpMultiFactorGenerator.assertionForEnrollment(totpSecret, verificationCode.text);
                            await Au.multiFactor(user).enroll(multiFactorAssertion, "multifactorUser");
                            //don't want to clear the user reference on this specific case, since I want to sign in right after and it would throw 'User not found' error in the update method when checking if the email is verified.
                            //Usually, you'd leave autoClear set to true as explained in the docs.
                            await Au.signOut(auth, false);
                            #pragma warning disable CS0618
                            Application.ExternalEval($"alert('enrolled {email.text} with totp and signed out.')");
                            createUser.GetComponentInChildren<Text>().text = "sign in";
                            createUser.interactable = true;
                        } else {//sign in
                            var multiFactorAssertion = TotpMultiFactorGenerator.assertionForSignIn(resolver.hints[0].uid, verificationCode.text);
                            var uc = await resolver.resolveSignIn(multiFactorAssertion);
                            #pragma warning disable CS0618
                            Application.ExternalEval($"alert('user {email.text} signed in with totp.')");
                        }

                    } catch(AuthError e) {
                        verificationCode.interactable = confirmVerificationCode.interactable = true;
                        Debug.LogError($"Failed to get user credential: {e}");
                    }
                });

            }


            private void Update() {
                if (user?.emailVerified is true && verifyEmail.activeSelf is true) {
                    verifyEmail.SetActive(false);
                    ((Action)(async () => {
                        try {
                            multiFactorSession = await Au.multiFactor(user).getSession();
                            if (resolver is null) {//enrollment
                                totpSecret = await TotpMultiFactorGenerator.generateSecret(multiFactorSession);
                                var secret = totpSecret.secretKey;
                                Debug.Log("==secret==");
                                Debug.Log(secret);
                                #pragma warning disable CS0618
                                Application.ExternalEval($"alert('secret {secret}')");
                                verificationCode.interactable = confirmVerificationCode.interactable = true;
                            }
                        } catch (AuthError e) {
                            Debug.LogError($"Could not get multifactor session: {e}");
                            return;
                        }
                    }))();
                    
                } else if (user?.emailVerified is false) {//email not verified
                    user?.reload();//reload user to check again.
                }
            }

        }
    }
}
