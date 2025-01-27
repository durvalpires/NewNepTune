using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Fundamental frequency estimation using Summation of Residual Harmonics (SRH)
// T. Drugman and A. Alwan: "Joint Robust Voicing Detection and Pitch Estimation Based on Residual Harmonics", Interspeech'11, 2011.

public class AudioPitchEstimator : MonoBehaviour
{
    [Tooltip("Lowest frequency that can be estimated [Hz]")]
    [Range(40, 150)]
    public int frequencyMin = 40;

    [Tooltip("Highest frequency that can be estimated [Hz]")]
    [Range(300, 1200)]
    public int frequencyMax = 600;

    [Tooltip("Number of overtones to use for estimation")]
    [Range(1, 8)]
    public int harmonicsToUse = 5;

    [Tooltip("Frequency bandwidth of the spectral smoothing filter [Hz]\nA wider bandwidth smooths the estimation but reduces accuracy.")]
    public float smoothingWidth = 500;

    [Tooltip("Threshold to determine if the signal is silent or not\nA higher value makes the judgment stricter.")]
    public float thresholdSRH = 7;

    const int spectrumSize = 1024;
    const int outputResolution = 200; // Frequency axis resolution (lowering this reduces computational load)
    float[] spectrum = new float[spectrumSize];
    float[] specRaw = new float[spectrumSize];
    float[] specCum = new float[spectrumSize];
    float[] specRes = new float[spectrumSize];
    float[] srh = new float[outputResolution];

    public List<float> SRH => new List<float>(srh);

    /// <summary>
    /// Estimates the fundamental frequency
    /// </summary>
    /// <param name="audioSource">Input audio source</param>
    /// <returns>Fundamental frequency [Hz] (float.NaN if it does not exist)</returns>
    public float Estimate(AudioSource audioSource)
    {
        var nyquistFreq = AudioSettings.outputSampleRate / 2.0f;

        // Get the audio spectrum
        if (!audioSource.isPlaying) return float.NaN;
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);

        // Calculate the logarithm of the amplitude spectrum
        // All subsequent spectra are processed as logarithmic amplitudes (different from the original paper)
        for (int i = 0; i < spectrumSize; i++)
        {
            // Add a small value to avoid -∞ when the amplitude is zero
            specRaw[i] = Mathf.Log(spectrum[i] + 1e-9f);
        }

        // Compute the cumulative sum of the spectrum (used later)
        specCum[0] = 0;
        for (int i = 1; i < spectrumSize; i++)
        {
            specCum[i] = specCum[i - 1] + specRaw[i];
        }

        // Compute the residual spectrum
        var halfRange = Mathf.RoundToInt((smoothingWidth / 2) / nyquistFreq * spectrumSize);
        for (int i = 0; i < spectrumSize; i++)
        {
            // Smooth the spectrum (using cumulative sum for moving average)
            var indexUpper = Mathf.Min(i + halfRange, spectrumSize - 1);
            var indexLower = Mathf.Max(i - halfRange + 1, 0);
            var upper = specCum[indexUpper];
            var lower = specCum[indexLower];
            var smoothed = (upper - lower) / (indexUpper - indexLower);

            // Subtract the smoothed component from the original spectrum
            specRes[i] = specRaw[i] - smoothed;
        }

        // Calculate the SRH (Summation of Residual Harmonics) score
        float bestFreq = 0, bestSRH = 0;
        for (int i = 0; i < outputResolution; i++)
        {
            var currentFreq = (float)i / (outputResolution - 1) * (frequencyMax - frequencyMin) + frequencyMin;

            // Calculate the SRH score at the current frequency (from the paper's equation (1))
            var currentSRH = GetSpectrumAmplitude(specRes, currentFreq, nyquistFreq);
            for (int h = 2; h <= harmonicsToUse; h++)
            {
                // Higher signal strength at harmonics improves the score
                currentSRH += GetSpectrumAmplitude(specRes, currentFreq * h, nyquistFreq);

                // Higher signal strength between harmonics reduces the score
                currentSRH -= GetSpectrumAmplitude(specRes, currentFreq * (h - 0.5f), nyquistFreq);
            }
            srh[i] = currentSRH;

            // Record the frequency with the highest score
            if (currentSRH > bestSRH)
            {
                bestFreq = currentFreq;
                bestSRH = currentSRH;
            }
        }

        // If the SRH score does not meet the threshold → assume no clear fundamental frequency exists
        if (bestSRH < thresholdSRH) return float.NaN;

        return bestFreq;
    }

    // Retrieve the amplitude at the given frequency [Hz] from the spectrum data
    float GetSpectrumAmplitude(float[] spec, float frequency, float nyquistFreq)
    {
        var position = frequency / nyquistFreq * spec.Length;
        var index0 = (int)position;
        var index1 = index0 + 1; // Boundary checks are omitted
        var delta = position - index0;
        return (1 - delta) * spec[index0] + delta * spec[index1];
    }
}