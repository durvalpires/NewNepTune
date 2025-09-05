using System;
using System.Collections;
using MarksAssets.FirebaseWebGL.Storage;
using UnityEngine;
using UnityEngine.Networking;
using St = MarksAssets.FirebaseWebGL.Storage.Storage;

/*
 * == BEFORE YOU BEGIN ==
 * Enable the Storage product on your firebase console or emulator.
 * Make sure the security rules allow all reads and writes for testing purposes.
 * Make sure you cleared all the data for a clean state for testing.
 * Make sure you have 'storage' set to true on the 'modules.jspre' file under Assets/MarksAssets/FirebaseWebGL/Plugins/Core.
 * Leave your browser's javascript console (web inspector) open to check if there are any errors in the process.
 * Now, follow https://firebase.google.com/docs/database/web/read-and-write
 * 
 */
namespace MarksAssets.FirebaseWebGL.Examples {
    namespace Storage {
        public class Quickstart : FirebaseExample {

            private IEnumerator GetRequest(string uri) {
                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) {
                    // Request and wait for the desired page.
                    yield return webRequest.SendWebRequest();

                    string[] pages = uri.Split('/');
                    int page = pages.Length - 1;

                    switch (webRequest.result) {
                        case UnityWebRequest.Result.ConnectionError:
                        case UnityWebRequest.Result.DataProcessingError:
                            Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.ProtocolError:
                            Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                            break;
                        case UnityWebRequest.Result.Success:
                            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                            break;
                    }

                }
            }

            async void Start() {
                var storage = await CommonSetup.setup(firebaseConfig, emulatorConfig);
                if (storage is null) return;

                var bytes = new byte[] { 0x68, 0x65, 0x6c, 0x6c, 0x6f, 0x20, 0x77, 0x6f, 0x72, 0x6c, 0x64, 0x21 };
                var metadata = new UploadMetadata() { contentType = "text/plain" };//https://firebase.google.com/docs/storage/web/upload-files#add_file_metadata

                UploadTask uploadTask = null;

                //https://firebase.google.com/docs/storage/web/upload-files#upload_from_a_byte_array
                try {
                    uploadTask = St.uploadBytesResumable(St.Ref(storage, "helloWorld"), bytes, new UploadMetadata() { contentType = "text/plain" });
                    //The below also works. If you don't care about monitoring the upload progress and only the result (if successful or not), you can just call await.
                    //UploadTaskSnapshot uploadTask = await St.uploadBytesResumable(St.Ref(storage, "helloWorld"), bytes, new UploadMetadata() { contentType = "text/plain" });

                } catch (StorageError e) {
                    Debug.LogError(e);
                }

                //https://firebase.google.com/docs/storage/web/upload-files#full_example
                //The listeners are automatically detached once the error or complete runs, so in the example below you don't have to call unsub().
                //If you don't want to define an error or complete callback, make sure to call unsub() later to make sure all callbacks are properly detached.
                var unsub = uploadTask.on(TaskEvent.STATE_CHANGED, res => {
                    var progress = ((float)res.bytesTransferred / (float)res.totalBytes) * 100;

                    Debug.Log("Upload is " + progress + "% done");
                    switch (res.state) {
                        case TaskState.PAUSED:
                            Debug.Log("Upload is paused");
                            break;
                        case TaskState.RUNNING:
                            Debug.Log("Upload is running");
                            break;
                    }

                }, err => {
                    switch (err.code) {
                        case StorageErrorCode.UNAUTHORIZED: Debug.LogError($"unauthorized: {err}"); break;
                        case StorageErrorCode.CANCELED: Debug.LogError($"cancelled: {err}"); break;
                        default: Debug.LogError($"Could not upload byte array file: {err}"); break;
                    }

                }, async () => {
                    Debug.Log("==BYTE ARRAY UPLOAD COMPLETED==");

                    //Internally, the uploadTask is disposed of on the next frame after completion.
                    //It was my design decision that the user doesn't have to clear the reference of uploadTask, because I can 100% determine when it should happen.
                    //However, on the code below I call await on the same uploadTask beacuse I want to download the file I just uploaded,
                    //so I have to store the information I want to use beforehand to avoid getting the 'UploadTask not found' error.

                    StorageReference @ref = uploadTask.snapshot.Ref;//if you use 'uploadTask.snapshot.Ref' on the code below directly you will get the aforementioned error.

                    try {
                        //https://firebase.google.com/docs/storage/web/download-files#download_data_via_url
                        var downloadUrl = await St.getDownloadURL(@ref);
                        Debug.Log("==DOWNLOADURL==");
                        Debug.Log(downloadUrl);
                        StartCoroutine(GetRequest(downloadUrl));//download file with Unity's webrequest, but there's a method that can be used
                        //for this, which is getBytes. See below.

                        try {
                            var bytes = await St.getBytes(@ref);
                            Debug.Log("==BYTES==");
                            Debug.Log(Newtonsoft.Json.JsonConvert.SerializeObject(Array.ConvertAll(bytes, c => (int)c)));//byte arrays are converted to base64 strings when serialized. So I convert the byte array to an int array first.
                        } catch(StorageError e) {
                            Debug.LogError($"Could not get bytes: {e}");
                        }
                    } catch (StorageError e) {
                        Debug.LogError($"Could not get downloadUrl: {e}");
                    }
                });

                //uploadTask can also be awaited. If you don't want to monitor the uploadTask and just care if it uploaded successfully or not
                //just use await uploadBytesResumable()
                await uploadTask;

                //https://firebase.google.com/docs/storage/web/upload-files#upload_from_a_string
                // Raw string is the default if no format is provided
                var message = "This is my message.";
                await St.uploadString(St.Ref(storage, "raw"), message);
                Debug.Log("uploaded a raw string!");

                // Base64 formatted string
                message = "5b6p5Y+344GX44G+44GX44Gf77yB44GK44KB44Gn44Go44GG77yB";
                await St.uploadString(St.Ref(storage, "base64"), message);
                Debug.Log("Uploaded a base64 string!");

                // Base64 formatted string
                message = "5b6p5Y-344GX44G-44GX44Gf77yB44GK44KB44Gn44Go44GG77yB";
                await St.uploadString(St.Ref(storage, "Base64url"), message);
                Debug.Log("Uploaded a base64url string!");

                // Base64 formatted string
                message = "data:text/plain;base64,5b6p5Y+344GX44G+44GX44Gf77yB44GK44KB44Gn44Go44GG77yB";
                await St.uploadString(St.Ref(storage, "data_url"), message);
                Debug.Log("Uploaded a data_url string!");

                //https://firebase.google.com/docs/storage/web/list-files#list_all_files
                try {
                    var list = await St.listAll(St.Ref(storage));
                    Debug.Log("== ALL ITEMS ==");
                    foreach(var item in list.items) {
                        Debug.Log(item.fullPath);
                    }
                    
                } catch(StorageError e) {
                    Debug.LogError($"could not get list: {e}");
                }


                await St.deleteObject(St.Ref(storage, "data_url"));
                Debug.Log("Deleted the data_url string!");
            }
        }
    }

    
}
