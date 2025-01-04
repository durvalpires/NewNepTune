using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using static RhythmLevelManager;

public class RhythmLevelManager : MonoBehaviour
{
   
    public RhythmLevelData levelData;         
    public AudioSource audioSource;          
    public LoudnessDetector loudnessDetector;
    public RhythmUIManager uiManager;           

    [System.Serializable]
    public class SoundWave
    {
        public float startTime;
        public float endTime;
        public float MiddleTime => (startTime + endTime) / 2f;
    }

    private List<SoundWave> idealWaves = new List<SoundWave>(); 
    private List<float> userClapTimes = new List<float>();      
    private bool isListeningForClaps = false;
    private float startListeningTime;

    private void Start()
    {
        if (levelData == null || audioSource == null || loudnessDetector == null || uiManager == null)
        {
            Debug.LogError("Please assign all references in RhythmLevelManager!");
            return;
        }

        idealWaves = DetectSoundWaves(levelData.musicClip,
                                      levelData.windowSize,
                                      levelData.minWaveSeparation,
        levelData.thresholdFactor);

        Debug.Log($"Total sound waves detected: {idealWaves.Count}");
        StartCoroutine(GameFlowRoutine());
    }


    private IEnumerator GameFlowRoutine()
    {
     
        yield return StartCoroutine(uiManager.CountdownRoutine(levelData.initialCountdown, "Get Ready!"));

        audioSource.clip = levelData.musicClip;
        audioSource.Play();
        uiManager.SetFeedbackText("Listen carefully...");

        yield return new WaitWhile(() => audioSource.isPlaying);

       
        uiManager.SetFeedbackText("Your turn!");
        yield return new WaitForSeconds(1f);
        yield return StartCoroutine(uiManager.CountdownRoutine(levelData.clapCountdown, "Clap on the beat!"));

     
        isListeningForClaps = true;
        startListeningTime = Time.time;
        uiManager.SetFeedbackText("Clap now!");

      
        if (levelData.showClapIndicators)
        {
            StartCoroutine(ShowClapIndicators());
        }
        float lastWaveTime = idealWaves.Count > 0 ? idealWaves.Last().endTime : 0f;
        float totalWaitTime = levelData.waitAfterTrack + lastWaveTime;
        yield return new WaitForSeconds(totalWaitTime);

      
        isListeningForClaps = false;

        EvaluateClaps();
    }

    private IEnumerator ShowClapIndicators()
    {
        
        foreach (SoundWave wave in idealWaves)
        {
            float idealTime = wave.MiddleTime;
            float elapsed = Time.time - startListeningTime;
            float waitTime = idealTime - elapsed;

            if (waitTime > 0)
                yield return new WaitForSeconds(waitTime);

          
            uiManager.SetFeedbackText("Clap!");
            yield return new WaitForSeconds(0.5f);

         
            uiManager.SetFeedbackText("");
        }
    }

    private void Update()
    {
       
        if (isListeningForClaps)
        {
            float loudness = loudnessDetector.GetLoudness();
            if (loudness >= levelData.minLoudnessThreshold)
            {
                float clapTime = Time.time - startListeningTime;
                userClapTimes.Add(clapTime);
                ShowInstantFeedback(clapTime);

                StartCoroutine(ClapCooldown(0.2f));
            }
        }
    }

    private IEnumerator ClapCooldown(float cooldown)
    {
        isListeningForClaps = false;
        yield return new WaitForSeconds(cooldown);
        isListeningForClaps = true;
    }


    private void EvaluateClaps()
    {
        uiManager.ClearFeedback();

        float buffer = levelData.levelTimingBuffer;
        int totalScore = 0; 
        int perfectCount = 0; 
        int totalWaves = idealWaves.Count;

        for (int i = 0; i < totalWaves; i++)
        {
            float targetTime = idealWaves[i].MiddleTime;
            float bestDistance = float.MaxValue;
            float bestClap = -1f;

          
            foreach (var userClap in userClapTimes)
            {
                float dist = Mathf.Abs(userClap - targetTime);
                if (dist < bestDistance)
                {
                    bestDistance = dist;
                    bestClap = userClap;
                }
            }

            if (bestDistance <= buffer)
            {
                // Determine Early, Late, or Perfect
                float offset = bestClap - targetTime;

                if (Mathf.Abs(offset) < buffer * 0.3f)
                {
                    totalScore += 10; 
                    perfectCount++;
                }
                else if (offset < 0)
                {
                    totalScore += 5; 
                }
                else
                {
                    totalScore += 5; 
                }
            }
           
        }
       
        int minScoreThreshold = levelData.minScoreThreshold;
        float accuracy = (float)perfectCount / totalWaves * 100; 

        if (totalScore >= minScoreThreshold)
        {
            uiManager.AppendFeedback($"You Win! Accuracy: {accuracy:F2}%");
        }
        else
        {
            uiManager.AppendFeedback($"Try Again! Accuracy: {accuracy:F2}%");
        }

      
    }
    private void ShowInstantFeedback(float clapTime)
    {
        float buffer = levelData.levelTimingBuffer;
        string feedback = "Miss!";
        Color feedbackColor = Color.red; 

     
        float bestDistance = float.MaxValue;
        float bestWaveTime = -1f;

        foreach (var wave in idealWaves)
        {
            float dist = Mathf.Abs(clapTime - wave.MiddleTime);
            if (dist < bestDistance)
            {
                bestDistance = dist;
                bestWaveTime = wave.MiddleTime;
            }
        }

      
        if (bestDistance <= buffer)
        {
            float offset = clapTime - bestWaveTime;
            if (Mathf.Abs(offset) < buffer * 0.3f)
            {
                feedback = "Perfect!";
                feedbackColor = Color.green;
            }
            else if (offset < 0)
            {
                feedback = "Early!";
                feedbackColor = Color.yellow;
            }
            else
            {
                feedback = "Late!";
                feedbackColor = Color.blue;
            }
        }

     
        uiManager.countdownText.text = feedback;
        uiManager.countdownText.color = feedbackColor;
        StartCoroutine(ClearFeedbackAfterDelay(0.5f));
    }
    private IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        uiManager.countdownText.text = "";
        uiManager.countdownText.color = Color.white; 
    }

    private List<SoundWave> DetectSoundWaves(AudioClip clip, float windowSize, float minSeparation, float thresholdFactor)
    {
        List<SoundWave> waves = new List<SoundWave>();
        if (clip == null) return waves;

      
        float[] samples = new float[clip.samples * clip.channels];
        clip.GetData(samples, 0);
        int sampleRate = clip.frequency;
        int windowSamples = Mathf.FloorToInt(windowSize * sampleRate);

     
        List<float> energyList = new List<float>();
        for (int i = 0; i < samples.Length; i += windowSamples)
        {
            float sum = 0f;
            for (int j = i; j < i + windowSamples && j < samples.Length; j++)
            {
                sum += Mathf.Abs(samples[j]);
            }
            float avg = sum / windowSamples;
            energyList.Add(avg);
        }

        List<float> sorted = energyList.OrderBy(e => e).ToList();
        float median = sorted[sorted.Count / 2];
        float energyThreshold = median * thresholdFactor;

       
        bool inWave = false;
        float waveStart = 0f;

        for (int i = 0; i < energyList.Count; i++)
        {
            float currentTime = i * windowSize;
            if (energyList[i] > energyThreshold)
            {
                if (!inWave)
                {
                    waveStart = currentTime;
                    inWave = true;
                }
            }
            else
            {
                if (inWave)
                {
                    float waveEnd = currentTime;
                    if (waves.Count == 0 ||
                        waveStart - waves[waves.Count - 1].endTime >= minSeparation)
                    {
                        waves.Add(new SoundWave { startTime = waveStart, endTime = waveEnd });
                    }
                    inWave = false;
                }
            }
        }

     
        if (inWave)
        {
            float waveEnd = energyList.Count * windowSize;
            waves.Add(new SoundWave { startTime = waveStart, endTime = waveEnd });
        }

        return waves;
    }
}
