using System;
using System.Collections.Generic;
using Audio;
using Enums;
using UnityEngine;
using UnityEngine.Analytics;

public class PopUpToggleManager : MonoBehaviour
{
    public GameObject popup1; // Kontrol etmek istediğiniz GameObject
    public GameObject popup2;
    public GameObject popup3;

    public GameObject popUpCanvas;
    private bool _popUpActive;
    
    private void Update()
    {
        if (_popUpActive && Input.GetMouseButtonDown(0))
        {
          Back();
        }
    }
    
    // TODO : Turn this into a single function with a parameter
    public void TogglePopup1(string pressedNote)
    {
        SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
        // GameObject'in şu anki aktiflik durumunun tersini ayarlayın
        popUpCanvas.SetActive(true);
        popup1.SetActive(true);
        StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, 1f));
        _popUpActive = true;
    }
    public void TogglePopup2(string pressedNote)
    {
        SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
        popUpCanvas.SetActive(true);
        popup2.SetActive(true);
        StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, 1f));
        _popUpActive = true;
    }
    public void TogglePopup3(string pressedNote)
    {
        SoundList soundToPlay = (SoundList)Enum.Parse(typeof(SoundList), "note_" + pressedNote.ToUpper());
        popUpCanvas.SetActive(true);
        popup3.SetActive(true);
        StartCoroutine(AudioManager.Instance.PlaySFX(soundToPlay, 1f));
        _popUpActive = true;
    }

    private void Back()
    {
        popup1.SetActive(false);
        popup2.SetActive(false);
        popUpCanvas.SetActive(!popUpCanvas.activeSelf);
        _popUpActive = false;
    }
}
