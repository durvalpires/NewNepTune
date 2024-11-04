// using UnityEngine;
// using UnityEngine.UI;
// using UnityEngine.SceneManagement;
// using System.Collections;
// using Firebase.Messaging;

// public class LoadingProcessControllerSC : MonoBehaviour
// {
//     public Slider progressBar;

//     void Start()
//     {
//         Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(continuationAction: task=>
//         {
//            FirebaseMessaging.TokenReceived += TokenReceived;
//            FirebaseMessaging.MessageReceived += MessageReceived; 
//         });
//         StartCoroutine(LoadAsyncOperation());
//     }

//     private void TokenReceived(object sender, TokenReceivedEventArgs e)
//     {
//         Debug.Log("TokenReceived: " + e.Token);
//     }
//     private void MessageReceived(object sender, MessageReceivedEventArgs e)
//     {
//         Debug.Log("MessageReceived: " + e.Message);
//     }

//     IEnumerator LoadAsyncOperation()
//     {
//         // Burada asıl yüklenecek sahnenin ismini belirtin
//         AsyncOperation gameLevel = SceneManager.LoadSceneAsync(1);

//         while (!gameLevel.isDone)
//         {
//             progressBar.value = gameLevel.progress;
//             //yield return new WaitForEndOfFrame();
//             yield return null;
//         }
//     }
// }
