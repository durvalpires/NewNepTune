using System.Numerics;
using UnityEngine;
using MathNet.Numerics.IntegralTransforms;
using MathNet.Numerics;

public class SpectrumSubtractor
{
    
    private float[] LoadAudioData(AudioClip clip)
    {
        int samples = clip.samples * clip.channels;
        float[] audioData = new float[samples];
        clip.GetData(audioData, 0); // Read the entire audio file
        return audioData;
    }
    
    private float[] ComputeSpectrum(float[] audioData)
    {
        int fftSize = 1024; // Same as your spectrumSize
        float[] spectrum = new float[fftSize];

        // Create a complex array for FFT input
        Complex[] fftBuffer = new Complex[fftSize];
    
        // Copy real part (ignore imaginary part for now)
        for (int i = 0; i < fftSize; i++)
        {
            fftBuffer[i] = new Complex(audioData[i], 0);
        }

        // Apply FFT
        Fourier.Forward(fftBuffer, FourierOptions.Default);

        // Extract magnitude (power spectrum)
        for (int i = 0; i < fftSize; i++)
        {
            spectrum[i] = (float)fftBuffer[i].Magnitude;
        }

        return spectrum;
    }
}
