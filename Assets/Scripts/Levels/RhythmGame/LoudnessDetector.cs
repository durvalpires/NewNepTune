using UnityEngine;

public class LoudnessDetector: MonoBehaviour
{

    public int sampleWindow = 64;

    private AudioClip micClip;
    private string microphoneName;

    private void Start()
    {
#if !UNITY_WEBGL
        if (Microphone.devices.Length > 0)
        {
            microphoneName = Microphone.devices[0];
            micClip = Microphone.Start(microphoneName, true, 210, AudioSettings.outputSampleRate);
        }
        else
        {
            Debug.LogError("No microphone detected!");
        }
#endif
    }

  
    public float GetLoudness()
    {
#if !UNITY_WEBGL
        return GetLoudnessFromClip(Microphone.GetPosition(microphoneName), micClip);
#endif
        return -1f;
    }

    private float GetLoudnessFromClip(int clipPosition, AudioClip clip)
    {
        if (clip == null) return 0f;

        int startPos = clipPosition - sampleWindow;
        if (startPos < 0) startPos = 0;

        float[] waveData = new float[sampleWindow];
        clip.GetData(waveData, startPos);

        float total = 0f;
        for (int i = 0; i < sampleWindow; i++)
        {
            total += Mathf.Abs(waveData[i]);
        }

        return total / sampleWindow;
    }
}
