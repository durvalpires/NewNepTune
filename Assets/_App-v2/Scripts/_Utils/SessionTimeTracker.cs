using System;
using UnityEngine;

public class SessionTimeTracker : MonoBehaviour
{
    private DateTime sessionStartTime;
    private TimeSpan sessionDuration;

    public static double TotalMinutesPlayed { get; private set; }
    

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
        TotalMinutesPlayed += Math.Round(sessionDuration.TotalMinutes, 1);
        SaveTotalPlayTime();
    }

    private void SaveTotalPlayTime()
    {
        PlayerPrefs.SetFloat("TotalMinutesPlayed", (float)TotalMinutesPlayed);
        PlayerPrefs.Save();
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        TotalMinutesPlayed = PlayerPrefs.GetFloat("TotalMinutesPlayed", 0f);
        Debug.Log("TotalMinutesPlayed: " + TotalMinutesPlayed);
    }
    
    /// <summary>
    /// Gets the total time played formatted as HH:MM:SS
    /// </summary>
    /// <returns>Formatted time string in HH:MM:SS format</returns>
    public static string GetFormattedTimePlayed()
    {
        TimeSpan timePlayed = TimeSpan.FromMinutes(TotalMinutesPlayed);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", 
            timePlayed.Days * 24 + timePlayed.Hours, 
            timePlayed.Minutes, 
            timePlayed.Seconds);
    }
    
    /// <summary>
    /// Gets the total time played formatted as HH:MM:SS
    /// </summary>
    /// <returns>Formatted time string in HH:MM:SS format</returns>
    public static string GetFormattedTimePlayed(double totalMinutesPlayed)
    {
        TimeSpan timePlayed = TimeSpan.FromMinutes(totalMinutesPlayed);
        return string.Format("{0:D2}:{1:D2}:{2:D2}", 
            timePlayed.Days * 24 + timePlayed.Hours, 
            timePlayed.Minutes, 
            timePlayed.Seconds);
    }
}