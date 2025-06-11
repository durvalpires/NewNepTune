using MarksAssets.FirebaseWebGL.Auth;
using UnityEngine;
using UnityEngine.UI;
using Au = MarksAssets.FirebaseWebGL.Auth.Auth;

/*  == BEFORE YOU BEGIN ==
 * Enable the Auth product and Phone sign-in method on your Firebase Console or emulator.
 * To use recaptcha,you have to use the widget mode https://firebase.google.com/docs/auth/web/phone-auth#use-the-recaptcha-widget
 * Which means, on the index.html of your build folder or custom webgl template, you have to add the following div
 * <div id="recaptcha-container" style="z-index: 1; position: absolute; "></div>
 * Make sure you cleared all the users (no users) for a clean state for testing.
 * Make sure you have 'auth' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/auth/web/phone-auth .
 * This example has PARTIAL emulator support. I recommend that you don't use it at all. Instead, I recommend using phone numbers for testing. See https://firebase.google.com/docs/auth/web/phone-auth#create-fictional-phone-numbers-and-verification-codes
 * More info about the emulator issues and other observations below.
 * 
 * == OTHER OBSERVATIONS ==
 * If you want to make the recaptcha invisible, setting the size to "invisible" as described here https://firebase.google.com/docs/auth/web/phone-auth#use-invisible-recaptcha
 * only hides the "I'm not a robot" text. See https://developers.google.com/recaptcha/docs/versions#recaptcha_v2_invisible_recaptcha_badge. 
 * To actually make it invisible, you have to set the recaptcha size to "invisible" AND use the following div instead of the aforementioned one.
 * <div id="recaptcha-container" style="z-index: 1; position: absolute; visibility: hidden;"></div>
 * If you don't want to make it invisible and you are not happy with the position it shows up, you can style the div yourself.
 * To style the div yourself, just make sure badge is set to "inline" and then apply your own CSS to the div. Example <div id="recaptcha-container" style="z-index: 1; position: absolute; top: 50%; left: 50% "></div>
 * Note that if you set the size to "invisible" you can use the badge property that
 * has a few alignment options out of the box that may work for you https://developers.google.com/recaptcha/docs/invisible#render_param, so you wouldn't have to style your own div.
 * Please note that when using the emulator, recaptcha is disabled and always resolves https://firebase.google.com/docs/emulator-suite/connect_auth#emulated_phonesms_authentication
 * Also, there's a bug with the emulator where you can't call signInWithPhoneNumber more than once even if you type an invalid number. See https://github.com/firebase/firebase-js-sdk/issues/7646
 * If you use this example with the emulator, it will seem that the example is bugged as all input fields and buttons will become grayed out after the first try or if you type an invalid
 * phone number, the confirmation code field and button will never become enabled. So the emulator works ONLY on the first try, and ONLY if you don't fill in an invalid phone number by mistake.
*/
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Auth {

        public class PhoneAuthentication : FirebaseExample {
            public InputField phoneNumber, verificationCode;
            public Button confirmPhoneNumber, confirmVerificationCode;
            public string recaptchaSize = "invisible";//possible values are "compact", "normal" and "invisible"
            public string recaptchaBadge = "bottomright";//possible values https://developers.google.com/recaptcha/docs/invisible#render_param . Badge only applies if size is set to "invisible"
            public string recaptchaTheme = "light";//possible values https://developers.google.com/recaptcha/docs/display#render_param

            private RecaptchaVerifier applicationVerifier = null;

            async void Start() {
                var auth = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (auth is null) return;

                
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
                    }
                    return;
                }

                var token = await applicationVerifier.verify();
                Debug.Log("application verified");
                ConfirmationResult confirmationResult = null;

                confirmPhoneNumber.onClick.AddListener(async () => {
                    try {
                        confirmPhoneNumber.interactable = phoneNumber.interactable = false;
                        confirmationResult = await Au.signInWithPhoneNumber(auth, phoneNumber.text, applicationVerifier);
                        verificationCode.interactable = confirmVerificationCode.interactable = true;
                    } catch(AuthError e) {
                        confirmPhoneNumber.interactable = phoneNumber.interactable = true;
                        Debug.LogError($"failed to sign in with phone number: {e}");
                    }
                });

                confirmVerificationCode.onClick.AddListener(async () => {
                    try {
                        verificationCode.interactable = confirmVerificationCode.interactable = false;
                        var userCredential = await confirmationResult.confirm(verificationCode.text);
                        confirmPhoneNumber.interactable = phoneNumber.interactable = true;
                        #pragma warning disable CS0618
                        Application.ExternalEval($"alert('{userCredential.user.phoneNumber} signed in')");
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

        }
    }
}
