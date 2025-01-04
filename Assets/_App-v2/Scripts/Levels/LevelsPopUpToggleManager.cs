using System;
using Audio;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopUpToggleManager : PopUpToggleManager
{
    
    private void Start()
    {
        
    }
    public void TogglePopup(string pressedNote)
    {
        
        TogglePopup1(pressedNote);
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
        
        AudioManager.Instance.PauseMusic();

        popup1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Instruments/Sprites/{instrument}");
        
        popUpCanvas.SetActive(true);
        popup1.SetActive(true);
        _popUpActive = true;
    }

    public override void Back()
    {
        AudioManager.Instance.UnPauseMusic();
        LevelCompletObserver.LevelComplete();
        base.Back();
    }
}
