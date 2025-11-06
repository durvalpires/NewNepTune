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
        public bool IsFullyInitialized { get; private set; } = false;
        protected override void Awake()
        {
            base.Awake();

            DontDestroyOnLoad(gameObject);

            Debug.Log("[FireService] Awake - Initializing AnalyticsService synchronously");

            // Initialize AnalyticsService
            AnalyticsService = new AnalyticsService();
            AnalyticsService.AddProvider(new DebugProvider());
            AnalyticsService.Initialize();

            Debug.Log("[FireService] AnalyticsService initialized in Awake - ready for immediate use");
        }

        private async void Start()
        {
            Debug.Log("Start Async FireService Initiated");

            // authServicesFactory = new AuthServicesFactory();
            // Debug.Log("AuthServicesFactory created");
            // Auth = authServicesFactory.Create();

#if UNITY_WEBGL && !UNITY_EDITOR
            Debug.Log("[FireService] Initializing for WEBGL build");

            try
            {
                var config = FirebaseConfigService.Instance.CurrentConfig;
                var emulatorConfig = FirebaseConfigService.Instance.CurrentEmulatorConfig;

                Debug.Log("[FireService] Waiting for modules to load...");
                await Ap.modulesLoaded();
                Debug.Log("[FireService] Modules loaded successfully");

                Debug.Log("[FireService] Initializing Firebase app...");
                WebGLFirebaseApp = Ap.initializeApp(new FirebaseOptions
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


                AnalyticsService.AddProvider(new FirebaseAnalyticsProvider());
                AnalyticsService.Initialize();
                Debug.Log("[FireService] FirebaseAnalyticsProvider added for WebGL");

                analyticsServiceWebGL = new AnalyticsServiceWebGL();
                await analyticsServiceWebGL.InitializeAsync(config, emulatorConfig);
                analyticsServiceWebGL.LogEvent("app_started");
                Debug.Log($"FireService Instance id = {GetInstanceID()}");

                // Check if Auth is initialized before subscribing
                if (Auth != null)
                {
                    Auth.OnAuthStateChanged += HandleAuth;
                    HandleAuth();
                }
                else
                {
                    Debug.LogWarning("[FireService] Auth is not initialized for WEBGL");
                }

                DB = new FirebaseWebGLDataPersistence(config, emulatorConfig);
                Debug.Log("[FireService] Initialization complete!");

                IsFullyInitialized = true;
                Debug.Log("[FireService] IsFullyInitialized set to TRUE");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"[FireService] CRITICAL ERROR during initialization: {ex.Message}");
                Debug.LogError($"[FireService] Stack trace: {ex.StackTrace}");
                Debug.LogError($"[FireService] Exception type: {ex.GetType().Name}");
            }

#else
            Debug.Log("[FireService] Initializing for non-WebGL or Editor mode");

            #if !UNITY_WEBGL
            _crashlytics = new CrashlyticsService();
            DB = new DBService();
            #endif

            // AnalyticsService already initialized in Awake()

            #if !UNITY_WEBGL
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(HandleDependenciesResult);
            #endif
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

                // Add FirebaseAnalyticsProvider (DebugProvider was already added in Awake)
                AnalyticsService.AddProvider(new FirebaseAnalyticsProvider());
                AnalyticsService.Initialize();
                Debug.Log("[FireService] FirebaseAnalyticsProvider added in Initialize()");

                // Mark as fully initialized
                IsFullyInitialized = true;
                Debug.Log("[FireService] IsFullyInitialized set to TRUE");
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