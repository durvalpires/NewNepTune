using System.Collections;
using Audio;
using UnityEngine;

public class RhythmMetronome : MonoBehaviour
{
    Coroutine tick;
    bool isRunning = false;

    public static void CreateAndPlay(string noteKey, float bpm = 80f)
    {
        var am = AudioManager.Instance;
        if (am.clickClip == null || am.RhythmMetronomeSource == null)
        {
            Debug.LogError("Assign clickClip and RhythmMetronomeSource on AudioManager.");
            return;
        }

        var rm = am.GetComponent<RhythmMetronome>() ??
                 am.gameObject.AddComponent<RhythmMetronome>();

        rm.StartMetronome(noteKey.ToUpper(), bpm, am.RhythmMetronomeSource, am.clickClip);
    }

    void StartMetronome(string key, float bpm, AudioSource src, AudioClip click)
    {
        if (tick != null) StopCoroutine(tick);

        float beats = BeatsPerBar(key);
        float step = 60f / bpm;

        isRunning = true;
        tick = StartCoroutine(Tick(src, click, step, beats));
    }

    static float BeatsPerBar(string k)
    {
        if (k.Contains("SEMIBREVE")) return 4;
        if (k.Contains("MINIM")) return 2;
        if (k.Contains("QUAVERS")) return 0.5f;
        if (k.Contains("CROTCHET")) return 1;
        return 1;
    }

    IEnumerator Tick(AudioSource src, AudioClip click, float interval, float beats)
    {
        Debug.Log(interval);
        int count = 1;
        while (isRunning)
        {
            //float t = 0f;
            //count = 0;
            //while (count < beats && isRunning)
            //{
            //    src.PlayOneShot(click, count % beats == 0 ? 1f : 0.5f);
            //    yield return new WaitForSeconds(interval);
            //    count++;
            //    t += interval;
            //}

            //count = 0;
            //while (count < beats && isRunning)
            //{
                //src.PlayOneShot(click, count % beats == 0 ? 1f : 0.5f);
                src.PlayOneShot(click, count % beats == 0 ? 1f : 0f);
                yield return new WaitForSeconds(interval);
                count++;
                //t += interval;
            //}
        }
    }

    public static void Stop()
    {
        var rm = AudioManager.Instance.GetComponent<RhythmMetronome>();
        if (rm == null) return;

        rm.isRunning = false;

        if (rm.tick != null)
        {
            rm.StopCoroutine(rm.tick);
            rm.tick = null;
        }

        AudioManager.Instance.RhythmMetronomeSource.Stop();
    }
}