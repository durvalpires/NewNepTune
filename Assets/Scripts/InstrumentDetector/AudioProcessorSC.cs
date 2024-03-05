using UnityEngine;
using System;
using TMPro;

public class AudioProcessorSC : MonoBehaviour
{
    #if UNITY_EDITOR || UNITY_ANDROID || UNITY_IOS
    AudioSource audioSource;
    float[] samples;
    float[] spectrum;
    int sampleRate;
    public TextMeshProUGUI noteText; 
    public bool sazSecili;
    public bool violinSecili;
    public TextMeshProUGUI chooseText;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        samples = new float[1024];
        spectrum = new float[1024];
        sampleRate = AudioSettings.outputSampleRate;
        StartMicrophone();
        violinSecili = true;
        sazSecili = false;
    }

    void Update()
    {
        GetSpectrumAudioSource();
        float dominantFrequency = FindDominantFrequency();
        string note = ConvertFrequencyToNote(dominantFrequency);
        noteText.text = "Note: " + note;
        Debug.Log($"Frequency: {dominantFrequency}, Note: {note}");
        if(sazSecili)
            chooseText.text = "Saz has been selected. Play your saz!";
        if(violinSecili)
            chooseText.text = "Violin has been selected. Play your violin!";
    }

    string ConvertFrequencyToNote(float frequency)
    {
        string note = "";
        /*if (frequency >= 310 && frequency <= 400) 
        {
            note = "G";
        } 
        else if (frequency >= 260 && frequency < 310) 
        {
            note = "D";
        } 
        else if (frequency >= 2900 && frequency < 3100) 
        {
            note = "A";
        }
        else if (frequency >= 4000 && frequency < 6000) 
        {
            note = "E";
        }*/
        //keman için aşağıdaki. saz için ya da fonksiyonu ile yap
        if(violinSecili == true && sazSecili == false)
        {
            if((frequency >= 150 && frequency <= 250) || (frequency < 2000 && frequency > 1300)) //1500
                note = "G";
            else if((frequency > 300 && frequency < 390) || (frequency > 1800 && frequency <= 2200)) //re 2000
                note = "D";
            else if((frequency >= 400 && frequency <= 500) || (frequency > 2400 && frequency < 3300)) //2400 3300 arası la
                note = "A";
            else if((frequency >= 600 && frequency <= 700) || (frequency > 4900 && frequency < 5600))
                note = "E";
            else
            {
                note = "No Sound";
            }
        }
        else if(violinSecili == false && sazSecili == true)
        {
            if((frequency > 240 && frequency < 260) || (frequency > 100 && frequency < 120))
                note = "Re";
            else if((frequency > 280 && frequency < 300) || (frequency > 120 && frequency < 140))
                note = "Mi";
            else if((frequency >= 300 && frequency < 315) || (frequency > 140 && frequency < 160))
                note = "Fa";
            else if((frequency > 315 && frequency < 335) || (frequency > 160 && frequency < 180))
                note = "Sol";
            else if((frequency > 335 && frequency < 360) || (frequency > 180 && frequency < 200))
                note = "La";
        }
        return note;
        
    }

    void StartMicrophone()
    {
        audioSource.clip = Microphone.Start(null, true, 10, 44100);
        audioSource.loop = true;
        while (!(Microphone.GetPosition(null) > 0)) {}
        audioSource.Play();
    }

    void GetSpectrumAudioSource()
    {
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);
    }

    float FindDominantFrequency()
    {
        float maxV = 0;
        var maxN = 0;
        for (int i = 0; i < spectrum.Length; i++)
        {
            if (spectrum[i] > maxV && spectrum[i] > 0.0f)
            {
                maxV = spectrum[i];
                maxN = i; // maxN is the index of max
            }
        }

        float freqN = maxN;
        if (maxN > 0 && maxN < spectrum.Length - 1)
        {
            var dL = spectrum[maxN - 1] / spectrum[maxN];
            var dR = spectrum[maxN + 1] / spectrum[maxN];
            freqN += 0.5f * (dR * dR - dL * dL);
        }
        
        float dominantFrequency = freqN * (sampleRate / 2) / spectrum.Length;
        return dominantFrequency;
    }
    public void SazSec()
    {
        sazSecili = true;
        violinSecili = false;
    }
    public void ViolinSec()
    {
        sazSecili = false;
        violinSecili = true;
    }

    #endif
}
