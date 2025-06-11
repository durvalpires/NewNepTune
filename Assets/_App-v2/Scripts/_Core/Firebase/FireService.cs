using System.Threading.Tasks;
using _App_v2.Scripts._Core.Firebase.Analytics;
using _App_v2.Scripts._Core.Firebase.Analytics.Interfaces;
using _App_v2.Scripts._Core.Firebase.Analytics.Providers.Debug;
using _App_v2.Scripts._Core.Firebase.Analytics.Providers.Firebase;
using _App_v2.Scripts._Core.Firebase.Auth;
using _App_v2.Scripts._Core.Firebase.Auth.Interfaces;
using _App_v2.Scripts._Core.Firebase.Crashlytics;
using _App_v2.Scripts._Core.Firebase.Databases.Interfaces;

#if !UNITY_WEBGL
using Firebase;
using FirebaseApp = Firebase.FirebaseApp;
#endif

using Extensions;
using UnityEngine;
using _App_v2.Scripts._Core.Firebase.Analytics.Implementations;
using _App_v2.Scripts._Core.Firebase.Databases;

#if UNITY_WEBGL
using MarksAssets.FirebaseWebGL.App;
using Ap = MarksAssets.FirebaseWebGL.App.App;
using _App_v2.Scripts._Core.Firebase.Config;
#endif

namespace _App_v2.Scripts._Core.Firebase
{
    public class FireService : MonoSingleton<FireService>
    {
#if UNITY_WEBGL 
        public FirebaseApp WebGLFirebaseApp { get; private set; }
#endif

        //  INTERNALLY SHOULD BE PREPARED TO IGNORE WEBGL
        private CrashlyticsService _crashlytics;

        public IAnalyticsService AnalyticsService { get; private set; }
        public IAuthService Auth { get; private set; }
        private AuthServicesFactory authServicesFactory;
        private AnalyticsServiceWebGL analyticsServiceWebGL;

        public IDBService DB { get; private set; }

        private async void Start()
        {
            Debug.Log("Start Async FireService Initiated");
#if UNITY_WEBGL
            var config = FirebaseConfigService.Instance.CurrentConfig;
            var emulatorConfig = FirebaseConfigService.Instance.CurrentEmulatorConfig;
            await Ap.modulesLoaded();
             WebGLFirebaseApp = await Ap.initializeAppTask(new FirebaseOptions
             {
                 apiKey = config.apiKey,
                 appId = config.appId,
                 authDomain = config.authDomain,
                 projectId = config.projectId,
                 storageBucket = config.storageBucket,
                 messagingSenderId = config.messagingSenderId,
                 databaseURL = config.databaseURL
             });
            Debug.Log("Firebase initialized: " + WebGLFirebaseApp.name + WebGLFirebaseApp!=null);
            Debug.Log(config.databaseURL);
#endif

            // authServicesFactory = new AuthServicesFactory();
            // Debug.Log("AuthServicesFactory created");
            // Auth = authServicesFactory.Create();
            
#if UNITY_WEBGL

            analyticsServiceWebGL = new AnalyticsServiceWebGL();
            await analyticsServiceWebGL.InitializeAsync(config, emulatorConfig);
            analyticsServiceWebGL.LogEvent("app_started");
            Debug.Log($"FireService Instance id = {GetInstanceID()}");
            Auth.OnAuthStateChanged += HandleAuth;
            DB = new FirebaseWebGLDataPersistence(config, emulatorConfig);
            HandleAuth();

#else
            _crashlytics = new CrashlyticsService();
            AnalyticsService = new AnalyticsService();
            DB = new DBService();
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(HandleDependenciesResult);
#endif
        }


#if !UNITY_WEBGL
        private void HandleDependenciesResult(Task<DependencyStatus> task)
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                Initialize();
            }
            else
            {
                Debug.LogError($"Could not resolve all Firebase dependencies: {dependencyStatus}");
            }
        }
#endif


        private void Initialize()
        {
            try
            {
                _crashlytics.Initialize();
                Auth.OnAuthStateChanged += HandleAuth;

                AnalyticsService.AddProvider(new DebugProvider());
                AnalyticsService.AddProvider(new FirebaseAnalyticsProvider());
                AnalyticsService.Initialize();
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to initialize Firebase services: {e.Message}");
            }
        }

        private void HandleAuth()
        {
            if (!Auth.SignedIn)
            {
                DB.Disconnect();
                return;
            }

            var uid = Auth.GetUserId();
            if (string.IsNullOrEmpty(uid))
            {
                Debug.LogError("User ID is null or empty");
                return;
            }

            if (DB.IsConnected)
            {
                DB.Disconnect();
            }

            DB.Connect(uid);
        }

#if !UNITY_WEBGL
        private void OnDestroy()
        {
            PlayerModelBase.SaveData();
            _crashlytics?.Dispose();
        }
#else
        private void OnApplicationQuit()
        {
            PlayerModelBase.SaveData();
            _crashlytics?.Dispose();
        }
#endif
    }
}