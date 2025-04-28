using UnityEngine;

public class PianoAudio : MonoBehaviour
{
    [SerializeField] private RhythmGameSettings settings;
    [SerializeField] private AudioSource audioSource;

    private string octave = "4";
    
    public void OnNotePlayed(string note, bool keyOn)
    {
        Debug.Log(note);
        if (!keyOn) return;
        audioSource.PlayOneShot(settings.GetNoteAudio(note + octave), 0.5f);
    }
}
