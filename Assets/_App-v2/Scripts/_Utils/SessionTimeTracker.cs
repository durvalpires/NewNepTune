using System;
using UnityEngine;

public class SessionTimeTracker : MonoBehaviour
{
    private DateTime sessionStartTime;
    private TimeSpan sessionDuration;

    public static double TotalSecondsPlayed { get; private set; }
    

    private void OnEnable()
    {
        sessionStartTime = DateTime.UtcNow;
    }

    private void OnDisable()
    {
        UpdateSessionDuration();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
            UpdateSessionDuration();
        else
            sessionStartTime = DateTime.UtcNow;
    }

    private void OnApplicationQuit()
    {
        UpdateSessionDuration();
    }

    private void UpdateSessionDuration()
    {
        sessionDuration = DateTime.UtcNow - sessionStartTime;
        TotalSecondsPlayed += sessionDuration.TotalSeconds;
        SaveTotalPlayTime();
    }

    private void SaveTotalPlayTime()
    {
        PlayerPrefs.SetFloat("TotalSecondsPlayed", (float)TotalSecondsPlayed);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        TotalSecondsPlayed = PlayerPrefs.GetFloat("TotalSecondsPlayed", 0f);
        Debug.Log("TotalSecondsPlayed: " + TotalSecondsPlayed);
    }
    
    /// <summary>
    /// Gets the total time played formatted as HH:MM:SS
    /// </summary>
    /// <returns>Formatted time string in HH:MM:SS format</returns>
    public static string GetFormattedTimePlayed()
    {
        TimeSpan timePlayed = TimeSpan.FromSeconds(TotalSecondsPlayed);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", 
            timePlayed.Hours + (timePlayed.Days * 24), 
            timePlayed.Minutes, 
            timePlayed.Seconds);
    }
    
    /// <summary>
    /// Gets the total time played formatted as HH:MM:SS
    /// </summary>
    /// <returns>Formatted time string in HH:MM:SS format</returns>
    public static string GetFormattedTimePlayed(double totalSecondsPlayed)
    {
        TimeSpan timePlayed = TimeSpan.FromSeconds(totalSecondsPlayed);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", 
            timePlayed.Hours + (timePlayed.Days * 24), 
            timePlayed.Minutes, 
            timePlayed.Seconds);
    }
}