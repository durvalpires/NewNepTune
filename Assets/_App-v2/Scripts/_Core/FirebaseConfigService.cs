using UnityEngine;
using Extensions;
using MarksAssets.FirebaseWebGL.App;

#if UNITY_WEBGL
using MarksAssets.FirebaseWebGL.Examples;


namespace _App_v2.Scripts._Core.Firebase.Config
{
    public class FirebaseConfigService : MonoSingleton<FirebaseConfigService>
    {
        [SerializeField] private FirebaseConfigObject firebaseConfig;
        [SerializeField] private EmulatorConfigObject emulatorConfig;

       
        public FirebaseConfigObject CurrentConfig => firebaseConfig;

        public EmulatorConfigObject CurrentEmulatorConfig => emulatorConfig;

#if UNITY_WEBGL
      
        public FirebaseOptions GetFirebaseOptions()
        {
            if (firebaseConfig == null)
            {
                Debug.LogError("FirebaseConfigObject is not available");
                return null;
            }

            return new FirebaseOptions
            {
                apiKey = firebaseConfig.apiKey,
                authDomain = firebaseConfig.authDomain,
                databaseURL = firebaseConfig.databaseURL,
                projectId = firebaseConfig.projectId,
                storageBucket = firebaseConfig.storageBucket,
                messagingSenderId = firebaseConfig.messagingSenderId,
                appId = firebaseConfig.appId
            };
        }
#endif

       
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            ValidateConfiguration();
        }

        
        private void ValidateConfiguration()
        {
            if (firebaseConfig == null)
            {
                Debug.LogError("not assigned to FirebaseConfigService");
            }

           
        }

        public void UpdateFirebaseConfig(FirebaseConfigObject config)
        {
            if (config == null)
            {
                Debug.LogError("Attempted to update Firebase config with null value");
                return;
            }

            firebaseConfig = config;
            Debug.Log("Firebase configuration updated");
        }

      
        public void UpdateEmulatorConfig(EmulatorConfigObject config)
        {
            emulatorConfig = config;
            Debug.Log("Firebase emulator configuration updated");
        }
    }
}
#endif