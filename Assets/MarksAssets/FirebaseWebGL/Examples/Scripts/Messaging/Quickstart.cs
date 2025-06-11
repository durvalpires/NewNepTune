using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Me = MarksAssets.FirebaseWebGL.Messaging.Messaging;
using MarksAssets.FirebaseWebGL.Messaging;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Messaging product on your firebase console.
 * Make sure you have 'messaging' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Follow https://firebase.google.com/docs/cloud-messaging/js/client
 * Make sure you have your VAPID key, that you can get from project settings -> Cloud Messaging -> Web Push certificates -> Key pair.
 * Make sure you have the firebase-messaging-sw.js set up on your build folder.
 * You can enter your VAPID key in the inspector.
 * Leave your browser's javascript console (web inspector) open to check the results and if there are any errors in the process.
 * This example does NOT support the emulator. Messaging is not on the supported list. See https://firebase.google.com/docs/emulator-suite#feature-matrix .
 * 
 * Below you can find an example firebase-messaging-sw.js file that you can use. Copy paste its contents into your firebase-messaging-sw.js file.
 * Based on https://firebase.google.com/docs/cloud-messaging/js/receive#setting_notification_options_in_the_service_worker
 * 
 * == START firebase-messaging-sw.js ==
 * 
 * importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-app-compat.js');
 * importScripts('https://www.gstatic.com/firebasejs/10.7.1/firebase-messaging-compat.js');
 * firebase.initializeApp({//fill in your app config settings below
 *      apiKey: xxxxx,
 *      authDomain: xxxxxx,
 *      databaseURL: xxxx,
 *      projectId: xxxx,
 *      storageBucket: xxxxx,
 *      messagingSenderId: xxxxx,
 *      appId: xxxxx,
 * });
 *  const messaging = firebase.messaging();
 *  
 *  //https://firebase.google.com/docs/cloud-messaging/concept-options
 *  messaging.onBackgroundMessage(function(payload) {
 *      console.log('[firebase-messaging-sw.js] Received background message ', payload);
 *      const notificationTitle = 'Background Message Title';
 *      const notificationOptions = { body: 'Background Message body.' };
 *      self.registration.showNotification(notificationTitle, notificationOptions);
 *  });
 *  
 *  == END firebase-messaging-sw.js ==
 *  
 *  
 *  == OTHER OBSERVATIONS ==
 *  
 * I could only make the test message work by using 2 browsers:
 * 1- First open this example on one browser. Say, Chrome. Tap on request permission, get the token that prints on the console. Now, minimize this browser. 
 * 2- Open another browser. Say, Safari. Now send the test message from the firebase console. The notification should show up. 
 * 
 * If you use the same browser for the whole process, even if using different windows, it might not work. It didn't for me.
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Messaging {
        public class Quickstart : FirebaseExample {

            public string vapidKey;

            //https://firebase.google.com/docs/cloud-messaging/js/first-message
            async void Start() {
                var messaging = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (messaging is null) return;

                bool isSupported = await Me.isSupported();
                if (isSupported == false) {
                    Debug.LogError("Messaging is not supported");
                    return;
                }

                //See GoogleAuthenticationPopup.cs for the reason of the setup below.
                var requestPermission = FindObjectOfType<Button>();
                requestPermission.GetComponent<EventTrigger>().triggers[0].callback.AddListener(e => {//get pointerdown trigger and add listener
                    Util.Util.runOnPointerDown(async () => {
                        try {
                            var permission = await Me.requestPermission();
                            if (permission == "granted") {
                                try {
                                    var currentToken = await Me.getToken(messaging, new GetTokenOptions() { vapidKey = vapidKey });
                                    Debug.Log("== CURRENT TOKEN ==");
                                    Debug.Log(currentToken);
                                } catch (Util.FirebaseError e) {
                                    Debug.LogError($"Could not get token: {e}");
                                }
                            } else if (permission == "denied") {
                                throw new Exception("Permission to use messaging was denied.");
                            }
                        } catch(Exception e) {
                            Debug.LogError($"Could not get permission: {e}");
                        }

                    });
                });

                //https://firebase.google.com/docs/cloud-messaging/js/receive#handle_messages_when_your_web_app_is_in_the_foreground
                var unsub = Me.onMessage(messaging, payload => {
                    Debug.Log($"Message received on foreground. ==Title==: {payload.notification.title} ==Body==: {payload.notification.body}");
                });
            }
        }
    }

    
}
