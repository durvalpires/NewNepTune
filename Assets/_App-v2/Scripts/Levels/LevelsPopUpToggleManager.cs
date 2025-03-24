using System;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopUpToggleManager : PopUpToggleManager
{
    //[SerializeField] private AudioSource backgroundTrack;
    
    public void TogglePopup(string pressedNote)
    {
        //backgroundTrack.volume = .2f;
        
        if (pressedNote != "")
        {
            SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
            StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, .6f));
        }
        // GameObject'in şu anki aktiflik durumunun tersini ayarlayın
        popUpCanvas.SetActive(true);
        //popup1.SetActive(true);
        _popUpActive = true;
    }

    public void ToggleRhythmPopup(string rhythm)
    {
        // if (pressedNote != "")
        // {
        //     SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
        //     StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, .6f));
        // }
        // GameObject'in şu anki aktiflik durumunun tersini ayarlayın
        popUpCanvas.SetActive(true);
        //popup1.SetActive(true);
        _popUpActive = true;
    }
    
    public void ToggleInstrument(string instrument)
    {
        // LevelCompletObserver.LevelStart(levelIndex, _worldIndex);
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
        
        //AudioManager.Instance.PauseMusic();
        //backgroundTrack.volume = .2f;

        //popup1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Instruments/Sprites/{instrument}");
        
        popUpCanvas.SetActive(true);
        //popup1.SetActive(true);
        _popUpActive = true;
    }

    public override void Back()
    {
        //AudioManager.Instance.UnPauseMusic();
        //backgroundTrack.volume = 1f;

        foreach (Transform child in PopupGOContainer.transform)
        {
            Destroy(child.gameObject);
        }

        LevelCompletObserver.LevelComplete();
        base.Back();
    }
}
