using System;
using UnityEngine;
#if UNITY_WEBGL && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

public class SessionTimeTracker : MonoBehaviour
{

    public enum SaveMode
    {
        AutoSave,   // periodically saves based on interval
        ManualSave  // only saves when SaveNow() is called
    }

    private static SessionTimeTracker instance;

    [Header("Session Tracking Settings")]
    [SerializeField] private SaveMode saveMode = SaveMode.AutoSave;
    [SerializeField] private float autoSaveIntervalSeconds = 60f;

    private DateTime sessionStartTime;
    private TimeSpan sessionDuration;
    private float autoSaveTimer;
    private bool trackingActive = false; // ✅ NEW: only true after login

    public static double TotalMinutesPlayed { get; private set; }

#if UNITY_WEBGL && !UNITY_EDITOR
    // import JavaScript function from VisibilityTracker.jslib
    [DllImport("__Internal")]
    private static extern void RegisterVisibilityCallback(string gameObjectName);
#endif
    [Header("Hidden Tab Tracking")]
    private bool isTimerPaused = false;
    private DateTime pauseTime;
    private TimeSpan accumulatedPausedTime;

    private void Awake()
    {
        // ✅ Singleton guard
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        Debug.Log("[SessionTimeTracker] Initialized. Waiting for login...");
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // register JavaScript callbacks for browser tab visibility changes
        RegisterVisibilityCallback(gameObject.name);
        Debug.Log("[SessionTimeTracker] WebGL visibility callbacks registered for: " + gameObject.name);
#endif
    }

    private void OnEnable()
    {
        autoSaveTimer = 0f;
    }

    private void Update()
    {
        if (!trackingActive) return; // ✅ Skip if not logged in
        if (isTimerPaused) return; // skip if timer is paused

        if (saveMode == SaveMode.AutoSave)
        {
            autoSaveTimer += Time.unscaledDeltaTime;
            if (autoSaveTimer >= autoSaveIntervalSeconds)
            {
                autoSaveTimer = 0f;
                UpdateSessionDuration();
            }
        }
    }

    private void OnDisable()
    {
        if (!trackingActive) return;
        Debug.Log("[SessionTimeTracker] OnDisable: Updating session duration");
        UpdateSessionDuration(saveMode == SaveMode.AutoSave);
    }

    private void OnApplicationPause(bool pause)
    {
        if (!trackingActive) return;

        if (pause)
        {
            Debug.Log("[SessionTimeTracker] App paused: Updating session duration");
            UpdateSessionDuration(saveMode == SaveMode.AutoSave);
        }
        else
        {
            sessionStartTime = DateTime.UtcNow;
        }
    }

    private void OnApplicationQuit()
    {
        if (!trackingActive) return;
        Debug.Log("[SessionTimeTracker] App quit: Updating session duration");
        UpdateSessionDuration(saveMode == SaveMode.AutoSave);
    }

    /// <summary>
    /// Called after login to start tracking and set initial play time.
    /// </summary>
    public static void StartTracking(double minutesFromBackend)
    {
        if (instance == null)
        {
            Debug.LogWarning("[SessionTimeTracker] StartTracking() called but no instance exists.");
            return;
        }

        TotalMinutesPlayed = minutesFromBackend;
        instance.sessionStartTime = DateTime.UtcNow;
        instance.trackingActive = true;

        // reset webgl pause tracking for new session
        instance.isTimerPaused = false;
        instance.accumulatedPausedTime = TimeSpan.Zero;

        Debug.Log($"[SessionTimeTracker] Tracking started. Loaded total play time: {TotalMinutesPlayed} minutes");
    }

    /// <summary>
    /// Stops tracking (e.g., on logout)
    /// </summary>
    public static void StopTracking(bool saveBeforeStop = true)
    {
        if (instance == null) return;

        if (saveBeforeStop)
            instance.UpdateSessionDuration(true);

        instance.trackingActive = false;
        Debug.Log("[SessionTimeTracker] Tracking stopped.");
    }

    /// <summary>
    /// Updates session time and optionally saves to Firebase
    /// </summary>
    private void UpdateSessionDuration(bool saveToFirebase = true)
    {
        sessionDuration = DateTime.UtcNow - sessionStartTime;

        if (sessionDuration.TotalSeconds > 1)
        {
            TotalMinutesPlayed += sessionDuration.TotalMinutes;
            if (saveToFirebase) SaveTotalPlayTime();
        }

        sessionStartTime = DateTime.UtcNow;
    }

    private void SaveTotalPlayTime()
    {
        int roundedMinutes = Mathf.RoundToInt((float)TotalMinutesPlayed);
        Debug.Log($"[SessionTimeTracker] Saving TotalMinutesPlayed = {roundedMinutes}");
        FirebaseProxyService.Instance.UpdateStudentTotalTime(roundedMinutes);
    }

    /// <summary>
    /// Forces a save (used in ManualSave mode or when player finishes a level)
    /// </summary>
    public static void SaveNow()
    {
        if (instance == null || !instance.trackingActive)
        {
            Debug.LogWarning("[SessionTimeTracker] SaveNow() called but tracking is not active.");
            return;
        }

        instance.UpdateSessionDuration(true);
    }

    public static string GetFormattedTimePlayed()
    {
        TimeSpan timePlayed = TimeSpan.FromMinutes(TotalMinutesPlayed);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timePlayed.Days * 24 + timePlayed.Hours,
            timePlayed.Minutes,
            timePlayed.Seconds);
    }

    public static string GetFormattedTimePlayed(double totalMinutesPlayed)
    {
        TimeSpan timePlayed = TimeSpan.FromMinutes(totalMinutesPlayed);
        return string.Format("{0:D2}:{1:D2}:{2:D2}",
            timePlayed.Days * 24 + timePlayed.Hours,
            timePlayed.Minutes,
            timePlayed.Seconds);
    }

    #region WebGL Browser Visibility Callbacks

    private void OnBrowserTabHidden()
    {
        if (!trackingActive) return;

        Debug.Log("[SessionTimeTracker] Browser tab HIDDEN - Pausing timer and saving session");
        PauseTimer();
        UpdateSessionDuration(saveToFirebase: true);
    }

    private void OnBrowserTabVisible()
    {
        if (!trackingActive) return;

        Debug.Log("[SessionTimeTracker] Browser tab VISIBLE - Resuming timer");
        ResumeTimer();
    }

    private void OnBrowserClosing()
    {
        if (!trackingActive) return;

        Debug.Log("[SessionTimeTracker] Browser CLOSING - Emergency save");
        UpdateSessionDuration(saveToFirebase: true);
    }

    private void PauseTimer()
    {
        if (isTimerPaused) return; 

        pauseTime = DateTime.UtcNow;
        isTimerPaused = true;

        Debug.Log($"[SessionTimeTracker] Timer PAUSED at {pauseTime:HH:mm:ss}");
    }

    private void ResumeTimer()
    {
        if (!isTimerPaused) return; 

        // calculate how long we were paused
        TimeSpan pausedDuration = DateTime.UtcNow - pauseTime;
        accumulatedPausedTime += pausedDuration;

        // adjust session start time to account for paused period
        // counting only active time
        sessionStartTime = sessionStartTime.Add(pausedDuration);

        isTimerPaused = false;

        Debug.Log($"[SessionTimeTracker] Timer RESUMED after {pausedDuration.TotalSeconds:F1} seconds paused. Total paused time: {accumulatedPausedTime.TotalMinutes:F2} minutes");
    }

    #endregion
}