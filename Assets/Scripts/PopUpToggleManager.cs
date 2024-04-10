using System;
using Audio;
using Enums;
using UnityEngine;

public class PopUpToggleManager : MonoBehaviour
{
    [SerializeField] private GameObject popUpCanvas;
    [SerializeField] private GameObject popup1;
    [SerializeField] private GameObject popup2;
    [SerializeField] private GameObject popup3;
    
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

    private void Back()
    {
        if (popup1 != null ) popup1.SetActive(false);
        if (popup2 != null ) popup2.SetActive(false);
        if (popup3 != null ) popup3.SetActive(false);
        popUpCanvas.SetActive(false);
        _popUpActive = false;
    }
}
