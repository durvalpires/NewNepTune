using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class BuildTimeLimiter : MonoBehaviour
{
    private DateTime firstLaunchDate;
    [FormerlySerializedAs("maxUsageHours")] [SerializeField]
    private float maxUsageTime = 2;
    [SerializeField]
    private float timeCheckInterval = 60;
    private float sessionStartTime;
    private float totalTime = 0;
    [SerializeField] private GameObject LimitReachedCanvas;

    void Start()
    {
        totalTime = PlayerPrefs.GetFloat("totalUsageTime", 0);
        sessionStartTime = Time.time;
        
        CheckTimeLimit();
        StartCoroutine(TimeLimitCoroutine());
    }

    void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
        {
            float elapsedTime = Time.time - sessionStartTime;
            totalTime += elapsedTime;
            PlayerPrefs.SetFloat("totalUsageTime", totalTime);
            PlayerPrefs.Save();
        }
        else
        {
            sessionStartTime = Time.time;
        }
    }

    private void OnApplicationQuit()
    {
        // Calculate elapsed time and save
        float elapsedTime = Time.time - sessionStartTime;
        totalTime += elapsedTime;
        PlayerPrefs.SetFloat("totalUsageTime", totalTime);
        PlayerPrefs.Save();
    }

    private void CheckTimeLimit()
    {
        TimeSpan elapsed = TimeSpan.FromSeconds(totalTime + (Time.time - sessionStartTime));
        if (elapsed.TotalSeconds > maxUsageTime)
        {
            LimitReachedCanvas.SetActive(true);
            Time.timeScale = 0;
        }
    }
    
    private IEnumerator TimeLimitCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeCheckInterval);
            CheckTimeLimit();
        }
    }
}
