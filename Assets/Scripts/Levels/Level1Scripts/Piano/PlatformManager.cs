using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public AudioSource audioSource; // The AudioSource component
    public AudioClip musicClip; // The music clip
    public float bpm;
    public PianoPlayer pianoPlayer; // The PianoPlayer script

    void Start()
    {
        StartMusic();
    }

    void StartMusic()
    {
        audioSource.clip = musicClip;
        audioSource.Play();
    }
    
    
   public void StopGame()
    {
        audioSource.Stop();
    }
}