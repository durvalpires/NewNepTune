using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPopUpToggleManager : PopUpToggleManager
{
    public void ToggleInstrument(string instrument)
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

        popup1.GetComponent<Image>().sprite = Resources.Load<Sprite>($"Instruments/Sprites/{instrument}");
        
        popUpCanvas.SetActive(true);
        popup1.SetActive(true);
        _popUpActive = true;
    }
}
