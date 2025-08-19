using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Video;
using TMPro;

public class AudioPitchHUD : AudioPitchEstimator
{
    [SerializeField] float windowMs = 300f;
    [SerializeField] float dominanceRatio = 0.6f;
    [SerializeField] int consecutiveToLock = 5;
    [SerializeField] int vibratoSmoothFrames = 6;
    [SerializeField] float centsSnapTolerance = 35f;
    [SerializeField] bool ignoreOctave = true;
    [SerializeField] float updateHz = 30f;
    [SerializeField] TextMeshProUGUI neckText;

    struct PitchFrame { public float time; public float freq; public int midi; public float cents; public float confidence; }
    Queue<PitchFrame> frames = new Queue<PitchFrame>();
    int lastLeaderKey = -1;
    int leaderStreak = 0;

    public struct StableNote { public bool hasNote; public string noteName; public int midi; public int octave; public float frequency; public float cents; public float confidence; }
    public StableNote LastStable { get; private set; }

    AudioSource inputSource;

    void Start()
    {
        StartCoroutine(BootstrapAudioInput());
    }

    IEnumerator BootstrapAudioInput()
    {
        var vp = FindObjectOfType<VideoPlayer>();
        if (vp != null)
        {
            inputSource = gameObject.AddComponent<AudioSource>();
            inputSource.playOnAwake = false;
            inputSource.loop = false;
            vp.audioOutputMode = VideoAudioOutputMode.AudioSource;
            vp.EnableAudioTrack(0, true);
            vp.SetTargetAudioSource(0, inputSource);
            if (!vp.isPrepared) { vp.Prepare(); while (!vp.isPrepared) yield return null; }
            vp.Play();
            inputSource.Play();
            SetBaseTargetSource(inputSource);
            StartCoroutine(Loop());
            yield break;
        }

        if (Microphone.devices == null || Microphone.devices.Length == 0) yield break;
        string dev = Microphone.devices[0];
        int sr = AudioSettings.outputSampleRate;
        inputSource = gameObject.AddComponent<AudioSource>();
        inputSource.loop = true;
        inputSource.clip = Microphone.Start(dev, true, 1, sr);
        while (Microphone.GetPosition(dev) <= 0) yield return null;
        inputSource.Play();
        SetBaseTargetSource(inputSource);
        StartCoroutine(Loop());
    }

    IEnumerator Loop()
    {
        var wait = new WaitForSeconds(1f / Mathf.Max(1f, updateHz));
        for (; ; )
        {
            var note = UpdateAndGetStableNote();
            if (neckText != null)
            {
                if (note.hasNote)
                {
                    var name = ignoreOctave ? note.noteName : note.noteName + note.octave.ToString();
                    var centsAbs = Mathf.Abs(note.cents).ToString("0");
                    var sign = note.cents >= 0 ? "+" : "-";
                    neckText.text = name + " " + sign + centsAbs + "c";
                }
                else
                {
                    neckText.text = "";
                }
            }
            yield return wait;
        }
    }

    void SetBaseTargetSource(AudioSource src)
    {
        var f = typeof(AudioPitchEstimator).GetField("targetSource", BindingFlags.NonPublic | BindingFlags.Instance);
        if (f != null) f.SetValue(this, src);
    }

    public StableNote UpdateAndGetStableNote()
    {
        float f0 = Estimate();
        float confidence = 0f;
        if (inputSource != null)
        {
            var buf = new float[256];
            inputSource.GetOutputData(buf, 0);
            float sum = 0f;
            for (int i = 0; i < buf.Length; i++) sum += buf[i] * buf[i];
            float rms = Mathf.Sqrt(sum / buf.Length);
            confidence = Mathf.Clamp01(rms * 5f);
        }

        if (float.IsNaN(f0) || confidence < 0.02f)
        {
            AgeWindow();
            ComputeStableFromWindow();
            return LastStable;
        }

        int midi = FreqToMidi(f0);
        float nearestFreq = MidiToFreq(midi);
        float cents = 1200f * Mathf.Log(f0 / nearestFreq, 2f);
        float centsSigma = Mathf.Max(10f, centsSnapTolerance);
        float centsWeight = Mathf.Exp(-0.5f * (cents * cents) / (centsSigma * centsSigma));
        var pf = new PitchFrame { time = Time.unscaledTime, freq = f0, midi = midi, cents = cents, confidence = Mathf.Clamp01(confidence) * centsWeight };
        frames.Enqueue(pf);
        AgeWindow();
        ComputeStableFromWindow();
        return LastStable;
    }

    void AgeWindow()
    {
        float cutoff = Time.unscaledTime - windowMs * 0.001f;
        while (frames.Count > 0 && frames.Peek().time < cutoff) frames.Dequeue();
    }

    void ComputeStableFromWindow()
    {
        if (frames.Count == 0) { LastStable = default; return; }
        var weights = new Dictionary<int, float>(32);
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
        int leaderKey = -1;
        float leaderW = -1f;
        foreach (var kv in weights) if (kv.Value > leaderW) { leaderW = kv.Value; leaderKey = kv.Key; }
        float leaderRatio = leaderW / total;
        if (leaderKey == lastLeaderKey) leaderStreak++; else leaderStreak = 1;
        bool canSwitch = leaderRatio >= dominanceRatio || leaderStreak >= consecutiveToLock;
        if (!canSwitch && lastLeaderKey >= 0) leaderKey = lastLeaderKey; else lastLeaderKey = leaderKey;
        var arr = frames.ToArray();
        float centsSum = 0f;
        int ccount = 0;
        for (int i = arr.Length - 1; i >= 0 && ccount < Mathf.Max(1, vibratoSmoothFrames); i--)
        {
            int key = ignoreOctave ? (arr[i].midi % 12) : arr[i].midi;
            if (key == leaderKey) { centsSum += arr[i].cents; ccount++; }
        }
        float smoothedCents = ccount > 0 ? centsSum / ccount : 0f;
        int leaderMidi = ignoreOctave ? SnapLeaderMidiFromPitchClass(leaderKey) : leaderKey;
        float leaderFreq = MidiToFreq(leaderMidi);
        string name = GetNameFromMidi(leaderMidi);
        int octave = (leaderMidi / 12) - 1;
        LastStable = new StableNote { hasNote = true, noteName = name, midi = leaderMidi, octave = octave, frequency = leaderFreq, cents = smoothedCents, confidence = Mathf.Clamp01(leaderRatio) };
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