using System;
using System.Collections;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class PopUpToggleManager : MonoBehaviour
{
    [SerializeField] protected GameObject popUpCanvas;
    [SerializeField] protected GameObject popup1;
    [SerializeField] private GameObject popup2;
    [SerializeField] private GameObject popup3;
    [SerializeField] protected GameObject PopupGOContainer;
    
    protected bool _popUpActive;
    
    private void Update()
    {
        // if (_popUpActive && Input.GetMouseButtonDown(0))
        // {
        //   Back();
        // }
    }
    
    // TODO : Turn this into a single function with a parameter
    public void TogglePopup1(string pressedNote)
    {
        if (pressedNote != "")
        {
            SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
            StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, .6f));
        }
        // GameObject'in şu anki aktiflik durumunun tersini ayarlayın
        popUpCanvas.SetActive(true);
        popup1.SetActive(true);
        _popUpActive = true;
    }
    public void TogglePopup2(string pressedNote)
    {
        if (pressedNote != "")
        {
            SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
            StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, .6f));
        }
        popUpCanvas.SetActive(true);
        popup2.SetActive(true);
        _popUpActive = true;
    }
    public void TogglePopup3(string pressedNote)
    {
        if (pressedNote != "")
        {
            SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
            StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, .6f));
        }
        popUpCanvas.SetActive(true);
        popup3.SetActive(true);
        _popUpActive = true;
    }

    public void ToggleInstrument1(string instrument)
    {
        GameObject tempGO = GameObject.Find("TempAudioSource");
        if (tempGO != null)
        {
            Destroy(tempGO);
        }

        if (instrument == null) return;

        AudioClip[] instrumentSounds = Resources.LoadAll<AudioClip>($"Instruments/Sounds/");
        AudioClip soundToPlay = Array.Find(instrumentSounds, clip => clip.name == instrument);
        GameObject tempAudioSourceGO = new GameObject() {name = "TempAudioSource"};
        AudioSource tempAudioSource = tempAudioSourceGO.AddComponent<AudioSource>();
        tempAudioSource.PlayOneShot(soundToPlay);

        popup1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"InstrumentPNGs/{instrument}");
        
        popUpCanvas.SetActive(true);
        popup1.SetActive(true);
        _popUpActive = true;
    }
    
    public void ToggleInstrument2(string instrument)
    {
        GameObject tempGO = GameObject.Find("TempAudioSource");
        if (tempGO != null)
        {
            Destroy(tempGO);
        }

        if (instrument == null) return;

        AudioClip[] instrumentSounds = Resources.LoadAll<AudioClip>($"Instruments/Sounds/");
        AudioClip soundToPlay = Array.Find(instrumentSounds, clip => clip.name == instrument);
        GameObject tempAudioSourceGO = new GameObject() {name = "TempAudioSource"};
        AudioSource tempAudioSource = tempAudioSourceGO.AddComponent<AudioSource>();
        tempAudioSource.PlayOneShot(soundToPlay);

        popup2.GetComponent<Image>().sprite = Resources.Load<Sprite>($"InstrumentPNGs/{instrument}");
        
        popUpCanvas.SetActive(true);
        popup2.SetActive(true);
        _popUpActive = true;
    }
    
    private IEnumerator SlowlyDestroyAudio(GameObject audioSourceGO)
    {
        AudioSource audioSource = audioSourceGO.GetComponent<AudioSource>();

        while (audioSource&& audioSource.volume > 0)
        {
            audioSource.volume -= 0.1f;
            yield return new WaitForSeconds(0.15f);
        }
        
        Destroy(audioSourceGO);
    }

    public virtual void Back()
    {
        if (popup1 != null ) popup1.SetActive(false);
        if (popup2 != null ) popup2.SetActive(false);
        if (popup3 != null ) popup3.SetActive(false);

        GameObject tempAudioSource = GameObject.Find("TempAudioSource");
        if (tempAudioSource != null)
        {
            if (tempAudioSource.GetComponent<AudioSource>().isPlaying)
            {
                StartCoroutine(SlowlyDestroyAudio(tempAudioSource));   
            }
        }

        popUpCanvas.SetActive(false);
        _popUpActive = false;
    }
}
