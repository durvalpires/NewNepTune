using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class AudioPitchHUD : AudioPitchEstimator
{
   
  
    [SerializeField] public float waitForVideoSeconds = 3f;
    [SerializeField] public float updateHz = 30f;

    [SerializeField] public float windowMs = 300f;
    [SerializeField] public float dominanceRatio = 0.6f;
    [SerializeField] public int consecutiveToLock = 5;
    [SerializeField] public int vibratoSmoothFrames = 6;
    [SerializeField] public float centsSnapTolerance = 35f;
    [SerializeField] public bool ignoreOctave = true;

     //[SerializeField] TextMeshProUGUI StableNoteText;

    const int SpectrumSize = 1024;
    const int OutputResolution = 200;

    readonly float[] spectrum = new float[SpectrumSize];
    readonly float[] specRaw = new float[SpectrumSize];
    readonly float[] specCum = new float[SpectrumSize];
    readonly float[] specRes = new float[SpectrumSize];
    readonly float[] srhBuf = new float[OutputResolution];

    struct PitchFrame { public float time; public float freq; public int midi; public float cents; public float confidence; }
    readonly Queue<PitchFrame> frames = new Queue<PitchFrame>();
    int lastLeaderKey = -1;
    int leaderStreak = 0;

    public struct StableNote { public bool hasNote; public string noteName; public int midi; public int octave; public float frequency; public float cents; public float confidence; }
    public StableNote LastStable { get; private set; }

    private VideoPlayer videoPlayer;
    private AudioSource micSource;
    private bool useVideo;

    [System.Obsolete]
    public void Start()
    {

        StartCoroutine(Bootstrap());
    }

    [System.Obsolete]
    public IEnumerator Bootstrap()
    {

        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))
        {
            yield return Application.RequestUserAuthorization(UserAuthorization.Microphone);
        }
        if (!Application.HasUserAuthorization(UserAuthorization.Microphone))    
        {
            Debug.LogWarning("Microphone permission denied.");
            yield break; 
        }

        float t = 0f;
        if (videoPlayer == null)
        {
            while (t < waitForVideoSeconds && videoPlayer == null)
            {
                var vps = FindObjectsOfType<VideoPlayer>(true);
                for (int i = 0; i < vps.Length; i++)
                {
                    if (vps[i].gameObject.activeInHierarchy && vps[i].audioTrackCount > 0)
                    {
                        videoPlayer = vps[i];
                        break;
                    }
                }
                if (videoPlayer != null) break;
                t += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        if (videoPlayer != null)
        {
            ForceRouteVideoToAudioSource(videoPlayer);
            if (!videoPlayer.isPrepared) { videoPlayer.Prepare(); while (!videoPlayer.isPrepared) yield return null; }
            videoPlayer.Play();
            useVideo = true;
            StartCoroutine(Loop());
            yield break;
        }

        if (Microphone.devices == null || Microphone.devices.Length == 0) yield break;
        string dev = Microphone.devices[0];
        int sr = AudioSettings.outputSampleRate;
        micSource = gameObject.AddComponent<AudioSource>();
        micSource.loop = true;
        micSource.spatialBlend = 0f;
        micSource.clip = Microphone.Start(dev, true, 1, sr);
        while (Microphone.GetPosition(dev) <= 0) yield return null;
        micSource.Play();
        useVideo = false;
        StartCoroutine(Loop());
    }

    void ForceRouteVideoToAudioSource(VideoPlayer vp)
    {
        var src = vp.GetTargetAudioSource(0);
        if (src == null)
        {
            src = vp.gameObject.GetComponent<AudioSource>();
            if (src == null) src = vp.gameObject.AddComponent<AudioSource>();
        }
        src.playOnAwake = false;
        src.loop = false;
        src.spatialBlend = 0f;
        src.volume = 1f;
        vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
        vp.EnableAudioTrack(0, true);
        vp.SetTargetAudioSource(0, src);
    }

    IEnumerator Loop()
    {
        var wait = new WaitForSeconds(1f / Mathf.Max(1f, updateHz));
        for (; ; )
        {
            var note = UpdateAndGetStableNote();
            //if (StableNoteText != null)
            //{
                if (note.hasNote)
                {
                    var name = ignoreOctave ? note.noteName : note.noteName + note.octave.ToString();
                    var centsAbs = Mathf.Abs(note.cents).ToString("0");
                    var sign = note.cents >= 0 ? "+" : "-";
                    Debug.Log("AudioPitchHUD Note:" + name);
                    //StableNoteText.text = name;// + " " + sign + centsAbs + "c";
                }
            //    else
            //    {
            //        StableNoteText.text = "";
            //    }
            ////}
            yield return wait;
        }
    }

    public StableNote UpdateAndGetStableNote()
    {
        float f0 = useVideo ? EstimateF0FromListener() : EstimateF0FromSource(micSource);
        float confidence = useVideo ? EstimateLoudnessFromListener() : EstimateLoudnessFromSource(micSource);

        if (float.IsNaN(f0) || confidence < 0.02f)
        {
            AgeWindow();
            ComputeStableFromWindow();
            return LastStable;
        }

        int midi = FreqToMidi(f0);
        float nearest = MidiToFreq(midi);
        float cents = 1200f * Mathf.Log(f0 / nearest, 2f);
        float centsSigma = Mathf.Max(10f, centsSnapTolerance);
        float centsWeight = Mathf.Exp(-0.5f * (cents * cents) / (centsSigma * centsSigma));

        frames.Enqueue(new PitchFrame
        {
            time = Time.unscaledTime,
            freq = f0,
            midi = midi,
            cents = cents,
            confidence = Mathf.Clamp01(confidence) * centsWeight
        });

        AgeWindow();
        ComputeStableFromWindow();

        return LastStable;
    }

    float EstimateF0FromListener()
    {
        if (AudioListener.pause) return float.NaN;
        AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);
        return RunSRHOnSpectrum();
    }

    float EstimateF0FromSource(AudioSource src)
    {
        if (src == null || !src.isPlaying) return float.NaN;
        src.GetSpectrumData(spectrum, 0, FFTWindow.Hanning);
        return RunSRHOnSpectrum();
    }

    float RunSRHOnSpectrum()
    {
        float nyquist = AudioSettings.outputSampleRate * 0.5f;

        for (int i = 0; i < SpectrumSize; i++)
            specRaw[i] = Mathf.Log(spectrum[i] + 1e-9f);

        specCum[0] = 0f;
        for (int i = 1; i < SpectrumSize; i++)
            specCum[i] = specCum[i - 1] + specRaw[i];

        int halfRange = Mathf.RoundToInt((smoothingWidth / 2f) / nyquist * SpectrumSize);
        if (halfRange < 1) halfRange = 1;

        for (int i = 0; i < SpectrumSize; i++)
        {
            int indexUpper = Mathf.Min(i + halfRange, SpectrumSize - 1);
            int indexLower = Mathf.Max(i - halfRange + 1, 0);
            float upper = specCum[indexUpper];
            float lower = specCum[indexLower];
            float smoothed = (upper - lower) / Mathf.Max(1, (indexUpper - indexLower));
            specRes[i] = specRaw[i] - smoothed;
        }

        float bestFreq = 0f;
        float bestSRH = float.NegativeInfinity;

        for (int i = 0; i < OutputResolution; i++)
        {
            float f = (float)i / (OutputResolution - 1) * (frequencyMax - frequencyMin) + frequencyMin;
            float s = GetSpecAmp(specRes, f, nyquist);
            for (int h = 2; h <= harmonicsToUse; h++)
            {
                s += GetSpecAmp(specRes, f * h, nyquist);
                s -= GetSpecAmp(specRes, f * (h - 0.5f), nyquist);
            }
            srhBuf[i] = s;
            if (s > bestSRH) { bestSRH = s; bestFreq = f; }
        }

        if (bestSRH < thresholdSRH) return float.NaN;
        return bestFreq;
    }

    float GetSpecAmp(float[] spec, float freq, float nyquist)
    {
        if (freq <= 0f) return 0f;
        float pos = freq / nyquist * (spec.Length - 1);
        int i0 = Mathf.Clamp((int)pos, 0, spec.Length - 1);
        int i1 = Mathf.Min(i0 + 1, spec.Length - 1);
        float t = pos - i0;
        return spec[i0] * (1f - t) + spec[i1] * t;
    }

    float EstimateLoudnessFromListener()
    {
        var buf = new float[256];
        AudioListener.GetOutputData(buf, 0);
        float sum = 0f; for (int i = 0; i < buf.Length; i++) sum += buf[i] * buf[i];
        return Mathf.Clamp01(Mathf.Sqrt(sum / buf.Length) * 5f);
    }

    float EstimateLoudnessFromSource(AudioSource src)
    {
        if (src == null) return 0f;
        var buf = new float[256];
        src.GetOutputData(buf, 0);
        float sum = 0f; for (int i = 0; i < buf.Length; i++) sum += buf[i] * buf[i];
        return Mathf.Clamp01(Mathf.Sqrt(sum / buf.Length) * 5f);
    }

    void AgeWindow()
    {
        float cutoff = Time.unscaledTime - windowMs * 0.001f;
        while (frames.Count > 0 && frames.Peek().time < cutoff) frames.Dequeue();
    }

    void ComputeStableFromWindow()
    {
        if (frames.Count == 0) { LastStable = default; return; }

        var weights = new Dictionary<int, float>(16);
        float total = 0f;
        foreach (var pf in frames)
        {
            int key = ignoreOctave ? (pf.midi % 12) : pf.midi;
            float w = pf.confidence;
            if (w <= 0f) continue;
            total += w;
            if (!weights.ContainsKey(key)) weights[key] = w; else weights[key] += w;
        }
        if (total <= 0f) { LastStable = default; return; }

        int leaderKey = -1; float leaderW = -1f;
        foreach (var kv in weights) if (kv.Value > leaderW) { leaderW = kv.Value; leaderKey = kv.Key; }
        float leaderRatio = leaderW / total;

        if (leaderKey == lastLeaderKey) leaderStreak++; else leaderStreak = 1;
        bool canSwitch = leaderRatio >= dominanceRatio || leaderStreak >= consecutiveToLock;
        if (!canSwitch && lastLeaderKey >= 0) leaderKey = lastLeaderKey; else lastLeaderKey = leaderKey;

        var arr = frames.ToArray();
        float centsSum = 0f; int cnt = 0;
        for (int i = arr.Length - 1; i >= 0 && cnt < Mathf.Max(1, vibratoSmoothFrames); i--)
        {
            int key = ignoreOctave ? (arr[i].midi % 12) : arr[i].midi;
            if (key == leaderKey) { centsSum += arr[i].cents; cnt++; }
        }
        float smoothedCents = cnt > 0 ? centsSum / cnt : 0f;

        int leaderMidi = ignoreOctave ? SnapLeaderMidiFromPitchClass(leaderKey) : leaderKey;
        float leaderFreq = MidiToFreq(leaderMidi);
        string name = GetNameFromMidi(leaderMidi);
        int octave = (leaderMidi / 12) - 1;

        LastStable = new StableNote
        {
            hasNote = true,
            noteName = name,
            midi = leaderMidi,
            octave = octave,
            frequency = leaderFreq,
            cents = smoothedCents,
            confidence = Mathf.Clamp01(leaderRatio)
        };
    }


    int FreqToMidi(float freq)
    {
        if (freq <= 0f || float.IsNaN(freq) || float.IsInfinity(freq)) return 0;
        return Mathf.RoundToInt(12f * Mathf.Log(freq / 440f, 2f) + 69f);
    }

    float MidiToFreq(int midi)
    {
        return 440f * Mathf.Pow(2f, (midi - 69) / 12f);
    }

    string GetNameFromMidi(int midi)
    {
        string[] names = { "C", "C#", "D", "D#", "E", "F", "F#", "G", "G#", "A", "A#", "B" };
        return names[((midi % 12) + 12) % 12];
    }

    int SnapLeaderMidiFromPitchClass(int pc)
    {
        return pc + 12 * (4 + 1);
    }
}