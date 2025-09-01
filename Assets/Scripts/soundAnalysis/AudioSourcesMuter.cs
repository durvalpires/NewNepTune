using UnityEngine;
using System.Collections;

public class AudioSourcesMuter : MonoBehaviour
{
    public AudioSource Metronome;
    public AudioSource BackgroundTrack;
    public float fadeSeconds = 0.25f;
    public bool useHardMute = true;
    public float mutedVolume = 0f;
    public float normalVolume = 1f;

    private Coroutine _fadeCo;

    public void SetMuted(bool muted)
    {
        if (useHardMute)
        {
            if (Metronome) Metronome.mute = muted;
            if (BackgroundTrack) BackgroundTrack.mute = muted;
            return;
        }

        if (_fadeCo != null) StopCoroutine(_fadeCo);
        _fadeCo = StartCoroutine(FadeTo(muted ? mutedVolume : normalVolume));
    }

    private IEnumerator FadeTo(float target)
    {
        if (Metronome == null && BackgroundTrack == null) yield break;

        float a0 = Metronome ? Metronome.volume : 0f;
        float b0 = BackgroundTrack ? BackgroundTrack.volume : 0f;

        if (fadeSeconds <= 0f)
        {
            if (Metronome) Metronome.volume = target;
            if (BackgroundTrack) BackgroundTrack.volume = target;
            yield break;
        }

        float t = 0f;
        while (t < fadeSeconds)
        {
            t += Time.unscaledDeltaTime;
            float k = Mathf.Clamp01(t / fadeSeconds);
            if (Metronome) Metronome.volume = Mathf.Lerp(a0, target, k);
            if (BackgroundTrack) BackgroundTrack.volume = Mathf.Lerp(b0, target, k);
            yield return null;
        }
        if (Metronome) Metronome.volume = target;
        if (BackgroundTrack) BackgroundTrack.volume = target;
    }
}