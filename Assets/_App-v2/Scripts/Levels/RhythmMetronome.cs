using System.Collections;
using Audio;
using UnityEngine;

public class RhythmMetronome : MonoBehaviour
{
    Coroutine tick;

    public static void CreateAndPlay(string noteKey, float bpm = 60f)
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

        int beats = BeatsPerBar(key);
        float step = 60f / bpm;            
        float length = step * beats;        
        tick = StartCoroutine(Tick(src, click, step, beats, length));
    }

    static int BeatsPerBar(string k)
    {
        if (k.Contains("SEMIBREVE")) return 4;   
        if (k.Contains("MINIM")) return 2;  
        return 1;                               
    }

    static IEnumerator Tick(AudioSource src, AudioClip click,
                             float interval, int beats, float total)
    {
        float t = 0f; int count = 0;
        while (t < total)
        {
            src.PlayOneShot(click, count % beats == 0 ? 1f : 0.5f); 
            yield return new WaitForSeconds(interval);
            t += interval;
            count++;
        }
    }
    public static void Stop()
    {
        var rm = AudioManager.Instance.GetComponent<RhythmMetronome>();
        if (rm == null) return;

        if (rm.tick != null)
        {
            rm.StopCoroutine(rm.tick);
            rm.tick = null;
        }
        AudioManager.Instance.RhythmMetronomeSource.Stop();
    }
}