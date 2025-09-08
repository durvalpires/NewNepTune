using System;
using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.UI;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*  == BEFORE YOU BEGIN ==
 * Enable the Auth product, Email sign-in method, Phone sign-in method, and SMS Multi-factor Authentication on your Firebase Console or emulator.
 * To use recaptcha,you have to use the widget mode https://firebase.google.com/docs/auth/web/phone-auth#use-the-recaptcha-widget
 * Which means, on the index.html of your build folder or custom webgl template, you have to add the following div
 * <div id="recaptcha-container" style="z-index: 1; position: absolute; "></div>
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/multi-factor .
 * This example has PARTIAL emulator support. I recommend that you don't use it at all. More info below.
 * 
 * == OTHER OBSERVATIONS ==
/* Please check PhoneAuthentication.cs file for more details about recaptcha, recommendations, and emulator issues. Everything applies here.
 * I implemented multifactor auth with email and phone on this example. First, you have to create a user with an email, and you will have to verify it.
 * I recommend using https://temp-mail.org/en/ for temporary emails. Then, you will enter the phone number. Finally, enter the verification code.
 * At this point, if everything went well, your user was created and enrolled with multifactor. Right after, the user is signed out, and you can sign in.
 * The create user button will have a new text, 'sign in'.
 * The steps are the same, but instead of enrolling, you will sign in the user.
 * 
 * If you are using the emulator, don't verify the email manually, open the link that you will receive from the console window that you are running the emulator on. See https://github.com/firebase/firebase-js-sdk/issues/7917.
 * To summarize emulator issues with this example, see https://github.com/firebase/firebase-js-sdk/issues/7646#issuecomment-1874320898 and https://github.com/firebase/firebase-js-sdk/issues/7917.
 * The sign in step will not work with the emulator, after you tap on 'verify phone', the confirm code button and input will not enable, because verifyPhoneNumber at this point is called twice.
*/
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class SMSMultiFactorAuthentication : FirebaseExample {
            public InputField phoneNumber, verificationCode, email;
            public Button confirmPhoneNumber, confirmVerificationCode, createUser;
            public GameObject verifyEmail;
            public string recaptchaSize = "invisible";//possible values are "compact", "normal" and "invisible"
            public string recaptchaBadge = "bottomright";//possible values https://developers.google.com/recaptcha/docs/invisible#render_param . Badge only applies if size is set to "invisible"
            public string recaptchaTheme = "light";//possible values https://developers.google.com/recaptcha/docs/display#render_param

            private RecaptchaVerifier applicationVerifier = null;
            private MultiFactorSession multiFactorSession = null;
            private MultiFactorResolver resolver = null;
            private User user = null;

            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;
                
                PhoneAuthProvider phoneAuthProvider = new PhoneAuthProvider(auth);

                applicationVerifier = null;
                try {//in practice you don't have to try catch the RecaptchaVerifier if you set up everything correctly.
                    applicationVerifier = new RecaptchaVerifier(auth, "recaptcha-container", new RecaptchaParameters() {
                        size = recaptchaSize,
                        callback = token => Debug.Log(token),
                        expiredCallback = () => Debug.Log("recaptcha expired"),//Never triggers. See https://github.com/firebase/firebase-js-sdk/issues/7631
                        errorCallback = async () => await applicationVerifier.verify(),//error(probably internet connection), try again!
                        badge = recaptchaBadge,
                        theme = recaptchaTheme
                    });
                } catch(AuthError e) {
                    if (e.code == AuthErrorCodes.ARGUMENT_ERROR) {
                        Debug.LogError("failed to get recaptcha verifier. Did you add the div to the index.html of your build?");
                        #pragma warning disable CS0618
                        Application.ExternalEval($"alert('Did not find recaptcha container div on html build')");
                    } else {
                        Debug.LogError($"failed to get recaptcha verifier: {e}");
                    }
                    return;
                }

                var token = await applicationVerifier.verify();
                Debug.Log("application verified");

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
                                confirmPhoneNumber.interactable = phoneNumber.interactable = true;
                                Debug.Log("got resolver");
                            } else {
                                createUser.interactable = true;
                                Debug.LogError($"could not sign in: {e}");
                            }
                        }
                    }

                    
                });

                string verificationId = null;
                confirmPhoneNumber.onClick.AddListener(async () => {
                    try {
                        confirmPhoneNumber.interactable = phoneNumber.interactable = false;
                        if (resolver is null) {//enrollment
                            verificationId = await phoneAuthProvider.verifyPhoneNumber(new PhoneMultiFactorEnrollInfoOptions() { phoneNumber = phoneNumber.text, session = multiFactorSession }, applicationVerifier);
                        } else {//sign in
                            verificationId = await phoneAuthProvider.verifyPhoneNumber(new PhoneMultiFactorSignInInfoOptions() { multiFactorHint = resolver.hints[0], session = resolver.session }, applicationVerifier);
                        }

                        verificationCode.interactable = confirmVerificationCode.interactable = true;
                    } catch (AuthError e) {
                        confirmPhoneNumber.interactable = phoneNumber.interactable = true;
                        Debug.LogError($"failed to sign in with phone number: {e}");
                    }
                });

                confirmVerificationCode.onClick.AddListener(async () => {
                    try {
                        verificationCode.interactable = confirmVerificationCode.interactable = false;

                        var cred = PhoneAuthProvider.credential(verificationId, verificationCode.text);
                        var multiFactorAssertion = PhoneMultiFactorGenerator.assertion(cred);

                        if (resolver is null) {//enroll
                            await Au.multiFactor(user).enroll(multiFactorAssertion, "multifactorUser");
                            //don't want to clear the user reference on this specific case, since I want to sign in right after and it would throw 'User not found' error in the update method when checking if the email is verified.
                            //Usually, you'd leave autoClear set to true as explained in the docs.
                            await Au.signOut(auth, false);
                            #pragma warning disable CS0618
                            Application.ExternalEval($"alert('enrolled {email.text} with phone number and signed out.')");
                            createUser.GetComponentInChildren<Text>().text = "sign in";
                            createUser.interactable = true;
                        } else {//sign in
                            var uc = await resolver.resolveSignIn(multiFactorAssertion);
                            #pragma warning disable CS0618
                            Application.ExternalEval($"alert('user {email.text} signed in with multifactor.')");
                        }


                    } catch(AuthError e) {
                        verificationCode.interactable = confirmVerificationCode.interactable = true;
                        Debug.LogError($"Failed to get user credential: {e}");
                    }
                });

            }

            private void OnDestroy() {
                //After you're done with the application verifier, you should clear it at some point.
                applicationVerifier.clear();
            }

            private void Update() {
                if (user?.emailVerified is true && verifyEmail.activeSelf is true) {
                    verifyEmail.SetActive(false);
                    ((Action)(async () => {
                        try {
                            multiFactorSession = await Au.multiFactor(user).getSession();
                            confirmPhoneNumber.interactable = phoneNumber.interactable = true;
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
